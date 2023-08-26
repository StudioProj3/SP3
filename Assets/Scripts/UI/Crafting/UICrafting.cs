using UnityEngine;

public class UICrafting : MonoBehaviour
{
    private GameObject _content;

    public void ShowCrafting()
    {
        gameObject.SetActive(true);
        _content.SetActive(true);
        UIManager.Instance.ShowHUD(false);
    }

    public void HideCrafting()
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
