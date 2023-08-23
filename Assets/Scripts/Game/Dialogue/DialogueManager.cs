using System;

using UnityEngine.SceneManagement;
using UnityEngine;

using static DebugUtils;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] 
    private string _playerCameraStateName = "Player Camera";

    private Dialogue _dialogue;

    // TODO (Chris): Might need to make an interface to store 'triggers'
    // of dialogues in the dialogue manager.
    private DialoguePoint _currentPoint = null;

    private Animator _stateDrivenCameraAnimator = null; 

    public bool CanStartDialogue => _currentPoint == null;

    public event Action<DialoguePoint> OnDialogueEnd;

    public void StartNewDialogue(DialoguePoint point, 
        DialogueInstance dialogueInstance, Transform talkingTransform)
    {
        Assert(CanStartDialogue, "Dialogue attempted to start.");
        _currentPoint = point;
        transform.position = talkingTransform.position
            + dialogueInstance.Data.DialogueBoxOffset;
        _dialogue.Initialize(dialogueInstance);
        _dialogue.StartDialogue();

        if (_stateDrivenCameraAnimator != null)
        {
            _stateDrivenCameraAnimator.Play(dialogueInstance.CameraStateName);
        }
    }

    public void Iterate()
    {
        if (_dialogue.NextText(out var text))
        {
            _dialogue.SetText(text);
        }
        else
        {
            _stateDrivenCameraAnimator.Play(_playerCameraStateName);
            _dialogue.EndDialogue();
            OnDialogueEnd?.Invoke(_currentPoint);
            _currentPoint = null;
        }
    }

    protected override void OnStart()
    {
        // NOTE (Chris): This function is should only be ran one time.
        _dialogue = GetComponentInChildren<Dialogue>(true); 
        UpdateCameraAnimator();

        SceneManager.activeSceneChanged += 
            (Scene oldScene, Scene newScene) => UpdateCameraAnimator();
    }

    private void UpdateCameraAnimator()
    {
        GameObject stateDrivenCameraObject = 
            GameObject.FindGameObjectWithTag("StateDrivenCamera");

        if (stateDrivenCameraObject == null)
        {
            return;
        }

        _stateDrivenCameraAnimator = stateDrivenCameraObject
            .TryGetComponent(out Animator animator) ? animator : null;
    }

    private void Update()
    {
        if (_currentPoint != null)        
        {
            transform.rotation = 
                Quaternion.Inverse(Camera.main.transform.rotation);

            if (Input.GetButtonDown("Fire1"))
            {
                Iterate();
            }
        }
    }
}