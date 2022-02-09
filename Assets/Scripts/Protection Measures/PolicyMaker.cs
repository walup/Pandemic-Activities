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
    public static int daysForVaccineDissipation = 100;

    //Semáforo
    public GameObject stoplight;
    public bool stoplightControl;
    public int hospitalCheckHour;

    public PolicyMaker()
    {
        MASK_DISTRIBUTION_JSON_PATH = Application.dataPath + "/ProtectionDistributionJSONS/data_mask_wearing_dists.json";
        string jsonString = File.ReadAllText(MASK_DISTRIBUTION_JSON_PATH, Encoding.UTF8);
        maskDistributions = JsonUtility.FromJson<MaskPlaceDistributions>(jsonString);
        vaccineDistributions = new VaccineDistributions();
        //Mask Scenario
        INFECTED_MASK_EFFICACY = 0.95f;
        SUSCEPTIBLE_MASK_EFFICACY = 0.85f;
        BOTH_PERSON_MASK_EFFICACY = 1.0f;
        //Time between first and second dose is one fourth of the total period of vaccination
        daysBetweenVaccinations = (int)Mathf.Floor(((float)vaccinationDayEnd - (float)vaccinationDayStart) / 4f);


        //
        stoplight = GameObject.Find("Stoplight");
        stoplightControl = false;
        hospitalCheckHour = 0;
    }

    //Este método va a inicializar los agentes con el tipo de política que 
    //se establezca (o combinación)
    public void startAgents(List<GameObject> agents,PolicyType policyType)
    {
        switch (policyType)
        {
            case PolicyType.MASK_WEARING:
                for (int j = 0; j < agents.Count; j++)
                {
                    agents[j].GetComponent<ProtectionStatus>().activateMaskWearing();
                }
                break;

            case PolicyType.SOCIAL_DISTANCING:
                break;

            case PolicyType.VACCINES:

                int nAgents = agents.Count;
                int agentsToBeScheduledForVaccine = nAgents;
                int[] vaccineSchedule = getVaccinationProgram(agentsToBeScheduledForVaccine, vaccinationDayStart, vaccinationDayEnd);
                ProtectionStatus.setVaccinationScenario(true);
                //Configuramos a los que si se vacunaran
                for (int i = 0; i < agentsToBeScheduledForVaccine; i++)
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
                for (int i = agentsToBeScheduledForVaccine; i < nAgents; i++)
                {
                    ProtectionStatus protectionStatus = agents[i].GetComponent<ProtectionStatus>();
                    protectionStatus.setScheduledForVaccine(false);
                    protectionStatus.setDosesApplied(0);
                    protectionStatus.setDosesRequired(0);
                    protectionStatus.setEfficacyForInfection(0);
                    protectionStatus.setEfficacyForCriticalInfection(0);

                }
                break;


            case PolicyType.FADING_VACCINES:

                int nAgents2 = agents.Count;
                int agentsToBeScheduled = nAgents2;
                int[] vaccineDates = getVaccinationProgram(agentsToBeScheduled, vaccinationDayStart, vaccinationDayEnd);
                ProtectionStatus.setVaccinationScenario(true);
                ProtectionStatus.setFadingScenario(true);


                for (int i = 0; i < agentsToBeScheduled; i++)
                {
                    //Obtenemos el estado de protección del agente
                    ProtectionStatus agentVaccineProtection = agents[i].GetComponent<ProtectionStatus>();
                    agentVaccineProtection.setVaccinationDay(vaccineDates[i]);
                    agentVaccineProtection.setScheduledForVaccine(true);
                    agentVaccineProtection.setDosesApplied(0);

                    VaccineType vaccineType = vaccineDistributions.getRandomVaccineType();
                    float efficacyForContagion = vaccineDistributions.getVaccineEfficacyForContagion(vaccineType);
                    float efficacyForCriticalIllness = vaccineDistributions.getVaccineEfficacyForCriticalIllness(vaccineType);
                    int applications = vaccineDistributions.getNumberOfDoses(vaccineType);

                    agentVaccineProtection.setDosesRequired(applications);
                    agentVaccineProtection.setEfficacyForInfection(efficacyForContagion);
                    agentVaccineProtection.setEfficacyForCriticalInfection(efficacyForCriticalIllness);
                }
                //Configuramos a los agentes que no se vacunaran 
                for (int i = agentsToBeScheduled + 1; i < nAgents2; i++)
                {
                    ProtectionStatus agentVaccineProtection = agents[i].GetComponent<ProtectionStatus>();
                    agentVaccineProtection.setScheduledForVaccine(false);
                    agentVaccineProtection.setDosesApplied(0);
                    agentVaccineProtection.setDosesRequired(0);
                    agentVaccineProtection.setEfficacyForInfection(0);
                    agentVaccineProtection.setEfficacyForCriticalInfection(0);

                }

                break;
            case PolicyType.ISOLATION:
                this.stoplightControl = true;

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
                
                return 1.0f - ((float)susceptibleProtection.getDosesApplied() / (float)susceptibleProtection.getDosesNeeded()) * susceptibleProtection.getEffectiveEfficacyForInfection();
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
                return ((float)susceptibleProtection.getDosesApplied() / (float)susceptibleProtection.getDosesNeeded()) * susceptibleProtection.getEffectiveEfficacyForCriticalInfection();
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

    //Methods isolaiton policy
    public bool isStopLightPolicy()
    {
        return stoplightControl;
    }

    public bool isStoplightOn()
    {
        return this.stoplight.GetComponent<StoplightControl>().isStopLightOn();
    }

    public bool endedIsolation()
    {
        return this.stoplight.GetComponent<StoplightControl>().stoppedIsolation();
    }

    public void isolateAgents(List<GameObject> agents)
    {

        int nAgents = agents.Count;
        for(int i = 0; i < nAgents; i++)
        {
            agents[i].GetComponent<ProtectionStatus>().setIsolating(true);
            agents[i].GetComponent<ActivityStatus>().goHome();
        }

        //Prendemos el semáforo
        StoplightControl stoplightControl = stoplight.GetComponent<StoplightControl>();
        stoplightControl.setStopLight(true);
        stoplightControl.setDayStart(Clock.dayVal);
    }

    public void freeAgents(List<GameObject> agents)
    {
        int nAgents = agents.Count;
        for(int i = 0; i < nAgents; i++)
        {
            agents[i].GetComponent<ProtectionStatus>().setIsolating(false);
        }
        StoplightControl stoplightControl = stoplight.GetComponent<StoplightControl>();
        stoplightControl.setEndedIsolation(false);
    }


}
