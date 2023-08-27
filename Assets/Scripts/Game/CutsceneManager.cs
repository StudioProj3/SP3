using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    private bool cutscenePlaying;
    private bool cutsceneFinished;

    public void PlayCutscene()
    {
        GameManager.Instance.CurrentState = GameState.Dialogue;
    }

    public void StopCutscene()
    {
        cutscenePlaying = false;
        GameManager.Instance.CurrentState = GameState.Play;
    }

    private void Awake()
    {
        cutscenePlaying = true;
        cutsceneFinished = false;
        PlayCutscene();
    }

    private void FixedUpdate()
    {
        if (cutscenePlaying)
        {
            PlayCutscene();
        }
        else if(!cutsceneFinished)
        {
            cutscenePlaying = false;
            cutsceneFinished = true;
            StopCutscene();
        }
    }
}
