using UnityEngine;

public class UIInventory : MonoBehaviour
{
    private GameObject _content;

    public void ShowInventory()
    {
        _content.SetActive(true);
    }

    public void HideInventory()
    {
        _content.SetActive(false);
    }

    private void Awake()
    {
        _content = transform.ChildGO(0);
    }
}
