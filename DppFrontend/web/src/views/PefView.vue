<script setup lang="ts">
import { onMounted, ref, nextTick } from 'vue';
import { useRoute } from 'vue-router';
import { useRefStore } from '@/stores/refStore';

const route = useRoute();
const refStore = useRefStore();
const resultsRef = ref<HTMLElement | null>(null);

onMounted(async () => {
  await loadPefData();
});

const loadPefData = async (): Promise<void> => {
  const id = route.params.id as string;
  
  // Try to load from cache first
  if (!refStore.loadCachedPefResult(id)) {
    // If not in cache, fetch from API
    await refStore.fetchPefResult(id);
    await nextTick();
    resultsRef.value?.scrollIntoView({ behavior: 'smooth', block: 'start' });
  }
};

const retryLoad = async (): Promise<void> => {
  await loadPefData();
};

const calculatePercentage = (value: number): string => {
  if (refStore.totalCo2Eq === 0) {
    return '0.0';
  }
  return ((value / refStore.totalCo2Eq) * 100).toFixed(1);
};
</script>

<template>
  <div class="container">
    <header class="header">
      <h1>Product Environmental Footprint (PEF)</h1>
      <p>Milieu-impact analyse voor Product ID: {{ route.params.id }}</p>
    </header>

    <main class="main">
      <section v-if="refStore.loading" class="section">
        <h2>PEF Data Laden...</h2>
        <p>Even geduld, de milieudata wordt opgehaald...</p>
      </section>

      <section v-else-if="refStore.error" class="section">
        <h2>Fout</h2>
        <div class="error">
          {{ refStore.error }}
        </div>
        <button class="calculate-btn" @click="retryLoad" style="margin-top: 1rem;">
          Opnieuw proberen
        </button>
      </section>

      <template v-else-if="refStore.currentPefResult">
        <section class="section results" ref="resultsRef">
          <h2>PEF Resultaten</h2>
          
          <div class="total-result">
            <div class="total-label">Totale CO₂ Equivalent</div>
            <div class="total-value">
              {{ refStore.totalCo2Eq.toFixed(2) }} {{ refStore.measurementUnit }}
            </div>
          </div>

          <p class="method">Berekeningsmethode: {{ refStore.calculationMethod.toUpperCase() }}</p>
          <p class="method">{{ refStore.currentPefResult.eenheidToelichting }}</p>

          <h3>Kerngegevens</h3>
          <table class="details-table">
            <caption class="sr-only">PEF kerngegevens overzicht</caption>
            <tbody>
              <tr v-if="refStore.highestEmissionPhase">
                <th>Hoogste Impact Fase</th>
                <td>{{ refStore.highestEmissionPhase.fase }} ({{ refStore.highestEmissionPhase.co2Eq.toFixed(2) }} kg CO₂-eq)</td>
              </tr>
              <tr v-if="refStore.highestEmissionSubstance">
                <th>Grootste Uitstootbron</th>
                <td>{{ refStore.highestEmissionSubstance.substance }} ({{ refStore.highestEmissionSubstance.co2Eq.toFixed(2) }} kg CO₂-eq)</td>
              </tr>              <tr>
                <th>Aantal Geanalyseerde Stoffen</th>
                <td>{{ refStore.totalUniqueSubstances }}</td>
              </tr>
              <tr>
                <th>Aantal Levenscyclusfasen</th>
                <td>{{ refStore.currentPefResult?.uitsplitsing?.length ?? 0 }}</td>
              </tr>
            </tbody>
          </table>

          <h3>Uitstoot per Levenscyclusfase</h3>
          <table class="breakdown-table">
            <caption class="sr-only">CO₂-uitstoot per levenscyclusfase</caption>
            <thead>
              <tr>
                <th>Fase</th>
                <th>Uitstoot (kg CO₂-eq)</th>
                <th>Percentage</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="phase in refStore.phasesSortedByEmissions" :key="phase.fase">
                <td>{{ phase.fase }}</td>
                <td>{{ phase.co2Eq.toFixed(2) }}</td>
                <td>{{ calculatePercentage(phase.co2Eq) }}%</td>
              </tr>
            </tbody>
          </table>
        </section>

        <section 
          v-for="phase in refStore.phasesSortedByEmissions" 
          :key="phase.fase"
          class="section"
        >
          <h2>{{ phase.fase }}</h2>
          <div class="phase-summary">
            <p><strong>Totale uitstoot:</strong> {{ phase.co2Eq.toFixed(2) }} kg CO₂-eq ({{ calculatePercentage(phase.co2Eq) }}%)</p>
            <p><strong>Aantal emissiebronnen:</strong> {{ phase.emissieDetails.length }}</p>
          </div>

          <h3>Gedetailleerde Emissies</h3>
          <table class="materials-table">
            <caption class="sr-only">Emissiebronnen voor {{ phase.fase }}</caption>
            <thead>
              <tr>
                <th>Stof</th>
                <th>Originele Waarde</th>
                <th>GWP Factor</th>
                <th>CO₂ Equivalent</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="emission in phase.emissieDetails" :key="emission.stof">
                <td>{{ emission.stof }}</td>
                <td>{{ emission.origineleWaarde.toFixed(4) }}</td>
                <td>{{ emission.gwpFactor.toFixed(2) }}</td>
                <td><strong>{{ emission.co2Eq.toFixed(4) }}</strong></td>
              </tr>
            </tbody>
          </table>
        </section>
      </template>

      <section v-else class="section">
        <h2>Geen Data Beschikbaar</h2>
        <p>Er is geen PEF data beschikbaar voor dit product.</p>
        <button class="calculate-btn" @click="retryLoad">
          Data Laden
        </button>
      </section>
    </main>
  </div>
</template>

<style scoped>
.container {
  max-width: 900px;
  margin: 0 auto;
  padding: 2rem 1rem;
}

.header {
  text-align: center;
  margin-bottom: 3rem;
  padding-bottom: 1.5rem;
  border-bottom: 2px solid #ddd;
}

.header h1 {
  font-size: 2rem;
  color: #333;
  margin-bottom: 0.5rem;
}

.header p {
  color: #666;
  font-size: 1rem;
}

.main {
  display: flex;
  flex-direction: column;
  gap: 2rem;
}

.section {
  background: #e0f2fe;
  border: 1px solid #ddd;
  border-radius: 4px;
  padding: 1.5rem;
}

.section h2 {
  font-size: 1.3rem;
  color: #333;
  margin-bottom: 1rem;
  padding-bottom: 0.5rem;
  border-bottom: 1px solid #eee;
}

.section h3 {
  font-size: 1.1rem;
  color: #333;
  margin: 1.5rem 0 1rem;
}

.phase-summary {
  background: white;
  padding: 1rem;
  border-radius: 4px;
  border: 1px solid #ddd;
  margin-bottom: 1rem;
}

.phase-summary p {
  margin: 0.5rem 0;
  color: #333;
}

.details-table,
.materials-table,
.breakdown-table {
  width: 100%;
  border-collapse: collapse;
  margin-bottom: 1rem;
  border: 2px solid #bbb;
}

.details-table th,
.details-table td,
.materials-table th,
.materials-table td,
.breakdown-table th,
.breakdown-table td {
  padding: 0.75rem;
  text-align: left;
  border: 1px solid #ccc;
}

.details-table th,
.materials-table th,
.breakdown-table th {
  font-weight: 600;
  color: #222;
  background-color: #e8f4f8;
}

.details-table td,
.materials-table td,
.breakdown-table td {
  color: #333;
  background-color: white;
}

.materials-table thead,
.breakdown-table thead {
  background-color: #e8f4f8;
}

.calculate-btn {
  width: 100%;
  padding: 1rem;
  background-color: #007bff;
  color: white;
  border: none;
  border-radius: 4px;
  font-size: 1rem;
  font-weight: 600;
  cursor: pointer;
  transition: background-color 0.2s;
  margin-top: 1rem;
}

.calculate-btn:hover:not(:disabled) {
  background-color: #0056b3;
}

.calculate-btn:disabled {
  background-color: #ccc;
  cursor: not-allowed;
}

.results {
  background-color: #f0f8ff;
  border-color: #007bff;
}

.total-result {
  background: white;
  border: 2px solid #007bff;
  border-radius: 4px;
  padding: 1.5rem;
  text-align: center;
  margin-bottom: 1.5rem;
}

.total-label {
  font-size: 0.9rem;
  color: #666;
  margin-bottom: 0.5rem;
  text-transform: uppercase;
  letter-spacing: 0.05em;
}

.total-value {
  font-size: 2.5rem;
  font-weight: 700;
  color: #007bff;
}

.method {
  font-size: 0.9rem;
  color: #666;
  margin-bottom: 1rem;
  font-style: italic;
}

.error {
  color: #dc3545;
  padding: 1rem;
  background: #f8d7da;
  border: 1px solid #f5c6cb;
  border-radius: 4px;
}

.sr-only {
  position: absolute;
  width: 1px;
  height: 1px;
  padding: 0;
  margin: -1px;
  overflow: hidden;
  clip: rect(0, 0, 0, 0);
  white-space: nowrap;
  border-width: 0;
}

@media (max-width: 640px) {
  .container {
    padding: 1rem 0.5rem;
  }

  .header h1 {
    font-size: 1.5rem;
  }

  .total-value {
    font-size: 2rem;
  }

  .details-table th,
  .details-table td,
  .materials-table th,
  .materials-table td,
  .breakdown-table th,
  .breakdown-table td {
    padding: 0.5rem;
    font-size: 0.9rem;
  }
}
</style>
