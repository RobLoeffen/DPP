<template>
  <div class="product-detail">
    <button @click="router.back()">← Back</button>

    <div v-if="dppStore.loading" class="loading">
      Loading...
    </div>

    <div v-else-if="dppStore.currentProduct">
      <h1>{{ dppStore.currentProduct.name }}</h1>
      <p>Product ID: {{ dppStore.currentProduct.id }}</p>

      <div class="section">
        <h2>Materials ({{ dppStore.totalMaterialsKg.toFixed(2) }} kg total)</h2>
        <ul>
          <li v-for="material in dppStore.currentProduct.materials" :key="material.name">
            {{ material.name }}: {{ material.kg }} kg
          </li>
        </ul>
      </div>

      <button 
        @click="loadCarbonFootprint" 
        :disabled="dppStore.loading"
        class="btn-primary"
      >
        Calculate Carbon Footprint
      </button>

      <!-- Carbon Footprint Results -->
      <div v-if="dppStore.carbonFootprint" class="carbon-results">
        <h2>Carbon Footprint</h2>
        <div class="total">
          <strong>Total CO₂:</strong> 
          {{ dppStore.carbonFootprint.totalCo2 }} {{ dppStore.carbonFootprint.unit }}
        </div>
        <p><em>Method: {{ dppStore.carbonFootprint.method }}</em></p>
        <p v-if="dppStore.highestEmissionCategory" class="highlight">
          Highest emissions: {{ dppStore.highestEmissionCategory }}
        </p>

        <h3>Breakdown:</h3>
        <ul class="breakdown">
          <li>Materials: {{ dppStore.carbonFootprint.breakdown.materials }} kg CO₂-eq</li>
          <li>Production: {{ dppStore.carbonFootprint.breakdown.production }} kg CO₂-eq</li>
          <li>Transport: {{ dppStore.carbonFootprint.breakdown.transport }} kg CO₂-eq</li>
          <li>Use: {{ dppStore.carbonFootprint.breakdown.use }} kg CO₂-eq</li>
          <li>End of Life: {{ dppStore.carbonFootprint.breakdown.endOfLife }} kg CO₂-eq</li>
        </ul>

        <!-- Visual Chart -->
        <div class="chart">
          <div 
            v-for="(value, key) in dppStore.carbonFootprint.breakdown" 
            :key="key"
            class="bar"
            :style="{ width: getBarWidth(value) }"
          >
            {{ key }}: {{ value }}
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useDppStore } from '@/stores/dppStore';

const route = useRoute();
const router = useRouter();
const dppStore = useDppStore();

onMounted(async () => {
  const id = route.params.id as string;
  await dppStore.fetchProductById(id);
});

const loadCarbonFootprint = async (): Promise<void> => {
  const id = route.params.id as string;
  await dppStore.fetchCarbonFootprint(id);
};

const getBarWidth = (value: number): string => {
  if (!dppStore.carbonFootprint) return '0%';
  const max = dppStore.carbonFootprint.totalCo2;
  return `${(value / max) * 100}%`;
};
</script>

<style scoped>
.carbon-results {
  margin-top: 2rem;
  padding: 1.5rem;
  background: #f5f5f5;
  border-radius: 8px;
}

.total {
  font-size: 1.5rem;
  margin: 1rem 0;
}

.highlight {
  color: #ff6b6b;
  font-weight: bold;
}

.breakdown {
  list-style: none;
  padding: 0;
}

.breakdown li {
  padding: 0.5rem 0;
  border-bottom: 1px solid #ddd;
}

.bar {
  background: #4CAF50;
  color: white;
  padding: 0.5rem;
  margin: 0.25rem 0;
  border-radius: 4px;
  transition: width 0.3s ease;
}

.btn-primary {
  background: #4CAF50;
  color: white;
  padding: 0.75rem 1.5rem;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 1rem;
}

.btn-primary:disabled {
  background: #ccc;
  cursor: not-allowed;
}
</style>