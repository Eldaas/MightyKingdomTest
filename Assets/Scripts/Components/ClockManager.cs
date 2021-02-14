using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockManager : MonoBehaviour
{
    public static ClockManager instance;

    [Header("Inspector References")]
    public Transform viewportContent;
    public Button addNewButton;
    public Button deleteAllButton;
    public Button exitButton;
    public GameObject clockPrefab;
    public AudioSource audioSource;
    public AudioClip exitSound;

    [Header("Data")]
    public List<Clock> activeClocks;
    public DateTime utcDateTime { get { return DateTime.UtcNow; } }
    public List<TimeZoneInfo> timezones;
    public List<Dropdown.OptionData> dropdownData;
    public int defaultTimeZone;

    private void Awake()
    {
        #region Singleton

        if(GameObject.FindObjectsOfType<ClockManager>().Length > 1)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        #endregion

        RegisterListeners();
        CheckForClocks();
        timezones = GetTimeZones();
        dropdownData = TimezonesToDropdown();
        SetDefaultTimeZone();
    }

    private void Update()
    {
        ButtonValidation();
    }

    /// <summary>
    /// Registers any event/onClick listeners required by this ClockManager.
    /// </summary>
    private void RegisterListeners()
    {
        addNewButton.onClick.AddListener(AddClock);
        deleteAllButton.onClick.AddListener(DeleteAll);
        exitButton.onClick.AddListener(ExitApplication);
    }

    /// <summary>
    /// Checks if any clocks have already been added and adds them to the tracker list.
    /// </summary>
    private void CheckForClocks()
    {
        Clock[] initialClocks = viewportContent.gameObject.GetComponentsInChildren<Clock>();
        activeClocks = new List<Clock>(initialClocks);
    }

    private void SetDefaultTimeZone()
    {
        TimeZoneInfo adelaide = TimeZoneInfo.FindSystemTimeZoneById("Cen. Australia Standard Time");

        if (timezones.Contains(adelaide))
        {
            defaultTimeZone = timezones.IndexOf(adelaide);
        }
        else
        {
            Debug.LogError("ClockManager => The default assigned timezone was not found on this system.");
        }
    }

    /// <summary>
    /// Checks the list type and spawns a new instance of the appropriate clock.
    /// </summary>
    private void AddClock()
    {
        GameObject instancedObject = Instantiate(clockPrefab, viewportContent);
        activeClocks.Add(instancedObject.GetComponent<Clock>());
    }

    /// <summary>
    /// Removes all objects from the list.
    /// </summary>
    private void DeleteAll()
    {
        Clock[] allClocks = activeClocks.ToArray();

        for (int i = 1; i < allClocks.Length; i++)
        {
            Clock thisClock = allClocks[i];
            activeClocks.Remove(thisClock);
            Destroy(thisClock.gameObject);
        }
    }

    private void ExitApplication()
    {
        deleteAllButton.interactable = false;
        audioSource.PlayOneShot(exitSound);
        Invoke("Quit", 3f);
    }

    private void Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Performs a validation check to test whether buttons in the scene should be interactable or not.
    /// </summary>
    private void ButtonValidation()
    {
        if(activeClocks.Count < 2)
        {
            deleteAllButton.interactable = false;
        }
        else
        {
            deleteAllButton.interactable = true;
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
