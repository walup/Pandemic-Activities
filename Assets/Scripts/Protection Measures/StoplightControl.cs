using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoplightControl : MonoBehaviour
{

    //Boolean indicates if the stoplight is on or not
    private bool stoplightOn;
    //The Stoplight Sprites
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;

    public static float OCCUPATION_THRESHOLD = 0.035f;
    //Dias que estará prendido el semáforo después de que baje la ocupación hospitalaria
    private int DAYS_STOPLIGHT = 20;
    private float dayStart;
    private bool endedIsolation = false;
    [SerializeField] public static float ISOLATION_RADIUS = 0;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(stoplightOn && Clock.dayVal - dayStart > DAYS_STOPLIGHT)
        {
            Debug.Log("Ended isolation");
            this.stoplightOn = false;
            this.endedIsolation = true;
            dayStart = 0;
        }
    }

    public bool isStopLightOn()
    {
        return stoplightOn;
    }

    public void setStopLight(bool stopValue)
    {
        this.stoplightOn = stopValue;

        if(stopValue == true)
        {
            this.GetComponent<SpriteRenderer>().sprite = onSprite;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().sprite = offSprite;
        }
    }

    public void setDayStart(float dayStart)
    {
        this.dayStart = dayStart;
    }

    public bool stoppedIsolation()
    {
        return endedIsolation;
    }

    public void setEndedIsolation(bool endedIsolation)
    {
        this.endedIsolation = endedIsolation;
    }



     

}
