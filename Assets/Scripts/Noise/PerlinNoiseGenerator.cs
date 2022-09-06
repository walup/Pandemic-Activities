using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseGenerator : Noise
{

    private float n;
    private List<float> valuesArray;
    private float persistance;
    private float octaves;
    private float frequency;

    public PerlinNoiseGenerator(float frequency, float persistance, float octaves)
    {
        this.frequency = frequency;
        this.persistance = persistance;
        this.octaves = octaves;
        //512 me suele funcionar así que usare esto
        /*Tecnicamente creo que lo que estoy implementando no es perlin como tal
         sino value noise*/
        this.n = 512;
        valuesArray = new List<float>();
        for(int i = 0; i<this.n; i++)
        {
            valuesArray.Add(Random.value);
        }
    }


    private float linearInterpolation(float x)
    {
        x = x % this.n;
        int index = (int)Mathf.Floor(x);
        float alpha = x % 1;
        if(index != this.n - 1)
        {
            return (1 - alpha) * (this.valuesArray[index]) + alpha * this.valuesArray[index + 1];
        }
        else
        {
            return (1 - alpha) * this.valuesArray[index] + alpha * this.valuesArray[0];

        }

    }


    private float computePerlinNoise(float x)
    {
        float perlinNoise = 0;

        float newX = x * this.frequency;

        for (int i = 0; i < this.octaves; i++)
        {
            perlinNoise = perlinNoise + Mathf.Pow(this.persistance, i) * this.linearInterpolation(Mathf.Pow(2, i) * newX);
        }

        float maxValue = (1 - Mathf.Pow(this.persistance, this.octaves))/(1 - this.persistance);
        //Esto es para llevar el ruido a valores entre 0 y 1
        perlinNoise = perlinNoise / maxValue;

        return perlinNoise;
    }


    private float translatedPerlinNoise(float x, float a, float b)
    {
        return a + (b - a) * this.computePerlinNoise(x);
    }

    public float getNoiseValue(float val)
    {
        return computePerlinNoise(val);
    }
}
