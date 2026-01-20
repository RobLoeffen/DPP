import api from '@/api/client';
import type { DppProductDto, CarbonFootprintDto } from '@/types/dpp';
import type { AxiosResponse } from 'axios';

export const dppService = {
  getAllProducts(): Promise<AxiosResponse<DppProductDto[]>> {
    return api.get<DppProductDto[]>('/dpp');
  },

  getProductById(id: string): Promise<AxiosResponse<DppProductDto>> {
    return api.get<DppProductDto>(`/dpp/${id}`);
  },

  getCarbonFootprint(id: string): Promise<AxiosResponse<CarbonFootprintDto>> {
    return api.get<CarbonFootprintDto>(`/dpp/${id}/carbon-footprint`);
  }
};