using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
public class PolicyMaker
{

    public static MaskPlaceDistributions maskDistributions;
    public static string MASK_DISTRIBUTION_JSON_PATH;

    //Estas son las eficacias de la mascarilla acorde a lo que investigué
    public static float INFECTED_MASK_EFFICACY;
    public static float SUSCEPTIBLE_MASK_EFFICACY;
    public static float BOTH_PERSON_MASK_EFFICACY;

    public VaccineDistributions vaccineDistributions;
    public VaccinationProgram program = VaccinationProgram.LINEAR;
    public int vaccinationDayStart = 2;
    public int vaccinationDayEnd = 50;
    public static int daysBetweenVaccinations;

    public PolicyMaker()
    {
        MASK_DISTRIBUTION_JSON_PATH = Application.dataPath + "/ProtectionDistributionJSONS/data_mask_wearing_dists.json";
        string jsonString = File.ReadAllText(MASK_DISTRIBUTION_JSON_PATH, Encoding.UTF8);
        maskDistributions = JsonUtility.FromJson<MaskPlaceDistributions>(jsonString);
        Debug.Log(maskDistributions);
        vaccineDistributions = new VaccineDistributions();
        INFECTED_MASK_EFFICACY = 0.95f;
        SUSCEPTIBLE_MASK_EFFICACY = 0.85f;
        BOTH_PERSON_MASK_EFFICACY = 1.0f;

        daysBetweenVaccinations = (int)Mathf.Floor(((float)vaccinationDayEnd - (float)vaccinationDayStart) / 4f);
    }

    //Este método va a inicializar los agentes con el tipo de política que 
    //se establezca (o combinación)
    public void startAgents(List<GameObject> agents,PolicyType policyType)
    {
        switch (policyType)
            {
            case PolicyType.MASK_WEARING:
                for(int j = 0; j < agents.Count; j++)
                {
                    agents[j].GetComponent<ProtectionStatus>().activateMaskWearing();
                }
                break;

             case PolicyType.SOCIAL_DISTANCING:
                break;

             case PolicyType.VACCINES:

                float[] vaccineProbs = vaccineDistributions.vaccinatedDistribution;

                int nAgents = agents.Count;
                int agentsToBeScheduledForVaccine = nAgents;
                int[] vaccineSchedule = getVaccinationProgram(agentsToBeScheduledForVaccine, vaccinationDayStart, vaccinationDayEnd);
                ProtectionStatus.setVaccinationScenario(true);
                //Configuramos a los que si se vacunaran
                for(int i = 0; i < agentsToBeScheduledForVaccine; i++)
                {
                    ProtectionStatus protectionStatus = agents[i].GetComponent<ProtectionStatus>();
                    //Prendemos el escenario de vacunación
                    protectionStatus.setVaccinationDay(vaccineSchedule[i]);
                    protectionStatus.setScheduledForVaccine(true);
                    protectionStatus.setDosesApplied(0);
                    VaccineType vaccineType = vaccineDistributions.getRandomVaccineType();
                    int applications = vaccineDistributions.getNumberOfDoses(vaccineType);
                    float efficacyForContagion = vaccineDistributions.getVaccineEfficacyForContagion(vaccineType);
                    float efficacyForCriticalIllness = vaccineDistributions.getVaccineEfficacyForCriticalIllness(vaccineType);

                    //Debug.Log(vaccineType);
                    //Debug.Log(applications);
                    //Debug.Log(efficacyForContagion);
                    //Debug.Log(efficacyForCriticalIllness);

                    protectionStatus.setDosesRequired(applications);
                    protectionStatus.setEfficacyForInfection(efficacyForContagion);
                    protectionStatus.setEfficacyForCriticalInfection(efficacyForCriticalIllness);

                }
                //Configuramos a los que no se vacunaran
                for(int i = agentsToBeScheduledForVaccine; i < nAgents; i++)
                {
                    ProtectionStatus protectionStatus = agents[i].GetComponent<ProtectionStatus>();
                    protectionStatus.setScheduledForVaccine(false);
                    protectionStatus.setDosesApplied(0);
                    protectionStatus.setDosesRequired(0);
                    protectionStatus.setEfficacyForInfection(0);
                    protectionStatus.setEfficacyForCriticalInfection(0);

                }
                break;

             case PolicyType.STOPLIGHT:
                break;

            case PolicyType.NONE:
                break;
            }
      
    }


    public float getVaccinationContagionProtectionFactor(ProtectionStatus susceptibleProtection)
    {
        if (ProtectionStatus.vaccinationScenario)
        {
            if (susceptibleProtection.isVaccinated())
            {
                //Debug.Log("Vaccinated");
                //Debug.Log(susceptibleProtection.getDosesApplied());
                //Debug.Log(susceptibleProtection.getDosesNeeded());
                //Debug.Log(susceptibleProtection.getEfficacyForInfection());
                //Debug.Log(1.0f - ((float)susceptibleProtection.getDosesApplied() / (float)susceptibleProtection.getDosesNeeded()) * susceptibleProtection.getEfficacyForInfection());
                return 1.0f - ((float)susceptibleProtection.getDosesApplied() / (float)susceptibleProtection.getDosesNeeded()) * susceptibleProtection.getEfficacyForInfection();
            }
            else
            {
                return 1;
            }
        }
        else
        {
            return 1;
        }
    }

    public float getVaccinationCriticalProtectionFactor(ProtectionStatus susceptibleProtection)
    {
        if (ProtectionStatus.vaccinationScenario)
        {
            if (susceptibleProtection.isVaccinated())
            {
                return 1.0f - ((float)susceptibleProtection.getDosesApplied() / (float)susceptibleProtection.getDosesNeeded()) * susceptibleProtection.getEfficacyForCriticalInfection();
            }
            else
            {
                return 1;
            }
        }
        else
        {
            return 1;
        }
    }





    public float getMaskProtectionFactor(ProtectionStatus susceptibleProtection, ProtectionStatus infectedProtection)
    {
        float diceRoll = Random.value;

        bool firstAgentWearingMask = false;
        bool secondAgentWearingMask = false;
        if(diceRoll <= susceptibleProtection.getMaskProtectionFactor())
        {
            firstAgentWearingMask = true;
        }

        diceRoll = Random.value;
        if(diceRoll <= infectedProtection.getMaskProtectionFactor())
        {
            secondAgentWearingMask = true;
        }

        if(!firstAgentWearingMask && !secondAgentWearingMask)
        {
            return 1.0f;
        }
        else if(firstAgentWearingMask && !secondAgentWearingMask)
        {
            return 1.0f - SUSCEPTIBLE_MASK_EFFICACY;
        }
        else if(secondAgentWearingMask && !firstAgentWearingMask)
        {
            return 1.0f - INFECTED_MASK_EFFICACY;
        }
        else if(firstAgentWearingMask && secondAgentWearingMask)
        {
            return 1.0f - BOTH_PERSON_MASK_EFFICACY;
        }


        return 1.0f;
    }


    public int[] getVaccinationProgram(int nAgents, int dayStart, int dayEnd)
    {
        int[] schedule = new int[nAgents];
        switch (program)
        {
            case VaccinationProgram.LINEAR:
                int vaccinationsPerDay = (int)(((float)nAgents) / (dayEnd - dayStart));
                int day = dayStart;
                int dayCounter = 0;
                int agentsScheduled = 0;
                while(agentsScheduled < nAgents)
                {
                    schedule[agentsScheduled] = day;
                    agentsScheduled++;
                    dayCounter++;
                    if(dayCounter > vaccinationsPerDay)
                    {
                        dayCounter = 0;
                        day = day + 1;
                    }

                }
                break;
        }

        return schedule;

    }
}
