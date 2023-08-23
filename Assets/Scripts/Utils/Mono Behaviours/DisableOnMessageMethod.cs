using UnityEngine;

public class DisableOnMessageMethod : MonoBehaviour
{
    public enum MessageMethod
    {
        Awake,
        Start,
        FirstUpdate,
    }

    [SerializeField]
    private MessageMethod _messageMethod;

    private bool _firstUpdate = true;

    private void Awake()
    {
        if (_messageMethod == MessageMethod.Awake)
        {
            Disable();
        }
    }

    private void Start()
    {
        if (_messageMethod == MessageMethod.Start)
        {
            Disable();
        }
    }

    private void Update()
    {
        if (_firstUpdate &&
            _messageMethod == MessageMethod.FirstUpdate)
        {
            Disable();
        }

        _firstUpdate = false;
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
