using System;
using System.IO;
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
    private SaveLocation _saveLocation;

    [SerializeField]
    [ShowIf("_saveLocation", SaveLocation.Custom)]
    private string _saveDirectory;

    [SerializeField]
    private string _saveFileName;

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

    public void SaveAll()
    {
        foreach (var pair in _callbacks)
        {
            Func<string> save = pair.Value.First;
            _saveDict[pair.Key] = save();
        }

        File.WriteAllText(GetSaveLocation(),
            JsonConvert.SerializeObject(_saveDict));
    }

    public void LoadAll()
    {
        string saveLocation = GetSaveLocation();

        bool exists = File.Exists(saveLocation);

        // Save file does not exist, no loading is required
        if (!exists)
        {
            return;
        }

        string loadSaveString = File.ReadAllText(saveLocation);
        Dictionary<string, string> saveDict =
            JsonConvert.DeserializeObject
            <Dictionary<string, string>>(loadSaveString);
        _saveDict = saveDict;

        foreach (var pair in _callbacks)
        {
            Action<string> load = pair.Value.Second;

            load(_saveDict[pair.Key]);
        }
    }

    private void Start()
    {
        LoadAll();
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
