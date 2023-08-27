using TMPro;
using UnityEngine;

using static DebugUtils;

public class UICrafting : MonoBehaviour
{
    public enum Page
    {
        Basic,
        Normal,
    }

    public enum Mode
    {
        Craft,
        Book,
    }

    private GameObject _content;
    private GameObject _basicTitle;
    private GameObject _basicCrafting;
    private GameObject _normalTitle;
    private GameObject _normalCrafting;
    private UIHoverPanel _hoverPanel;

    private GameObject _leftButton;
    private GameObject _rightButton;

    private TMP_Text _modeButtonText;

    private Page _page = Page.Basic;
    private Mode _mode = Mode.Craft;

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

        _leftButton.SetActive(false);
        _rightButton.SetActive(true);

        _page = Page.Basic;
    }

    public void SwitchToNormalCrafting()
    {
        _normalTitle.SetActive(true);
        _normalCrafting.SetActive(true);
        _basicTitle.SetActive(false);
        _basicCrafting.SetActive(false);

        _leftButton.SetActive(true);
        _rightButton.SetActive(false);

        _page = Page.Normal;
    }

    public void SwitchLeft()
    {
        switch (_page)
        {
            case Page.Basic:
                Fatal("Invalid page switch operation");
                break;

            case Page.Normal:
                SwitchToBasicCrafting();
                break;

            default:
                Fatal("Unhandled page type");
                break;
        }
    }

    public void SwitchRight()
    {
        switch (_page)
        {
            case Page.Basic:
                SwitchToNormalCrafting();
                break;

            case Page.Normal:
                Fatal("Invalid page switch operation");
                break;

            default:
                Fatal("Unhandled page type");
                break;
        }
    }

    public void ToggleMode()
    {
        _mode = _mode == Mode.Book ?
            Mode.Craft : Mode.Book;

        _modeButtonText.text = "Current: " +
            _mode.ToString() + " mode";
    }

    public void ShowCrafting()
    {
        gameObject.SetActive(true);
        UIManager.Instance.ShowHUD(false);
    }

    public void HideCrafting()
    {
        gameObject.SetActive(false);
        UIManager.Instance.ShowHUD(true);
    }

    private void Awake()
    {
        _content = transform.ChildGO(1);

        _basicTitle = _content.transform.ChildGO(0);
        _basicCrafting = _content.transform.ChildGO(4);
        _normalTitle = _content.transform.ChildGO(1);
        _normalCrafting = _content.transform.ChildGO(5);

        _hoverPanel = transform.GetChild(2).
            GetComponent<UIHoverPanel>();

        _leftButton = transform.ChildGO(4);
        _rightButton = transform.ChildGO(5);

        _modeButtonText = transform.GetChild(6).
            GetComponentInChildren<TMP_Text>();
    }
}
