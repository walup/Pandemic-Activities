using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class City : MonoBehaviour
{
    // Start is called before the first frame update

    private Tilemap buildingTileMap;
    [SerializeField] private List<PlaceTile> placeTiles;
    private Dictionary<TileBase, PlaceTile> placeDataDictionary;
    public static int cityPopulation;
    private static float averageCitySpeed;
    //Va a ser una ciudad pequeña que puedes recorrer en 2 horas.
    private float TIME_TO_MOVE_ACROSS_CITY = 5;

    public ToroidalWorld myWorld;

    //Los diferentes tipos de lugar que tenemos 

    private static Dictionary<BuildingType, List<Place>> cityBuildings;

    private void Awake()
    {
        cityBuildings = new Dictionary<BuildingType, List<Place>>();
        placeDataDictionary = new Dictionary<TileBase, PlaceTile>();

        foreach (PlaceTile pt in placeTiles)
        {
            foreach (TileBase b in pt.tiles)
            {
                placeDataDictionary.Add(b, pt);
            }
        }

        cityBuildings.Add(BuildingType.HOUSE, new List<Place>());
        cityBuildings.Add(BuildingType.FOOD_STORE, new List<Place>());
        cityBuildings.Add(BuildingType.GYM, new List<Place>());
        cityBuildings.Add(BuildingType.HOSPITAL, new List<Place>());
        cityBuildings.Add(BuildingType.MALL, new List<Place>());
        cityBuildings.Add(BuildingType.OFFICE, new List<Place>());
        cityBuildings.Add(BuildingType.PARK, new List<Place>());
        cityBuildings.Add(BuildingType.RESTAURANT, new List<Place>());

        myWorld = GameObject.Find("Main Camera").GetComponent<ToroidalWorld>();
        averageCitySpeed = Mathf.Max(myWorld.getWorldWidth(), myWorld.getWorldHeight()) / TIME_TO_MOVE_ACROSS_CITY;
        buildCityFromTileMap();


    }



 //   void Start()
   // {


     //   myWorld = GameObject.Find("Main Camera").GetComponent<ToroidalWorld>();
       //averageCitySpeed = Mathf.Max(myWorld.getWorldWidth(), myWorld.getWorldHeight()) / TIME_TO_MOVE_ACROSS_CITY;
       // buildCityFromTileMap();
   // }



    private void buildCityFromTileMap()
    {
        //Al comenzar contamos el número de habitantes
        cityPopulation = 0;
        buildingTileMap = GameObject.Find("Buildings").GetComponent<Tilemap>();
        BoundsInt bounds = buildingTileMap.cellBounds;
        TileBase[] allTiles = buildingTileMap.GetTilesBlock(bounds);

        for (int i = 0; i < bounds.size.x; i++)
        {
            for (int j = 0; j < bounds.size.y; j++)
            {
                TileBase tile = allTiles[i + j * bounds.size.x];
                if (tile != null)
                {
                    BuildingType type = placeDataDictionary[tile].type;
                    if (type == BuildingType.HOUSE)
                    {
                        cityPopulation += 1;
                    }
                }
            }
        }

        for (int i = 0; i < bounds.size.x; i++)
        {
            for (int j = 0; j < bounds.size.y; j++)
            {

                TileBase tile = allTiles[i + j * bounds.size.x];
                if (tile != null)
                {
                    PlaceTile placeTile = placeDataDictionary[tile];
                    Place place = PlaceFactory.buildPlaceSkeleton(placeTile.type, cityPopulation);
                    float x = buildingTileMap.cellBounds.xMin + ((buildingTileMap.cellBounds.xMax - buildingTileMap.cellBounds.xMin) / (bounds.size.x)) * i;
                    float y = buildingTileMap.cellBounds.yMin + ((buildingTileMap.cellBounds.yMax - buildingTileMap.cellBounds.yMin) / bounds.size.y) * j;
                    place.setPosition(x,y);
                    cityBuildings[placeTile.type].Add(place);
                }
            }
        }

    }

    public static Place requestAvailablePlace(BuildingType type)
    {
        List<Place> places = cityBuildings[type];
        shuffleArray(places);
        for (int i = 0; i < places.Count; i++)
        {
            if (!places[i].isPlaceFull())
            {
                return places[i];
            }
        }

        return null;
    }

    public static Place requestAvailablePlaceWithRestriction(BuildingType type, Vector2 currentPosition, bool isolated, HealthStatus health)
    {
        if (!isolated || health.isSick())
        {
            return requestAvailablePlace(type);
        }
        else
        {
            List<Place> places = cityBuildings[type];
            shuffleArray(places);
            for(int i = 0; i < places.Count; i++)
            {
                if(! places[i].isPlaceFull() && Vector2.Distance(places[i].getPlacePosition(), currentPosition) < StoplightControl.ISOLATION_RADIUS && places[i].getType() != BuildingType.HOSPITAL)
                {
                    return places[i];
                }
            }
        }
        return null;
    }

    public static void shuffleArray<T>(List<T> l)
    {
        for(int i = 0; i<l.Count; i++)
        {
            int newIndex = Random.Range(0, l.Count);
            T temp = l[i];
            l[i] = l[newIndex];
            l[newIndex] = temp;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    public static float getAverageCitySpeed()
    {
        return averageCitySpeed;
    }

    public float countPeopleInPlaces(BuildingType type)
    {
        int count = 0;
        List<Place> places = cityBuildings[type];
        for (int i = 0; i<places.Count; i++)
        {
            count += places[i].getPeopleInside();
        }

        return count;
    }
 }
