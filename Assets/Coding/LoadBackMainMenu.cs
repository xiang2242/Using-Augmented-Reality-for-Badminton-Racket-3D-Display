using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadBackMainMenu : MonoBehaviour
{
    public void LoadSceneByName(string MainMenu)
    {
        SceneManager.LoadScene(MainMenu);
    }
}
