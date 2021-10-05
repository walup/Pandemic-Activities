using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlaceFactory
{

    public static Place buildPlaceSkeleton(BuildingType type, int cityPopulation)
    {
        int maxCapacity = 0;
        switch (type)
        {
            case BuildingType.FOOD_STORE:
                maxCapacity = (int)(0.4* cityPopulation);
                return new Place(maxCapacity, type,0.8f);
            case BuildingType.OFFICE:
                maxCapacity = (int)(0.5 * cityPopulation);
                return new Place(maxCapacity, type, 0.2f);
            case BuildingType.GYM:
                maxCapacity = (int)(0.2 * cityPopulation);
                return new Place(maxCapacity, type, 0.2f);
            case BuildingType.HOSPITAL:
                maxCapacity = (int)(1 * cityPopulation);
                return new Place(maxCapacity, type, 0.1f);
            case BuildingType.PARK:
                maxCapacity = (int)(0.2 * cityPopulation);
                return new Place(maxCapacity, type, 0.3f);
            case BuildingType.MALL:
                maxCapacity = (int)(0.4 * cityPopulation);
                return new Place(maxCapacity, type, 0.8f);
            case BuildingType.HOUSE:
                maxCapacity = 2;
                return new Place(maxCapacity, type, 1);
            case BuildingType.RESTAURANT:
                maxCapacity = (int)(0.2*cityPopulation);
                return new Place(maxCapacity, type, 0.3f);
        }
        return null;
    }
}
