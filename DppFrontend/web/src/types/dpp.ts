export interface MaterialDto {
  name: string;
  kg: number;
}

export interface DppProductDto {
  id: string;
  name: string;
  materials: MaterialDto[];
}

export interface BreakdownDto {
  materials: number;
  production: number;
  transport: number;
  use: number;
  endOfLife: number;
}

export interface CarbonFootprintDto {
  totalCo2: number;
  unit: string;
  method: string;
  breakdown: BreakdownDto;
}

export interface ApiError {
  message: string;
  status?: number;
}