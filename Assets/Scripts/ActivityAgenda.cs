using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ActivityAgenda
{

    public List<Activity> activities;


    public void randomizeActivities()
    {
        for(int i = 0; i < activities.Count; i++)
        {
            if(activities[i].getName() == "Eat" || activities[i].getName() == "Socialize" || activities[i].getName() == "Exercise" || activities[i].getName() == "Buy Food")
            {
                activities[i].probability = UnityEngine.Random.Range(0, 1);
            }
            else if (activities[i].getName() == "Work")
            {
                activities[i].probability = UnityEngine.Random.Range(0.5f, 1);
            }
        }
    }
}


