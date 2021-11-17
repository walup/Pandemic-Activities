using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class MaskPlaceDistributions
{
    public SerializableDictBuildingFloat placesDistributionHashtable;
    public SerializableDictBuildingFloat placesDistributionValuesHashtable;

    public float[] movilizationDistribution;
    public float[] movilizationDistributionValues;

    override
    public string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("Distribuciones de uso de mascarilla en distintos lugares");
        builder.AppendLine("-----------------------------------------------------------");
        foreach (BuildingType key in placesDistributionHashtable.Keys)
        {
            builder.Append(key.ToString() + " -> ");
            SerializableFloatList distribution = placesDistributionHashtable[key];
            float[] distributionList = distribution.floatList;

            for(int i = 0; i < distributionList.Length; i++)
            {
                builder.Append(distributionList[i] + ", ");
            }
            builder.Append("\n");


        }

        builder.AppendLine("Distribución al moverse");
        builder.AppendLine("-----------------------------");
        for(int i = 0; i < movilizationDistribution.Length; i++)
        {
            builder.Append(movilizationDistribution[i] + ", ");
        }
        builder.Append("\n");



        return builder.ToString();
        
    }

}
