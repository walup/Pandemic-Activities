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
    private bool socialDistancingWhileMoving = false;
    private bool socialDistancingInPlaces = false;
    private float movingDistancingPercentage = 0.1f;
    private float insideDistancingPercentage = 1 - 0.3f;
    private float movingDistancing = 0;

    //Aislamiento (semáforo parcial)
    private bool isolating;

    //Obediencias
    private float obedienceMaskWearing = 0;
    private float obedienceIsolation = 0;
    private float obedienceSocialDistancing = 0;

    // Start is called before the first frame update
    void Start()
    {

        GameObject city = GameObject.Find("PopulationGenerator");
        CityPopulation population = city.GetComponent<CityPopulation>();
        //Inicializamos los distanciamientos
        movingDistancing = movingDistancingPercentage * population.getSickRadius();
    }

    // Update is called once per frame
    void Update()
    {
        if (vaccinationScenario)
        {
            if (scheduledForVaccine && Clock.dayVal == vaccinationDay)
            {
                nDosesApplied += 1;
                Debug.Log("Vaccinated nDoses = " + nDosesApplied);
                if (nDosesApplied == nDosesRequired)
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

    public void turnOffMaskWearing()
    {
        this.maskWearingScenario = false;
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
        if (!maskWearingScenario || obedienceMaskWearing == 0)
        {
            return 0;
        }
        else if (obedienceMaskWearing == 1) {

            return maskProtectionPercentage;
        }
        else
        {
            if(Random.value < obedienceMaskWearing)
            {
                return 0;
            }
            else
            {
                return maskProtectionPercentage;
            }
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
        if (dosesRequired == 0)
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
            if (dayFinalVaccine > 0)
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

        if (!isolating || obedienceIsolation == 0) {
            return false;

        }
        else if(obedienceIsolation == 1)
        {
            return true;
        }
        else
        {
            if(Random.value < obedienceIsolation)
            {
                return true;
            }
            return false;
        }
    }


    public void setSocialDistancingWhileMoving(bool socialDistancingMoving)
    {
        this.socialDistancingWhileMoving = socialDistancingMoving;
    }

    public void setSocialDistancingInPlaces(bool socialDistancingInPlaces)
    {
        this.socialDistancingInPlaces = socialDistancingInPlaces;
    }

    public bool isSocialDistancingMoving()
    {
        return socialDistancingWhileMoving;
    }

    public bool isSocialDistancingInPlaces()
    {
        return socialDistancingInPlaces;
    }

    public float getMovingDistancing()
    {
        if (!socialDistancingWhileMoving || obedienceSocialDistancing == 0)
        {
            return 0;
        }
        else if(obedienceSocialDistancing == 1)
        {
            return movingDistancing;
        }
        else
        {
            if(Random.value > obedienceSocialDistancing)
            {
                return 0;
            }
            return movingDistancing;
        }
    }

    public float getInPlacesDistancingFactor()
    {
        if (!socialDistancingInPlaces || obedienceSocialDistancing == 0)
        {
            return 1;
        }

        else if(obedienceSocialDistancing == 1)
        {
            return insideDistancingPercentage;
        }

        else
        {
            if(Random.value > obedienceSocialDistancing)
            {
                return 1;
            }
            return insideDistancingPercentage;
        }
    }



    public void setObedienceMaskWearing(float obedienceMaskWearing)
    {
        this.obedienceMaskWearing = obedienceMaskWearing;
    }

    public void setObedienceIsolation(float obedienceIsolation)
    {
        this.obedienceIsolation = obedienceIsolation;
    }

    public void setObedienceSocialDistancing(float obedienceSocialDistancing)
    {
        this.obedienceSocialDistancing = obedienceSocialDistancing;
    }
}