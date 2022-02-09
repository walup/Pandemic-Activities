using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectionStatus : MonoBehaviour
{
    //Mascarilla

    private bool maskWearingScenario = false;
    private float maskProtectionPercentage;

    //Vacunas
    private bool scheduledForVaccine = false;
    private int vaccinationDay;
    private int nDosesApplied;
    private int nDosesRequired;
    private float efficacyForInfection;
    private float efficacyForCriticalInfection;
    public static bool vaccinationScenario = false;
    public static bool fadingVaccine = false;
    private int dayFinalVaccine = 0;
    private int daysSinceVaccine = 0;

    //Distanciamiento social
    private bool socialDistancing = true;
    private float distancingRadius;

    //Aislamiento (semáforo parcial)
    private bool isolating;



    // Start is called before the first frame update
    void Start()
    {
        maskProtectionPercentage = 0;
        //efficacyForInfection = 0;
        //efficacyForCriticalInfection = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (vaccinationScenario)
        {
            if (scheduledForVaccine && Clock.dayVal == vaccinationDay)
            {
                nDosesApplied += 1;
                Debug.Log("Vaccinated nDoses = "+nDosesApplied);
                if(nDosesApplied == nDosesRequired)
                {
                    scheduledForVaccine = false;
                    dayFinalVaccine = (int)Clock.dayVal;
                }

                else
                {
                    vaccinationDay = vaccinationDay + PolicyMaker.daysBetweenVaccinations;
                }
            }
        }
    }


       
    public void activateMaskWearing()
    {
        this.maskWearingScenario = true;
    }


    public void updateAgentMaskProtection(BuildingType type, bool moving)
    {
        if (maskWearingScenario)
        {
            if (!moving)
            {

                float[] placeMaskDistribution = ((SerializableFloatList)PolicyMaker.maskDistributions.placesDistributionHashtable[type]).floatList;
                float[] placeMaskDistributionValues = ((SerializableFloatList)PolicyMaker.maskDistributions.placesDistributionValuesHashtable[type]).floatList;


                float cumSum = 0;
                float throwDice = Random.value;
                int index = 0;
                for (int i = 0; i < placeMaskDistribution.Length; i++)
                {
                    if (throwDice >= cumSum && throwDice <= cumSum + placeMaskDistribution[i])
                    {
                        index = i;
                        break;
                    }
                    cumSum += placeMaskDistribution[i];
                }

                this.maskProtectionPercentage = placeMaskDistributionValues[index];
            }
            else
            {
                float[] travelDistribution = (float[])PolicyMaker.maskDistributions.movilizationDistribution;
                float[] travelDistributionValues = (float[])PolicyMaker.maskDistributions.movilizationDistributionValues;

                float cumSum = 0;
                float throwDice = Random.value;
                int index = 0;
                for (int i = 0; i < travelDistribution.Length; i++)
                {
                    if (throwDice >= cumSum && throwDice <= cumSum + travelDistribution[i])
                    {
                        index = i;
                        break;
                    }
                    cumSum += travelDistribution[i];
                }
                this.maskProtectionPercentage = travelDistributionValues[index];
            }
        }
    }

    public float getMaskProtectionFactor()
    {
        if (maskWearingScenario)
        {
            return maskProtectionPercentage;
        }
        else
        {
            return 0;
        }
    }

    public static void setVaccinationScenario(bool activation)
    {
        vaccinationScenario = activation;
    }

    public void setScheduledForVaccine(bool scheduledForVaccine)
    {
        this.scheduledForVaccine = scheduledForVaccine;
    }

    public void setVaccinationDay(int vaccinationDay)
    {
        this.vaccinationDay = vaccinationDay;
    }

    public void setDosesRequired(int dosesRequired)
    {
        this.nDosesRequired = dosesRequired;
        if(dosesRequired == 0)
        {
            scheduledForVaccine = false;
            Debug.Log("Not Scheduled for vaccine");
        }
    }

    public void setDosesApplied(int dosesApplied)
    {
        this.nDosesApplied = dosesApplied;
    }

    public void setEfficacyForInfection(float efficacy)
    {

        this.efficacyForInfection = efficacy;
    }

    public void setEfficacyForCriticalInfection(float efficacy)
    {
        this.efficacyForCriticalInfection = efficacy;
    }
        
    public float getEfficacyForInfection()
    {
         return this.efficacyForInfection;
    }

    public float getEffectiveEfficacyForInfection()
    {
        if (!fadingVaccine)
        {
            return this.efficacyForInfection;
        }
        else
        {
            if (dayFinalVaccine > 0)
            {
                daysSinceVaccine = (int)Clock.dayVal - dayFinalVaccine;
                if (PolicyMaker.daysForVaccineDissipation > daysSinceVaccine)
                {
                    return this.efficacyForInfection * (1 - (float)daysSinceVaccine / (float)PolicyMaker.daysForVaccineDissipation);
                }
                else
                {
                    return 0;
                }
            }

        }
        return this.efficacyForInfection;
    }

    public float getEfficacyForCriticalInfection()
    {
        return this.efficacyForCriticalInfection;
    }

    public float getEffectiveEfficacyForCriticalInfection()
    {
        if (!fadingVaccine)
        {
            return this.efficacyForCriticalInfection;
        }
        else
        {
            if(dayFinalVaccine > 0)
            {
                daysSinceVaccine = (int)Clock.dayVal - dayFinalVaccine;
                if (PolicyMaker.daysForVaccineDissipation > daysSinceVaccine)
                {
                    return this.efficacyForCriticalInfection * (1 - (float)daysSinceVaccine / (float)PolicyMaker.daysForVaccineDissipation);
                }
                else
                {
                    return 0;
                }
            }

        }
        return this.efficacyForCriticalInfection;
    }

    public int getDosesApplied()
    {
        return nDosesApplied;
    }


    public int getDosesNeeded()
    {
        return nDosesRequired;
    } 

    public bool isVaccinated()
    {
        return nDosesApplied >= 1;
    }

    public static void setFadingScenario(bool scenario)
    {
        fadingVaccine = scenario;
    }

    public void setIsolating(bool isolating)
    {
        this.isolating = isolating;
    }

    public bool isIsolating()
    {
        return this.isolating;
    }

    } 
