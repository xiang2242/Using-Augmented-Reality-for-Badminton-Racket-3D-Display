using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections.Generic;

public class MediaManager : MonoBehaviour
{
    // ✅ Singleton 实例
    public static MediaManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        // ✅ 加载音频资源并注册
        LoadAllAudioClips();
    }

    void LoadAllAudioClips()
{
    // 加载 Resources/Audio 下的所有 AudioClip
    AudioClip[] audioClips = Resources.LoadAll<AudioClip>("Audio");

    foreach (AudioClip clip in audioClips)
    {
        string id = clip.name.ToUpper();  // 假设你的 AudioClip 名称为 "A", "B", "C"
        RegisterAudio(id, clip);
        Debug.Log($"Registered audio clip: {id}");
    }
}

    // ✅ 当前识别的 Image Target ID（如 "A", "B", "C"）
    private string currentTarget;

    public void SetCurrentTarget(string targetName)
    {
        currentTarget = targetName;
        Debug.Log("Current target set to: " + currentTarget);
    }

    public string GetCurrentTarget()
    {
        return currentTarget;
    }

    [Header("Video Settings")]
    public RawImage videoPanel;
    public VideoPlayer videoPlayer;
    public Dictionary<string, string> targetToVideoURL = new Dictionary<string, string>()
    {
        {"A", "Videos/VideoA.mp4"},
        {"B", "Videos/VideoB.mp4"},
        {"C", "Videos/VideoC.mp4"}
    };

    private bool videoPlaying = false;

    public void ToggleVideo()
    {
        // ✅ 防止 videoPanel 丢失时崩溃
        if (videoPanel == null || videoPlayer == null)
        {
            Debug.LogWarning("VideoPanel or VideoPlayer is not assigned!");
            return;
        }

        if (videoPlaying)
        {
            StopVideo();
        }
        else
        {
            string id = GetCurrentTarget();
            if (!string.IsNullOrEmpty(id) && targetToVideoURL.ContainsKey(id))
            {
                videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, targetToVideoURL[id]);

                // ✅ 解绑旧回调，避免多次添加重复回调
                videoPlayer.prepareCompleted -= OnVideoPrepared;
                videoPlayer.prepareCompleted += OnVideoPrepared;

                videoPlayer.Prepare();
            }
            else
            {
                Debug.LogWarning("Video not found for target: " + id);
            }
        }
    }

    private void OnVideoPrepared(VideoPlayer vp)
    {
        if (videoPanel == null) return;

        videoPanel.gameObject.SetActive(true);
        vp.Play();
        videoPlaying = true;

        Debug.Log("Video started.");
    }

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public Dictionary<string, AudioClip> targetToAudioClip = new Dictionary<string, AudioClip>();
    private bool audioPlaying = false;

    public void RegisterAudio(string id, AudioClip clip)
    {
        if (!targetToAudioClip.ContainsKey(id))
        {
            targetToAudioClip.Add(id, clip);
        }
    }

    public void ToggleAudio()
    {
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource is not assigned!");
            return;
        }

        if (audioPlaying)
        {
            StopAudio();
        }
        else
        {
            string id = GetCurrentTarget();
            if (!string.IsNullOrEmpty(id) && targetToAudioClip.ContainsKey(id))
            {
                audioSource.clip = targetToAudioClip[id];
                audioSource.Play();
                audioPlaying = true;

                Debug.Log("Audio started.");
            }
            else
            {
                Debug.LogWarning("Audio not found for target: " + id);
            }
        }
    }

    public void StopVideo()
    {
        if (videoPlayer != null && videoPlayer.isPlaying)
            videoPlayer.Stop();

        if (videoPanel != null)
            videoPanel.gameObject.SetActive(false);

        videoPlaying = false;

        Debug.Log("Video stopped.");
    }

    public void StopAudio()
    {
        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();

        audioPlaying = false;

        Debug.Log("Audio stopped.");
    }

    public void StopAllMedia()
    {
        StopVideo();
        StopAudio();
    }
}
