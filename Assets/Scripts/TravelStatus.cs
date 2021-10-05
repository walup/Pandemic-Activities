using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelStatus : MonoBehaviour
{
    private bool onTheMove;
    private Vector3 destination;
    private Vector3 velocity;
    private float RADIUS2 =Mathf.Pow(0.9f,2);
    private Place place;
    private bool enteredSick;


    void Start()
    {
        onTheMove = false;
        enteredSick = false;
    }

    void Update()
    {
        if (isOnTheMove() && GetComponent<HealthStatus>().isAlive())
        {
            transform.position = transform.position + velocity * Clock.hourDelta;
            //Si ya lleg� a su destino el agente 
            if ((transform.position - this.destination).sqrMagnitude < RADIUS2)
            {
                //Debug.Log("reached " + transform.position);
                HealthStatus health = GetComponent<HealthStatus>();
                setMoving(false);
                enteredSick = health.isContagious();
                this.place.agentEnter(enteredSick);
            }
        }
    }

    public bool isOnTheMove()
    {
        return onTheMove;
    }

    public void setMoving(bool moving)
    {
        onTheMove = moving;
    }

    public void setVelocity(Vector2 velocity)
    {
        this.velocity = velocity;
    }

    public void moveAgent(Place place)
    {
        if (this.place != null && !isOnTheMove())
        {
            this.place.agentLeave(enteredSick);
        }
        this.place = place;
        this.destination = place.getPlacePosition();
        //Debug.Log("New destination " + this.destination);
        setMoving(true);
    }

    public Place getPlace()
    {
        return this.place;
    }

}
