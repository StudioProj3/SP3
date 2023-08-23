using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ScrollingImage : MonoBehaviour
{
    private Image _image;
    [SerializeField] private Vector2 _scrollSpeed;

    private void Awake()
    {
        _image = GetComponent<Image>();
        
        // Create a new runtime material instance for this image.
        _image.material = new Material(_image.material);
    }

    private void Update()
    {
        _image.material.mainTextureOffset += _scrollSpeed * Time.deltaTime;
    }
}
