using TMPro;
using UnityEngine;

using static DebugUtils;

public class UINormalCrafting : MonoBehaviour
{
    private NormalCraft _normalCraft;
    private UICrafting _uicrafting;
    private GameObject _craftButton;
    private GameObject _book;
    private GameObject _bookLeftButton;
    private GameObject _bookRightButton;
    private TMP_Text _bookNumber;
    private uint _number = 1;

    public void BookLeft()
    {
        _number--;
    }

    public void BookRight()
    {
        _number++;
    }

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

                int count = _normalCraft.AllRecipes.Count;

                _bookNumber.text = _number.ToString() +
                    " / " + count.ToString();

                _bookLeftButton.SetActive(_number > 1);
                _bookRightButton.SetActive(_number < count);

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
        _bookNumber = _book.GetComponentInChildren<TMP_Text>();
        _bookLeftButton = _book.transform.ChildGO(1);
        _bookRightButton = _book.transform.ChildGO(2);
    }
}
