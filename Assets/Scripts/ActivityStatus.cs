using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityStatus : MonoBehaviour
{
    [SerializeField] private AgentType agentType;
    private List<Activity> activities;
    private List<Activity> sickActivities;
    [SerializeField] private Activity currentActivity;
    private int startActivityHour;
    private bool doingActivity;
    private HealthStatus healthStatus;
    private Place dummyPlace;

    //Noise
    private Noise noiseGenerator;
    
    // Start is called before the first frame update
    void Start()
    {

        healthStatus = GetComponent<HealthStatus>();
        doingActivity = false;

        //noiseGenerator = new PerlinNoiseGenerator(0.01f, 0.6f, 8);
        noiseGenerator = new WhiteNoise();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.GetComponent<HealthStatus>().isAlive())
        {
            if (!healthStatus.isSick())
            {
                if (!doingActivity)
                {
                    //Buscamos la actividad de probabilidad mas alta que podamos hacer 

                    activities.Sort((x, y) => x.probability.CompareTo(y.probability));

                    for (int i = activities.Count - 1; i >= 0; i--)
                    {

                        if (activities[i].isViable() && Random.value <= activities[i].probability)
                        {

                            if (dummyPlace != null && activities[i].doPlaces.Contains(dummyPlace.getType()))
                            {
                                startActivityHour = Clock.hour;
                                doingActivity = true;
                                currentActivity = activities[i];
                                currentActivity.probability = 0;
                                currentActivity.updateProbabilityDeltaWithNoise(noiseGenerator.getNoiseValue(currentActivity.noiseCounter));
                                GetComponent<TravelStatus>().moveAgent(dummyPlace);
                                Vector2 velocity = (dummyPlace.getPlacePosition() - (Vector2)transform.position).normalized * City.getAverageCitySpeed();
                                GetComponent<TravelStatus>().setVelocity(velocity);
                                break;
                            }
                            else
                            {
                         
                                for (int j = 0; j < activities[i].doPlaces.Count; j++)
                                {
                                    //Intentaremos encontrar un lugar disponible
                                    BuildingType type = activities[i].getBuildingTypeAccordingToDistribution();

                                    dummyPlace = City.requestAvailablePlace(type);
                                    if (dummyPlace != null)
                                    {
                                        startActivityHour = Clock.hour;
                                        doingActivity = true;
                                        currentActivity = activities[i];
                                        currentActivity.probability = 0;
                                        currentActivity.updateProbabilityDeltaWithNoise(noiseGenerator.getNoiseValue(currentActivity.noiseCounter));
                                        GetComponent<TravelStatus>().moveAgent(dummyPlace);
                                        //Ahora debemos establecer la velocidad
                                        Vector2 velocity = (dummyPlace.getPlacePosition() - (Vector2)transform.position).normalized * City.getAverageCitySpeed();
                                        GetComponent<TravelStatus>().setVelocity(velocity);
                                        break;
                                    }
                                }
                            }

                        }

                    }

                    //Actualizamos la probabilidad de las otras actividades
                    for (int i = 0; i < activities.Count; i++)
                    {
                        if (activities[i] != currentActivity)
                        {
                            activities[i].updateProbability();
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < activities.Count; i++)
                    {
                        if (activities[i] != currentActivity)
                        {
                            activities[i].updateProbability();
                        }
                    }

                    if (currentActivity.finishedActivity(startActivityHour))
                    {
                        currentActivity = null;
                        doingActivity = false;
                    }
                }

            }
            else
            {
                if (!doingActivity)
                {

                    //Buscamos la actividad de probabilidad mas alta que podamos hacer y 

                    sickActivities.Sort((x, y) => x.probability.CompareTo(y.probability));
                    for (int i = sickActivities.Count - 1; i >= 0; i--)
                    {
                        if (Random.value <= sickActivities[i].probability && sickActivities[i].isViable())
                        {

                            if (dummyPlace != null && sickActivities[i].doPlaces.Contains(dummyPlace.getType()))
                            {
                                startActivityHour = Clock.hour;
                                doingActivity = true;
                                currentActivity = sickActivities[i];
                                currentActivity.probability = 0;
                                currentActivity.updateProbabilityDeltaWithNoise(noiseGenerator.getNoiseValue(currentActivity.noiseCounter));
                                GetComponent<TravelStatus>().moveAgent(dummyPlace);
                                Vector2 velocity = (dummyPlace.getPlacePosition() - (Vector2)transform.position).normalized * City.getAverageCitySpeed();
                                GetComponent<TravelStatus>().setVelocity(velocity);
                                break;
                            }
                            else
                            {
                                for (int j = 0; j < sickActivities[i].doPlaces.Count; j++)
                                {
                                    BuildingType type = sickActivities[i].getBuildingTypeAccordingToDistribution();
                                    dummyPlace = City.requestAvailablePlace(type);

                                    if (dummyPlace != null)
                                    {
                                        startActivityHour = Clock.hour;
                                        doingActivity = true;
                                        currentActivity = sickActivities[i];
                                        currentActivity.probability = 0;
                                        currentActivity.updateProbabilityDeltaWithNoise(noiseGenerator.getNoiseValue(currentActivity.noiseCounter));
                                        GetComponent<TravelStatus>().moveAgent(dummyPlace);
                                        //Ahora establecemos la velocidad del agente
                                        Vector2 velocity = (dummyPlace.getPlacePosition() - (Vector2)transform.position).normalized * City.getAverageCitySpeed();
                                        GetComponent<TravelStatus>().setVelocity(velocity);
                                        break;
                                    }
                                }
                            }
                        }

                    }

                    //Actualizamos la probabilidad de las otras actividades
                    for (int i = 0; i < sickActivities.Count; i++)
                    {
                        if (sickActivities[i] != currentActivity)
                        {
                            sickActivities[i].updateProbability();
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < activities.Count; i++)
                    {
                        if (sickActivities[i] != currentActivity)
                        {
                            sickActivities[i].updateProbability();
                        }
                    }

                    if (currentActivity.finishedActivity(startActivityHour))
                    {
                        currentActivity = null;
                        doingActivity = false;
                    }
                }
            }
        }
    }

    
    public void setAgentType(AgentType type)
    {
        this.agentType = type;
    }

    public void setActivities(List<Activity> activities)
    {
        this.activities = activities;
    }

    public void setSickActivities(List<Activity> sickActivities)
    {
        this.sickActivities = sickActivities;
    }

    public Place getPlace()
    {
        return this.dummyPlace;
    }

    public bool isDoingActivity()
    {
        return doingActivity;
    }

    public void stopActivity()
    {
        currentActivity = null;
        doingActivity = false;
    }
}
