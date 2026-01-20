<script setup lang="ts">
// import ProductDetail from '../components/ProductDetail.vue'
import ProductList from '../components/ProductList.vue'
</script>

<template>
  <div class="about">
    <ProductList />
  </div>
</template>

<style>
@media (min-width: 1024px) {
  .about {
    min-height: 100vh;
    display: flex;
    align-items: center;
  }
}
</style>
