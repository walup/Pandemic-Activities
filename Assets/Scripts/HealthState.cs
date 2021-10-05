using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HealthState
{
    SUSCEPTIBLE = 0,
    EXPOSED = 1,
    ASYMPTOMATIC_INFECTED = 2,
    SYMPTOMATICALLY_INFECTED = 3,
    RECOVERED = 4,
    DEAD = 5
}
