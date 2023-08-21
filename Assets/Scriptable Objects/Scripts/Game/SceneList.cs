using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneList",
    menuName = "Scriptable Objects/Game/SceneList")]

public class SceneList : ScriptableObject
{
    public string shopScene;
    public string surfaceLayerScene;
    public string HUDScene;
    public string layer1Scene;
    public string layer2Scene;
    public string layer3Scene;
    public string layer4Scene;
    public string layer4Scene2;
}
