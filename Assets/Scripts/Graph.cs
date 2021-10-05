using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class Graph : MonoBehaviour
{
    //El máximo número de series de tiempo que vamos a admitir

    private float MAX_TIME_SERIES = 5;
    private Hashtable timeSeriesDictionary;
    //We only want to redraw if something actually changed
    private bool blitState = false;
    private RectTransform graphCanvas;
    private float maxX;
    private float maxY;
    private float graphHeight;
    private float graphWidth;
    private float minX;
    private float minY;

    // Start is called before the first frame update
    void Start()
    {
        graphCanvas = transform.Find("GraphArea").GetComponent<RectTransform>();
        graphWidth = Mathf.Abs(graphCanvas.rect.width);
        graphHeight = Mathf.Abs(graphCanvas.rect.height);

        maxX = float.MinValue;
        maxY = float.MinValue;
        minX = float.MaxValue;
        minY = float.MaxValue;


        timeSeriesDictionary = new Hashtable();

        //Debuggear
        float[] xValues = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        float[] yValues = { 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2 };

        TimeSeries debugSeries = new TimeSeries("Debug");

        for (int i = 0; i < xValues.Length; i++)
        {
            Point point = new Point(xValues[i],yValues[i]);
            debugSeries.addPoint(point);
        }
        debugSeries.setColor("#ffee00");
        debugSeries.setLineWidth(3);

        this.addTimeSeries(debugSeries);
        this.blitState = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (this.blitState)
        {
            this.clear();

            float ay = ((this.graphHeight) / (this.maxY - this.minY));
            float by = -ay * this.minY;
            float ax = ((this.graphWidth) / (this.maxX - this.minX));
            float bx = -ax * this.minX;
            foreach(DictionaryEntry entry in timeSeriesDictionary)
            {
                TimeSeries ts = (TimeSeries)entry.Value;
                List<Point> points = ts.getPoints();
                int nPoints = ts.getPoints().Count;
                //Vamos a calcular el intervalo 


                for(int i = 1; i<ts.getPoints().Count; i++)
                {
                    float xPos =  ax*points[i].getX() + bx;
                    float yPos = ay * points[i].getY() + by;
                    float prevXPos = ax*points[i-1].getX() +bx;
                    float prevYPos = ay*points[i-1].getY() + by;
                    this.drawLineBetweenPoints(new Vector2(prevXPos, prevYPos), new Vector2(xPos, yPos), ts.getColor(), ts.getLineWidth());
                }
            } 
            blitState = false;
        }
    }



    public void drawLineBetweenPoints(Vector2 point1, Vector2 point2, Color lineColor, float lineWidth)
    {

        float distance = Vector2.Distance(point1, point2);
        GameObject gameObject = new GameObject("connection", typeof(Image));
        gameObject.transform.SetParent(graphCanvas);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        //Establecemos el ancho de la linea
        rectTransform.sizeDelta = new Vector2(distance, lineWidth);
        //Establecemos el color de la linea
        gameObject.GetComponent<Image>().color = lineColor;
        //Obtenemos la dirección del vector
        Vector2 dir = (point2 - point1).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //Esto es para que rotemos 
        rectTransform.anchoredPosition = point1 + dir * distance * 0.5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, angle);


    }

    public void addTimeSeries(TimeSeries series) { 
        if(timeSeriesDictionary.Count < MAX_TIME_SERIES)
        {
            timeSeriesDictionary.Add(series.getName(), series);
            List<Point> points = series.getPoints();
            for (int i = 0; i<points.Count; i++)
            {
                if(points[i].getX() > this.maxX)
                {
                    this.maxX = points[i].getX();
                }

                if(points[i].getY() > this.maxY)
                {
                    this.maxY = points[i].getY();
                }

                if (points[i].getX() < this.minX)
                {
                    this.minX = points[i].getX();
                }

                if(points[i].getY() < this.minY)
                {
                    this.minY = points[i].getY();
                }
            }
        }
    }

    public void addPointToTimeSeries(string key, Point point)
    {
        if(!((TimeSeries)this.timeSeriesDictionary[key]).isFull())
        {
            ((TimeSeries)this.timeSeriesDictionary[key]).addPoint(point);
            if(point.getX() > this.maxX)
            {
                this.maxX = point.getX();
            }

            if (point.getY() > this.maxY)
            {
                this.maxY = point.getY();
            }

            if (point.getX() < this.minX)
            {
                this.minX = point.getX();
            }

            if (point.getY() < this.minY)
            {
                this.minY = point.getY();
            }
            this.blitState = true;
        }
    }

    public void clear()
    {
        //Destruimos todas las conexiones que se hayan creado.
        for(int i = 0; i <graphCanvas.childCount; i++)
        {
            Destroy(graphCanvas.GetChild(i).gameObject);
        }
    }
    

    public void clearAllTimeSeries()
    {
        //Destruimos todas las conexiones que se hayan creado.
        Debug.Log(graphCanvas.childCount);
        for (int i = 0; i < graphCanvas.childCount; i++)
        {
            Destroy(graphCanvas.GetChild(i).gameObject);
            Debug.Log("Destroyed");
        }

        timeSeriesDictionary.Clear();
    }

    public void exportGraph()
    {
        //Recolectamos todas las llaves 
        List <string> keys = new List<string>();
        foreach(DictionaryEntry s in timeSeriesDictionary){
            keys.Add((string)s.Key);
        }

        StringBuilder builder = new StringBuilder();
        float dataCount = Mathf.Infinity;
        string headers = "";
        for(int i = 0; i < keys.Count; i++)
        {
            List<Point> points = ((TimeSeries)(timeSeriesDictionary[keys[i]])).getPoints() ;
            //El número de puntos que almacenaremos es el de la serie que menos puntos tenga
            if (points.Count < dataCount)
            {
                dataCount = points.Count;
            }

            if(i < keys.Count - 1)
            {
                headers += keys[i] + ",";
            }
            else if(i == keys.Count - 1)
            {
                headers += keys[i];
            }

        }

        builder.AppendLine(headers);

        for (int i = 0; i < dataCount; i++)
        {
            string row = "";
            for (int j = 0; j <keys.Count; j++)
            {
                Point point = ((TimeSeries)(timeSeriesDictionary[keys[j]])).getPoints()[i];
                if(j <keys.Count - 1)
                {
                    row += point.getY().ToString() + ",";
                }
                else if(j == keys.Count - 1)
                {
                    row += point.getY().ToString(); 
                }
            }
            builder.AppendLine(row);
        }

        File.WriteAllText(Application.dataPath + "/CSV_DATA/" + "grafica.csv", builder.ToString());
        Debug.Log("Graph exported");



    }
    

}
