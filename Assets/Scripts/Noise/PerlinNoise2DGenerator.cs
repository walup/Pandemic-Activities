using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise2DGenerator
{


    private float persistance;
    private float octaves;
    private List<int> permutation;
    private float n;
    private float frequency;
    //Default vals that could work are
    //frequency = 0.01
    //octaves = 8
    //persistence = 0.6
    public PerlinNoise2DGenerator(float frequency, float persistance, float octaves)
    {
        this.persistance = persistance;
        this.octaves = octaves;
        this.n = 256;
        this.frequency = frequency;

        //Vamos a llenar nuestro arreglo de 0 a 255
        for (int i = 0; i < this.n; i++)
        {
            this.permutation.Add(i);
        }

        //Ahora asignamos posiciones aleatorias a los números 
        for(int i = 0; i < this.n; i++)
        {
            int temp = permutation[i];
            int nextIndex = (int)(Random.value * this.n);
            if(nextIndex == this.n)
            {
                nextIndex = (int)(this.n - 1);
            }
            this.permutation[i] = this.permutation[nextIndex];
            this.permutation[nextIndex] = temp;
        }
    }




    public float perlin(float x, float y)
    {
        int col = (int)(Mathf.Floor(x) % this.n);
        int row = (int)(Mathf.Floor(y) % this.n);

        float xRes = x - Mathf.Floor(x);
        float yRes = y - Mathf.Floor(y);

        Vector2 trToPoint = new Vector2(xRes - 1, yRes - 1);
        Vector2 tlToPoint = new Vector2(xRes, yRes - 1);
        Vector2 brToPoint = new Vector2(xRes - 1, yRes);
        Vector2 blToPoint = new Vector2(xRes, yRes);

        float valueTopRight = (float)this.permutation[(int)((this.permutation[(int)((col + 1)%this.n)] + row + 1)%this.n)];
        float valueTopLeft = (float)this.permutation[(int)((this.permutation[col] + row + 1)%this.n)];
        float valueBottomRight = (float)this.permutation[(int)((this.permutation[(int)((col + 1)%this.n)] + row)%this.n)];
        float valueBottomLeft = (float)this.permutation[(int)((this.permutation[col] + row)%this.n)];

        float dotTopRight = Vector2.Dot(trToPoint, this.getCornerVector(valueTopRight));
        float dotTopLeft = Vector2.Dot(tlToPoint, this.getCornerVector(valueTopLeft));
        float dotBottomRight = Vector2.Dot(brToPoint, this.getCornerVector(valueBottomRight));
        float dotBottomLeft = Vector2.Dot(blToPoint, this.getCornerVector(valueBottomLeft));

        float u = this.fade(xRes);
        float v = this.fade(yRes);

        float perlinNoise = this.linearInterpolation(u, linearInterpolation(v, dotBottomLeft, dotTopLeft), linearInterpolation(v, dotBottomRight, dotTopRight));
        return perlinNoise;
    }

    public float computePerlinNoise(float x, float y)
    {
        float perlinNoise = 0;
        float newXVal = x * this.frequency;
        float newYVal = y * this.frequency;

        for(int i = 0; i < this.octaves; i++)
        {
            perlinNoise = perlinNoise + Mathf.Pow(this.persistance, i) * this.perlin(Mathf.Pow(2, i) * newXVal, Mathf.Pow(2, i) * newYVal);
        }

        return perlinNoise;
    }


    public Vector2 getCornerVector(float val)
    {
        float h = val % 4;
        switch (h)
        {
            case 0:
                return new Vector2(1, 1);
            case 1:
                return new Vector2(-1, 1);
            case 2:
                return new Vector2(-1, -1);

            case 3:
                return new Vector2(1, -1);
            default:
                return new Vector2(1, -1);
        }

    }



    //Algunas funciones auxiliares que necesitamos
    public float fade(float t)
    {
        return 6 * Mathf.Pow(t, 5) - 15 * Mathf.Pow(t, 4) + 10 * Mathf.Pow(t, 3);
    }

    public float linearInterpolation(float t, float a1, float a2)
    {
        return a1 + t * (a2 - a1);
    }

}
