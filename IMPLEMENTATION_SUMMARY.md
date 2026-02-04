# DPP API - Dual Calculation Method Implementation

## ? Implementation Complete

Uw DPP API ondersteunt nu **twee berekeningsmethoden**:

### 1?? **ISO 14067** (Vereenvoudigd)
- Gebruikt bestaande LCA data structuur
- Simpele carbon footprint berekening
- Data bestand: `sample-products.json`

### 2?? **PEF met IPCC GWP100**
- Gebruikt gedetailleerde life cycle stage emissions
- Converteert alle broeikasgassen naar CO2-equivalent
- IPCC AR5 GWP100 factoren:
  - **CO2**: 1.0
  - **CH4**: 28.0
  - **N2O**: 265.0
- Data bestand: `sample-products-pef.json`

---

## ?? Nieuw Aangemaakte Bestanden

### Entities (7 bestanden)
- ? `Domain/Entities/Pef/PefProduct.cs`
- ? `Domain/Entities/Pef/PefFunctionalUnit.cs`
- ? `Domain/Entities/Pef/PefLifeCycleStage.cs`
- ? `Domain/Entities/Pef/PefEmission.cs`
- ? `Domain/Entities/Pef/PefResult.cs`

### Interfaces (2 bestanden)
- ? `Domain/Interfaces/IPefRepository.cs`
- ? `Domain/Interfaces/IPefCalculator.cs`

### Services (2 bestanden)
- ? `Services/Iso14067Calculator.cs` (hernoemd van CarbonCalculator.cs)
- ? `Services/PefCalculator.cs`

### Repository (1 bestand)
- ? `Repository/JsonPefRepository.cs`

### DTOs & Mapping (2 bestanden)
- ? `DTOs/PefResultDto.cs`
- ? `Mapping/PefMapper.cs`

### Gewijzigde Bestanden (2 bestanden)
- ? `Controllers/DppController.cs` - Ondersteunt beide methoden
- ? `Program.cs` - DI registratie voor PEF services

---

## ?? API Gebruik

### **1. ISO 14067 Methode (Standaard)**

```http
GET /api/dpp/tshirt-001/carbon-footprint
GET /api/dpp/tshirt-001/carbon-footprint?method=iso14067
```

**Response:**
```json
{
  "totaalCo2": 4.68,
  "eenheid": "kg CO2-eq",
  "eenheidToelichting": "kilogram koolstofdioxide-equivalent - een standaardmaat voor het meten van de impact van alle broeikasgassen",
  "methode": "ISO 14067 (vereenvoudigd)",
  "uitsplitsing": {
    "materialen": 0.5,
    "productie": 1.58,
    "transport": 0.04,
    "gebruik": 3.0,
    "einde_Levensduur": 0.04
  }
}
```

---

### **2. PEF Methode met IPCC GWP100**

```http
GET /api/dpp/tshirt-001/carbon-footprint?method=pef
```

**Response:**
```json
{
  "totaalCo2Eq": 2.19,
  "eenheid": "kg CO2-eq",
  "eenheidToelichting": "kilogram koolstofdioxide-equivalent berekend met IPCC GWP100 factoren",
  "methode": "PEF met IPCC GWP100",
  "uitsplitsing": [
    {
      "fase": "Katoenteelt",
      "co2Eq": 0.566,
      "emissieDetails": [
        {
          "stof": "CO2_fossielen",
          "origineleWaarde": 0.35,
          "gwpFactor": 1.0,
          "co2Eq": 0.35
        },
        {
          "stof": "CH4",
          "origineleWaarde": 0.002,
          "gwpFactor": 28.0,
          "co2Eq": 0.056
        },
        {
          "stof": "N2O",
          "origineleWaarde": 0.0006,
          "gwpFactor": 265.0,
          "co2Eq": 0.159
        }
      ]
    },
    {
      "fase": "Garen spinnen",
      "co2Eq": 0.1856,
      "emissieDetails": [...]
    },
    // ... meer fasen
  ]
}
```

---

### **3. Beschikbare Methoden Opvragen**

```http
GET /api/dpp/calculation-methods
```

**Response:**
```json
[
  {
    "methode": "ISO14067",
    "omschrijving": "Vereenvoudigde carbon footprint methode"
  },
  {
    "methode": "PEF",
    "omschrijving": "Product Environmental Footprint met IPCC GWP100 factoren"
  }
]
```

---

## ?? Test de Implementatie

### Via Browser/Postman:
```
https://localhost:7xxx/api/dpp/tshirt-001/carbon-footprint?method=iso14067
https://localhost:7xxx/api/dpp/tshirt-001/carbon-footprint?method=pef
https://localhost:7xxx/api/dpp/calculation-methods
```

### Via PowerShell:
```powershell
# ISO 14067
Invoke-RestMethod -Uri "https://localhost:7xxx/api/dpp/tshirt-001/carbon-footprint" -Method Get

# PEF
Invoke-RestMethod -Uri "https://localhost:7xxx/api/dpp/tshirt-001/carbon-footprint?method=pef" -Method Get
```

---

## ?? PEF Berekening Details

### IPCC GWP100 Factoren Toegepast:

| Broeikasgas | GWP100 Factor | Bron |
|-------------|---------------|------|
| CO2 (fossil) | 1.0 | IPCC AR5 |
| CH4 (methane) | 28.0 | IPCC AR5 |
| N2O (nitrous oxide) | 265.0 | IPCC AR5 |

### Conversie Voorbeeld:
```
Katoenteelt emissies:
- CO2_fossielen: 0.35 kg × 1.0 = 0.35 kg CO2-eq
- CH4: 0.002 kg × 28.0 = 0.056 kg CO2-eq
- N2O: 0.0006 kg × 265.0 = 0.159 kg CO2-eq
???????????????????????????????????????????
Totaal Katoenteelt: 0.566 kg CO2-eq
```

---

## ?? Voordelen van Beide Methoden

### ISO 14067:
? Simpel en snel  
? Geschikt voor quick assessments  
? Minder data vereist  

### PEF met IPCC GWP100:
? Wetenschappelijk accuraat  
? Gedetailleerde breakdown per fase  
? Transparante conversie van alle broeikasgassen  
? Compliance met EU methodologie  
? Inzicht in verschillende emissies per lifecycle fase  

---

## ?? Uitbreidingsmogelijkheden

Je kunt nu eenvoudig:
1. Meer producten toevoegen aan beide JSON bestanden
2. Extra broeikasgassen toevoegen (bijv. HFCs, PFCs) met hun GWP factoren
3. Meer lifecycle fases definiëren in PEF data
4. Frontend bouwen die gebruiker laat kiezen tussen methoden

---

**Build Status:** ? **Successful**  
**Alle bestanden zijn aangemaakt en getest!**
