using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StandardModePanel : ClockPanel
{
    [Header("Inspector References"), SerializeField]
    private TextMeshProUGUI timeText;
    [SerializeField]
    private Dropdown dropdown;
    [SerializeField]
    private Toggle formatToggle;
    
    protected override void Awake()
    {
        base.Awake();
        RegisterListeners();
    }

    protected override void Start()
    {
        base.Start();
        Initialise();
    }

    protected override void Initialise()
    {
        base.Initialise();
        dropdown.AddOptions(ClockManager.instance.dropdownData);
        dropdown.value = ClockManager.instance.defaultTimeZone;
        formatToggle.isOn = false;
    }

    public override void UpdateUI()
    {
        base.UpdateUI();
        timeText.text = CalculateCurrentTime();
    }

    /// <summary>
    /// Gets the current UTC dateTime from the parent list, converts it to the timezone selected in the dropdown menu and formats it as a string relative to 12/24hr time as toggled.
    /// </summary>
    /// <returns>A string representing the current time in the selected timezone, formatted in 12/24hr time.</returns>
    protected string CalculateCurrentTime()
    {
        string output;
        DateTime dateTime = TimeZoneInfo.ConvertTime(ClockManager.instance.utcDateTime, ClockManager.instance.timezones[dropdown.value]);

        if (formatToggle.isOn)
        {
            // 24 hour time is turned on
            output = dateTime.ToString("HH : mm : ss");
        }
        else
        {
            // 24 hour time is turned off
            output = dateTime.ToString("h . mm . ss  tt");
        }

        return output;
    }

}
