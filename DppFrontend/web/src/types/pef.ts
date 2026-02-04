export interface PefEmissionDetailDto {
  stof: string;
  origineleWaarde: number;
  gwpFactor: number;
  co2Eq: number;
}

export interface PefStageBreakdownDto {
  fase: string;
  co2Eq: number;
  emissieDetails: PefEmissionDetailDto[];
}

export interface PefResultDto {
  totaalCo2Eq: number;
  eenheid: string;
  eenheidToelichting: string;
  methode: string;
  uitsplitsing: PefStageBreakdownDto[];
}
