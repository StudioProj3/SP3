using System;

using UnityEngine.Assertions;

// Contains all information needed to add a particular
// action to a single or multiple corresponding
// `StateMessageMethod` in a `State`
public class ActionEntry
{
    // Since this is a flag enum, this class allows for setting
    // all the different bits in the flag independently
    public StateMessageMethodFlag Method { get; private set; } = 0;
    public Action Action { get; private set; }

    // `Priority` being null means it it not explicitly set
    public uint? Priority { get; private set; }
    
    public ActionEntry(StateMessageMethodFlag method, Action action,
        uint? priority = null)
    {
        Method = method;
        SetActionAndPriority(action, priority);
    }

    public ActionEntry(string method, Action action,
        uint? priority = null)
    {
        AddMethodsFromString(method);
        SetActionAndPriority(action, priority);
    }

    // FIXME: Use overloading for now as all the methods should be
    // the first few arguments and using a `List` is undesirable
    public ActionEntry(string method1, string method2,
        Action action, uint? priority = null)
    {
        AddMethodsFromString(method1, method2);
        SetActionAndPriority(action, priority);
    }

    // FIXME: Use overloading for now as all the methods should be
    // the first few arguments and using a `List` is undesirable
    public ActionEntry(string method1, string method2, string method3,
        Action action, uint? priority = null)
    {
        AddMethodsFromString(method1, method2, method3);
        SetActionAndPriority(action, priority);
    }

    // Ensure that all of `methods` does successfully
    // convert to the underlying enum and add it (bitwise or)
    // to allow for multiple methods to be set at once
    private void AddMethodsFromString(params string[] methods)
    {
        foreach (string method in methods)
        {
            bool result = Enum.TryParse<StateMessageMethodFlag>
                (method, out var flag);

            Assert.IsTrue(result,
                "Invalid method string while parsing `StateMessageMethod`");

            Method |= flag;
        }
    }

    // Simple helper method to set both `Action` and `Priority`
    private void SetActionAndPriority(Action action, uint? priority)
    {
        Action = action;
        Priority = priority;
    }
}
