using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class AgentFactory : MonoBehaviour 
{
    private string ACTIVITIES_H_PATH; 
    private string ACTIVITIES_S_PATH; 
    private  ToroidalWorld world;
    [SerializeField] private  GameObject agentPrefab;
    [SerializeField] private GameObject saverPrefab;
    private string baseName = "data_";
    private int nameCounter = 0;
    private float samplingFrequency = 1;

    void Awake()
    {
        //La ruta de las actividades para sanos
        ACTIVITIES_H_PATH = Application.dataPath + "/ActivityJsons/activities_h.json";
        //La ruta de las actividades para enfermos
        ACTIVITIES_S_PATH = Application.dataPath + "/ActivityJsons/activities_s.json";
        world = GameObject.Find("Main Camera").GetComponent<ToroidalWorld>();
    }

    public GameObject getAgent(AgentType type)
    {
        switch (type)
        {
            case AgentType.LIGHT_AGENT:
                string activitiesString = File.ReadAllText(ACTIVITIES_H_PATH, Encoding.UTF8);
                ActivityAgenda agenda = JsonUtility.FromJson<ActivityAgenda>(activitiesString);
                //Inicializamos las actividades
                agenda.initializeActivities();
                string sickActivitiesString = File.ReadAllText(ACTIVITIES_S_PATH, Encoding.UTF8);
                ActivityAgenda sickAgenda = JsonUtility.FromJson<ActivityAgenda>(sickActivitiesString);
                sickAgenda.initializeActivities();

                float randPositionX = Random.Range(world.getAnchorPoint().x, world.getAnchorPoint().x +world.getWorldWidth());
                float randPositionY = Random.Range(world.getAnchorPoint().y, world.getAnchorPoint().y + world.getWorldHeight());
                GameObject lightAgent= GameObject.Instantiate(agentPrefab, new Vector2(randPositionX, randPositionY), Quaternion.Euler(0,0,0));
                ActivityStatus activityComponent = lightAgent.GetComponent<ActivityStatus>();
                activityComponent.setAgentType(AgentType.LIGHT_AGENT);
                activityComponent.setActivities(agenda.activities);
                activityComponent.setSickActivities(sickAgenda.activities);

                return lightAgent;

            case AgentType.DATA_SAVER:
                string activitiesSString = File.ReadAllText(ACTIVITIES_H_PATH, Encoding.UTF8);
                ActivityAgenda agendaSaver = JsonUtility.FromJson<ActivityAgenda>(activitiesSString);
                string sickActivitiesSString = File.ReadAllText(ACTIVITIES_S_PATH, Encoding.UTF8);
                ActivityAgenda sickAgendaSaver = JsonUtility.FromJson<ActivityAgenda>(sickActivitiesSString);
                float randPositionXSaver = Random.Range(world.getAnchorPoint().x, world.getAnchorPoint().x + world.getWorldWidth());
                float randPositionYSaver = Random.Range(world.getAnchorPoint().y, world.getAnchorPoint().y + world.getWorldHeight());
                GameObject agentSaver = GameObject.Instantiate(saverPrefab, new Vector2(randPositionXSaver, randPositionYSaver), Quaternion.Euler(0, 0, 0));
                ActivityStatus activityComponentSaver = agentSaver.GetComponent<ActivityStatus>();

                activityComponentSaver.setAgentType(AgentType.DATA_SAVER);
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


    /*La idea con este método es exportar los valores de deltas de probabilidad etc. para asegurarnos que sigan las distribuciones
     del cuestionario, este lo implementaré despues*/
    public void debugExport()
    {

    }
    
}
