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

    public void updateHealthState(bool contact,float alpha, float beta, float beta1, float gamma, float gamma1, float omega, float omega1, float omega2)
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
                }
                break;

            case HealthState.SYMPTOMATICALLY_INFECTED:
                float symptomaticThrow = Random.value;
                if(symptomaticThrow < omega)
                {

                    symptomaticThrow = Random.value;
                    if(symptomaticThrow < omega1)
                    {
                        this.healthState = HealthState.RECOVERED;
                        renderer.sprite = recoveredSprite;
                    }
                    else if (symptomaticThrow >= omega1 && symptomaticThrow <omega1 + omega2 )
                    {
                        this.healthState = HealthState.DEAD;
                        renderer.sprite = deceasedSprite;
                    }
                    else
                    {
                        this.healthState = HealthState.SUSCEPTIBLE;
                        renderer.sprite = susceptibleSprite;
                    }
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
