using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
[Serializable]
public class ObedienceStoplightDistributions
{
    //En general la distribución de obediencia al semáforo acorde a lo que respondíó la
    //gente
    public float[] obedienceValues;
    public float[] obedienceDistribution;
    //Obediencia a aislamiento social
    public float[] obedienceIsolationValues;
    public float[] obedienceIsolationDistribution;
    //Uso de mascarilla
    public float[] obedienceMaskUseValues;
    public float[] obedienceMaskUseDistribution;
    //Distanciamiento social 
    public float[] obedienceSocialDistancingValues;
    public float[] obedienceSocialDistancingDistribution;


    public string ToString()
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("Obediencia al semáforo \n");
        builder.Append("--------------------------\n");
        for (int i = 0; i < obedienceValues.Length; i++)
        {
            builder.Append(obedienceValues[i] + " -> " + obedienceDistribution[i] + " ");
        }
        builder.Append("\n\n");
        builder.Append("Obediencia al aislamiento social \n");
        builder.Append("--------------------------------- \n");
        for(int i = 0; i < obedienceIsolationValues.Length; i++)
        {
            builder.Append(obedienceIsolationValues[i] + " -> " + obedienceIsolationDistribution[i] + " ");
        }
        builder.Append("\n\n");
        builder.Append("Obediencia al uso de cubrebocas \n");
        builder.Append("----------------------------------\n");
        for(int i = 0; i < obedienceMaskUseValues.Length; i++)
        {
            builder.Append(obedienceMaskUseValues[i] + " -> " + obedienceMaskUseDistribution[i] + " ");
        }
        builder.Append("\n\n");
        builder.Append("Obediencia al distanciamiento social al salir\n");
        builder.Append("--------------------------------------------\n");
        for(int i = 0; i < obedienceSocialDistancingValues.Length; i++)
        {
            builder.Append(obedienceSocialDistancingValues[i] + " -> " + obedienceSocialDistancingDistribution[i] + " ");
        }

        return builder.ToString();

    }

}
