using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    [SerializeField] GameObject controlsUI;
    public void Play(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ShowControls()
    {
        controlsUI?.SetActive(true);
    }
    public void CloseControls()
    {
        controlsUI?.SetActive(false);
    }
}
