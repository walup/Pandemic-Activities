using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place
{

    private BuildingType type;
    private int maxCapacity;
    private int peopleInside;
    private int sickPeopleInside;
    private float placeX;
    private float placeY;
    //Con que fracción de personas interactua cada una cada agente
    private float placeInteractionFactor;

    // Start is called before the first frame update

    public Place(int maxCapacity, BuildingType type, float placeInteractionFactor)
    {
        this.peopleInside = 0;
        this.sickPeopleInside = 0;
        this.maxCapacity = maxCapacity;
        this.type = type;
        this.placeInteractionFactor = placeInteractionFactor;
    }

    public void setPosition(float x, float y)
    {
        this.placeX = x;
        this.placeY = y;
    }

    public BuildingType getType()
    {
        return this.type;
    }

    public int getMaxCapacity()
    {
        return maxCapacity;
    }

    public bool isPlaceFull()
    {
        return peopleInside >= maxCapacity;
    }

    public void agentEnter(bool sick)
    {
        if (sick)
        {
            sickPeopleInside += 1;
        }
        peopleInside += 1;
    }

    public void agentLeave(bool enteredSick)
    {
        if (enteredSick)
        {
            sickPeopleInside -= 1;
        }
        peopleInside -= 1;
    }

    public Vector2 getPlacePosition()
    {
        return new Vector2(placeX, placeY);
    }

    public void setPlaceInteractionFactor(float interactionFactor)
    {
        this.placeInteractionFactor = interactionFactor;
    }

    public float getPlaceInteractionFactor()
    {
        return this.placeInteractionFactor;
    }

    public float getSickPeopleInside()
    {
        return sickPeopleInside;
    }

    public float getDiseaseInteractionProbability()
    {
        return ((sickPeopleInside / peopleInside) * placeInteractionFactor);
    } 

    public bool hasSickPeople()
    {
        return sickPeopleInside > 0;
    }

    public int getPeopleInside()
    {
        return peopleInside;
    }
}
