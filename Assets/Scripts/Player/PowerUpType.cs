using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PowerUpType : ScriptableObject
{
    public Gesture Gesture;
    public Image arrow;
    public string arrowText;
    public GameObject Effect;
    public float Duration;

    public abstract void Execute();
}
