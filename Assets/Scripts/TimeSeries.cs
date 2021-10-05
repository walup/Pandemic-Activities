using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSeries
{
    private List<Point> points;
    private string color;
    private float lineWidth;
    private string timeSeriesName;
    private Color seriesColor;
    //Máximo número de puntos, para no saturar la memoria
    public static float MAX_POINTS = 500;

    public TimeSeries(string timeSeriesName)
    {
        this.timeSeriesName = timeSeriesName;
        //Initicializamos la lista de puntos
        this.points = new List<Point>();

    }

    public void setColor(string colorHex)
    {
        //Lo primero que vamos a hacer aquí es obtener el rojo verde y azul
        colorHex = colorHex.TrimStart('#');
        string redHex = colorHex.Substring(0, 2);
        string greenHex = colorHex.Substring(2, 2);
        string blueHex = colorHex.Substring(4, 2);

        float r = ((float)Convert.ToInt32((byte)Convert.ToSByte(redHex, 16)))/255.0f;
        float g = ((float)Convert.ToInt32((byte)Convert.ToSByte(greenHex, 16)))/255.0f;
        float b = ((float)Convert.ToInt32((byte)Convert.ToSByte(blueHex, 16)))/255.0f;

        //Establecemos el color de la serie de tiempo
        this.seriesColor = new Color(r,g,b);
    }

    public void resetTimeSeries()
    {
        this.points = new List<Point>();
    }

    public void addPoint(Point point)
    {
        if (this.points.Count < MAX_POINTS)
        {
            this.points.Add(point);
        }
    }

    public void setTimeSeries(List<Point> points)
    {
        this.points = points;
    }

    public void setLineWidth(float lineWidth)
    {
        this.lineWidth = lineWidth;
    }

    public float getLineWidth()
    {
        return lineWidth;
    }

    public Color getColor()
    {
        return seriesColor;
    }


    public List<Point> getPoints()
    {
        return this.points;
    }

    public string getName()
    {
        return this.timeSeriesName;
    }

    public bool isFull()
    {
        return this.points.Count >= MAX_POINTS;
    }


}
