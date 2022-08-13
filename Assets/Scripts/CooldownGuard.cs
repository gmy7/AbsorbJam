using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownGuard 
{
    public bool actionReady = true;
    //Allows for callbacks once the duration of the cooldown is over;
    private bool durationOver = true;
    public bool DurationOver
    {
        get
        {
            return durationOver;
        }
        set
        {
            durationOver = value;
        }
    }
}
