using UnityEngine;

public class UIInventory : MonoBehaviour
{
    private GameObject _content;
    private bool _disabled = false;

    public void ShowInventory()
    {
        gameObject.SetActive(true);
        _content.SetActive(true);
    }

    public void HideInventory()
    {
        gameObject.SetActive(false);
        _content.SetActive(false);
    }

    private void Update()
    {
        if (!_disabled)
        {
            gameObject.SetActive(false);
            _disabled = true;
        }
    }

    private void Awake()
    {
        _content = transform.ChildGO(0);
    }
}
