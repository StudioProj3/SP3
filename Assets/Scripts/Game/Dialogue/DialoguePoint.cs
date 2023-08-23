using UnityEngine;
using TMPro;

public class DialoguePoint : MonoBehaviour, IInteractable
{
    [SerializeField] private string _interactTextData;
    private TMP_Text _interactText;
    private DialogueInstance _dialogueInstance;

    private bool _canStart = false;
    public string InteractText => _interactTextData; 

    public bool CheckPlayer()
    {
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
        // TODO (Chris): Might need to make an interface to store 'triggers'
        // of dialogues in the dialogue manager.
        if (_dialogueInstance is not null && 
            DialogueManager.Instance.CanStartDialogue)
        {
            DialogueManager.Instance.StartNewDialogue(this,
                _dialogueInstance, transform);
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

    private void Update()
    {
        if (_canStart && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }
}