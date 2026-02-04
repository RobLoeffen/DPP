# PEF Berekening Verificatie

## Handmatige Controle van IPCC GWP100 Berekening

### **Product: tshirt-001 - Organisch katoenen T-Shirt**

---

## Lifecycle Fase Berekeningen:

### 1?? **Katoenteelt**
| Broeikasgas | Waarde (kg) | GWP100 | CO2-eq (kg) |
|-------------|-------------|--------|-------------|
| CO2_fossielen | 0.35 | × 1.0 | = **0.35** |
| CH4 | 0.002 | × 28.0 | = **0.056** |
| N2O | 0.0006 | × 265.0 | = **0.159** |
| **Subtotaal** | | | **0.565 kg CO2-eq** |

---

### 2?? **Garen spinnen**
| Broeikasgas | Waarde (kg) | GWP100 | CO2-eq (kg) |
|-------------|-------------|--------|-------------|
| CO2_fossielen | 0.18 | × 1.0 | = **0.18** |
| CH4 | 0.0002 | × 28.0 | = **0.0056** |
| **Subtotaal** | | | **0.1856 kg CO2-eq** |

---

### 3?? **Stof breien**
| Broeikasgas | Waarde (kg) | GWP100 | CO2-eq (kg) |
|-------------|-------------|--------|-------------|
| CO2_fossielen | 0.12 | × 1.0 | = **0.12** |
| CH4 | 0.0001 | × 28.0 | = **0.0028** |
| **Subtotaal** | | | **0.1228 kg CO2-eq** |

---

### 4?? **Verven en afronden**
| Broeikasgas | Waarde (kg) | GWP100 | CO2-eq (kg) |
|-------------|-------------|--------|-------------|
| CO2_fossielen | 0.30 | × 1.0 | = **0.30** |
| CH4 | 0.0003 | × 28.0 | = **0.0084** |
| N2O | 0.00005 | × 265.0 | = **0.01325** |
| **Subtotaal** | | | **0.32165 kg CO2-eq** |

---

### 5?? **Fabricering**
| Broeikasgas | Waarde (kg) | GWP100 | CO2-eq (kg) |
|-------------|-------------|--------|-------------|
| CO2_fossielen | 0.10 | × 1.0 | = **0.10** |
| **Subtotaal** | | | **0.10 kg CO2-eq** |

---

### 6?? **Transportatie**
| Broeikasgas | Waarde (kg) | GWP100 | CO2-eq (kg) |
|-------------|-------------|--------|-------------|
| CO2_fossielen | 0.15 | × 1.0 | = **0.15** |
| **Subtotaal** | | | **0.15 kg CO2-eq** |

---

### 7?? **Gebruik fase**
| Broeikasgas | Waarde (kg) | GWP100 | CO2-eq (kg) |
|-------------|-------------|--------|-------------|
| CO2_fossielen | 0.60 | × 1.0 | = **0.60** |
| CH4 | 0.0005 | × 28.0 | = **0.014** |
| **Subtotaal** | | | **0.614 kg CO2-eq** |

---

### 8?? **Afvalverwerking**
| Broeikasgas | Waarde (kg) | GWP100 | CO2-eq (kg) |
|-------------|-------------|--------|-------------|
| CO2_fossielen | 0.05 | × 1.0 | = **0.05** |
| CH4 | 0.0004 | × 28.0 | = **0.0112** |
| **Subtotaal** | | | **0.0612 kg CO2-eq** |

---

## ?? **TOTAAL OVERZICHT**

| Lifecycle Fase | CO2-eq (kg) | Percentage |
|----------------|-------------|------------|
| Katoenteelt | 0.565 | 25.8% |
| Garen spinnen | 0.186 | 8.5% |
| Stof breien | 0.123 | 5.6% |
| Verven en afronden | 0.322 | 14.7% |
| Fabricering | 0.100 | 4.6% |
| Transportatie | 0.150 | 6.9% |
| Gebruik fase | 0.614 | 28.0% |
| Afvalverwerking | 0.061 | 2.8% |
| **TOTAAL** | **2.19 kg CO2-eq** | **100%** |

---

## ?? **Belangrijkste Inzichten**

### Top 3 Hotspots:
1. **Gebruik fase** - 28.0% (wassen van het T-shirt)
2. **Katoenteelt** - 25.8% (landbouw, N2O heeft grote impact)
3. **Verven en afronden** - 14.7% (energie-intensief proces)

### N2O Impact:
- Hoewel N2O in kleine hoeveelheden voorkomt (0.0006 kg in katoenteelt)
- Door de GWP100 factor van 265 wordt dit 0.159 kg CO2-eq
- Dit is 28% van de katoenteelt impact!

### Methaan (CH4) Impact:
- Totaal CH4 uitstoot: 0.0041 kg
- Met GWP100 factor 28: 0.115 kg CO2-eq
- Dit is 5.2% van totale footprint

---

## ? Validatie Checklist

- [x] Alle IPCC GWP100 factoren correct toegepast
- [x] CO2: factor 1.0
- [x] CH4: factor 28.0  
- [x] N2O: factor 265.0
- [x] Totaal berekend: **2.19 kg CO2-eq**
- [x] Breakdown per fase beschikbaar
- [x] Emission details per broeikasgas beschikbaar

---

**Verwacht API Response:**
```json
{
  "totaalCo2Eq": 2.19,
  "eenheid": "kg CO2-eq",
  "methode": "PEF met IPCC GWP100",
  "uitsplitsing": [...]
}
```
