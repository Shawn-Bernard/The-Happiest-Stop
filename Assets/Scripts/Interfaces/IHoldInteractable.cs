using System;
using UnityEngine;

public interface IHoldInteractable 
{
    void HoldInteractCompleted();

    void HoldInteractFailed();

    public float HoldTime {  get;}
}