using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
public class CityPopulation : MonoBehaviour
{
    private List<GameObject> agents;
    [SerializeField] private float portionLightAgents = 1f;
    private AgentFactory agentFactory;
    private City city;
    private int nAgents;
    [Range(0, 1)]
    [SerializeField] private float visionFraction = 0.1f;
    private float sickRadius;
    [Header("Parámetros de infección")]
    [Range(0, 1)]
    [SerializeField] private float alpha = 0.8f;
    [Range(0, 1)]
    [SerializeField] private float beta = 0.06f;
    [Range(0, 1)]
    [SerializeField] private float beta1 = 0.7f;
    [Range(0, 1)]
    [SerializeField] private float gamma = 0.05f;
    [Range(0, 1)]
    [SerializeField] private float gamma1 = 0.06f;
    [Range(0, 1)]
    [SerializeField] private float omega = 0.7f;
    [Range(0, 1)]
    [SerializeField] private float omega1 = 0.7f;
    [Range(0, 1)]
    [SerializeField] private float omega2 = 0.7f;
    [SerializeField] private PolicyType policy = PolicyType.NONE;
    int timeCount = 0;
    Graph graph;

    //Prueba para la tasa de mutación
    private float nInfected;

    PolicyMaker policyMaker;

    // Start is called before the first frame update
    void Start()
    {
        initializeAgents();
        city = GameObject.Find("City").GetComponent<City>();
        ToroidalWorld world = GameObject.Find("Main Camera").GetComponent<ToroidalWorld>();
        float radius = Mathf.Min(world.getWorldHeight(), world.getWorldWidth());
        sickRadius = radius * visionFraction;
        Debug.Log("Número de habitantes " + nAgents);
        //Inicialización de los gráficos
        initializeGraphs();
        //Inicializamos el aplicador de políticas
        policyMaker = new PolicyMaker();
        policyMaker.startAgents(agents, policy);
    }

    // Update is called once per frame
    void Update()
    {
        //updateAgentsInteractionsForR0Calibration();
        updateAgentsInteractions();
        updateGraphs();

        if (this.policyMaker.isStopLightPolicy() && Clock.hour != policyMaker.hospitalCheckHour)
        {
            updateStoplight();
            policyMaker.hospitalCheckHour = Clock.hour;
        }
    }


    void OnApplicationQuit()
    {
        //Al finalizar exportamos las graficas de susceptibles, expuestos, infectados y recuperados
        graph.exportGraph();
        //exportReproductiveNumber();
    }

    private void initializeAgents()
    {
        
        agentFactory = GetComponent<AgentFactory>();
        agents = new List<GameObject>();
        nAgents = City.cityPopulation;
        int numberOfLightAgents = (int)Mathf.Floor(portionLightAgents * nAgents);
        int numberOfSavers = nAgents - numberOfLightAgents;

        for(int i = 0; i < numberOfLightAgents; i++)
        {
            GameObject agent = agentFactory.getAgent(AgentType.LIGHT_AGENT);
            agents.Add(agent);
        }

        for (int i = 0; i < numberOfSavers; i++)
        {
            GameObject agent = agentFactory.getAgent(AgentType.DATA_SAVER);
            agents.Add(agent);
        }

        //El primer infectado
        nInfected = 10;
        for(int i = 0; i < nInfected; i++)
        {
            agents[i].GetComponent<HealthStatus>().setHealthState(HealthState.ASYMPTOMATIC_INFECTED);
        }

    }

    private void initializeGraphs()
    {
        graph = GameObject.Find("Graph").GetComponent<Graph>();
        graph.clearAllTimeSeries();
        //Serie de tiempo de susceptibles
        TimeSeries susceptibleSeries = new TimeSeries("Susceptibles");
        susceptibleSeries.setColor("#00bdff");
        susceptibleSeries.setLineWidth(2);
        //Serie de tiempo de expuestos
        TimeSeries exposedSeries = new TimeSeries("Expuestos");
        exposedSeries.setColor("#ffb624");
        exposedSeries.setLineWidth(2);
        //Serie de tiempo de infectados
        TimeSeries infectedSeries = new TimeSeries("Infectados");
        infectedSeries.setColor("#ff2424");
        infectedSeries.setLineWidth(2);
        //Serie de tiempo de los recuperados
        TimeSeries recoveredSeries = new TimeSeries("Recuperados");
        recoveredSeries.setColor("#2bff36");
        recoveredSeries.setLineWidth(2);
        //Serie de tiempo de los fallecidos 
        TimeSeries deceasedSeries = new TimeSeries("Fallecidos");
        deceasedSeries.setColor("#917c6f");
        deceasedSeries.setLineWidth(2);
        //Añadimos las series de tiempo
        graph.addTimeSeries(susceptibleSeries);
        graph.addTimeSeries(exposedSeries);
        graph.addTimeSeries(infectedSeries);
        graph.addTimeSeries(recoveredSeries);
        graph.addTimeSeries(deceasedSeries);
        //Agregamos los valores iniciales
        graph.addPointToTimeSeries("Susceptibles", new Point(0, nAgents-2));
        graph.addPointToTimeSeries("Expuestos", new Point(0, 0));
        graph.addPointToTimeSeries("Infectados", new Point(0, 2));
        graph.addPointToTimeSeries("Recuperados", new Point(0, 0));
        graph.addPointToTimeSeries("Fallecidos", new Point(0, 0));
    }

    private void updateGraphs()
    {
        int[] healthCounts = countHealthOfAgents();

        if(timeCount != Clock.dayVal && timeCount + 1 <= TimeSeries.MAX_POINTS)
        {
            Debug.Log("updated graphs");
            timeCount = timeCount + 1;
            graph.addPointToTimeSeries("Susceptibles", new Point(timeCount, healthCounts[0]));
            graph.addPointToTimeSeries("Expuestos", new Point(timeCount, healthCounts[1]));
            graph.addPointToTimeSeries("Infectados", new Point(timeCount, healthCounts[2]));
            graph.addPointToTimeSeries("Recuperados", new Point(timeCount, healthCounts[3]));
            graph.addPointToTimeSeries("Fallecidos", new Point(timeCount, healthCounts[4]));
        }
    }

    private void updateAgentsInteractions()
    {

        float[] newProbs = Clock.recalibrateProbabilities(alpha, beta, beta1, gamma, gamma1, omega, omega1, omega2);
        //Debug.Log(newProbs[0] +" "+newProbs[1] +" "+newProbs[2]);
        for (int i = 0; i < nAgents; i++)
        {
            if (agents[i].GetComponent<HealthStatus>().isAlive())
            {
                ProtectionStatus exposedProtection = agents[i].GetComponent<ProtectionStatus>();
                //Si el paciente está moviendose vamos a usar un criterio de distancia
                if (agents[i].GetComponent<TravelStatus>().isOnTheMove() && agents[i].GetComponent<HealthStatus>().getHealth() == HealthState.SUSCEPTIBLE)
                {
                    for (int j = 0; j < nAgents; j++)
                    {
                        if (j != i)
                        {
                            
                            if (agents[j].GetComponent<HealthStatus>().isContagious() && agents[j].GetComponent<TravelStatus>().isOnTheMove())
                            {

                                //Actualizamos el estado de salud
                                if (Vector2.Distance(agents[i].transform.position, agents[j].transform.position) < sickRadius - exposedProtection.getMovingDistancing())
                                {
                                    ProtectionStatus infectedProtection = agents[j].GetComponent<ProtectionStatus>();
                                    float protectionMask = policyMaker.getMaskProtectionFactor(exposedProtection, infectedProtection);
                                    float infectionVaccineProtection = policyMaker.getVaccinationContagionProtectionFactor(exposedProtection);
                                    float criticalCaseVaccineProtection = policyMaker.getVaccinationCriticalProtectionFactor(exposedProtection);


                                    //Debug.Log(protectionMask);
                                    //Debug.Log(infectionVaccineProtection);
                                    //Debug.Log(criticalCaseVaccineProtection);
                                    agents[i].GetComponent<HealthStatus>().updateHealthState(true, newProbs[0]*protectionMask*infectionVaccineProtection, newProbs[1], newProbs[2]  + criticalCaseVaccineProtection*(1 - newProbs[2]), newProbs[3], newProbs[4], newProbs[5], newProbs[6], newProbs[7], nInfected);

                                    break;
                                }
                                
                            }
                        }
                    }
                }
                else if(agents[i].GetComponent<TravelStatus>().isOnTheMove())
                {
                    float criticalCaseVaccineProtection = policyMaker.getVaccinationCriticalProtectionFactor(exposedProtection);
                    agents[i].GetComponent<HealthStatus>().updateHealthState(false, newProbs[0], newProbs[1], newProbs[2] + criticalCaseVaccineProtection*(1 - newProbs[2]), newProbs[3], newProbs[4], newProbs[5], newProbs[6], newProbs[7],nInfected);
                }
                //Si el agente se encuentra en un lugar vamos a usar las dimensiones del lugar y los enfermos dentro
                else
                {
                    Place place = agents[i].GetComponent<ActivityStatus>().getPlace();
                    //Esto es totalmente inventado pero veremos que tal va
                    if (place != null && place.hasSickPeople())
                    {
                        //Debug.Log("Número de gente dentro " + place.getPeopleInside() + " Gente enferma " + place.getSickPeopleInside());
                        ProtectionStatus infectedStatus = place.getRandomSickPeopleProtectionStatus();
                        float maskProtection = policyMaker.getMaskProtectionFactor(exposedProtection,infectedStatus);
                        float infectionVaccineProtection = policyMaker.getVaccinationContagionProtectionFactor(exposedProtection);
                        float criticalCaseVaccineProtection = policyMaker.getVaccinationCriticalProtectionFactor(exposedProtection);

                        agents[i].GetComponent<HealthStatus>().updateHealthState(true, newProbs[0]*maskProtection*place.getDiseaseInteractionProbability()*infectionVaccineProtection*exposedProtection.getInPlacesDistancingFactor(), newProbs[1], newProbs[2] + criticalCaseVaccineProtection*(1 - newProbs[2]), newProbs[3], newProbs[4], newProbs[5], newProbs[6], newProbs[7],nInfected);

                    }
                    else
                    {
                        float criticalCaseVaccineProtection = policyMaker.getVaccinationCriticalProtectionFactor(exposedProtection);
                        agents[i].GetComponent<HealthStatus>().updateHealthState(false, newProbs[0], newProbs[1], newProbs[2] + criticalCaseVaccineProtection*(1 - newProbs[2]), newProbs[3], newProbs[4], newProbs[5], newProbs[6], newProbs[7],nInfected);
                    }
                }

            }
        }
    }

    private int[] countHealthOfAgents()
    {
        int[] healthCounts = new int[5];
        for (int i = 0; i < agents.Count; i++)
        {
            HealthState healthState = agents[i].GetComponent<HealthStatus>().getHealth();
            switch (healthState)
            {
                case HealthState.SUSCEPTIBLE:
                    healthCounts[0] += 1;
                    break;

                case HealthState.EXPOSED:
                    healthCounts[1] += 1;
                    break;

                case HealthState.ASYMPTOMATIC_INFECTED:
                    healthCounts[2] += 1;
                    break;

                case HealthState.SYMPTOMATICALLY_INFECTED:
                    healthCounts[2] += 1;
                    break;

                case HealthState.RECOVERED:
                    healthCounts[3] += 1;
                    break;

                case HealthState.DEAD:
                    healthCounts[4] += 1;
                    break;
            }
        }
        nInfected = (float)healthCounts[0]/(float)nAgents;
        return healthCounts;
    }


    public void updateAgentsInteractionsForR0Calibration()
    {
        float[] newProbs = Clock.recalibrateProbabilities(alpha, beta, beta1, gamma, gamma1, omega, omega1, omega2);
        //Debug.Log(newProbs[0] +" "+newProbs[1] +" "+newProbs[2]);
        for (int i = 0; i < nAgents; i++)
        {
            if (agents[i].GetComponent<HealthStatus>().isAlive())
            {
                //Si el paciente está moviendose vamos a usar un criterio de distancia
                if (agents[i].GetComponent<TravelStatus>().isOnTheMove() && agents[i].GetComponent<HealthStatus>().getHealth() == HealthState.SUSCEPTIBLE)
                {
                    for (int j = 0; j < nAgents; j++)
                    {
                        if (j != i)
                        {
                            if (agents[j].GetComponent<HealthStatus>().isContagious() && agents[j].GetComponent<TravelStatus>().isOnTheMove())
                            {
                                //Actualizamos el estado de salud
                                if (Vector2.Distance(agents[i].transform.position, agents[j].transform.position) < sickRadius)
                                {
                                    agents[i].GetComponent<HealthStatus>().updateHealthState(true, newProbs[0], newProbs[1], newProbs[2], newProbs[3], newProbs[4], newProbs[5], newProbs[6], newProbs[7],nInfected);
                                    //Si el agente i se contagio, aumentamos el numero de gente que ha contagiado ael agente j
                                    if(agents[i].GetComponent<HealthStatus>().getHealth() != HealthState.SUSCEPTIBLE)
                                    {
                                        agents[j].GetComponent<HealthStatus>().increaseNumberOfAgentsInfected();
                                        Debug.Log("Infectado en la calle");
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (agents[i].GetComponent<TravelStatus>().isOnTheMove())
                {
                    agents[i].GetComponent<HealthStatus>().updateHealthState(false, newProbs[0], newProbs[1], newProbs[2], newProbs[3], newProbs[4], newProbs[5], newProbs[6], newProbs[7],nInfected);
                }
                //Si el agente se encuentra en un lugar vamos a usar las dimensiones del lugar y los enfermos dentro
                else
                {
                    Place place = agents[i].GetComponent<ActivityStatus>().getPlace();
                    //Esto es totalmente inventado pero veremos que tal va
                    if (place != null && place.hasSickPeople())
                    {
                        //Debug.Log("Número de gente dentro " + place.getPeopleInside() + " Gente enferma " + place.getSickPeopleInside());
                        HealthState prevHealthState = agents[i].GetComponent<HealthStatus>().getHealth();
                        agents[i].GetComponent<HealthStatus>().updateHealthState(true, newProbs[0] * place.getDiseaseInteractionProbability(), newProbs[1], newProbs[2], newProbs[3], newProbs[4], newProbs[5], newProbs[6], newProbs[7],nInfected);
                        if(prevHealthState == HealthState.SUSCEPTIBLE && agents[i].GetComponent<HealthStatus>().getHealth() != HealthState.SUSCEPTIBLE)
                        {
                            place.incrementRandomSickPersonTransmissionCount();
                            Debug.Log("Infectado en un lugar");
                        }
                    }
                    else
                    {

                        agents[i].GetComponent<HealthStatus>().updateHealthState(false, newProbs[0], newProbs[1], newProbs[2], newProbs[3], newProbs[4], newProbs[5], newProbs[6], newProbs[7],nInfected);

                    }
                }
            }
        }
    }

    public void exportReproductiveNumber()
    {
        float reproductiveNumber = 0;
        float nTransmitters = 0;
        for(int i = 0; i < agents.Count; i++)
        {
            HealthState healthState = agents[i].GetComponent<HealthStatus>().getHealth();
            if(healthState == HealthState.RECOVERED || healthState == HealthState.DEAD)
            {
                nTransmitters += 1;
                reproductiveNumber += agents[i].GetComponent<HealthStatus>().getNumberOfOtherAgentsInfected();
            }
        }

        reproductiveNumber = reproductiveNumber / nTransmitters;
        File.WriteAllText(Application.dataPath + "/CSV_DATA/" + "R0.csv", "Reproductive Number R0: "+reproductiveNumber);
        Debug.Log("Exported reproductive number " + reproductiveNumber);
    }


    private void updateStoplight()
    {
        if (!policyMaker.isStoplightOn())
        {
            //Obtenemos la ocupación hospitalaria
            float hospitalOccupation = this.city.countPeopleInPlaces(BuildingType.HOSPITAL) / this.nAgents;
            //Debug.Log("Hospital occupation " + hospitalOccupation);
            if (hospitalOccupation > StoplightControl.OCCUPATION_THRESHOLD && !policyMaker.isAllProtectionPolicy())
            {
                Debug.Log("Stoplight turned on isolation");
                Debug.Log("Ocupación hospitalaria " + hospitalOccupation);
                policyMaker.isolateAgents(agents);
            }

            else if(policyMaker.isAllProtectionPolicy() && hospitalOccupation > StoplightControl.OCCUPATION_THRESHOLD)
            {
                Debug.Log("Stoplight turned on ");
                Debug.Log("Ocupación hospitalaria " + hospitalOccupation);
                policyMaker.protectAgents(agents);
            }
        }
        if (policyMaker.endedIsolation())
        {
            if (!policyMaker.isAllProtectionPolicy())
            {
                policyMaker.freeAgents(agents);
                Debug.Log("Stoplight turned off isolation");
            }
            else
            {
                policyMaker.unProtectAgents(agents);
                Debug.Log("Stopligh turned off");
            }
        }
    }


    public float getSickRadius()
    {
        return sickRadius;
    }
}
