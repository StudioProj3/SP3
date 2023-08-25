using UnityEngine;
using TMPro;

public class DialoguePoint : MonoBehaviour
{
    [SerializeField] private string _interactTextData;
    private TMP_Text _interactText;
    private DialogueInstance _dialogueInstance;

    private bool _canStart = false;
    private bool _inConversation = false;
    public string InteractText => _interactTextData; 

    public bool CheckPlayer()
    {
        // TODO (Chris): Should IInteractable even exist?
        throw new System.NotImplementedException();
    }

    public void InjectData(DialogueInstance dialogueInstance) 
    {
        _dialogueInstance = dialogueInstance;
    }

    public void Interact()
    {
        TriggerDialogue();
    }

    public void TriggerDialogue()
    {
        if (_dialogueInstance is not null && 
            DialogueManager.Instance.CanStartDialogue)
        {
            DialogueManager.Instance.StartNewDialogue(this,
                _dialogueInstance, transform);
            _interactText.gameObject.SetActive(false);
            _inConversation = true;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        _interactText.gameObject.SetActive(true);
        _canStart = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _interactText.gameObject.SetActive(false);
        _canStart = false;
    }

    private void Awake()
    {
        _interactText = GetComponentInChildren<TMP_Text>(true);
        _interactText.text = _interactTextData;
    }

    private void Start()
    {
        // We want to bind the event at start lest this object
        // is additively loaded to the scene
        DialogueManager.Instance.OnDialogueEnd += DialogueEndHandler;
    }

    private void OnDestroy()
    {
        DialogueManager.Instance.OnDialogueEnd -= DialogueEndHandler;
    }

    private void DialogueEndHandler(DialoguePoint point)
    {
        if (point == this)
        {
            _interactText.gameObject.SetActive(_canStart);
            _inConversation = false;

            // We set the dialogue instance to null to allow for new dialogue
            // instances to come in later and to prevent the player
            // from doing the dialogue a second time
            _dialogueInstance = null;
        }
    }

    private void Update()
    {
        if (!_inConversation && _canStart && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }
}