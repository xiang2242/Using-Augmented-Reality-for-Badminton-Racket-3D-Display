using UnityEngine;
using Vuforia;

public class TargetTracker : MonoBehaviour
{
    private ObserverBehaviour observerBehaviour;

    private void Start()
    {
        observerBehaviour = GetComponent<ObserverBehaviour>();
        if (observerBehaviour)
        {
            observerBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
        }
    }

    private void OnDestroy()
    {
        if (observerBehaviour)
        {
            observerBehaviour.OnTargetStatusChanged -= OnTargetStatusChanged;
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            var data = GetComponent<TargetMediaData>();
            if (data != null && MediaManager.Instance != null)
            {
                MediaManager.Instance.SetCurrentTarget(data.modelID);
            }
        }
        else
        {
            MediaManager.Instance?.StopAllMedia();
        }
    }
}
