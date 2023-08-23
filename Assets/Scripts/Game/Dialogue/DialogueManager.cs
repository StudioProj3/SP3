using UnityEngine.SceneManagement;
using UnityEngine;

using static DebugUtils;

public class DialogueManager : Singleton<DialogueManager>
{
    private Dialogue _dialogue;
    private DialoguePoint _currentPoint = null;

    private Animator _stateDrivenCameraAnimator = null; 

    public bool CanStartDialogue => _currentPoint == null;

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
        }
    }
}