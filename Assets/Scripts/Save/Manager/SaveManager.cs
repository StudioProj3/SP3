using System;
using System.Collections.Generic;

using static DebugUtils;

public class SaveManager : Singleton<SaveManager>
{
    // Maps `SaveID` to a `Pair` of `Save` and `Load` callbacks
    private Dictionary<string, Pair<Func<string>, Action<string>>>
        _callbacks = new();

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

    private void Start()
    {
        foreach (var pair in _callbacks)
        {
            Pair<Func<string>, Action<string>> callback =
                pair.Value;
            Func<string> save = callback.First;
            Action<string> load = callback.Second;

            // TODO (Cheng Jun): Continue implementation
        }
    }
}
