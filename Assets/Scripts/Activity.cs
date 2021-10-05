using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Activity
{
    //El nombre de la actividad
    public string name;
    public float duration;
    //Una lista de lugares donde se realiza
    public List<BuildingType> doPlaces;
    //La probabilidad de realizarla
    public float probability;
    public float probabilityDelta;
    public float minProbabilityDelta;
    public float maxProbabilityDelta;
    //Cada cuanto debe realizarse si o si
    public float hourPeriodicity;
    public int[] timeConstraints;
    public int noiseCounter;

    public void setProbabilityDelta(float probDelta, float maxProbabilityDelta)
    {
        this.probabilityDelta = probDelta;
        this.maxProbabilityDelta = maxProbabilityDelta;
        this.minProbabilityDelta = 2 * (this.probabilityDelta - this.maxProbabilityDelta / 2);

        if(this.minProbabilityDelta < 0)
        {
            this.minProbabilityDelta = 0;
        }

    }

    //Actualización de la probabilidad
    public void updateProbability()
    {
        probability += probabilityDelta * Clock.hourDelta;
        if(probability >= 1)
        {
            probability = 1;
        }

        if(probability <= 0)
        {
            this.probability = 0;
        }
    }

    public string getName()
    {
        return name;
    }

    public List<BuildingType> getDoPlaces()
    {
        return doPlaces;
    }

    public float getProbability()
    {
        return probability;
    }

    public bool isViable()
    {
        int hour = Clock.hour;
        if (timeConstraints[0] > timeConstraints[1])
        {
            return hour >= timeConstraints[0] || (hour >= 0 && hour <= timeConstraints[1]);
        }
        else
        {
            return timeConstraints[1] >= hour && timeConstraints[0] <= hour;
        }
    }


    public bool finishedActivity(int startHour)
    {
        return !isViable() || Clock.getHourDistance(startHour, Clock.hour) >= duration;
    }

    //Vamos a suponer que el valor de ruido que nos proporcionan está entre 0 y 1
    public void updateProbabilityDeltaWithNoise(float noiseVal)
    {
        
        if(noiseVal > 1)
        {
            noiseVal = 1;
        }

        if(noiseVal < 0)
        {
            noiseVal = 0;
        }
        //Debug.Log("Previous delta of " +name+ " "+probabilityDelta); 
        this.probabilityDelta = (this.maxProbabilityDelta - this.minProbabilityDelta)*noiseVal + this.minProbabilityDelta;
        //Debug.Log("New delta of " +name+" "+ probabilityDelta);
        this.noiseCounter += 1;
    }


}
