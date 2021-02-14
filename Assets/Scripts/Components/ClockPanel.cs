using UnityEngine;

public class ClockPanel : MonoBehaviour
{
    [Header("Inherited Inspector References [ClockPanel]"), SerializeField]
    protected Clock parentClock;

    [Header("Inherited Data [ClockPanel]")]
    protected bool isRunning = false;

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
}
