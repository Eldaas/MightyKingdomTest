using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockPanel : MonoBehaviour
{
    [Header("Inherited Inspector References")]
    public Clock parentClock;

    [Header("Inherited Data")]
    public bool isRunning = false;

    protected virtual void Awake(){}

    protected virtual void Start(){}

    /// <summary>
    /// Overriden by the children subclasses and used to register any event/button onClick listeners.
    /// </summary>
    protected virtual void RegisterListeners(){}

    /// <summary>
    /// Overridden by the children subclasses and used to initialise clocks with any initial values.
    /// </summary>
    protected virtual void Initialise(){}

    /// <summary>
    /// Overridden by the children subclasses and used within the clock state update loop to update the UI upon each frame.
    /// </summary>
    public virtual void UpdateUI(){}

    protected virtual void StartTimer()
    {
        isRunning = true;
    }

    protected virtual void StopTimer()
    {
        isRunning = false;
    }

    /// <summary>
    /// Resets the clock back to its default state.
    /// </summary>
    public virtual void ResetClock()
    {
        StopTimer();
    }
}
