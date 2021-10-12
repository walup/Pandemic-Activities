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
    //La distribución de probabilidad de los distintos lugares
    public float[] distributionPlaces;

    private int nStandardDeviationsForMaxValue = 3;
    //Distribución de comportamiento de las personas (aquí se incorporará la información de la encuesta de actividades realizada)
    public float[] frequencyValues;
    public float[] frequencyDistribution;
    public float frequencyBinSize;
    public TimeRate frequencyTimeRate;


    //Distribución de tiempos que invierten las personas realizando las actividades 
    public float[] durationValues;
    public float[] durationDistribution;
    public float durationBinSize;

    //La probabilidad de realizarla
    public float probability;
    public float probabilityDelta;
    public float minProbabilityDelta;
    public float maxProbabilityDelta;

    //Cada cuanto debe realizarse si o si
    public float hourPeriodicity;
    public int[] timeConstraints;
    public int noiseCounter;


    //En este método se van a inicializar 4 cosas acorde a las distribuciones de probabilidad:
    //Delta de probabilidad de la actividad acorde a la frecuencia
    //Máximo de delta de probabilidad ubicado a 2 desviaciones estándar
    //Duración acorde a la distribución de duración de las actividades
    //La probabilidad inicial (esta considerando que la simulación inicia a las 8 de la mañana)

    public void initializeActivity()
    {

        StatisticCalculator statisticCalculator = new StatisticCalculator();


        int nFreq = frequencyDistribution.Length;
        float[] cumSumFrequency = new float[nFreq + 1];
        cumSumFrequency[0] = 0;
        //Obtenemos la suma acumulada de la distribución de frecuencias
        for (int i = 1; i < cumSumFrequency.Length; i++)
        {
            cumSumFrequency[i] = cumSumFrequency[i - 1] + frequencyDistribution[i - 1];
        }

        int nDuration = durationDistribution.Length;
        float[] cumSumDuration = new float[nDuration + 1];
        cumSumDuration[0] = 0;
        //Obtenemos la suma acumulada de la distribución de duraciones
        for (int i = 1; i < cumSumDuration.Length; i++)
        {
            cumSumDuration[i] = cumSumDuration[i - 1] + durationDistribution[i - 1];
        }


        //Ahora vamos con la duración de la actividad
        float diceThrow = UnityEngine.Random.value;
        float durationValue = 0;

        for (int i = 1; i < cumSumDuration.Length; i++)
        {
            if (diceThrow <= cumSumDuration[i] && diceThrow >= cumSumDuration[i - 1])
            {
                float minValue = durationValues[i - 1] - durationBinSize / 2;
                float maxValue = durationValues[i - 1] + durationBinSize / 2;

                durationValue = minValue + (maxValue - minValue) * UnityEngine.Random.value;
            }
        }
        duration = durationValue;

        //Ahora vamos a obtener un valor de frecuencia acorde a la distribución 
        diceThrow = UnityEngine.Random.value;
        float valueFrequency = 0;
        for (int i = 1; i < cumSumFrequency.Length; i++)
        {
            if(diceThrow >= cumSumFrequency[i-1] && diceThrow <= cumSumFrequency[i])
            {
                float minVal = frequencyValues[i - 1] - frequencyBinSize / 2;
                float maxVal = frequencyValues[i - 1] + frequencyBinSize / 2;

                valueFrequency = minVal + (maxVal - minVal) * UnityEngine.Random.value;
                break;
            }
        }

        //Con este valor de frecuencia vamos a asignar el delta de probabilidad
        float stdDev = 0;
        float mean = 0;
        float timeRateFactor = 0;
        switch (frequencyTimeRate)
        {
            case TimeRate.TIMES_PER_DAY:
                timeRateFactor = 1 / 24.0f;
                valueFrequency = valueFrequency*timeRateFactor;
                stdDev = timeRateFactor* statisticCalculator.getStandardDeviation(frequencyValues, frequencyDistribution);
                mean = timeRateFactor* statisticCalculator.getMeanDistribution(frequencyValues, frequencyDistribution);
                setProbabilityDelta(valueFrequency, mean + nStandardDeviationsForMaxValue*stdDev);
                break;
            case TimeRate.TIMES_PER_WEEK:
                timeRateFactor = 1 / (168.0f);
                valueFrequency = valueFrequency*timeRateFactor;
                stdDev = timeRateFactor* statisticCalculator.getStandardDeviation(frequencyValues, frequencyDistribution);
                mean = timeRateFactor* statisticCalculator.getMeanDistribution(frequencyValues, frequencyDistribution);
                setProbabilityDelta(valueFrequency, mean + nStandardDeviationsForMaxValue * stdDev);

                break;
            case TimeRate.TIMES_PER_MONTH:
                timeRateFactor = 1 / (720.0f);
                valueFrequency = valueFrequency*timeRateFactor;
                stdDev = timeRateFactor* statisticCalculator.getStandardDeviation(frequencyValues, frequencyDistribution);
                mean = timeRateFactor * statisticCalculator.getMeanDistribution(frequencyValues, frequencyDistribution);
                setProbabilityDelta(valueFrequency, mean + nStandardDeviationsForMaxValue * stdDev);
                break;
            default:
                break;
        }
    }


    //Regresa un tipo de edificio acorde a la distribución dada
    public BuildingType getBuildingTypeAccordingToDistribution()
    {
        float diceThrow = UnityEngine.Random.value;
        int nPlaceTypes = doPlaces.Count;

        float[] cumSumPlaces = new float[nPlaceTypes + 1];
        cumSumPlaces[0] = 0;
        for(int i = 1; i < cumSumPlaces.Length; i++)
        {
            cumSumPlaces[i] = cumSumPlaces[i] + distributionPlaces[i - 1];

            if(diceThrow >= cumSumPlaces[i-1] && diceThrow <= cumSumPlaces[i])
            {
                return doPlaces[i - 1];
            }
        }
        return doPlaces[0];
    }


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

    //




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
