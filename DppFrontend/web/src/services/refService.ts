import api from '@/api/client';
import type { PefResultDto } from '@/types/pef';
import type { AxiosResponse } from 'axios';

export const refService = {
    getPefResult: async (id: string): Promise<AxiosResponse<PefResultDto>> => {
        return api.get(`/dpp/${id}/carbon-footprint?method=pef`);
    }
};