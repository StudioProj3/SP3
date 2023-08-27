using UnityEngine;

public class UICrafting : MonoBehaviour
{
    private GameObject _content;
    private GameObject _basicTitle;
    private GameObject _basicCrafting;
    private GameObject _normalTitle;
    private GameObject _normalCrafting;
    private UIHoverPanel _hoverPanel;

    // To be used when player is dragging items to
    // reduce visual clutter
    public void HideHoverPanel()
    {
        _hoverPanel.MakeHidden();
    }

    // To be used when player is dragging items to
    // reduce visual clutter
    public void ShowHoverPanel()
    {
        _hoverPanel.MakeVisible();
    }

    public void SwitchToBasicCrafting()
    {
        _basicTitle.SetActive(true);
        _basicCrafting.SetActive(true);
        _normalTitle.SetActive(false);
        _normalCrafting.SetActive(false);
    }

    public void SwitchToNormalCrafting()
    {
        _normalTitle.SetActive(true);
        _normalCrafting.SetActive(true);
        _basicTitle.SetActive(false);
        _basicCrafting.SetActive(false);
    }

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

        _basicTitle = _content.transform.ChildGO(0);
        _basicCrafting = _content.transform.ChildGO(4);
        _normalTitle = _content.transform.ChildGO(1);
        _normalCrafting = _content.transform.ChildGO(5);

        _hoverPanel = transform.GetChild(2).
            GetComponent<UIHoverPanel>();
    }
}
