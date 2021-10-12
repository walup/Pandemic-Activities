using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticCalculator 
{

    public StatisticCalculator()
    {

    }


    public float getMeanDistribution(float[] values, float[] probabilityDistribution)
    {
        float mean = 0;
        int nPoints = values.Length;
        
        for(int i = 0; i < nPoints; i++)
        {
            mean = mean + values[i] * probabilityDistribution[i];
        }

        return mean;
    }

    public float getStandardDeviation(float[] values, float[] probabilityDistribution)
    {
        int nPoints = values.Length;
        float[] valuesSquared = new float[nPoints];

        for(int i = 0; i < nPoints; i++)
        {
            valuesSquared[i] = Mathf.Pow(values[i], 2);
        }

        float meanValues = this.getMeanDistribution(values, probabilityDistribution);
        float meanValuesSq = this.getMeanDistribution(valuesSquared, probabilityDistribution);

        float standardDeviation = Mathf.Sqrt(meanValuesSq - Mathf.Pow(meanValues, 2));

        return standardDeviation;
    }


}
