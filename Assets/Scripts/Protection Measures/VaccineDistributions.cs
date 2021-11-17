using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaccineDistributions
{
    public float[] vaccinatedDistribution;
    public bool[] vaccinatedDistributionValues;
    public float[] vaccinationTypeDistribution;
    public VaccineType[] vaccinationTypeValues;
    public float[] vaccinationEfficaciesContagion;
    public float[] vaccinationEfficaciesCriticalIllness;
    public int[] vaccinationDoses;



    public VaccineDistributions()
    {
        //Comencemos con las distribuciones de vacunación 
        float[] vaccDist = { 0.8560f, 1 - 0.8560f };
        vaccinatedDistribution = vaccDist;
        bool[] vaccDistVals = { true, false };
        vaccinatedDistributionValues = vaccDistVals;

        vaccinationTypeValues = (VaccineType[])System.Enum.GetValues(typeof(VaccineType));
        float[] vaccTypeDistribution = { 0.2101f, 0.2941f, 0.1008f, 0.0672f, 0.1597f, 0, 0.0168f, 0.1092f, 0.0420f };
        vaccinationTypeDistribution = vaccTypeDistribution;
        int[] vaccDoses = {2, 2, 2, 2, 1, 2, 1, 0, 2};
        vaccinationDoses = vaccDoses;
        float[] vaccEfficaciesContagion = { 0.89f, 0.6f, 0.6f, 0.51f, 0.65f, 0.78f, 0.66f, 0, 0.9f };
        vaccinationEfficaciesContagion = vaccEfficaciesContagion;
        float[] vaccEfficaciesIllness = { 0.95f, 0.93f, 0.9f, 0.9f, 0.9f, 0.9f, 0.9f, 0.8f, 0, 0.94f };
        vaccinationEfficaciesCriticalIllness = vaccEfficaciesIllness;
    }
}
