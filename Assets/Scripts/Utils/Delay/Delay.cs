using System;
using System.Collections;
using System.Threading.Tasks;

using UnityEngine;

// Static class with a bunch of helper functions that
// allow for delayed execution
public static class Delay
{
    // Add force to `rigidbody2d` using mode `mode` after `delay`
    // (in seconds)
    public static void DelayAddForce(this MonoBehaviour monoBehaviour,
        Rigidbody2D rigidbody2d, Vector2 force, ForceMode2D mode,
        float delay)
    {
        monoBehaviour.StartCoroutine(
            InternalDelayAddForce(rigidbody2d, force,
            mode, delay));
    }

    // Convenience helper function to prevent the need to construct
    // a `Vector2` directly as an argument
    //
    // Add force to `rigidbody2d` using mode `mode` after `delay`
    // (in seconds)
    public static void DelayAddForce(this MonoBehaviour monoBehaviour,
        Rigidbody2D rigidbody2d, float xForce, float yForce,
        ForceMode2D mode, float delay)
    {
        DelayAddForce(monoBehaviour, rigidbody2d,
            new Vector2(xForce, yForce), mode, delay);
    }

    // Add force to `rigidbody2d` using mode `Impulse` after `delay`
    // (in seconds)
    public static void DelayAddForce(this MonoBehaviour monoBehaviour,
        Rigidbody2D rigidbody2d, Vector2 force, float delay)
    {
        monoBehaviour.StartCoroutine(
            InternalDelayAddForce(rigidbody2d, force,
            ForceMode2D.Impulse, delay));
    }

    // Convenience helper function to prevent the need to construct
    // a `Vector2` directly as an argument
    //
    // Add force to `rigidbody2d` using mode `Impulse` after `delay`
    // (in seconds)
    public static void DelayAddForce(this MonoBehaviour monoBehaviour,
        Rigidbody2D rigidbody2d, float xForce, float yForce,
        float delay)
    {
        DelayAddForce(monoBehaviour, rigidbody2d,
            new Vector2(xForce, yForce), delay);
    }

    // Execute an action after `delay` (in seconds) without
    // creating another coroutine
    public static void DelayExecute(this MonoBehaviour monoBehaviour,
        Action action, float delay)
    {
        monoBehaviour.StartCoroutine(
            InternalDelayExecute(action, delay));
    }

    // For usage on types that are not derived from `MonoBehaviour`
    // and thus cannot use `StartCoroutine`
    public static async Task Execute(Action action,
        float delay)
    {
        await Task.Delay((int)(delay * 1000f));
        action();
    }

    private static IEnumerator InternalDelayExecute(Action action,
        float delay)
    {
        yield return new WaitForSeconds(delay);

        action();
    }

    private static IEnumerator InternalDelayAddForce(
        Rigidbody2D rigidbody2d, Vector2 force, ForceMode2D mode,
        float delay)
    {
        yield return new WaitForSeconds(delay);

        rigidbody2d.AddForce(force, mode);
    }
}
