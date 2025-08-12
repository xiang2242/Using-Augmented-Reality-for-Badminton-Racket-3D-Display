using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene3DRG : MonoBehaviour
{
    public void LoadSceneByName(string RacketGalleries)
    {
        SceneManager.LoadScene(RacketGalleries);
    }
}
