using UnityEngine;

public class DisableOnMessageMethod : MonoBehaviour
{
    public enum MessageMethod
    {
        Awake,
        Start,
        FirstUpdate,
        NTHUpdate,
    }

    [SerializeField]
    private MessageMethod _messageMethod;

    [SerializeField]
    [ShowIf("_messageMethod", MessageMethod.NTHUpdate)]
    private uint _frameNumber = 2;

    private uint _updateFrameNumber = 1;

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
        if ((_updateFrameNumber == 1 &&
            _messageMethod == MessageMethod.FirstUpdate) ||
            (_updateFrameNumber == _frameNumber &&
            _messageMethod == MessageMethod.NTHUpdate))
        {
            Disable();
        }

        _updateFrameNumber++;
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
