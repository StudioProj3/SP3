using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    private bool _cutscenePlaying;
    private bool _cutsceneFinished;
    private GameObject _boss;

    public void WinCutscene()
    {
        GameManager.Instance.CurrentState = GameState.Win;
    }

    public void PlayCutscene()
    {
        GameManager.Instance.CurrentState = GameState.Dialogue;
    }

    public void StopCutscene()
    {
        _cutscenePlaying = false;
        GameManager.Instance.CurrentState = GameState.Play;
    }

    private void Awake()
    {
        _cutscenePlaying = true;
        _cutsceneFinished = false;
        _boss = GameObject.FindWithTag("Enemy");
        PlayCutscene();
    }

    private void FixedUpdate()
    {
        if(_boss && !_boss.activeSelf)
        {
            WinCutscene();
        }

        if (_cutscenePlaying)
        {
            PlayCutscene();
        }
        else if(!_cutsceneFinished)
        {
            _cutscenePlaying = false;
            _cutsceneFinished = true;
            StopCutscene();
        }
    }
}
