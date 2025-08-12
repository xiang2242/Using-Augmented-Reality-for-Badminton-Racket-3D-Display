using UnityEngine;

public class AppController : MonoBehaviour
{
    public void QuitApp()
    {
        // 在编辑器中无法退出，实际打包后有效
        Application.Quit();

        // 仅用于 Unity 编辑器测试（可选）
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
