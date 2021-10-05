using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{

    private float SECONDS_PER_HOUR = 0.7f;
    public static int hour;
    public static float hourDelta;
    public static float dayVal;
    private float deltaTime;
    private float secondCounter;
    private SpriteRenderer renderer;
    [SerializeField] private Sprite face1Clock;
    [SerializeField] private Sprite face2Clock;
    [SerializeField] private Sprite face3Clock;
    [SerializeField] private Sprite face4Clock;
    private int faceCounter;
    private Sprite[] spriteList;
    private Light sun;
    private bool day;
    // Start is called before the first frame update
    void Start()
    {
        sun = GameObject.Find("Sun").GetComponent<Light>();
        renderer = this.GetComponent<SpriteRenderer>();
        secondCounter = 0;
        faceCounter = 2;
        renderer.sprite = face3Clock;
        hour = 6;
        spriteList = new Sprite[4] { face1Clock, face2Clock, face3Clock, face4Clock};
        dayVal = 0;
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime = Time.deltaTime;
        hourDelta = deltaTime *(1/SECONDS_PER_HOUR);
        secondCounter += deltaTime;
        if (secondCounter >= SECONDS_PER_HOUR)
        {
            secondCounter = 0;
            hour += 1;
            hour = hour % 24;
            //Actualizamos la caráctula del reloj si es necesario
            if(hour % 3 == 0)
            {
                faceCounter += 1;
                faceCounter = faceCounter % 4;
                this.renderer.sprite = spriteList[faceCounter];
            }
            if(hour == 0)
            {
                dayVal += 1;
            }

            if(day && (hour >= 0 && hour <= 6) || (hour >= 22 && hour <= 24))
            {
                day = false;
                sun.intensity = 0.5f;
            }
            else if(!day && hour >= 7 && hour <= 22)
            {
                day = true;
                sun.intensity = 1f;
            }
        }
    }

    public static float nfmod(float a, float b)
    {
        return (float)(a - b * Mathf.Floor(a / b));
    }

    public static int getHourDistance(int hour1, int hour2)
    {
        return (int)nfmod(hour2 - hour1, 24);
    }

    public static bool isNight()
    {
        return (hour > 0 && hour <= 6) || (hour >= 22 && hour <= 24);
    }

    public static float[] recalibrateProbabilities(float alpha, float beta, float beta1, float gamma, float gamma1, float omega, float omega1, float omega2)
    {
        float[] newProbabilities = new float[8];
        //Actualmente las probabilidades están por dia, sin embargo nosotros las queremos por ciclo
        newProbabilities[0] = alpha * (1 / 24f) * Clock.hourDelta;
        newProbabilities[1] = beta * (1 / 24f) * Clock.hourDelta;
        newProbabilities[2] = beta1;
        newProbabilities[3] = gamma * (1 / 24f) * Clock.hourDelta;
        newProbabilities[4] = gamma1;
        newProbabilities[5] = omega * (1 / 24f) * Clock.hourDelta;
        newProbabilities[6] = omega1;
        newProbabilities[7] = omega2;
        return newProbabilities;
    }
}
