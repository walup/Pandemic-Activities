using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ActivityCreator : MonoBehaviour
{
    // Start is called before the first frame update
    List<Activity> typeAActivities;
    List<Activity> typeBActivities;
    List<Activity> sickActivities;
    void Start()
    {
        //Actividades para personas tipo A
        typeAActivities = new List<Activity>();
        //Dormir
        Activity sleepA = new Activity();
        sleepA.name = "Sleep";
        sleepA.hourPeriodicity = 17;
        sleepA.duration = 6;
        sleepA.setProbabilityDelta(1 / sleepA.hourPeriodicity, 1 / 4f);
        sleepA.probability = 0;
        sleepA.timeConstraints = new int[2] { 20, 10 };
        sleepA.doPlaces = new List<BuildingType>();
        sleepA.doPlaces.Add(BuildingType.HOUSE);
        sleepA.noiseCounter = 0;

        //Comer
        Activity eatA = new Activity();
        eatA.name = "Eat";
        eatA.hourPeriodicity = 3;
        eatA.duration = 0.8f;
        eatA.setProbabilityDelta(1 / eatA.hourPeriodicity, 1 / 2f);
        eatA.probability = Random.Range(0,1);
        eatA.timeConstraints = new int[2] { 6, 22 };
        eatA.doPlaces = new List<BuildingType>();
        eatA.doPlaces.Add(BuildingType.HOUSE);
        eatA.doPlaces.Add(BuildingType.RESTAURANT);
        eatA.doPlaces.Add(BuildingType.MALL);
        eatA.noiseCounter = 0;

        //Trabajar 
        Activity workA = new Activity();
        workA.name = "Work";
        workA.hourPeriodicity = 11;
        workA.duration = 8;
        workA.setProbabilityDelta(1 / workA.hourPeriodicity, 1 / 4f);
        workA.probability = Random.Range(0.5f,1);
        workA.timeConstraints = new int[2] { 8, 22 };
        workA.doPlaces = new List<BuildingType>();
        workA.doPlaces.Add(BuildingType.OFFICE);
        workA.noiseCounter = 0;

        //Ejercitarse
        Activity exerciseA = new Activity();
        exerciseA.name = "Exercise";
        exerciseA.duration = 1;
        exerciseA.hourPeriodicity = 72;
        exerciseA.setProbabilityDelta(1 / exerciseA.hourPeriodicity, 1 / 12f);
        exerciseA.probability = Random.Range(0,1);
        exerciseA.timeConstraints = new int[2] { 6, 20 };
        exerciseA.doPlaces = new List<BuildingType>();
        exerciseA.doPlaces.Add(BuildingType.GYM);
        exerciseA.doPlaces.Add(BuildingType.PARK);
        exerciseA.noiseCounter = 0;

        //Socializar
        Activity socializeA = new Activity();
        socializeA.name = "Socialize";
        socializeA.hourPeriodicity = 72;
        socializeA.duration = 1.5f;
        socializeA.setProbabilityDelta(1 / socializeA.hourPeriodicity, 1 / 12f);
        socializeA.probability = Random.Range(0,1);
        socializeA.timeConstraints = new int[2] {10, 20 };
        socializeA.doPlaces = new List<BuildingType>();
        socializeA.doPlaces.Add(BuildingType.MALL);
        socializeA.doPlaces.Add(BuildingType.PARK);
        socializeA.doPlaces.Add(BuildingType.RESTAURANT);
        socializeA.noiseCounter = 0;

        //Comprar comida
        Activity buyFoodA = new Activity();
        buyFoodA.name = "Buy Food";
        buyFoodA.hourPeriodicity = 90;
        buyFoodA.setProbabilityDelta(1 / buyFoodA.hourPeriodicity, 1 / 24f);
        buyFoodA.probability = Random.Range(0,1);
        buyFoodA.duration = 3;
        buyFoodA.timeConstraints = new int[2] { 10, 20 };
        buyFoodA.doPlaces = new List<BuildingType>();
        buyFoodA.doPlaces.Add(BuildingType.FOOD_STORE);
        buyFoodA.noiseCounter = 0;

        //Estar enfermo
        Activity sickActivityA = new Activity();
        sickActivityA.name = "Sicko";
        sickActivityA.hourPeriodicity = 300;
        sickActivityA.setProbabilityDelta(1 / sickActivityA.hourPeriodicity, 1 / 48f);
        sickActivityA.probability = 0;
        sickActivityA.duration = 23;
        sickActivityA.timeConstraints = new int[2] { 0, 24 };
        sickActivityA.doPlaces = new List<BuildingType>();
        sickActivityA.doPlaces.Add(BuildingType.HOSPITAL);
        sickActivityA.doPlaces.Add(BuildingType.HOUSE);
        sickActivityA.noiseCounter = 0;

        typeAActivities.Add(sleepA);
        typeAActivities.Add(eatA);
        typeAActivities.Add(workA);
        typeAActivities.Add(exerciseA);
        typeAActivities.Add(socializeA);
        typeAActivities.Add(buyFoodA);
        typeAActivities.Add(sickActivityA);

        ActivityAgenda agendaA = new ActivityAgenda();
        agendaA.activities = typeAActivities;
        string json = JsonUtility.ToJson(agendaA);
        File.WriteAllText(Application.dataPath + "/ActivityJsons/activities_a.json", json);

        /*------------------------------------------------------------------------------------------------------------------------------------------*/
        //Actividades para personas tipo B
        typeBActivities = new List<Activity>();
        //Dormir
        Activity sleepB = new Activity();
        sleepB.name = "Sleep";
        sleepB.hourPeriodicity = 14;
        sleepB.duration = 8;
        sleepB.setProbabilityDelta(1 / sleepB.hourPeriodicity, 1 / 4f);
        sleepB.probability = 0f;
        sleepB.timeConstraints = new int[2] { 20, 10 };
        sleepB.doPlaces = new List<BuildingType>();
        sleepB.doPlaces.Add(BuildingType.HOUSE);
        sleepB.noiseCounter = 0;
        //Comer
        Activity eatB = new Activity();
        eatB.name = "Eat";
        eatB.hourPeriodicity = 6;
        eatB.duration = 1.5f;
        eatB.setProbabilityDelta(1 / eatB.hourPeriodicity, 1 / 2f);
        eatB.probability = Random.Range(0,1);
        eatB.timeConstraints = new int[2] {6, 22 };
        eatB.doPlaces = new List<BuildingType>();
        eatB.doPlaces.Add(BuildingType.HOUSE);
        eatB.doPlaces.Add(BuildingType.RESTAURANT);
        eatB.doPlaces.Add(BuildingType.MALL);
        eatB.noiseCounter = 0;

        //Trabajar 
        Activity workB = new Activity();
        workB.name = "Work";
        workB.hourPeriodicity = 12;
        workB.duration = 6;
        workB.setProbabilityDelta(1 / workB.hourPeriodicity, 1 / 4f);
        workB.probability = Random.Range(0.5f,1);
        workB.timeConstraints = new int[2] {8, 22 };
        workB.doPlaces = new List<BuildingType>();
        workB.doPlaces.Add(BuildingType.OFFICE);
        workB.noiseCounter = 0;

        //Ejercitarse
        Activity exerciseB = new Activity();
        exerciseB.name = "Exercise";
        exerciseB.duration = 2;
        exerciseB.hourPeriodicity = 48;
        exerciseB.setProbabilityDelta(1 / exerciseB.hourPeriodicity, 1 / 12f);
        exerciseB.probability = Random.Range(0,1);
        exerciseB.timeConstraints = new int[2] {6, 20 };
        exerciseB.doPlaces = new List<BuildingType>();
        exerciseB.doPlaces.Add(BuildingType.GYM);
        exerciseB.doPlaces.Add(BuildingType.PARK);
        exerciseB.noiseCounter = 0;

        //Socializar
        Activity socializeB = new Activity();
        socializeB.name = "Socialize";
        socializeB.hourPeriodicity = 48;
        socializeB.duration = 3f;
        socializeB.setProbabilityDelta(1 / socializeB.hourPeriodicity, 1 / 12f);
        socializeB.probability = Random.Range(0,1);
        socializeB.timeConstraints = new int[2] { 10, 20};
        socializeB.doPlaces = new List<BuildingType>();
        socializeB.doPlaces.Add(BuildingType.MALL);
        socializeB.doPlaces.Add(BuildingType.PARK);
        socializeB.doPlaces.Add(BuildingType.RESTAURANT);
        socializeB.noiseCounter = 0;


        //Comprar comida
        Activity buyFoodB = new Activity();
        buyFoodB.name = "Buy Food";
        buyFoodB.hourPeriodicity = 70;
        buyFoodB.setProbabilityDelta(1 / buyFoodB.hourPeriodicity, 1 / 24f);
        buyFoodB.probabilityDelta = 1 / buyFoodB.hourPeriodicity;
        buyFoodB.maxProbabilityDelta = 1 / 48f;
        buyFoodB.probability = Random.Range(0,1);
        buyFoodB.duration = 3.5f;
        buyFoodB.timeConstraints = new int[2] {10, 20 };
        buyFoodB.doPlaces = new List<BuildingType>();
        buyFoodB.doPlaces.Add(BuildingType.FOOD_STORE);
        buyFoodB.noiseCounter = 0;


        //Estar enfermo
        Activity sickActivityB = new Activity();
        sickActivityB.name = "Sicko";
        sickActivityB.hourPeriodicity = 360;
        sickActivityB.setProbabilityDelta(1 / sickActivityB.hourPeriodicity, 1 / 48f);
        sickActivityB.probability = 0;
        sickActivityB.duration = 23;
        sickActivityB.timeConstraints = new int[2] { 0, 24 };
        sickActivityB.doPlaces = new List<BuildingType>();
        sickActivityB.doPlaces.Add(BuildingType.HOSPITAL);
        sickActivityB.doPlaces.Add(BuildingType.HOUSE);
        sickActivityB.noiseCounter = 0;


        typeBActivities.Add(sleepB);
        typeBActivities.Add(eatB);
        typeBActivities.Add(workB);
        typeBActivities.Add(exerciseB);
        typeBActivities.Add(socializeB);
        typeBActivities.Add(buyFoodB);
        typeBActivities.Add(sickActivityB);

        ActivityAgenda agendaB = new ActivityAgenda();
        agendaB.activities = typeBActivities;
        string jsonB = JsonUtility.ToJson(agendaB);
        File.WriteAllText(Application.dataPath + "/ActivityJsons/activities_b.json", jsonB);

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //Actividades para personas tipo enfermas
        sickActivities = new List<Activity>();
        //Dormir
        Activity sleepS = new Activity();
        sleepS.name = "Sleep";
        sleepS.hourPeriodicity = 8;
        sleepS.duration = 3;
        sleepS.setProbabilityDelta(1 / sleepS.hourPeriodicity, 1 / 2f);
        sleepS.probabilityDelta = 1 / sleepS.hourPeriodicity;
        sleepS.maxProbabilityDelta= 1 / 4f;
        sleepS.probability = 0f;
        sleepS.timeConstraints = new int[2] {0, 24};
        sleepS.doPlaces = new List<BuildingType>();
        sleepS.doPlaces.Add(BuildingType.HOUSE);
        sleepS.doPlaces.Add(BuildingType.HOSPITAL);
        sleepS.noiseCounter = 0;

        //Comer
        Activity eatS = new Activity();
        eatS.name = "Eat";
        eatS.hourPeriodicity = 4;
        eatS.duration = 0.5f;
        eatS.setProbabilityDelta(1 / eatS.hourPeriodicity, 1 / 2f);
        eatS.probability = 0f;
        eatS.timeConstraints = new int[2] { 6, 22 };
        eatS.doPlaces = new List<BuildingType>();
        eatS.doPlaces.Add(BuildingType.HOUSE);
        eatS.doPlaces.Add(BuildingType.RESTAURANT);
        eatS.doPlaces.Add(BuildingType.MALL);
        eatS.noiseCounter = 0;

        //Trabajar 
        Activity workS = new Activity();
        workS.name = "Work";
        workS.hourPeriodicity = 192;
        workS.duration = 8;
        workS.setProbabilityDelta(1 / workS.hourPeriodicity, 1 / 98f);
        workS.probability = 0;
        workS.timeConstraints = new int[2] { 8, 22 };
        workS.doPlaces = new List<BuildingType>();
        workS.doPlaces.Add(BuildingType.OFFICE);
        workS.noiseCounter = 0;

        //Ejercitarse
        Activity exerciseS = new Activity();
        exerciseS.name = "Exercise";
        exerciseS.duration = 1;
        exerciseS.hourPeriodicity = 192;
        exerciseS.setProbabilityDelta(1 / exerciseS.hourPeriodicity, 1 / 24f);
        exerciseS.probability = 0;
        exerciseS.timeConstraints = new int[2] { 6, 20 };
        exerciseS.doPlaces = new List<BuildingType>();
        exerciseS.doPlaces.Add(BuildingType.GYM);
        exerciseS.doPlaces.Add(BuildingType.PARK);
        exerciseS.noiseCounter = 0;

        //Socializar
        Activity socializeS = new Activity();
        socializeS.name = "Socialize";
        socializeS.hourPeriodicity = 192;
        socializeS.duration = 1.5f;
        socializeS.setProbabilityDelta(1 / socializeS.hourPeriodicity, 1 / 98f);
        socializeS.probability = 0;
        socializeS.timeConstraints = new int[2] {10, 20 };
        socializeS.doPlaces = new List<BuildingType>();
        socializeS.doPlaces.Add(BuildingType.MALL);
        socializeS.doPlaces.Add(BuildingType.PARK);
        socializeS.doPlaces.Add(BuildingType.RESTAURANT);
        socializeS.noiseCounter = 0;


        //Comprar comida
        Activity buyFoodS = new Activity();
        buyFoodS.name = "Buy Food";
        buyFoodS.hourPeriodicity = 48;
        buyFoodS.setProbabilityDelta(1 / buyFoodS.hourPeriodicity, 1 / 12f);
        buyFoodS.probability = 0.5f;
        buyFoodS.duration = 4;
        buyFoodS.timeConstraints = new int[2] {10, 20 };
        buyFoodS.doPlaces = new List<BuildingType>();
        buyFoodS.doPlaces.Add(BuildingType.FOOD_STORE);
        buyFoodS.noiseCounter = 0;

        //Estar enfermo
        Activity sickActivityS = new Activity();
        sickActivityS.name = "Sicko";
        sickActivityS.hourPeriodicity = 18;
        sickActivityS.setProbabilityDelta(1 / sickActivityS.hourPeriodicity, 1 / 4f);
        sickActivityS.probability = 0.5f;
        sickActivityS.duration = 4;
        sickActivityS.timeConstraints = new int[2] {0, 24 };
        sickActivityS.doPlaces = new List<BuildingType>();
        sickActivityS.doPlaces.Add(BuildingType.HOSPITAL);
        sickActivityS.doPlaces.Add(BuildingType.HOUSE);
        sickActivityS.noiseCounter = 0;

        sickActivities.Add(sleepS);
        sickActivities.Add(eatS);
        sickActivities.Add(workS);
        sickActivities.Add(exerciseS);
        sickActivities.Add(socializeS);
        sickActivities.Add(buyFoodS);
        sickActivities.Add(sickActivityS);

        ActivityAgenda agendaS = new ActivityAgenda();
        agendaS.activities = sickActivities;
        string jsonS = JsonUtility.ToJson(agendaS);
        File.WriteAllText(Application.dataPath + "/ActivityJsons/activities_s.json", jsonS);

        Debug.Log("Success");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
