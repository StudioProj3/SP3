using System.Collections.Generic;

using UnityEngine;

using static DebugUtils;

public class CameraFadeTrack : MonoBehaviour
{
    [HorizontalDivider]
    [Header("Sphere Cast Settings")]

    [SerializeField]
    [Range(0.01f, 1f)]
    [Tooltip("Radius of the sphere cast")]
    private float _radius = 0.05f;

    [SerializeField]
    [Range(1f, 100f)]
    [Tooltip("Max distance of the sphere cast")]
    private float _castDistance = 20f;

    [HorizontalDivider]
    [Header("Transition Settings")]

    [SerializeField]
    [Range(0.01f, 1f)]
    [Tooltip("Time (in seconds) it takes to go from " +
        "opaque to translucent and vice versa")]
    private float _duration = 0.3f;

    [SerializeField]
    [Range(0f, 1f)]
    [Tooltip("Final lowest opacity reached")]
    private float _lowOpacity = 0.55f;

    private Camera _camera;
    private Transform _player;

    // Stores dictionary of active objects involved in the
    // transition to and from opaque and translucent, with
    // the respective alpha and whether its still blocking
    private Dictionary<Renderer, Pair<float, bool>>
        _activeObjects = new();

    // Change in opacity per iteration
    private float OpacityStep()
    {
        return (Time.fixedDeltaTime / _duration) * (1f - _lowOpacity);
    }

    private void FixedUpdate()
    {
        Vector3 cameraPos = _camera.transform.position;
        Vector3 playerPos = _player.position;

        Vector3 direction = (playerPos - cameraPos).normalized;

        RaycastHit[] results =
            Physics.SphereCastAll(cameraPos, _radius,
            direction, _castDistance);

        foreach (RaycastHit result in results)
        {
            Transform transform = result.transform;

            // Ignore all other objects that cannot block
            // the player
            if (!result.collider.CompareTag("Scene Object"))
            {
                continue;
            }

            // Player is in front of the object
            if (transform.position.z > playerPos.z)
            {
                continue;
            }

            Renderer renderer =
                transform.GetComponent<Renderer>();

            bool found = _activeObjects.
                TryGetValue(renderer, out Pair<float, bool> pair);

            // Not found in dictionary, add to it
            if (!found)
            {
                // Set it to the current alpha of the object to account
                // for the case where the object's opacity is not 1 yet
                // and it has resumed to blocking the player
                AddActiveObjectPair(_activeObjects, renderer,
                    flag: true);

                continue;
            }

            float newAlpha = pair.First;

            // Perform the decrease in alpha by opacity step and
            // clamp it to the appropriate bounds
            newAlpha -= OpacityStep();
            newAlpha = Mathf.Clamp(newAlpha, _lowOpacity, 1f);

            // Store the new alpha into the dictionary
            _activeObjects[renderer] = new(newAlpha, true);

            renderer.SetAlpha(newAlpha);
        }

        // New dictionary with updated active objects
        Dictionary<Renderer, Pair<float, bool>> newActiveObjects =
            new();

        // Loop through the dictionary and restore opacity for
        // those objects that are no longer blocking before removing
        // them
        foreach (KeyValuePair<Renderer, Pair<float, bool>>
            keyValuePair in _activeObjects)
        {
            Renderer renderer = keyValuePair.Key;
            float alpha = keyValuePair.Value.First;
            
            // This object was blocking the player this cycle
            if (keyValuePair.Value.Second)
            {
                AddActiveObjectPair(newActiveObjects, renderer,
                    alpha);

                continue;
            }

            float newAlpha = alpha;

            // Perform the increase in alpha by opacity step and
            // clamp it to the appropriate bounds
            newAlpha += OpacityStep();
            newAlpha = Mathf.Clamp(newAlpha, _lowOpacity, 1f);

            renderer.SetAlpha(newAlpha);

            // Only add the object entry back into the new
            // dictionary if it has yet to transit back to
            // fully opaque
            if (newAlpha < 1f)
            {
                AddActiveObjectPair(newActiveObjects, renderer,
                    newAlpha);
            }
        }

        _activeObjects = newActiveObjects;
    }

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void AddActiveObjectPair(
        Dictionary<Renderer, Pair<float, bool>> objectDict,
        Renderer renderer, float alpha = -1f, bool flag = false)
    {
        if (renderer is SpriteRenderer spriteRenderer)
        {
            alpha = alpha == -1f ? spriteRenderer.color.a : alpha;

            objectDict.Add(renderer, new(alpha, flag));
        }
        else if (renderer is MeshRenderer meshRenderer)
        {
            alpha = alpha == -1f ? meshRenderer.material.color.a :
                alpha;

            objectDict.Add(renderer, new(alpha, flag));
        }
        else
        {
            Fatal("Unhandled renderer type");
        }
    }
}
