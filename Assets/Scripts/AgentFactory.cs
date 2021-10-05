using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class AgentFactory : MonoBehaviour 
{
    private string ACTIVITIES_A_PATH; 
    private string ACTIVITIES_B_PATH; 
    private string ACTIVITIES_S_PATH; 
    private  ToroidalWorld world;
    [SerializeField] private  GameObject agentPrefab;
    [SerializeField] private GameObject saverPrefab;
    private string baseName = "data_";
    private int nameCounter = 0;
    private float samplingFrequency = 1;

    void Awake()
    {
        ACTIVITIES_A_PATH = Application.dataPath + "/ActivityJsons/activities_a.json";
        ACTIVITIES_B_PATH = Application.dataPath + "/ActivityJsons/activities_b.json";
        ACTIVITIES_S_PATH = Application.dataPath + "/ActivityJsons/activities_s.json";
        world = GameObject.Find("Main Camera").GetComponent<ToroidalWorld>();
    }

    public GameObject getAgent(AgentType type)
    {
        switch (type)
        {
            case AgentType.TYPE_A:
                string activitiesString = File.ReadAllText(ACTIVITIES_A_PATH, Encoding.UTF8);
                ActivityAgenda agenda = JsonUtility.FromJson<ActivityAgenda>(activitiesString);
                //agenda.randomizeActivities();
                string sickActivitiesString = File.ReadAllText(ACTIVITIES_S_PATH, Encoding.UTF8);
                ActivityAgenda sickAgenda = JsonUtility.FromJson<ActivityAgenda>(sickActivitiesString);
                float randPositionX = Random.Range(world.getAnchorPoint().x, world.getAnchorPoint().x +world.getWorldWidth());
                float randPositionY = Random.Range(world.getAnchorPoint().y, world.getAnchorPoint().y + world.getWorldHeight());
                GameObject agentA = GameObject.Instantiate(agentPrefab, new Vector2(randPositionX, randPositionY), Quaternion.Euler(0,0,0));
                ActivityStatus activityComponent = agentA.GetComponent<ActivityStatus>();

                activityComponent.setAgentType(AgentType.TYPE_A);
                activityComponent.setActivities(agenda.activities);
                activityComponent.setSickActivities(sickAgenda.activities);

                return agentA;

            case AgentType.TYPE_B:

                string activitiesBString = File.ReadAllText(ACTIVITIES_B_PATH, Encoding.UTF8);
                ActivityAgenda agendaB = JsonUtility.FromJson<ActivityAgenda>(activitiesBString);
                string sickActivitiesStringB = File.ReadAllText(ACTIVITIES_S_PATH, Encoding.UTF8);
                ActivityAgenda sickAgendaB = JsonUtility.FromJson<ActivityAgenda>(sickActivitiesStringB);
                float randPositionXB = Random.Range(world.getAnchorPoint().x, world.getAnchorPoint().x + world.getWorldWidth());
                float randPositionYB = Random.Range(world.getAnchorPoint().y, world.getAnchorPoint().y + world.getWorldHeight());
                GameObject agentB = GameObject.Instantiate(agentPrefab, new Vector2(randPositionXB, randPositionYB), Quaternion.Euler(0, 0, 0));
                ActivityStatus activityComponentB = agentB.GetComponent<ActivityStatus>();
                activityComponentB.setAgentType(AgentType.TYPE_B);
                activityComponentB.setActivities(agendaB.activities);
                activityComponentB.setSickActivities(sickAgendaB.activities);

                return agentB;
            case AgentType.DATA_SAVER:
                string activitiesSString = File.ReadAllText(ACTIVITIES_A_PATH, Encoding.UTF8);
                ActivityAgenda agendaSaver = JsonUtility.FromJson<ActivityAgenda>(activitiesSString);
                string sickActivitiesSString = File.ReadAllText(ACTIVITIES_S_PATH, Encoding.UTF8);
                ActivityAgenda sickAgendaSaver = JsonUtility.FromJson<ActivityAgenda>(sickActivitiesSString);
                float randPositionXSaver = Random.Range(world.getAnchorPoint().x, world.getAnchorPoint().x + world.getWorldWidth());
                float randPositionYSaver = Random.Range(world.getAnchorPoint().y, world.getAnchorPoint().y + world.getWorldHeight());
                GameObject agentSaver = GameObject.Instantiate(saverPrefab, new Vector2(randPositionXSaver, randPositionYSaver), Quaternion.Euler(0, 0, 0));
                ActivityStatus activityComponentSaver = agentSaver.GetComponent<ActivityStatus>();

                activityComponentSaver.setAgentType(AgentType.TYPE_A);
                activityComponentSaver.setActivities(agendaSaver.activities);
                activityComponentSaver.setSickActivities(sickAgendaSaver.activities);

                DataSaver saverComponent = agentSaver.GetComponent<DataSaver>();
                saverComponent.setFileName(baseName + nameCounter+".csv");
                nameCounter++;
                saverComponent.setFrequency(samplingFrequency);
                saverComponent.activateSaving();

                return agentSaver;
        }
        return null;
    }
}
