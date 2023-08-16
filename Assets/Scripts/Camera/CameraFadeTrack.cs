using System.Collections.Generic;

using UnityEngine;

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

    // Stores dictionary of active sprites involved in the
    // transition to and from opaque and translucent, with
    // the respective alpha and whether its still blocking
    private Dictionary<SpriteRenderer, Pair<float, bool>>
        _activeSprites = new();

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

            SpriteRenderer spriteRenderer =
                transform.GetComponent<SpriteRenderer>();

            bool found = _activeSprites.
                TryGetValue(spriteRenderer, out Pair<float, bool> pair);

            // Not found in dictionary, add to it
            if (!found)
            {
                // Set it to the current opacity of the sprite to account
                // for the case where the sprite's opacity is not 1 yet and
                // it has resumed to blocking the player
                _activeSprites.Add(spriteRenderer,
                    new(spriteRenderer.color.a, true));

                continue;
            }

            float newAlpha = pair.First;

            // Perform the decrease in alpha by opacity step and
            // clamp it to the appropriate bounds
            newAlpha -= OpacityStep();
            newAlpha = Mathf.Clamp(newAlpha, _lowOpacity, 1f);

            // Store the new alpha into the dictionary
            _activeSprites[spriteRenderer] = new(newAlpha, true);

            spriteRenderer.SetAlpha(newAlpha);
        }

        // New dictionary with updated active sprites
        Dictionary<SpriteRenderer, Pair<float, bool>> newActiveSprites =
            new();

        // Loop through the dictionary and restore opacity for
        // those sprites that are no longer blocking before removing
        // them
        foreach (KeyValuePair<SpriteRenderer, Pair<float, bool>>
            keyValuePair in _activeSprites)
        {
            SpriteRenderer spriteRenderer = keyValuePair.Key;
            float alpha = keyValuePair.Value.First;
            
            // This sprite was blocking the player this cycle
            if (keyValuePair.Value.Second)
            {
                newActiveSprites.Add(spriteRenderer,
                    new(alpha, false));

                continue;
            }

            float newAlpha = alpha;

            // Perform the increase in alpha by opacity step and
            // clamp it to the appropriate bounds
            newAlpha += OpacityStep();
            newAlpha = Mathf.Clamp(newAlpha, _lowOpacity, 1f);

            spriteRenderer.SetAlpha(newAlpha);

            // Only add the sprite entry back into the new
            // dictionary if it has yet to transit back to
            // fully opaque
            if (newAlpha < 1f)
            {
                newActiveSprites.Add(spriteRenderer,
                    new(newAlpha, false));
            }
        }

        _activeSprites = newActiveSprites;
    }

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _player = GameObject.FindWithTag("Player").transform;
    }
}
