using System;

using UnityEngine;
using UnityEngine.Assertions;

// Helper static class with a bunch of convenience
// functions for testing and debugging
public static class DebugUtils
{
    private static bool _enableLog = true;

    public static void EnableLog()
    {
        _enableLog = true;
    }

    public static void DisableLog()
    {
        _enableLog = false;
    }

    public static void Log(string message)
    {
        if (_enableLog)
        {
            Debug.Log(message);
        }
    }

    public static void Log(int number)
    {
        Log(number.ToString());
    }

    public static void Assert(bool condition, string message)
    {
        InternalAssert(condition, message);
    }

    // Check whether an assertion holds true after `delay`
    // amount of seconds
    public static void DelayAssert(Func<bool> condition,
        float delay, string message)
    {
        _ = Delay.Execute(() =>
        {
            InternalAssert(condition(), message);
        }, delay);
    }

    // A fatal path of code execution was taken
    public static void Fatal(string message)
    {
        throw new AssertionException(message, message);
    }

    private static void InternalAssert(bool condition, string message)
    {
        UnityEngine.Assertions.Assert.IsTrue(condition, message);
    }
}
