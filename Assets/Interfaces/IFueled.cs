using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFueled
{
    public void AddFuel(float hoursOfFuel);
    public float GetRemainingFuelTime();
    public void TurnOn();
    public void TurnOff();
}
