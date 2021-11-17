using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ActivityCreator : MonoBehaviour
{
    // Start is called before the first frame update
    List<Activity> healthyActivities;
    List<Activity> sickActivities;
    void Start()
    {
        //Actividades para personas tipo A
        healthyActivities = new List<Activity>();


        //Trabajar
        Activity workHealthy = new Activity();
        workHealthy.name = "Work";
        //Distribución de duración 
        float[] durationDistWork = { 0.044f, 0.053f, 0.1061f, 0.1061f, 0.0884f, 0.1858f, 0.1769f, 0.2397f };
        workHealthy.durationDistribution = durationDistWork;
        float[] durationValsWork = { 2f, 3.5f, 4.5f, 5.5f, 6.5f, 7.5f, 8.5f, 10f};
        workHealthy.durationValues = durationValsWork;
        workHealthy.durationBinSize = 1f;
        //Distribución de frecuencia
        float[] freqValsWork = {0f,1f};
        float[] freqDistWork = {0.144f, 0.856f};
        workHealthy.frequencyValues = freqValsWork;
        workHealthy.frequencyDistribution = freqDistWork;
        workHealthy.frequencyTimeRate = TimeRate.TIMES_PER_DAY;
        workHealthy.frequencyBinSize = 1;
        workHealthy.timeConstraints = new int[2] { 8, 22 };
        //Distribución de lugares
        workHealthy.doPlaces = new List<BuildingType>();
        workHealthy.doPlaces.Add(BuildingType.HOUSE);
        workHealthy.doPlaces.Add(BuildingType.OFFICE);
        float[] placesDistWork = { 0.6899f, 0.3101f};
        workHealthy.distributionPlaces = placesDistWork; 
        //El contador de ruido
        workHealthy.noiseCounter = 0;

        //Sick Activity
        Activity sickActivityHealthy = new Activity();
        sickActivityHealthy.name = "Sicko";
        //Distribución de frecuencia 
        float [] freqValsSick = {0f,1f,2f,3f,4f};
        float[] freqDistSick = { 0.72f, 0.16f, 0.08f, 0.008f, 0.032f };
        sickActivityHealthy.frequencyValues = freqValsSick;
        sickActivityHealthy.frequencyDistribution = freqDistSick;
        sickActivityHealthy.frequencyTimeRate = TimeRate.TIMES_PER_MONTH;
        sickActivityHealthy.frequencyBinSize = 1;
        //Distribución de duración 
        float[] durationValsSick = { 0.25f, 0.75f, 1.25f, 1.75f, 2.25f, 2.75f, 3.25f};
        float[] durationDistSick = { 0.2148f, 0.4710f, 0.1818f, 0.0909f, 0.02479f, 0.0082f, 0.0085f };
        sickActivityHealthy.durationDistribution = durationDistSick;
        sickActivityHealthy.durationValues = durationValsSick;
        sickActivityHealthy.durationBinSize = 0.5f;
        //Distribución de lugares
        sickActivityHealthy.doPlaces = new List<BuildingType>();
        sickActivityHealthy.doPlaces.Add(BuildingType.HOSPITAL);
        float[] placesDistSick = { 1f };
        sickActivityHealthy.distributionPlaces = placesDistSick;
        //Constricciones de tiempo
        sickActivityHealthy.timeConstraints = new int[2] { 0, 24};
        //El contador de ruido
        sickActivityHealthy.noiseCounter = 0;

        //Ejercicio
        Activity exerciseHealthy = new Activity();
        exerciseHealthy.name = "Exercise";
        //Distribución de frecuencia
        float[] freqValsExercise = { 0, 1, 2, 3, 4, 5, 6, 7 };
        float[] freqDistExercise = { 0, 0.1212f, 0.2020f, 0.2121f, 0.1111f, 0.2323f, 0.0909f, 0.0304f };
        exerciseHealthy.frequencyValues = freqValsExercise;
        exerciseHealthy.frequencyDistribution = freqDistExercise;
        exerciseHealthy.frequencyBinSize = 1;
        exerciseHealthy.frequencyTimeRate = TimeRate.TIMES_PER_WEEK;
        //Distribución de duración 
        float[] durationValsExercise = { 0.25f, 0.75f, 1.25f, 1.75f, 2.25f, 2.75f,3.25f};
        float[] durationDistExercise = { 0.3070f, 0.4824f, 0.0877f, 0.0614f, 0.02631f, 0.01754f, 0.01754f };
        exerciseHealthy.durationValues = durationValsExercise;
        exerciseHealthy.durationDistribution = durationDistExercise;
        exerciseHealthy.durationBinSize = 0.5f;
        //Distribución de lugares
        exerciseHealthy.doPlaces = new List<BuildingType>();
        exerciseHealthy.doPlaces.Add(BuildingType.HOUSE);
        exerciseHealthy.doPlaces.Add(BuildingType.GYM);
        exerciseHealthy.doPlaces.Add(BuildingType.PARK);
        float[] placesDistExercise = { 0.5248f, 0.1418f, 0.3334f };
        exerciseHealthy.distributionPlaces = placesDistExercise;
        //Constricciones de tiempo 
        exerciseHealthy.timeConstraints = new int[2] { 6, 20 };
        //Contador de ruido
        exerciseHealthy.noiseCounter = 0;

        //Comer
        Activity eatHealthy = new Activity();
        eatHealthy.name = "Eat";
        //Distribución de frecuencia
        float[] freqValsEat = { 1, 2, 3, 4, 5 };
        float[] freqDistEat = { 0.0086f, 0.1565f, 0.6869f, 0.1478f, 0f };
        eatHealthy.frequencyValues = freqValsEat;
        eatHealthy.frequencyDistribution = freqDistEat;
        eatHealthy.frequencyBinSize = 1;
        eatHealthy.frequencyTimeRate = TimeRate.TIMES_PER_DAY;
        //Distribución de duración
        float[] durationValsEat = { 0.25f, 0.75f, 1.25f, 1.75f, 2.25f, 2.75f,3.25f};
        float[] durationDistEat = { 0.344f, 0.544f, 0.088f, 0.016f, 0f, 0.008f, 0 };
        eatHealthy.durationValues = durationValsEat;
        eatHealthy.durationDistribution = durationDistEat;
        eatHealthy.durationBinSize = 0.5f;

        //Distribución de lugares
        eatHealthy.doPlaces = new List<BuildingType>();
        eatHealthy.doPlaces.Add(BuildingType.HOUSE);
        eatHealthy.doPlaces.Add(BuildingType.OFFICE);
        eatHealthy.doPlaces.Add(BuildingType.RESTAURANT);
        float[] placesDistEat = { 0.8333f, 0.1231f, 0.0436f };
        eatHealthy.distributionPlaces = placesDistEat;
        //Constricciones de tiempo
        eatHealthy.timeConstraints = new int[2] { 6, 22 };
        //Contador de ruido
        eatHealthy.noiseCounter = 0;

        //Buy Food
        Activity buyFoodHealthy = new Activity();
        buyFoodHealthy.name = "Buy Food";
        //Distribución de frecuencia
        float[] freqValsBuyFood = { 1, 2, 3, 4, 5, 6 };
        float[] freqDistBuyFood = { 0.5210f, 0.2857f, 0.15966f, 0.0168f, 0.0168f, 0 };
        buyFoodHealthy.frequencyValues = freqValsBuyFood;
        buyFoodHealthy.frequencyDistribution = freqDistBuyFood;
        buyFoodHealthy.frequencyBinSize = 1;
        buyFoodHealthy.frequencyTimeRate = TimeRate.TIMES_PER_WEEK;
        //Distribución de duración
        float[] durationValsBuyFood = { 0.25f, 0.75f, 1.25f, 1.75f, 2.25f, 2.75f, 3.25f };
        float[] durationDistBuyFood = {0.1048f, 0.4919f, 0.2741f, 0.0725f, 0.0241f, 0.0161f, 0.0161f};
        buyFoodHealthy.durationValues = durationValsBuyFood;
        buyFoodHealthy.durationDistribution = durationDistBuyFood;
        buyFoodHealthy.durationBinSize = 0.5f;
        //Distribución de lugares
        buyFoodHealthy.doPlaces = new List<BuildingType>();
        buyFoodHealthy.doPlaces.Add(BuildingType.FOOD_STORE);
        float[] placesDistBuyFood = { 1 };
        buyFoodHealthy.distributionPlaces = placesDistBuyFood;
        //Constricciones de tiempo
        buyFoodHealthy.timeConstraints = new int[2] { 10, 20 };
        //Contador de ruido
        buyFoodHealthy.noiseCounter = 0;

        //Socializar
        Activity socializeHealthy = new Activity();
        socializeHealthy.name = "Socialize";
        //Distribución de frecuencia
        float[] freqValsSocialize = { 0, 1, 2, 3, 4, 5, 6 };
        float[] freqDistSocialize = { 0.2377f, 0.5327f, 0.1393f, 0.0737f, 0.0081f, 0.0081f, 0 };
        socializeHealthy.frequencyValues = freqValsSocialize;
        socializeHealthy.frequencyDistribution = freqDistSocialize;
        socializeHealthy.frequencyBinSize = 1;
        socializeHealthy.frequencyTimeRate = TimeRate.TIMES_PER_WEEK;
        //Distribución de duración 
        float[] durationValsSocialize = { 0.25f, 0.75f, 1.25f, 1.75f, 2.25f, 2.75f, 3.25f };
        float[] durationDistSocialize = { 0.1735f, 0.1157f, 0.1074f, 0.0991f, 0.1157f, 0.1157f, 0.2727f };
        socializeHealthy.durationValues = durationValsSocialize;
        socializeHealthy.durationDistribution = durationDistSocialize;
        socializeHealthy.durationBinSize = 0.5f;
        //Distribución de lugares
        socializeHealthy.doPlaces = new List<BuildingType>();
        socializeHealthy.doPlaces.Add(BuildingType.MALL);
        socializeHealthy.doPlaces.Add(BuildingType.HOUSE);
        socializeHealthy.doPlaces.Add(BuildingType.PARK);
        socializeHealthy.doPlaces.Add(BuildingType.RESTAURANT);
        float[] placesDistSocialize = { 0.2107f, 0.4754f, 0.1865f, 0.1274f };
        socializeHealthy.distributionPlaces = placesDistSocialize;
        //Constricciones de tiempo 
        socializeHealthy.timeConstraints = new int[2]{10, 20 };
        //Contador de ruido
        socializeHealthy.noiseCounter = 0;

        //Dormir
        Activity sleepHealthy= new Activity();
        sleepHealthy.name = "Sleep";
        //Distribución de frecuencia
        float[] freqValsSleep = {0f, 1f };
        float[] freqDistSleep = {0, 1f};
        sleepHealthy.frequencyValues = freqValsSleep;
        sleepHealthy.frequencyDistribution = freqDistSleep;
        sleepHealthy.frequencyBinSize = 1;
        sleepHealthy.frequencyTimeRate = TimeRate.TIMES_PER_DAY;
        //Distribución de duración 
        float[] durationValsSleep = { 3, 4, 5, 6, 7, 8, 9, 10,11};
        float[] durationDistSleep = { 0, 0, 0.1048f, 0.3548f, 0.34677f, 0.1693f, 0, 0.0080f, 0.0161f };
        sleepHealthy.durationValues = durationValsSleep;
        sleepHealthy.durationDistribution = durationDistSleep;
        sleepHealthy.durationBinSize = 1;
        //Distribución de lugares
        sleepHealthy.doPlaces = new List<BuildingType>();
        sleepHealthy.doPlaces.Add(BuildingType.HOUSE);
        float[] placesDistSleep = { 1f };
        sleepHealthy.distributionPlaces = placesDistSleep;
        //Constricciones de tiempo 
        sleepHealthy.timeConstraints = new int[2] { 20, 10 };
        //Contador de ruido
        sleepHealthy.noiseCounter = 0;

        healthyActivities.Add(sleepHealthy);
        healthyActivities.Add(eatHealthy);
        healthyActivities.Add(workHealthy);
        healthyActivities.Add(exerciseHealthy);
        healthyActivities.Add(socializeHealthy);
        healthyActivities.Add(buyFoodHealthy);
        healthyActivities.Add(sickActivityHealthy);

        ActivityAgenda agendaHealthy = new ActivityAgenda();
        agendaHealthy.activities = healthyActivities;
        string json = JsonUtility.ToJson(agendaHealthy);
        File.WriteAllText(Application.dataPath + "/ActivityJsons/activities_h.json", json);

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //Actividades para personas de tipo enfermas
        List<Activity> sickActivities = new List<Activity>();

        //Trabajar
        Activity workSick = new Activity();
        workSick.name = "Work";
        //Distribución de duración 
        float[] durationDistWorkSick = {0.4f, 0.4f, 0.2f,0, 0, 0, 0, 0, 0 };
        workSick.durationDistribution = durationDistWorkSick;
        float[] durationValsWorkSick = { 2f, 3.5f, 4.5f, 5.5f, 6.5f, 7.5f, 8.5f, 10f };
        workSick.durationValues = durationValsWorkSick;
        workSick.durationBinSize = 1f;
        //Distribución de frecuencia
        float[] freqValsWorkSick = { 0f, 1f };
        float[] freqDistWorkSick = { 1f, 0f };
        workSick.frequencyValues = freqValsWork;
        workSick.frequencyDistribution = freqDistWork;
        workSick.frequencyTimeRate = TimeRate.TIMES_PER_DAY;
        workSick.frequencyBinSize = 1;
        workSick.timeConstraints = new int[2] { 8, 22 };
        //Distribución de lugares
        workSick.doPlaces = new List<BuildingType>();
        workSick.doPlaces.Add(BuildingType.HOUSE);
        workSick.doPlaces.Add(BuildingType.OFFICE);
        float[] placesDistWorkSick = { 0.6899f, 0.3101f };
        workSick.distributionPlaces = placesDistWork;
        //El contador de ruido
        workHealthy.noiseCounter = 0;

        //Sick Activity
        Activity sickActivitySick = new Activity();
        sickActivitySick.name = "Sicko";
        //Distribución de frecuencia 
        float[] freqValsSickSick = { 0f, 1f, 2f, 3f, 4f };
        float[] freqDistSickSick = { 0, 0.1f, 0.1f, 0.6f, 0.2f };
        sickActivitySick.frequencyValues = freqValsSickSick;
        sickActivitySick.frequencyDistribution = freqDistSickSick;
        sickActivitySick.frequencyTimeRate = TimeRate.TIMES_PER_DAY;
        sickActivitySick.frequencyBinSize = 1;
        //Distribución de duración 
        float[] durationValuesSickSick = { 0.25f, 0.75f, 1.25f, 1.75f, 2.25f, 2.75f, 3.25f };
        float[] durationDistSickSick = { 0.2148f, 0.4710f, 0.1818f, 0.0909f, 0.02479f, 0.0082f, 0.0085f };
        sickActivitySick.durationDistribution = durationDistSickSick;
        sickActivitySick.durationValues = durationValuesSickSick;
        sickActivitySick.durationBinSize = 0.5f;
        //Distribución de lugares
        sickActivitySick.doPlaces = new List<BuildingType>();
        sickActivitySick.doPlaces.Add(BuildingType.HOSPITAL);
        float[] placesDistSickSick = { 1f };
        sickActivitySick.distributionPlaces = placesDistSickSick;
        //Constricciones de tiempo
        sickActivitySick.timeConstraints = new int[2] { 0, 24 };
        //El contador de ruido
        sickActivitySick.noiseCounter = 0;

        //Ejercicio
        Activity exerciseSick = new Activity();
        exerciseSick.name = "Exercise";
        //Distribución de frecuencia
        float[] freqValsExerciseSick = { 0, 1, 2, 3, 4, 5, 6, 7 };
        float[] freqDistExerciseSick = { 0.8f, 0.1f, 0.05f, 0.05f, 0, 0, 0, 0 };
        exerciseSick.frequencyValues = freqValsExerciseSick;
        exerciseSick.frequencyDistribution = freqDistExerciseSick;
        exerciseSick.frequencyBinSize = 1;
        exerciseSick.frequencyTimeRate = TimeRate.TIMES_PER_WEEK;
        //Distribución de duración 
        float[] durationValsExerciseSick = { 0.25f, 0.75f, 1.25f, 1.75f, 2.25f, 2.75f, 3.25f };
        float[] durationDistExerciseSick = { 0.3070f, 0.4824f, 0.0877f, 0.0614f, 0.02631f, 0.01754f, 0.01754f };
        exerciseSick.durationValues = durationValsExerciseSick;
        exerciseSick.durationDistribution = durationDistExerciseSick;
        exerciseSick.durationBinSize = 0.5f;
        //Distribución de lugares
        exerciseSick.doPlaces = new List<BuildingType>();
        exerciseSick.doPlaces.Add(BuildingType.HOUSE);
        exerciseSick.doPlaces.Add(BuildingType.GYM);
        exerciseSick.doPlaces.Add(BuildingType.PARK);
        float[] placesDistExerciseSick = { 0.5248f, 0.1418f, 0.3334f };
        exerciseSick.distributionPlaces = placesDistExerciseSick;
        //Constricciones de tiempo 
        exerciseSick.timeConstraints = new int[2] { 6, 20 };
        //Contador de ruido
        exerciseSick.noiseCounter = 0;

        //Comer
        Activity eatSick = new Activity();
        eatSick.name = "Eat";
        //Distribución de frecuencia
        float[] freqValsEatSick = { 1, 2, 3, 4, 5 };
        float[] freqDistEatSick = { 0.4f, 0.4f, 0.2f, 0, 0f };
        eatSick.frequencyValues = freqValsEatSick;
        eatSick.frequencyDistribution = freqDistEatSick;
        eatSick.frequencyBinSize = 1;
        eatSick.frequencyTimeRate = TimeRate.TIMES_PER_DAY;
        //Distribución de duración
        float[] durationValsEatSick = { 0.25f, 0.75f, 1.25f, 1.75f, 2.25f, 2.75f, 3.25f };
        float[] durationDistEatSick = { 0.344f, 0.544f, 0.088f, 0.016f, 0f, 0.008f, 0 };
        eatSick.durationValues = durationValsEatSick;
        eatSick.durationDistribution = durationDistEatSick;
        eatSick.durationBinSize = 0.5f;

        //Distribución de lugares
        eatSick.doPlaces = new List<BuildingType>();
        eatSick.doPlaces.Add(BuildingType.HOUSE);
        eatSick.doPlaces.Add(BuildingType.OFFICE);
        eatSick.doPlaces.Add(BuildingType.RESTAURANT);
        float[] placesDistEatSick = { 0.8333f, 0.1231f, 0.0436f };
        eatSick.distributionPlaces = placesDistEatSick;
        //Constricciones de tiempo
        eatSick.timeConstraints = new int[2] { 6, 22 };
        //Contador de ruido
        eatSick.noiseCounter = 0;

        //Buy Food
        Activity buyFoodSick = new Activity();
        buyFoodSick.name = "Buy Food";
        //Distribución de frecuencia
        float[] freqValsBuyFoodSick = { 1, 2, 3, 4, 5, 6 };
        float[] freqDistBuyFoodSick = { 0.1f, 0.1f, 0.5f, 0.1f, 0.1f, 0 };
        buyFoodSick.frequencyValues = freqValsBuyFoodSick;
        buyFoodSick.frequencyDistribution = freqDistBuyFoodSick;
        buyFoodSick.frequencyBinSize = 1;
        buyFoodSick.frequencyTimeRate = TimeRate.TIMES_PER_WEEK;
        //Distribución de duración
        float[] durationValsBuyFoodSick = { 0.25f, 0.75f, 1.25f, 1.75f, 2.25f, 2.75f, 3.25f };
        float[] durationDistBuyFoodSick = { 0.1048f, 0.4919f, 0.2741f, 0.0725f, 0.0241f, 0.0161f, 0.0161f };
        buyFoodSick.durationValues = durationValsBuyFoodSick;
        buyFoodSick.durationDistribution = durationDistBuyFoodSick;
        buyFoodSick.durationBinSize = 0.5f;
        //Distribución de lugares
        buyFoodSick.doPlaces = new List<BuildingType>();
        buyFoodSick.doPlaces.Add(BuildingType.FOOD_STORE);
        float[] placesDistBuyFoodSick = { 1 };
        buyFoodSick.distributionPlaces = placesDistBuyFoodSick;
        //Constricciones de tiempo
        buyFoodSick.timeConstraints = new int[2] { 10, 20 };
        //Contador de ruido
        buyFoodSick.noiseCounter = 0;

        //Socializar
        Activity socializeSick= new Activity();
        socializeSick.name = "Socialize";
        //Distribución de frecuencia
        float[] freqValsSocializeSick = { 0, 1, 2, 3, 4, 5, 6 };
        float[] freqDistSocializeSick = { 0.7f, 0.2f, 0.1f, 0, 0, 0, 0 };
        socializeSick.frequencyValues = freqValsSocializeSick;
        socializeSick.frequencyDistribution = freqDistSocializeSick;
        socializeSick.frequencyBinSize = 1;
        socializeSick.frequencyTimeRate = TimeRate.TIMES_PER_WEEK;
        //Distribución de duración 
        float[] durationValsSocializeSick = { 0.25f, 0.75f, 1.25f, 1.75f, 2.25f, 2.75f, 3.25f };
        float[] durationDistSocializeSick = { 0.1735f, 0.1157f, 0.1074f, 0.0991f, 0.1157f, 0.1157f, 0.2727f };
        socializeSick.durationValues = durationValsSocializeSick;
        socializeSick.durationDistribution = durationDistSocializeSick;
        socializeSick.durationBinSize = 0.5f;
        //Distribución de lugares
        socializeSick.doPlaces = new List<BuildingType>();
        socializeSick.doPlaces.Add(BuildingType.MALL);
        socializeSick.doPlaces.Add(BuildingType.HOUSE);
        socializeSick.doPlaces.Add(BuildingType.PARK);
        socializeSick.doPlaces.Add(BuildingType.RESTAURANT);
        float[] placesDistSocializeSick = { 0.2107f, 0.4754f, 0.1865f, 0.1274f };
        socializeSick.distributionPlaces = placesDistSocializeSick;
        //Constricciones de tiempo 
        socializeSick.timeConstraints = new int[2] { 10, 20 };
        //Contador de ruido
        socializeSick.noiseCounter = 0;

        //Dormir
        Activity sleepSick = new Activity();
        sleepSick.name = "Sleep";
        //Distribución de frecuencia
        float[] freqValsSleepSick = { 0f, 1f, 2f, 3f };
        float[] freqDistSleepSick = { 0, 0.5f, 0.25f, 0.25f };
        sleepSick.frequencyValues = freqValsSleepSick;
        sleepSick.frequencyDistribution = freqDistSleepSick;
        sleepSick.frequencyBinSize = 1;
        sleepSick.frequencyTimeRate = TimeRate.TIMES_PER_DAY;
        //Distribución de duración 
        float[] durationValsSleepSick = { 3, 4, 5, 6, 7, 8, 9, 10, 11};
        float[] durationDistSleepSick = { 0, 0, 0.1048f, 0.3548f, 0.34677f, 0.1693f, 0, 0.0080f, 0.0161f };
        sleepSick.durationValues = durationValsSleepSick;
        sleepSick.durationDistribution = durationDistSleepSick;
        sleepSick.durationBinSize = 1;
        //Distribución de lugares
        sleepSick.doPlaces = new List<BuildingType>();
        sleepSick.doPlaces.Add(BuildingType.HOUSE);
        float[] placesDistSleepSick = { 1f };
        sleepSick.distributionPlaces = placesDistSleepSick;
        //Constricciones de tiempo 
        sleepSick.timeConstraints = new int[2] {0,24};
        //Contador de ruido
        sleepSick.noiseCounter = 0;



        sickActivities.Add(sleepSick);
        sickActivities.Add(eatSick);
        sickActivities.Add(workSick);
        sickActivities.Add(exerciseSick);
        sickActivities.Add(socializeSick);
        sickActivities.Add(buyFoodSick);
        sickActivities.Add(sickActivitySick);

        ActivityAgenda agendaSick = new ActivityAgenda();
        agendaSick.activities = sickActivities;
        string jsonSick = JsonUtility.ToJson(agendaSick);
        File.WriteAllText(Application.dataPath + "/ActivityJsons/activities_s.json", jsonSick);

        Debug.Log("Success");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
