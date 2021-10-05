using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point
{

    private float x;
    private float y;


    public Point(float x, float y)
    {
        this.x = x;
        this.y = y;
    }


    public float getX()
    {
        return x;
    }

    public float getY()
    {
        return y;
    }
}
