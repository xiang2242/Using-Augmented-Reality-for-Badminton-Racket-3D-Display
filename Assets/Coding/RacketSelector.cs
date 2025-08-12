using UnityEngine;
using UnityEngine.SceneManagement;

public class RacketSelector : MonoBehaviour
{
    public void SelectRacket(string racketName)
    {
        // 保存用户选择的球拍名称
        PlayerPrefs.SetString("SelectedRacket", racketName);

        // 跳转到展示 3D 模型的场景
        SceneManager.LoadScene("RacketDisplay");
    }
}
