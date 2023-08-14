using System;

// Ensure both enums below are in sync when adding or
// removing method types
//
// An assertion is in place at `GameChecks.cs`'s `Start`

// Used for keeping track of actions tied to
// a particular `StateMessageMethod`
public enum StateMessageMethod
{
    Enter,

    // Update message methods
    Update,
    FixedUpdate,
    LateUpdate,

    Exit,
}

// Used for binding actions to multiple
// `StateMessageMethod`s at once
[Flags]
public enum StateMessageMethodFlag
{
    Enter = 1 << 0,

    // Update message methods
    Update = 1 << 1,
    FixedUpdate = 1 << 2,
    LateUpdate = 1 << 3,

    Exit = 1 << 4,
}
