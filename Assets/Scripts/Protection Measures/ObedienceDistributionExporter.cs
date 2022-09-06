using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ObedienceDistributionExporter : MonoBehaviour
{
    // Start is called before the first frame update


    private string fileName = "obedience_distributions.json";
    void Awake()
    {
        float[] obedienceValues = {0f, 1f};
        float[] obedienceDistribution = { 0.6855f, 0.3145f };

        float[] obedienceIsolationValues = { 0f, 0.5f, 1f };
        float[] obedienceIsolationDistribution = { 0.0878f, 0.2418f, 0.6703f };

        float[] obedienceMaskUseValues = { 0f, 0.5f, 1f };
        float[] obedienceMaskUseDistribution = { 0.1573f, 0.2809f, 0.5618f };

        float[] obedienceSocialDistancingValues = { 0f, 0.5f, 1f };
        float[] obedienceSocialDistancingDistribution = { 0.1011f, 0.2022f, 0.6966f };

        ObedienceStoplightDistributions obedienceDistributions = new ObedienceStoplightDistributions();
        obedienceDistributions.obedienceValues = obedienceValues;
        obedienceDistributions.obedienceDistribution = obedienceDistribution;
        obedienceDistributions.obedienceIsolationValues = obedienceIsolationValues;
        obedienceDistributions.obedienceIsolationDistribution = obedienceIsolationDistribution;
        obedienceDistributions.obedienceMaskUseValues = obedienceMaskUseValues;
        obedienceDistributions.obedienceMaskUseDistribution = obedienceMaskUseDistribution;
        obedienceDistributions.obedienceSocialDistancingValues = obedienceSocialDistancingValues;
        obedienceDistributions.obedienceSocialDistancingDistribution = obedienceSocialDistancingDistribution;

        string jsonString = JsonUtility.ToJson(obedienceDistributions);
        File.WriteAllText(Application.dataPath + "/ProtectionDistributionJSONS/" + fileName, jsonString);
        Debug.Log("Succesfully exported obedience distributions");

        

    }
}
