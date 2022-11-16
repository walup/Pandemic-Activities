using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthStatus : MonoBehaviour
{

    private HealthState healthState;
    //Los Sprites de los distintos estados de salud
    [SerializeField] private Sprite susceptibleSprite;
    [SerializeField] private Sprite exposedSprite;
    [SerializeField] private Sprite infectedSprite;
    [SerializeField] private Sprite recoveredSprite;
    [SerializeField] private Sprite deceasedSprite;
    

    private SpriteRenderer renderer;
    // Start is called before the first frame update
    private int nOtherAgentsInfected = 0;
    private int nDaysRecovered = 0;
    //Vamos a poner la tasa de perdida de antigenos por dia
    //private static float mu0 = 3f;
    private static float mu0 = 0f;
    //private static float mu0 = 3f;
    private float immuneLossProbability = 0;
    private float immunityFactor = 1;
    //private float saturationPoint = 7f;
    private float saturationPoint = 20f;
    //private float saturationPoint = 5;

    // 7 y 3 normal 
    // 15 y 1.5 aislamiento
    // 15 y 3 distanciamiento social  
    // 15 y 1 vacunación
    // 15 y 3 Mascarilla 
    // 20 y 1 Semáforo 


    void Awake()
    {
        healthState = HealthState.SUSCEPTIBLE;
        renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = susceptibleSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateHealthState(bool contact,float alpha, float beta, float beta1, float gamma, float gamma1, float omega, float omega1, float omega2, float nInfected)
    {
        switch (healthState)
        {
            case HealthState.SUSCEPTIBLE:
                float susceptibleDiceThrow = Random.value;
                if (contact && susceptibleDiceThrow < alpha)
                {
                    this.healthState = HealthState.EXPOSED;
                    renderer.sprite = exposedSprite;
                }
                break;

            case HealthState.EXPOSED:
                float exposedDiceThrow = Random.value;
                if (exposedDiceThrow < beta)
                {
                    exposedDiceThrow = Random.value;
                    if (exposedDiceThrow < beta1)
                    {
                        this.healthState = HealthState.ASYMPTOMATIC_INFECTED;
                        renderer.sprite = infectedSprite;
                    }
                    else
                    {
                        this.healthState = HealthState.SYMPTOMATICALLY_INFECTED;
                        renderer.sprite = infectedSprite;
                    }
                }
                break;
            case HealthState.ASYMPTOMATIC_INFECTED:
                float asymptomaticThrow = Random.value;
                if (asymptomaticThrow < gamma)
                {
                    asymptomaticThrow = Random.value;
                    if(asymptomaticThrow < gamma1)
                    {
                        this.healthState = HealthState.RECOVERED;
                        renderer.sprite = recoveredSprite;
                    }
                    else
                    {
                        this.healthState = HealthState.SUSCEPTIBLE;
                        renderer.sprite = susceptibleSprite;
                    }

                    //Si estamos en el escenario de aislamiento, y el agente se acaba de curar en el hospital, le ordenaremos que vaya a su casa 
                    if(GetComponent<ProtectionStatus>().isIsolating() && GetComponent<TravelStatus>().getPlace().getType() == BuildingType.HOSPITAL)
                    {
                        GetComponent<ActivityStatus>().goHome();
                    }
                }
                break;

            case HealthState.SYMPTOMATICALLY_INFECTED:
                float symptomaticThrow = Random.value;
                if(symptomaticThrow < omega)
                {

                    symptomaticThrow = Random.value;


                    if(symptomaticThrow < omega2 * immunityFactor)
                    {
                        this.healthState = HealthState.DEAD;
                        renderer.sprite = deceasedSprite;
                    }

                    else if(symptomaticThrow >= omega2*immunityFactor && symptomaticThrow < omega1 + omega2 * immunityFactor)
                    {
                        this.healthState = HealthState.RECOVERED;
                        renderer.sprite = recoveredSprite;

                        if (GetComponent<ProtectionStatus>().isIsolating())
                        {
                            GetComponent<ActivityStatus>().goHome();
                        }
                    }
                    else
                    {
                        this.healthState = HealthState.SUSCEPTIBLE;
                        renderer.sprite = susceptibleSprite;
                        if(GetComponent<ProtectionStatus>().isIsolating())
                        {
                            GetComponent<ActivityStatus>().goHome();
                        }
                    }
                }
                break;

            case HealthState.RECOVERED:
                //immuneLossProbability = nInfected * (mu0 / (saturationPoint + nInfected)) * (1 / 24f) * Clock.hourDelta;
                immuneLossProbability = ((Mathf.Pow(nInfected, 2)) * mu0/(saturationPoint + nInfected))*(1/24f)*Clock.hourDelta;
                float recoveredThrow = Random.value;
                if(recoveredThrow < immuneLossProbability)
                {
                    immuneLossProbability = 0;
                    immunityFactor = immunityFactor * 0.5f;
                    this.healthState = HealthState.SUSCEPTIBLE;
                    renderer.sprite = susceptibleSprite;
                    
                }

                break;
            
        }
    }



    public HealthState getHealth()
    {
        return healthState;
    }

    public void setHealthState(HealthState state)
    {
        switch (state)
        {
            case HealthState.SUSCEPTIBLE:
                this.healthState = state;
                renderer.sprite = susceptibleSprite;
                break;
            case HealthState.EXPOSED:
                this.healthState = state;
                renderer.sprite = exposedSprite;
                break;

            case HealthState.ASYMPTOMATIC_INFECTED:
                this.healthState = state;
                renderer.sprite = infectedSprite;
                break;

            case HealthState.SYMPTOMATICALLY_INFECTED:
                this.healthState = state;
                renderer.sprite = infectedSprite;
                break;

            case HealthState.DEAD:
                this.healthState = state;
                renderer.sprite = deceasedSprite;
                break;

            case HealthState.RECOVERED:
                this.healthState = state;
                renderer.sprite = recoveredSprite;
                break;

        }
    }

    public bool isContagious()
    {
        return healthState == HealthState.ASYMPTOMATIC_INFECTED || healthState == HealthState.SYMPTOMATICALLY_INFECTED;
    }

    public bool isSick()
    {
        return healthState == HealthState.SYMPTOMATICALLY_INFECTED;
    }

    public bool isAlive()
    {
        return !(healthState == HealthState.DEAD);
    }

    public int getNumberOfOtherAgentsInfected()
    {
        return nOtherAgentsInfected;
    }

    public void increaseNumberOfAgentsInfected()
    {
        this.nOtherAgentsInfected++;
    }

}
