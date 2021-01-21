using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;

public class ClockList : MonoBehaviour
{
    [Header("Inspector References")]
    public Button addNewButton;
    public Button deleteAllButton;
    public GameObject clockPrefab;

    [Header("Data")]
    public List<Clock> activeClocks;
    public DateTime utcDateTime { get { return DateTime.UtcNow; } }
    public List<TimeZoneInfo> timezones;
    public List<Dropdown.OptionData> dropdownData;

    private void Awake()
    {
        RegisterListeners();
        CheckForClocks();
        timezones = GetTimeZones();
        dropdownData = TimezonesToDropdown();
    }

    /// <summary>
    /// Registers any event/onClick listeners required by this ClockList.
    /// </summary>
    private void RegisterListeners()
    {
        addNewButton.onClick.AddListener(AddClock);
        deleteAllButton.onClick.AddListener(DeleteAll);
    }

    /// <summary>
    /// Checks if any clocks have already been added and adds them to the tracker list.
    /// </summary>
    private void CheckForClocks()
    {
        Clock[] initialClocks = GetComponentsInChildren<Clock>();
        activeClocks = new List<Clock>(initialClocks);
    }

    /// <summary>
    /// Checks the list type and spawns a new instance of the appropriate clock.
    /// </summary>
    private void AddClock()
    {
        GameObject instancedObject = Instantiate(clockPrefab, transform);
        activeClocks.Add(instancedObject.GetComponent<Clock>());
    }

    /// <summary>
    /// Removes all objects from the list.
    /// </summary>
    private void DeleteAll()
    {
        foreach(Clock child in activeClocks.ToArray())
        {
            activeClocks.Remove(child);
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Gets a list of timezones from the system registry and sorts them by UTC and location.
    /// </summary>
    /// <returns>A properly sorted list of UTC timezones.</returns>
    private List<TimeZoneInfo> GetTimeZones()
    {
        List<TimeZoneInfo> systemTimeZones = new List<TimeZoneInfo>(TimeZoneInfo.GetSystemTimeZones());
        systemTimeZones.Sort(delegate (TimeZoneInfo left, TimeZoneInfo right) {
            int comparison = left.BaseUtcOffset.CompareTo(right.BaseUtcOffset);
            return comparison == 0 ? string.CompareOrdinal(left.DisplayName, right.DisplayName) : comparison;
        });
        return systemTimeZones;
    }

    /// <summary>
    /// Converts the UTC timezone list into a list of dropdown options which can be used by multiple clocks.
    /// </summary>
    /// <returns>All system UTC timezones as a list of dropdown option data.</returns>
    private List<Dropdown.OptionData> TimezonesToDropdown()
    {
        List<Dropdown.OptionData> output = new List<Dropdown.OptionData>();

        foreach (TimeZoneInfo timezone in timezones)
        {
            Dropdown.OptionData option = new Dropdown.OptionData(timezone.DisplayName);
            output.Add(option);
        }

        return output;
    }
}
