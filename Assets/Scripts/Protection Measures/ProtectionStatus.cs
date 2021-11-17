using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectionStatus : MonoBehaviour
{
    //Mascarilla

    private bool maskWearingScenario = false;
    private float maskProtectionPercentage;

    //Vacunas
    private bool vaccinationScenario = false;
    private bool scheduledForVaccine = false;
    private int vaccinationDay;
    private int nDosesApplied;
    private int nDosesRequired;
    private float efficacyForInfection;
    private float efficacyForCriticalInfection;

    //Distanciamiento social
    private bool socialDistancing = true;
    private float distancingRadius;

    //Aislamiento (semáforo)



    // Start is called before the first frame update
    void Start()
    {
        maskProtectionPercentage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
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
                float[] placeMaskDistribution = ((SerializableFloatList) PolicyMaker.maskDistributions.placesDistributionHashtable[type]).floatList;
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
                for(int i = 0; i < travelDistribution.Length; i++)
                {
                    if(throwDice >= cumSum &&  throwDice <= cumSum + travelDistribution[i])
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

    public void setVaccinationScenario(bool activation)
    {
        this.vaccinationScenario = activation;
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
    }

    public void setDosesApplied(int dosesApplied)
    {
        this.nDosesApplied = dosesApplied;
    }

    public void setEfficacyForInfection(float efficacy)
    {
        this.efficacyForInfection= efficacy;
    }

    public void setEfficacyForCriticalInfection(float efficacy)
    {
        this.efficacyForCriticalInfection = efficacy;
    }

}
