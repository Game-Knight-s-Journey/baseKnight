using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("LevelLoader");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
