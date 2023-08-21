using UnityEditor;

using static DebugUtils;

public static class AssetAutoRefreshController
{
    // Use a unified string to guarantee all `GetInt` and `SetInt` calls
    // are called using same key
    private static readonly string _autoRefreshKey = "kAutoRefreshMode";

    private static int GetAssetAutoRefreshValue()
    {
        int currentState = EditorPrefs.GetInt(_autoRefreshKey, -1);

        // Ensure that the key supplied above exists and only
        // contains the valid values [0, 1]
        Assert(currentState is >= 0 and <= 1,
            "`_autoRefreshKey` key is invalid");

        return currentState;
    }

    [MenuItem("Utils/Asset Auto Refresh")]
    private static void ToggleAssetAutoRefresh()
    {
        int currentState = GetAssetAutoRefreshValue();

        // Toggle the asset auto refresh editor preference
        EditorPrefs.SetInt(_autoRefreshKey, currentState == 1 ? 0 : 1);
    }

    [MenuItem("Utils/Asset Auto Refresh", true)]
    private static bool ValidateAssetAutoRefresh()
    {
        // Set the check status to the current value
        // Compare value with more than 0 to convert to bool
        Menu.SetChecked("Utils/Asset Auto Refresh",
            GetAssetAutoRefreshValue() > 0);

        return true;
    }
}
