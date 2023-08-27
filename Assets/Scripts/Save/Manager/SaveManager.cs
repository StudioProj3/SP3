using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json;

using UnityEngine;

using static DebugUtils;

public class SaveManager : Singleton<SaveManager>
{
    public enum SaveLocation
    {
        CurrentDirectory,
        Custom,
    }

    [HorizontalDivider]
    [Header("Save Parameters")]

    [SerializeField]
    [Range(0f, 60f)]
    [Tooltip("Time in seconds for auto save")]
    private float _autoSaveFrequency = 30f;

    [HorizontalDivider]
    [Header("Save Location Parameters")]

    [SerializeField]
    private SaveLocation _saveLocation;

    [SerializeField]
    [ShowIf("_saveLocation", SaveLocation.Custom)]
    private string _saveDirectory;

    [SerializeField]
    private string _saveFileName;

    [HorizontalDivider]
    [Header("Event Hooks")]

    [SerializeField]
    private List<ScriptableObject> _toHooks;

#if UNITY_EDITOR

    [HorizontalDivider]
    [Header("Editor Override Parameters")]

    [SerializeField]
    private string _editorDirectoryOverride;

#endif

    // Maps `SaveID` to a `Pair` of `Save` and `Load` callbacks
    private Dictionary<string, Pair<Func<string>, Action<string>>>
        _callbacks = new();
    private Dictionary<string, string> _saveDict = new();

    public void Hook(string saveID, Func<string> saveCallback,
        Action<string> loadCallback)
    {
        Assert(saveCallback != null,
            "`saveCallback` should not be null");
        Assert(loadCallback != null,
            "`loadCallback` should not be null");

        Assert(!_callbacks.ContainsKey(saveID),
            "`saveID` already exist");

        _callbacks.Add(saveID, new(saveCallback, loadCallback));
    }

    public void Save(string saveID)
    {
        bool result = _callbacks.ContainsKey(saveID);

        Assert(result, "key not found");

        _saveDict[saveID] = _callbacks[saveID].First();

        WriteToDisk();
    }

    public void SaveAll()
    {
        foreach (var pair in _callbacks)
        {
            string saveID = pair.Key;

            Func<string> save = pair.Value.First;
            _saveDict[saveID] = save();
        }

        WriteToDisk();
    }

    // Also handles case where a new key value pair needs
    // to be added to the local save file of the user
    public void LoadAll()
    {
        string saveLocation = GetSaveLocation();

        bool exists = File.Exists(saveLocation);

        // Save file does not exist, no loading is required
        if (!exists)
        {
            // Save the initial state of all savables
            SaveAll();
            return;
        }

        string loadSaveString = File.ReadAllText(saveLocation);
        Dictionary<string, string> saveDict =
            JsonConvert.DeserializeObject
            <Dictionary<string, string>>(loadSaveString);

        _saveDict = saveDict;

        foreach (var pair in _callbacks)
        {
            string saveID = pair.Key;

            Action<string> load = pair.Value.Second;

            bool result = _saveDict.TryGetValue(
                saveID, out string saveString);

            // This is a new key value pair and is currently
            // not in the load save file of the user
            if (!result)
            {
                // Save a fresh initial state to disk
                Save(saveID);

                continue;
            }

            load(saveString);
        }
    }

    protected override void OnStart()
    {
        StartCoroutine(Init());
    }

    private void WriteToDisk()
    {
        File.WriteAllText(GetSaveLocation(),
            JsonConvert.SerializeObject(_saveDict));
    }

    private IEnumerator Init()
    {
        yield return null;
        LoadAll();

        // The individual savables already call save when they are
        // changed this is just as a fallback but technically should
        // be fully functional without
        InvokeRepeating(nameof(SaveAll), 0f, _autoSaveFrequency);
    }

    private void Awake()
    {
        foreach (ScriptableObject obj in _toHooks)
        {
            if (obj is ISavable savable)
            {
                savable.HookEvents();

                if (obj is IResettable resettable)
                {
                    resettable.ResetAll();
                }
            }
        }
    }

    private string GetSaveLocation()
    {
        string saveDirectory = "";

        switch (_saveLocation)
        {
            case SaveLocation.CurrentDirectory:
                saveDirectory = AppDomain.CurrentDomain.BaseDirectory +
                    @"\" + _saveFileName;
                break;

            case SaveLocation.Custom:
                saveDirectory = _saveDirectory + @"\" + _saveFileName;
                break;

            default:
                Fatal("Unhandled `SaveLocation` type");
                break;
        }

#if UNITY_EDITOR

        saveDirectory = _editorDirectoryOverride + @"\" + _saveFileName;

#endif

        return saveDirectory;
    }
}
