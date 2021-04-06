using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
    public bool actrive { get; set; } = false;

    public abstract void StartAction();
    public abstract void StopAction();
}
