using UnityEngine;

public class UIInventory : MonoBehaviour
{
    private GameObject _content;

    public void ShowInventory()
    {
        gameObject.SetActive(true);
        _content.SetActive(true);
        UIManager.Instance.ShowHUD(false);
    }

    public void HideInventory()
    {
        gameObject.SetActive(false);
        _content.SetActive(false);
        UIManager.Instance.ShowHUD(true);
    }

    private void Awake()
    {
        _content = transform.ChildGO(0);
    }
}
