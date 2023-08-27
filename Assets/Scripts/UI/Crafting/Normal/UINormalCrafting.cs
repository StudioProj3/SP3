using UnityEngine;

using static DebugUtils;

public class UINormalCrafting : MonoBehaviour
{
    private NormalCraft _normalCraft;
    private UICrafting _uicrafting;
    private GameObject _craftButton;
    private GameObject _book;

    private void Update()
    {
        switch (_uicrafting.CurrentMode)
        {
            case UICrafting.Mode.Craft:
                _craftButton.SetActive(true);
                _book.SetActive(false);
                break;

            case UICrafting.Mode.Book:
                _book.SetActive(true);
                _craftButton.SetActive(false);
                break;

            default:
                Fatal("Unhandled mode");
                break;
        }
    }

    private void Awake()
    {
        _normalCraft = GetComponent<NormalCraft>();
        _uicrafting = GameObject.FindWithTag("UICrafting").
            GetComponent<UICrafting>();
        _craftButton = transform.ChildGO(2);
        _book = transform.ChildGO(3);
    }
}
