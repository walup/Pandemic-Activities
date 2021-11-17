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

    //Lista de los modulos de salud de las personas dentro (este realmente solo lo utilizaré para calibrar el número reproductivo)
    private List<HealthStatus> sickPeopleStatusList;
    private List<ProtectionStatus> sickPeopleProtectionStatusList;

    public Place(int maxCapacity, BuildingType type, float placeInteractionFactor)
    {
        this.peopleInside = 0;
        this.sickPeopleInside = 0;
        this.maxCapacity = maxCapacity;
        this.type = type;
        this.placeInteractionFactor = placeInteractionFactor;

        sickPeopleStatusList = new List<HealthStatus>();
        sickPeopleProtectionStatusList = new List<ProtectionStatus>();
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
        return (((float)sickPeopleInside / peopleInside) * placeInteractionFactor);
    } 

    public bool hasSickPeople()
    {
        return sickPeopleInside > 0;
    }

    public int getPeopleInside()
    {
        return peopleInside;
    }


    public void agentEnterAndRegister(bool sick, HealthStatus healthStatus, ProtectionStatus protectionStatus)
    {
        if (sick)
        {
            sickPeopleInside += 1;
            sickPeopleStatusList.Add(healthStatus);
            sickPeopleProtectionStatusList.Add(protectionStatus);

        }
        peopleInside += 1;
    }

    
    public void agentLeaveAndDeregister(bool enteredSick, HealthStatus healthStatus, ProtectionStatus protectionStatus)
    {
        if (enteredSick)
        {
            sickPeopleInside -= 1;
            sickPeopleStatusList.Remove(healthStatus);
            sickPeopleProtectionStatusList.Remove(protectionStatus);
            //Debug.Log("Count registered people " + sickPeopleStatusList.Count);
        }
      
    peopleInside -= 1;
    }

    public void incrementRandomSickPersonTransmissionCount()
     {
         int index = Random.Range(0, sickPeopleStatusList.Count - 1);
        sickPeopleStatusList[index].increaseNumberOfAgentsInfected();

     }

    public ProtectionStatus getRandomSickPeopleProtectionStatus()
    {
        int randIndex = Random.Range(0, this.sickPeopleProtectionStatusList.Count - 1);
        return sickPeopleProtectionStatusList[randIndex];

    }
}
