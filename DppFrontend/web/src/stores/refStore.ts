import { defineStore } from 'pinia';
import { refService } from '@/services/refService';
import type { PefResultDto, PefStageBreakdownDto, PefEmissionDetailDto } from '@/types/pef';

interface RefState {
  pefResults: Map<string, PefResultDto>; // Cache PEF results by product ID
  currentPefResult: PefResultDto | null;
  loading: boolean;
  error: string | null;
}

export const useRefStore = defineStore('ref', {
  state: (): RefState => ({
    pefResults: new Map(),
    currentPefResult: null,
    loading: false,
    error: null
  }),

  getters: {
    // Get cached PEF result by product ID
    getPefResultById: (state) => {
      return (id: string): PefResultDto | undefined => {
        return state.pefResults.get(id);
      };
    },

    // Check if we have any PEF results cached
    hasPefResults: (state): boolean => state.pefResults.size > 0,

    // Get total CO2 equivalent from current PEF result
    totalCo2Eq: (state): number => {
      return state.currentPefResult?.totaalCo2Eq ?? 0;
    },

    // Get the unit of measurement (e.g., "kg CO2-eq")
    measurementUnit: (state): string => {
      return state.currentPefResult?.eenheid ?? '';
    },

    // Get method used (should be "pef")
    calculationMethod: (state): string => {
      return state.currentPefResult?.methode ?? '';
    },

    // Get the highest emission phase
    highestEmissionPhase: (state): PefStageBreakdownDto | null => {
      if (!state.currentPefResult?.uitsplitsing) return null;

      return state.currentPefResult.uitsplitsing.reduce((max, current) =>
        current.co2Eq > max.co2Eq ? current : max
      );
    },

    // Get all phases sorted by emissions (highest first)
    phasesSortedByEmissions: (state): PefStageBreakdownDto[] => {
      if (!state.currentPefResult?.uitsplitsing) return [];

      return [...state.currentPefResult.uitsplitsing].sort(
        (a, b) => b.co2Eq - a.co2Eq
      );
    },

    // Get the substance with highest emission across all phases
    highestEmissionSubstance: (state): { 
      substance: string; 
      co2Eq: number; 
      phase: string 
    } | null => {
      if (!state.currentPefResult?.uitsplitsing) return null;
      
      let maxEmission: PefEmissionDetailDto | null = null;
      let maxPhase = '';

      for (const phase of state.currentPefResult.uitsplitsing) {
        for (const emission of phase.emissieDetails) {
          if (!maxEmission || emission.co2Eq > maxEmission.co2Eq) {
            maxEmission = emission;
            maxPhase = phase.fase;
          }
        }
      }
      
      if (!maxEmission) return null;
      
      return {
        substance: maxEmission.stof,
        co2Eq: maxEmission.co2Eq,
        phase: maxPhase
      };
    },

    // Get total number of unique substances tracked
    totalUniqueSubstances: (state): number => {
      if (!state.currentPefResult?.uitsplitsing) return 0;

      const substances = new Set<string>();
      for (const phase of state.currentPefResult.uitsplitsing) {
        for (const emission of phase.emissieDetails) {
          substances.add(emission.stof);
        }
      }
      
      return substances.size;
    },

    // Get emissions breakdown by phase as percentages
    phasePercentages: (state): Array<{ phase: string; percentage: number }> => {
      if (!state.currentPefResult?.uitsplitsing || state.currentPefResult.totaalCo2Eq === 0) {
        return [];
      }

      return state.currentPefResult.uitsplitsing.map(phase => ({
        phase: phase.fase,
        percentage: (phase.co2Eq / state.currentPefResult!.totaalCo2Eq) * 100
      }));
    }
  },

  actions: {
    // Fetch PEF result for a specific product
    async fetchPefResult(productId: string): Promise<PefResultDto> {
      this.loading = true;
      this.error = null;
      try {
        const response = await refService.getPefResult(productId);
        this.currentPefResult = response.data;
        
        // Cache the result
        this.pefResults.set(productId, response.data);
        
        return response.data;
      } catch (error) {
        const message = error instanceof Error 
          ? error.message 
          : 'Failed to fetch PEF result';
        this.error = message;
        throw error;
      } finally {
        this.loading = false;
      }
    },

    // Load a cached PEF result as current (if exists)
    loadCachedPefResult(productId: string): boolean {
      const cached = this.pefResults.get(productId);
      if (cached) {
        this.currentPefResult = cached;
        this.error = null;
        return true;
      }
      return false;
    },

    // Clear current PEF result
    clearCurrentPefResult(): void {
      this.currentPefResult = null;
      this.error = null;
    },

    // Clear all cached results
    clearCache(): void {
      this.pefResults.clear();
      this.currentPefResult = null;
      this.error = null;
    },

    // Reset entire store to initial state
    resetStore(): void {
      this.$reset();
    }
  }
});
