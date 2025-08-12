// BackToGallery.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToGallery : MonoBehaviour
{
    public void Back()
    {
        SceneManager.LoadScene("RacketGalleries");
    }
}
