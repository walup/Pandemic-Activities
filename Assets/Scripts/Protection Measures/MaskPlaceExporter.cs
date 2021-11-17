using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MaskPlaceExporter : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        SerializableDictBuildingFloat maskPlaceDistribution = new SerializableDictBuildingFloat();
        SerializableDictBuildingFloat maskPlaceDistributionValues = new SerializableDictBuildingFloat();

        // Food Store
        float[] foodStoreWearMaskDistribution = { 0.080f, 0.320f, 0.9440f, 0.0160f };
        float[] values = { 0, 0.5f, 1, 0 };

        SerializableFloatList floatListFoodStore = new SerializableFloatList();
        SerializableFloatList valuesList = new SerializableFloatList();
        floatListFoodStore.floatList = foodStoreWearMaskDistribution;
        valuesList.floatList = values;
        maskPlaceDistribution.Add(BuildingType.FOOD_STORE, floatListFoodStore);
        maskPlaceDistributionValues.Add(BuildingType.FOOD_STORE, valuesList);


        // House
        float[] houseWearMaskDistribution = { 0.544f, 0.2640f, 0.0480f, 0.1440f };
        SerializableFloatList floatListHouse = new SerializableFloatList();
        floatListHouse.floatList = houseWearMaskDistribution;
        maskPlaceDistribution.Add(BuildingType.HOUSE, floatListHouse);
        maskPlaceDistributionValues.Add(BuildingType.HOUSE, valuesList);


        // Office
        float[] officeWearMaskDistribution = { 0.0080f, 0.0400f, 0.4480f, 0.5040f };
        SerializableFloatList floatListOffice = new SerializableFloatList();
        floatListOffice.floatList = officeWearMaskDistribution;

        maskPlaceDistribution.Add(BuildingType.OFFICE, floatListOffice);
        maskPlaceDistributionValues.Add(BuildingType.OFFICE, valuesList);

        // Gym
        float[] gymWearMaskDistribution = { 0.0240f, 0.0240f, 0.2960f, 0.6560f };
        SerializableFloatList floatListGym = new SerializableFloatList();
        floatListGym.floatList = gymWearMaskDistribution;
        maskPlaceDistribution.Add(BuildingType.GYM, floatListGym);
        maskPlaceDistributionValues.Add(BuildingType.GYM, valuesList);

        //Hospital
        float[] hospitalWearMaskDistribution = { 0.0160f, 0.0240f, 0.5840f, 0.3760f };
        SerializableFloatList floatListHospital = new SerializableFloatList();
        floatListHospital.floatList = hospitalWearMaskDistribution;
        maskPlaceDistribution.Add(BuildingType.HOSPITAL, floatListHospital);
        maskPlaceDistributionValues.Add(BuildingType.HOSPITAL, valuesList);

        // Park 
        float[] parkWearMaskDistribution = { 0.0860f, 0.2880f, 0.5520f, 0.0720f };
        SerializableFloatList floatListPark = new SerializableFloatList();
        floatListPark.floatList = parkWearMaskDistribution;
        maskPlaceDistribution.Add(BuildingType.PARK, floatListPark);
        maskPlaceDistributionValues.Add(BuildingType.PARK, valuesList);

        //Mall
        float[] mallWearMaskDistribution = { 0.0240f, 0.0240f, 0.8800f, 0.0720f };
        SerializableFloatList floatListMall = new SerializableFloatList();
        floatListMall.floatList = mallWearMaskDistribution;
        maskPlaceDistribution.Add(BuildingType.MALL, floatListMall);
        maskPlaceDistributionValues.Add(BuildingType.MALL, valuesList);

        //Restaurantes
        float[] restaurantWearMaskDistribution = { 0.0320f, 0.1840f, 0.6640f, 0.1200f };
        SerializableFloatList floatListRestaurant = new SerializableFloatList();
        floatListRestaurant.floatList = restaurantWearMaskDistribution;
        maskPlaceDistribution.Add(BuildingType.RESTAURANT, floatListRestaurant);
        maskPlaceDistributionValues.Add(BuildingType.RESTAURANT, valuesList); 
        //maskPlaceDistribution.Add(BuildingType.RESTAURANT, restaurantWearMaskDistribution);
        //maskPlaceDistributionValues.Add(BuildingType.RESTAURANT, values);


        // Distribución uso mascarillas al movilizarse
        float[] movilizationDistribution = { 0.04f, 0.16f, 0.7760f, 0.0240f };
        float[] movilizationValues = values;

        MaskPlaceDistributions maskAgenda = new MaskPlaceDistributions();
        maskAgenda.placesDistributionHashtable = maskPlaceDistribution;
        maskAgenda.placesDistributionValuesHashtable = maskPlaceDistributionValues;
        maskAgenda.movilizationDistribution = movilizationDistribution;
        maskAgenda.movilizationDistributionValues = movilizationValues;

       //
        string json = JsonUtility.ToJson(maskAgenda);
        File.WriteAllText(Application.dataPath + "/ProtectionDistributionJSONS/data_mask_wearing_dists.json", json);
        Debug.Log("Succesfully exported mask distributions");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
