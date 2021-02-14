using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockManager : MonoBehaviour
{
    public static ClockManager instance;

    [Header("Inspector References"), SerializeField]
    private Transform viewportContent;
    [SerializeField]
    private Button addNewButton;
    [SerializeField]
    private Button deleteAllButton;
    [SerializeField]
    private Button exitButton;
    [SerializeField]
    private GameObject clockPrefab;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip exitSound;

    [Header("Data")]
    public List<TimeZoneInfo> timezones;
    public List<Dropdown.OptionData> dropdownData;
    [HideInInspector]
    public int defaultTimeZone;
    public int activeClockCount { get { return activeClocks.Count; } }
    public DateTime utcDateTime { get { return DateTime.UtcNow; } }
    [SerializeField]
    private int initialPooledClocks;
    [SerializeField]
    private List<Clock> activeClocks;
    [SerializeField]
    private List<Clock> pooledClocks;
    

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
        CheckForEditorClocks();
        timezones = GetTimeZones();
        dropdownData = TimezonesToDropdown();
        SetDefaultTimeZone();
    }

    private void Start()
    {
        SpawnInitialPooledClocks();
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
    private void CheckForEditorClocks()
    {
        Clock[] initialClocks = viewportContent.gameObject.GetComponentsInChildren<Clock>();
        activeClocks = new List<Clock>(initialClocks);
    }

    /// <summary>
    /// Spawns the inspector-defined number of clocks and adds them to the pool.
    /// </summary>
    private void SpawnInitialPooledClocks()
    {
        if(initialPooledClocks > 0)
        {
            for (int i = 0; i < initialPooledClocks; i++)
            {
                InstantiateClock(false);
            }
        }
    }

    /// <summary>
    /// Looks for 'Cen. Australia Standard Time' and checks this against the list of obtained timezones. If it exists, it gets its index and sets this as the application's default timezone.
    /// </summary>
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

        if(pooledClocks.Count > 0)
        {
            Clock clock = GetPooledClock();
            pooledClocks.Remove(clock);
            activeClocks.Add(clock);
            clock.gameObject.SetActive(true);
        }
        else
        {
            InstantiateClock(true);
        }
    }

    /// <summary>
    /// Instantiates a new clock.
    /// </summary>
    /// <param name="isActive">If true, adds the clock to the list of active clocks and sets it active immediately. If false, adds the clock to the pooled clocks and sets inactive.</param>
    /// <returns></returns>
    private Clock InstantiateClock(bool isActive)
    {
        GameObject instancedObject = Instantiate(clockPrefab, viewportContent);
        Clock instancedClock = instancedObject.GetComponent<Clock>();
        
        if(isActive)
        {
            activeClocks.Add(instancedClock);
            instancedObject.SetActive(true);
        }
        else
        {
            pooledClocks.Add(instancedClock);
            instancedObject.SetActive(false);
        }

        return instancedClock;        
    }

    /// <summary>
    /// Returns the pooled clock at index 0 of the pooled clocks list.
    /// </summary>
    private Clock GetPooledClock()
    {
        return pooledClocks[0];
    }

    /// <summary>
    /// Resets the clock, moves it into the pooled clocks list, and sets it inactive.
    /// </summary>
    /// <param name="clock">The clock being 'deleted'.</param>
    public void DeleteClock(Clock clock)
    {
        clock.timerModePanel.ResetClock();
        clock.stopwatchModePanel.ResetClock();
        activeClocks.Remove(clock);
        pooledClocks.Add(clock);
        clock.gameObject.SetActive(false);
    }

    /// <summary>
    /// Gets all active clocks and calls the DeleteClock function for each. All of the clocks will be reset, sent back to the clock pool and set inactive.
    /// </summary>
    private void DeleteAll()
    {
        Clock[] allClocks = activeClocks.ToArray();

        for (int i = 1; i < allClocks.Length; i++)
        {
            Clock thisClock = allClocks[i];
            DeleteClock(thisClock);
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
