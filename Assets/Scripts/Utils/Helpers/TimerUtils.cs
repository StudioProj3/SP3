using System;

using static DebugUtils;

public static class TimerUtils
{
    private static long timestamp = 0;

    // Returns time in seconds since the last `DT` call,
    // if this is the first call, it returns `0.0`
    public static double DT()
    {
        if (timestamp == 0)
        {
            timestamp = DateTime.Now.ToFileTime();

            return 0.0;
        }

        long now = DateTime.Now.ToFileTime();
        long elapsed = now - timestamp;

        timestamp = now;

        return elapsed / (1_000_000_000.0 / 100.0);
    }

    public static void DTLog(string text = null)
    {
        Log("TimerUtils.dt [" + (text ?? "-") +
            "]: " + DT() + "s");
    }
}
