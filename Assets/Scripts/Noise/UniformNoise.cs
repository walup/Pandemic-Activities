using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniformNoise : Noise
{
    public float getNoiseValue(float val)
    {
        return Random.value;
    }
}
