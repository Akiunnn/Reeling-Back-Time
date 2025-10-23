using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Reeling Back Time");
    }

    
    public void QuitGame()
    {
        Application.Quit();
    }
}
