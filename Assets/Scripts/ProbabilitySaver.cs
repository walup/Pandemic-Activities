using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO;
public class ProbabilitySaver : MonoBehaviour
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
            if (dataSaveClock > deltaTime)
            {
                dataSaveClock = 0;
                ActivityStatus activityStatus = GetComponent<ActivityStatus>();
                Activity workActivity = activityStatus.getHealthyNamedActivity("Work");
                Activity exerciseActivity = activityStatus.getHealthyNamedActivity("Exercise");


                if (workActivity != null && exerciseActivity!= null)
                {
                    builder.AppendLine(getFormattedPoint5D(Clock.hour, workActivity.getProbability(), exerciseActivity.getProbability(), workActivity.getProbabilityDelta(), exerciseActivity.getProbabilityDelta()));
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        File.WriteAllText(this.pathName, builder.ToString());
        Debug.Log("Written probabilities ");
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



    private string getFormattedPoint(float x, float y, float z)
    {
        string newLine = string.Format("{0},{1},{2}", x, y, z);
        return newLine;

    }

    private string getFormattedPoint5D(float x, float y, float z, float m, float n)
    {
        string newLine = string.Format("{0},{1},{2},{3},{4}",x,y,z,m,n);
        return newLine;
    }

    private string getFormatted2D(float x, float y)
    {
        string newLine = string.Format("{0}", x);

        return newLine;
    }
}

