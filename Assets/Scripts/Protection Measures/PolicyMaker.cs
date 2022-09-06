using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
public class PolicyMaker
{

    public static MaskPlaceDistributions maskDistributions;
    public static string MASK_DISTRIBUTION_JSON_PATH;

    //Estas son las eficacias de la mascarilla acorde a lo que investigué
    public static float INFECTED_MASK_EFFICACY;
    public static float SUSCEPTIBLE_MASK_EFFICACY;
    public static float BOTH_PERSON_MASK_EFFICACY;

    //Vacunación
    public VaccineDistributions vaccineDistributions;
    public VaccinationProgram program = VaccinationProgram.LINEAR;
    public int vaccinationDayStart = 2;
    public int vaccinationDayEnd = 50;
    public static int daysBetweenVaccinations;
    public static int daysForVaccineDissipation = 100;

    //Semáforo (aislamiento y política de semáforo)
    public GameObject stoplight;
    public bool stoplightControl;
    public int hospitalCheckHour;

    //Distanciamiento social 
    public static float REPULSION_RADIUS = 0.1f;
    //El distanciamiento social comienza ´después de que empieza la simulación
    //esto ayuda a que no se atasque al principio cuando todos estan moviendose.
    public static int DAY_START_SOCIAL_DISTANCING = 1;
    public bool allPoliciesStoplight = false;

    // Semáforo (todas combinadas)
    public string OBEDIENCE_DISTRIBUTIONS_PATH;
    public ObedienceStoplightDistributions obedienceDistributions;

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


        //Semáforo control
        stoplight = GameObject.Find("Stoplight");
        stoplightControl = false;
        hospitalCheckHour = 0;

        //Distribuciones de obediencia al semáforo
        OBEDIENCE_DISTRIBUTIONS_PATH = Application.dataPath + "/ProtectionDistributionJSONS/obedience_distributions.json";
        string obedienceJSONString = File.ReadAllText(OBEDIENCE_DISTRIBUTIONS_PATH);
        obedienceDistributions = JsonUtility.FromJson<ObedienceStoplightDistributions>(obedienceJSONString);
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
                    ProtectionStatus protectionStatus = agents[j].GetComponent<ProtectionStatus>();
                    protectionStatus.activateMaskWearing();
                    //Todos los agentes obedecen el uso de mascarilla en esta política.
                    protectionStatus.setObedienceMaskWearing(1);
                }
                break;

            case PolicyType.SOCIAL_DISTANCING:
                for (int i = 0; i < agents.Count; i++)
                {
                    ProtectionStatus protectionStatus = agents[i].GetComponent<ProtectionStatus>();
                    protectionStatus.setSocialDistancingWhileMoving(true);
                    protectionStatus.setSocialDistancingInPlaces(true);
                    //Todos los agentes obedecen el distancimiento social en esta política
                    protectionStatus.setObedienceSocialDistancing(1);
                }
                break;

            case PolicyType.VACCINES:

                int nAgents = agents.Count;
                int agentsToBeScheduledForVaccine = nAgents;
                int[] vaccineSchedule = getVaccinationProgram(agentsToBeScheduledForVaccine, vaccinationDayStart, vaccinationDayEnd);
                ProtectionStatus.setVaccinationScenario(true);
                ProtectionStatus.setFadingScenario(false);
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
                //Todos los agentes obedecen el aislamiento en esta política 
                for(int i = 0; i < agents.Count; i++)
                {
                    ProtectionStatus protectionStatus = agents[i].GetComponent<ProtectionStatus>();
                    protectionStatus.setObedienceIsolation(1);
                }
                break;

            case PolicyType.STOPLIGHT:
                this.allPoliciesStoplight = true;
                this.stoplightControl = true;
                //Voy a suponer que todos los agentes obedecen ´tanto el distanciamiento social como
                //el uso de mascarilla
                for(int i = 0; i < agents.Count; i++)
                {
                    ProtectionStatus protectionStatus = agents[i].GetComponent<ProtectionStatus>();
                    protectionStatus.activateMaskWearing();
                    protectionStatus.setSocialDistancingWhileMoving(true);
                    protectionStatus.setSocialDistancingInPlaces(true);
                    protectionStatus.setObedienceMaskWearing(1);
                    protectionStatus.setObedienceSocialDistancing(1);
                    protectionStatus.setIsolating(false);



                    float randVal = UnityEngine.Random.value;
                    float cumSumObedienceIsolation = 0;
                    int indexObedienceIsolation = 0;
                    for(int j = 0; j < obedienceDistributions.obedienceIsolationDistribution.Length; j++)
                    {
                        if(randVal >= cumSumObedienceIsolation && randVal <= cumSumObedienceIsolation + obedienceDistributions.obedienceIsolationDistribution[j])
                        {
                            indexObedienceIsolation = j;
                            break;
                        }
                        cumSumObedienceIsolation = cumSumObedienceIsolation + obedienceDistributions.obedienceIsolationDistribution[j];
                    }

                    protectionStatus.setObedienceIsolation(obedienceDistributions.obedienceIsolationValues[indexObedienceIsolation]);
                }


                break;




             //case PolicyType.STOPLIGHT:
                //this.allPoliciesStoplight = true;
                //this.stoplightControl = true;

                //float randVal = 0;
                
                //for(int i = 0; i < agents.Count; i++)
                //{
                    //randVal = UnityEngine.Random.value;
                    //float cumSum = 0;
                    //int index = 0;
                    //ProtectionStatus protectionStatus = agents[i].GetComponent<ProtectionStatus>() ;

                    //for(int j = 0; i < obedienceDistributions.obedienceDistribution.Length; j++)
                    
                    //{
                        //if(randVal >= cumSum && randVal <= cumSum + obedienceDistributions.obedienceDistribution[j])
                        //{
                            //index = j;
                            //break;
                        //}
                        //cumSum = cumSum + obedienceDistributions.obedienceDistribution[j];
                    //}

                    //Defina si obedecerán el semáforo
                    //bool stoplightObedience = Convert.ToBoolean(obedienceDistributions.obedienceValues[index]);
                    //if (!stoplightObedience)
                    //{
                        //protectionStatus.setObedienceIsolation(0);
                        //protectionStatus.setObedienceMaskWearing(0);
                        //protectionStatus.setObedienceSocialDistancing(0);
                    //}
                    //else
                    //{
                        //randVal = UnityEngine.Random.value;
                        //int indexMask = 0;
                        //float cumSumMask = 0;
                        //Obediencia al uso de mascarilla
                        //for(int j =0; j < obedienceDistributions.obedienceMaskUseDistribution.Length; j++)
                        //{
                            //if(randVal >= cumSumMask && randVal <= cumSumMask + obedienceDistributions.obedienceMaskUseDistribution[j])
                            //{
                                //indexMask = j;
                                //break;
                            //}
                            //cumSumMask += obedienceDistributions.obedienceMaskUseDistribution[j];
                        //}
                        //protectionStatus.setObedienceMaskWearing(obedienceDistributions.obedienceMaskUseValues[indexMask]);

                        //Obediencia al aislamiento
                        //int indexIsolation = 0;
                        //float cumSumIsolation = 0;
                        //randVal = UnityEngine.Random.value;
                        //for(int j = 0; j < obedienceDistributions.obedienceIsolationDistribution.Length; j++)
                        //{
                             //if(randVal >= cumSumIsolation && randVal <= cumSumIsolation + obedienceDistributions.obedienceIsolationDistribution[j])
                            //{
                                //indexIsolation = j;
                                //break;
                            //}
                            //cumSumIsolation += obedienceDistributions.obedienceIsolationDistribution[j];
                        //}
                        //protectionStatus.setObedienceIsolation(obedienceDistributions.obedienceIsolationDistribution[indexIsolation]);

                        //int indexSocialDistancing = 0;
                        //float cumSumSocialDistancing = 0;

                        //randVal = UnityEngine.Random.value;
                        //for(int j = 0; j < obedienceDistributions.obedienceSocialDistancingDistribution.Length; j++)
                        //{
                            //if(randVal >= cumSumSocialDistancing && randVal <= cumSumSocialDistancing + obedienceDistributions.obedienceSocialDistancingDistribution[j])
                            //{
                                //indexSocialDistancing = j;
                                //break;
                            //}
                            //cumSumSocialDistancing += obedienceDistributions.obedienceSocialDistancingDistribution[j];
                        //}
                        //protectionStatus.setObedienceSocialDistancing(obedienceDistributions.obedienceSocialDistancingValues[indexSocialDistancing]);

                        //protectionStatus.activateMaskWearing();
                        //protectionStatus.setIsolating(true);
                    //}


                //}

                //break;

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
                return 1.0f;
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
                return 0;
            }
        }
        else
        {
            return 0;
        }
    }





    public float getMaskProtectionFactor(ProtectionStatus susceptibleProtection, ProtectionStatus infectedProtection)
    {
        float diceRoll = UnityEngine.Random.value;

        bool firstAgentWearingMask = false;
        bool secondAgentWearingMask = false;
        if(diceRoll <= susceptibleProtection.getMaskProtectionFactor())
        {
            firstAgentWearingMask = true;
        }

        diceRoll = UnityEngine.Random.value;
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
        //Reiniciamos la bandera stopLightControl para que el semáforo pueda prenderse
        //de nuevo.
        StoplightControl stoplightControl = stoplight.GetComponent<StoplightControl>();
        stoplightControl.setEndedIsolation(false);
    }

    //Métodos para la política de semáforo
    public void protectAgents(List<GameObject> agents)
    {
        int nAgents = agents.Count;
        for(int i = 0; i < nAgents; i++)
        {
            ProtectionStatus protection = agents[i].GetComponent<ProtectionStatus>();
            protection.setIsolating(true);
            //protection.activateMaskWearing();
            agents[i].GetComponent<ActivityStatus>().goHome();
        }

        StoplightControl stoplightControl = stoplight.GetComponent<StoplightControl>();
        stoplightControl.setStopLight(true);
        stoplightControl.setDayStart(Clock.dayVal);

    }

    public void unProtectAgents(List<GameObject> agents)
    {
        int nAgents = agents.Count;
        for(int i = 0; i < nAgents; i++)
        {
            ProtectionStatus protection = agents[i].GetComponent<ProtectionStatus>();
            protection.setIsolating(false);
            //protection.turnOffMaskWearing();
        }
        StoplightControl stoplightControl = stoplight.GetComponent<StoplightControl>();
        stoplightControl.setEndedIsolation(false);
    }

    public bool isAllProtectionPolicy()
    {
        return allPoliciesStoplight;
    }

}
