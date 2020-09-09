using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
