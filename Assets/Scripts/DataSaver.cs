using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO;
public class DataSaver : MonoBehaviour
{
    // Start is called before the first frame update

    private bool saveActivated;
    private StringBuilder builder;
    //Frecuencia de muestreo en horas virtuales
    private float samplingFrequency;
    private float deltaTime;
    private float dataSaveClock;
    private string pathName;

    // Update is called once per frame
    void Update()
    {
        if (saveActivated)
        {
            dataSaveClock += Clock.hourDelta;
            if(dataSaveClock > deltaTime)
            {
                dataSaveClock = 0;
                builder.AppendLine(getFormattedPoint(transform.position.x, transform.position.y, (int)GetComponent<HealthStatus>().getHealth()));
            }
        }
    }

    void OnApplicationQuit()
    {
        File.WriteAllText(this.pathName, builder.ToString());
        Debug.Log("Written files ");
    }

    public void activateSaving()
    {
        this.saveActivated = true;
    }

    public void turnSavingOff()
    {
        this.saveActivated = false;
    }

    public void setFrequency(float frequency)
    {
        this.samplingFrequency = frequency;
        this.deltaTime = 1 / frequency;

        this.builder = new StringBuilder();
        this.builder.AppendLine("Frecuencia de muestreo (1/h): " + this.samplingFrequency.ToString());
    }



    public void setFileName(string name)
    {
        this.pathName = Application.dataPath + "/CSV_DATA/" + name;
       
    }



    private string getFormattedPoint(float x, float y,float z)
    {
        string newLine = string.Format("{0},{1},{2}", x, y, z);
        return newLine;

    }
}
