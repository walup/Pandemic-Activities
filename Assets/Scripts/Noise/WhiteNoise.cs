using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteNoise : Noise
{
    public WhiteNoise()
    {

    }

    //Vamos a suponer que el ruido esta entre -2 y 2 desv, estandar en realidad no importa demasiado.
    public float getNoiseValue(float val)
    {
        return translateNoise(computeWhiteNoise(), -2, 2);
    }

    //Muller noise
    public float computeWhiteNoise()
    {
        float u0 = Random.value;
        float u1 = Random.value;

        float e = -2 * Mathf.Log(u0);
        float f= Mathf.Sqrt(e);
        float g0 = Mathf.Sin(2 * Mathf.PI * u1);
        float x0 = f * g0;

        return x0;
    }

    public float translateNoise(float val, float a, float b)
    {
        return (1 / (b - a)) * val -(a/(b - a));
    }
}
