using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class CityDataSaver : MonoBehaviour
{
    // Start is called before the first frame update

    private float samplingFrequency = 2f;
    private float samplingPeriod;
    private StringBuilder stringBuilder;

    private float timer = 0;
    private City city;
    private StoplightControl stoplightControl;
    private string fileName;

    void Start()
    {

        samplingPeriod = 1 / samplingFrequency;
        city = GetComponent<City>();
        fileName = Application.dataPath + "/CSV_DATA/" + "city_info.csv";
        stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Frec. de muestreo " + samplingFrequency);
        //stringBuilder.AppendLine("Tiendas de comida, Oficinas, Casas, Hospital, Gimnasios, Restaurants, Centros comerciales, Parques");
        stringBuilder.AppendLine("Tiendas de comida, Oficinas, Casas, Hospital, Gimnasios, Restaurants, Centros comerciales, Parques, Semáforo");
        stoplightControl = GameObject.Find("Stoplight").GetComponent<StoplightControl>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Clock.hourDelta;
        if(timer >= samplingPeriod)
        {
            //stringBuilder.AppendLine(formatPopulations(city.countPeopleInPlaces(BuildingType.FOOD_STORE), city.countPeopleInPlaces(BuildingType.OFFICE), city.countPeopleInPlaces(BuildingType.HOUSE), city.countPeopleInPlaces(BuildingType.HOSPITAL), city.countPeopleInPlaces(BuildingType.GYM), city.countPeopleInPlaces(BuildingType.RESTAURANT), city.countPeopleInPlaces(BuildingType.MALL), city.countPeopleInPlaces(BuildingType.PARK)));
            stringBuilder.AppendLine(formatPopulationsHeavy(city.countPeopleInPlaces(BuildingType.FOOD_STORE), city.countPeopleInPlaces(BuildingType.OFFICE), city.countPeopleInPlaces(BuildingType.HOUSE), city.countPeopleInPlaces(BuildingType.HOSPITAL), city.countPeopleInPlaces(BuildingType.GYM), city.countPeopleInPlaces(BuildingType.RESTAURANT), city.countPeopleInPlaces(BuildingType.MALL), city.countPeopleInPlaces(BuildingType.PARK), System.Convert.ToSingle(stoplightControl.isStopLightOn())));
            timer = 0;
        }
    }

    public string formatPopulations(float foodStores, float office, float houses, float hospital, float gym, float restaurants, float malls, float parks)
    {
        string newLine = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", foodStores, office, houses, hospital, gym, restaurants, malls, parks);
        return newLine;
    }

    public string formatPopulationsHeavy(float foodStores, float office, float houses, float hospital, float gym, float restaurants, float malls, float parks, float stopLight)
    {
        string newLine = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", foodStores, office, houses, hospital, gym, restaurants, malls, parks, stopLight);
        return newLine;
    }


    void OnApplicationQuit()
    {
        File.WriteAllText(this.fileName, stringBuilder.ToString());
        Debug.Log("Written city file ");
    }

}
