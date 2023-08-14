using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine.Assertions;

// A `SortedList` containing all actions of a particular
// `StateMessageMethod` ordered by their priorities
using ActionList = System.Collections.Generic.
    SortedList<uint, System.Action>;

// All custom states should inherit from this class
// instead of `StateBase`
public abstract class State<TStateID> :
    StateBase<TStateID>
{
    // Store all delegates with respect to all the
    // message methods and their respective priorities
    private Dictionary<StateMessageMethod, ActionList> _allMethods = new();

    public override void Enter()
    {
        LoopActionsIfKeyExists(StateMessageMethod.Enter);
    }

    public override void Update()
    {
        LoopActionsIfKeyExists(StateMessageMethod.Update);
    }

    public override void FixedUpdate()
    {
        LoopActionsIfKeyExists(StateMessageMethod.FixedUpdate);
    }

    public override void LateUpdate()
    {
        LoopActionsIfKeyExists(StateMessageMethod.LateUpdate);
    }

    public override void Exit()
    {
        LoopActionsIfKeyExists(StateMessageMethod.Exit);
    }

    // Forward the `stateID` to the readonly `StateID`
    // in `StateBase`
    //
    // Constructor that allows adding any amount of
    // action entries during initialization
    protected State(TStateID stateID,
        params ActionEntry[] initActions) :
        base(stateID)
    {
        // Process all `initActions` passed in
        AddAction(initActions);
    }

    // Main `AddAction` method that handles most of the
    // heavy lifting
    //
    // Loop through and handle all entries then bits in `methods`.
    // If the `method` key does not exist in the dictionary,
    // add it. If the key exist but the `list` is null, add it.
    // If `priority` is null, it means that priority should be
    // automatically filled (`1` for a empty `list`, `MAX + 1`
    // otherwise). If the `priority` is not null and already exist
    // use multicast delegation to handle (execution order not
    // guaranteed), else just add it.
    protected void AddAction(params ActionEntry[] entries)
    {
        foreach (ActionEntry entry in entries)
        {
            StateMessageMethodFlag methods = entry.Method;
            uint? priority = entry.Priority;
            Action action = entry.Action;

            // Check and handle any number of bits that
            // are set in `methods`
            foreach (StateMessageMethodFlag flag in
                Enum.GetValues(typeof(StateMessageMethodFlag)))
            {
                // If this particular bit in the flag is not
                // set skip to the next one
                if (!methods.HasFlag(flag))
                {
                    continue;
                }

                // Create a `StateMessageMethod` for usage
                bool result = Enum.TryParse<StateMessageMethod>(flag.ToString(),
                    out var method);
                Assert.IsTrue(result);

                // Check to see if the dictionary `allMethods` has the key
                // `method`
                bool keyExists = _allMethods.TryGetValue(method, out var list);

                // `method` key already exists in the dictionary and `list`
                // is not null
                if (keyExists && list != null)
                {
                    // If `list` is empty, just add entry directly
                    if (!list.Any())
                    {
                        // If `priority` is not null use it, else use `1`
                        _allMethods[method].Add(priority ?? 1, action);
                    }
                    else
                    {
                        // `priority` not explicitly set, use `MAX + 1`
                        if (priority == null)
                        {
                            list.Add(list.Keys.Max() + 1, action);
                        }
                        // `priority` is not null and not unique,
                        // use multicast delegation
                        else if (list.ContainsKey((uint)priority))
                        {
                            list[(uint)priority] += action;
                        }
                        // `priority` is not null and is unique,
                        // add directly
                        else
                        {
                            list.Add((uint)priority, action);
                        }
                    }
                }
                // `method` key already exists in the dictionary and `list`
                // is null
                else if (keyExists && list == null)
                {
                    // If `priority` is not null use it, else use `1`
                    _allMethods[method] = new ActionList
                        { { priority ?? 1, action  } };
                }
                // `method` key does not yet exist, add the key entry to
                // the dictionary with a new `ActionList` that contains
                // the action entry
                else
                {
                    // If `priority` is not null use it, else use `1`
                    _allMethods.Add(method, new ActionList
                        { { priority ?? 1, action } });
                }
            }
        }
    }

    // Convenience helper method to make the call site a
    // little cleaner without the `new`
    protected void AddAction(StateMessageMethodFlag method,
        Action action, uint? priority = null)
    {
        AddAction(new ActionEntry(method, action, priority));
    }

    // Convenience helper method to make the call site a
    // little cleaner without the `new`
    protected void AddAction(string method, Action action,
        uint? priority = null)
    {
        AddAction(new ActionEntry(method, action, priority));
    }

    // Since `priority` is enforced to be unique among all the
    // action entries of a particular `StateMessageMethod`, it can be used here
    // as an ID to remove a action entry from the `ActionList`
    //
    // Returns whether an action entry is successfully removed
    protected bool RemoveAction(StateMessageMethod method, uint priority)
    {
        // Check to see if the dictionary `allMethods` has the key
        // `method`
        bool keyExists = _allMethods.TryGetValue(method, out var list);

        return keyExists && list.Remove(priority);
    }

    // Returns whether the key exists and at least 1 action is completed
    // (i.e. the corresponding `SortedList` is not empty)
    private bool LoopActionsIfKeyExists(StateMessageMethod method)
    {
        // Check to see if the dictionary `allMethods` has the key
        // `method`
        bool keyExists = _allMethods.TryGetValue(method, out var list);

        if (keyExists)
        {
            foreach (var actionPair in list)
            {
                actionPair.Value();
            }

            // Checks that the `SortedList` is not empty
            return list.Count > 0;
        }

        return false;
    }
}

// A helper abstract class `State` with the generic
// argument defaulted to a common type
public abstract class State : State<string>
{
    // Forward the `stateID` to the readonly `StateID`
    // in `StateBase` through `State`
    //
    // Constructor that allows adding any amount of
    // action entries during initialization
    protected State(string stateID,
        params ActionEntry[] initActions) :
        base(stateID, initActions)
    {

    }
}
