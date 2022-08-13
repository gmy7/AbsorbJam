using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownGuard 
{
    //Allows for callbacks once the duration of the cooldown is over;
    public delegate void DurationEnded();
    public DurationEnded durationEnded;
    public bool actionReady = true;
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
            if(value == true)
            {
                durationEnded();
            }
        }
    }
}
