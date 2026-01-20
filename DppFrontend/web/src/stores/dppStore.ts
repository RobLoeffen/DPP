import { defineStore } from 'pinia';
import { dppService } from '@/services/dppService';
import type { DppProductDto, CarbonFootprintDto } from '@/types/dpp';

interface DppState {
  products: DppProductDto[];
  currentProduct: DppProductDto | null;
  carbonFootprint: CarbonFootprintDto | null;
  loading: boolean;
  error: string | null;
}

export const useDppStore = defineStore('dpp', {
  state: (): DppState => ({
    products: [],
    currentProduct: null,
    carbonFootprint: null,
    loading: false,
    error: null
  }),

  getters: {
    // Get product by ID from cached products
    getProductById: (state) => {
      return (id: string): DppProductDto | undefined => {
        return state.products.find(p => p.id === id);
      };
    },

    // Check if data is loaded
    hasProducts: (state): boolean => state.products.length > 0,

    // Get total materials for current product
    totalMaterialsKg: (state): number => {
      if (!state.currentProduct?.materials) return 0;
      return state.currentProduct.materials.reduce((sum, m) => sum + m.kg, 0);
    },

    // Get highest emission category
    highestEmissionCategory: (state): string | null => {
      if (!state.carbonFootprint) return null;
      
      const breakdown = state.carbonFootprint.breakdown;
      const categories = Object.entries(breakdown);
      const highest = categories.reduce((max, curr) => 
        curr[1] > max[1] ? curr : max
      );
      
      return highest[0];
    }
  },

  actions: {
    // Fetch all products
    async fetchAllProducts(): Promise<DppProductDto[]> {
      this.loading = true;
      this.error = null;
      try {
        const response = await dppService.getAllProducts();
        this.products = response.data;
        return response.data;
      } catch (error) {
        const message = error instanceof Error 
          ? error.message 
          : 'Failed to fetch products';
        this.error = message;
        throw error;
      } finally {
        this.loading = false;
      }
    },

    // Fetch single product by ID
    async fetchProductById(id: string): Promise<DppProductDto> {
      this.loading = true;
      this.error = null;
      try {
        const response = await dppService.getProductById(id);
        this.currentProduct = response.data;
        
        // Also update in products array if it exists
        const index = this.products.findIndex(p => p.id === id);
        if (index !== -1) {
          this.products[index] = response.data;
        } else {
          this.products.push(response.data);
        }
        
        return response.data;
      } catch (error) {
        const message = error instanceof Error 
          ? error.message 
          : 'Failed to fetch product';
        this.error = message;
        throw error;
      } finally {
        this.loading = false;
      }
    },

    // Fetch carbon footprint for a product
    async fetchCarbonFootprint(id: string): Promise<CarbonFootprintDto> {
      this.loading = true;
      this.error = null;
      try {
        const response = await dppService.getCarbonFootprint(id);
        this.carbonFootprint = response.data;
        return response.data;
      } catch (error) {
        const message = error instanceof Error 
          ? error.message 
          : 'Failed to fetch carbon footprint';
        this.error = message;
        throw error;
      } finally {
        this.loading = false;
      }
    },

    // Clear current product and footprint
    clearCurrentData(): void {
      this.currentProduct = null;
      this.carbonFootprint = null;
      this.error = null;
    },

    // Reset entire store
    resetStore(): void {
      this.$reset();
    }
  }
});