<script setup lang="ts">
import { computed, onMounted, ref, nextTick } from 'vue';
import { useDppStore } from '@/stores/dppStore';

const dppStore = useDppStore();
const selectedId = ref<string | null>(null);
const productDetailsRef = ref<HTMLElement | null>(null);
const resultsRef = ref<HTMLElement | null>(null);

onMounted(async () => {
  if (!dppStore.hasProducts) {
    await dppStore.fetchAllProducts();
  }
});

const currentProduct = computed(() => {
  if (!selectedId.value) return null;
  return dppStore.getProductById(selectedId.value) ?? dppStore.currentProduct;
});

const selectProduct = async (id: string): Promise<void> => {
  selectedId.value = id;
  dppStore.carbonFootprint = null;
  await dppStore.fetchProductById(id);
  await nextTick();
  productDetailsRef.value?.scrollIntoView({ behavior: 'smooth', block: 'start' });
};

const calculateFootprint = async (): Promise<void> => {
  if (!selectedId.value) return;
  await dppStore.fetchCarbonFootprint(selectedId.value);
  await nextTick();
  resultsRef.value?.scrollIntoView({ behavior: 'smooth', block: 'start' });
};

const calculatePercentage = (value: number): string => {
  if (dppStore.carbonFootprint?.totalCo2 === 0) {
    return '0.0';
  }
  return ((value / (dppStore.carbonFootprint?.totalCo2 || 1)) * 100).toFixed(1);
};
</script>

<template>
  <div class="container">
    <header class="header">
      <h1>CO₂-voetafdruk Calculator</h1>
      <p>Selecteer een product om de milieu-impact te berekenen</p>
    </header>

    <main class="main">
      <section class="section">
        <h2>Selecteer Product</h2>
        <div v-if="dppStore.loading && dppStore.products.length === 0">
          Producten laden...
        </div>
        <div v-else-if="dppStore.error" class="error">
          Fout: {{ dppStore.error }}
        </div>
        <div v-else class="product-list">
          <label 
            v-for="product in dppStore.products" 
            :key="product.id"
            class="product-option"
          >
            <input 
              type="radio" 
              :value="product.id"
              v-model="selectedId"
              @change="selectProduct(product.id)"
            />
            <span>{{ product.name }}</span>
          </label>
        </div>
      </section>

      <section v-if="currentProduct" class="section" ref="productDetailsRef">
        <h2>Productdetails</h2>
        <table class="details-table">
          <caption class="sr-only">Productdetails overzicht</caption>
          <tbody>
            <tr>
              <th>Product ID</th>
              <td>{{ currentProduct.id }}</td>
            </tr>
            <tr>
              <th>Productnaam</th>
              <td>{{ currentProduct.name }}</td>
            </tr>
            <tr>
              <th>Totaal Materialen</th>
              <td>{{ dppStore.totalMaterialsKg.toFixed(2) }} kg</td>
            </tr>
          </tbody>
        </table>

        <h3>Materialen Overzicht</h3>
        <table class="materials-table">
          <caption class="sr-only">Materialen en gewichten</caption>
          <thead>
            <tr>
              <th>Materiaal</th>
              <th>Gewicht (kg)</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="material in currentProduct.materials" :key="material.name">
              <td>{{ material.name }}</td>
              <td>{{ material.kg }}</td>
            </tr>
          </tbody>
        </table>

        <button 
          class="calculate-btn"
          :disabled="dppStore.loading"
          @click="calculateFootprint"
        >
          {{ dppStore.loading ? 'Berekenen...' : 'Bereken CO₂-voetafdruk' }}
        </button>
      </section>

      <section v-if="dppStore.carbonFootprint" class="section results" ref="resultsRef">
        <h2>CO₂-voetafdruk Resultaten</h2>
        
        <div class="total-result">
          <div class="total-label">Totale CO₂-voetafdruk</div>
          <div class="total-value">
            {{ dppStore.carbonFootprint.totalCo2 }} {{ dppStore.carbonFootprint.unit }}
          </div>
        </div>

        <p class="method">Berekeningsmethode: {{ dppStore.carbonFootprint.method }}</p>

        <h3>Uitstoot Overzicht</h3>
        <table class="breakdown-table">
          <caption class="sr-only">CO₂-uitstoot per categorie</caption>
          <thead>
            <tr>
              <th>Categorie</th>
              <th>Uitstoot (kg CO₂-eq)</th>
              <th>Percentage</th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td>Materialen</td>
              <td>{{ dppStore.carbonFootprint.breakdown.materials.toFixed(2) }}</td>
              <td>{{ calculatePercentage(dppStore.carbonFootprint.breakdown.materials) }}%</td>
            </tr>
            <tr>
              <td>Productie</td>
              <td>{{ dppStore.carbonFootprint.breakdown.production.toFixed(2) }}</td>
              <td>{{ calculatePercentage(dppStore.carbonFootprint.breakdown.production) }}%</td>
            </tr>
            <tr>
              <td>Transport</td>
              <td>{{ dppStore.carbonFootprint.breakdown.transport.toFixed(2) }}</td>
              <td>{{ calculatePercentage(dppStore.carbonFootprint.breakdown.transport) }}%</td>
            </tr>
            <tr>
              <td>Gebruiksfase</td>
              <td>{{ dppStore.carbonFootprint.breakdown.use.toFixed(2) }}</td>
              <td>{{ calculatePercentage(dppStore.carbonFootprint.breakdown.use) }}%</td>
            </tr>
            <tr>
              <td>Einde Levensduur</td>
              <td>{{ dppStore.carbonFootprint.breakdown.endOfLife.toFixed(2) }}</td>
              <td>{{ calculatePercentage(dppStore.carbonFootprint.breakdown.endOfLife) }}%</td>
            </tr>
          </tbody>
        </table>

        <p v-if="dppStore.highestEmissionCategory" class="insight">
          Hoogste impact categorie: {{ dppStore.highestEmissionCategory }}
        </p>
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

.product-list {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.product-option {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  padding: 0.75rem;
  background: white;
  border: 1px solid #ddd;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.2s;
}

.product-option:hover {
  background-color: #f9f9f9;
}

.product-option input[type="radio"] {
  cursor: pointer;
}

.product-option span {
  font-size: 1rem;
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
  margin-bottom: 1.5rem;
  font-style: italic;
}

.insight {
  margin-top: 1.5rem;
  padding: 1rem;
  background: #fff3cd;
  border: 1px solid #ffc107;
  border-radius: 4px;
  color: #856404;
  font-weight: 500;
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