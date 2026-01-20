<script setup lang="ts">
import { onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { useDppStore } from '@/stores/dppStore';

const router = useRouter();
const dppStore = useDppStore();

onMounted(async () => {
  if (!dppStore.hasProducts) {
    await dppStore.fetchAllProducts();
  }
});

const viewProduct = (id: string): void => {
  router.push({ name: 'product-detail', params: { id } });
};
</script>

<template>
  <div class="product-list">
    <h2>Digital Product Passports</h2>
    
    <!-- Loading State -->
    <div v-if="dppStore.loading" class="loading">
      Loading products...
    </div>

    <!-- Error State -->
    <div v-if="dppStore.error" class="error">
      {{ dppStore.error }}
    </div>

    <!-- Products List -->
    <div v-else class="products">
      <div 
        v-for="product in dppStore.products" 
        :key="product.id"
        class="product-card"
        @click="viewProduct(product.id)"
      >
        <h3>{{ product.name }}</h3>
        <p>ID: {{ product.id }}</p>
        <div class="materials">
          <h4>Materials:</h4>
          <ul>
            <li v-for="material in product.materials" :key="material.name">
              {{ material.name }}: {{ material.kg }} kg
            </li>
          </ul>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.product-card {
  border: 1px solid #ccc;
  padding: 1rem;
  margin: 1rem 0;
  cursor: pointer;
  transition: box-shadow 0.3s;
}

.product-card:hover {
  box-shadow: 0 2px 8px rgba(0,0,0,0.1);
}

.loading, .error {
  padding: 1rem;
  text-align: center;
}

.error {
  color: red;
}
</style>