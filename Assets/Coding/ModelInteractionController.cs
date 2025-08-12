using UnityEngine;
using UnityEngine.EventSystems;

public class ModelInteractionController : MonoBehaviour
{
    public Transform model; // 拖入模型 OBJ

    private Vector3 initialScale;
    private Quaternion initialRotation;
    private Vector3 initialPosition;

    private float rotationSpeed = 4.0f;  // 原本是 0.2
    private float zoomSpeed = 1.0f;      // 原本是 0.5

    private bool autoRotate = false;

    void Start()
    {
        if (model == null)
        {
            Debug.LogError("❌ Model is not assigned!");
            return;
        }

        initialScale = model.localScale;
        initialRotation = model.localRotation;
        initialPosition = model.localPosition;
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        

        HandleMouseInput();
#else
        HandleTouchInput();
#endif

        if (autoRotate && model != null)
{
    model.Rotate(Vector3.up, 20f * Time.deltaTime, Space.Self);
}

    }

    void HandleMouseInput()
{

// 只有鼠标控制要判断 UI 挡住没
    if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        return;

    if (Input.GetMouseButton(0))
    {
        float deltaX = Input.GetAxis("Mouse X");
        float deltaY = Input.GetAxis("Mouse Y");

        // 绕世界 Y 轴水平旋转（左右）
        model.Rotate(Vector3.up, -deltaX * rotationSpeed, Space.Self);

        // 绕局部 X 轴垂直旋转（上下）
        model.Rotate(Vector3.right, deltaY * rotationSpeed, Space.Self);
    }

    float scroll = Input.GetAxis("Mouse ScrollWheel");
if (Mathf.Abs(scroll) > 0.01f)
{
    Vector3 scale = model.localScale + Vector3.one * scroll * zoomSpeed;
    scale = ClampScale(scale);
    
    // 缩放后调整位置
    float scaleFactor = scale.x / model.localScale.x;
    model.localScale = scale;

    //model.localPosition += Vector3.back * scroll * 0.1f; // 缩小时靠近，放大时后移
}

}


    void HandleTouchInput()
{

    Debug.Log("Touch Count: " + Input.touchCount);
    if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(0))
        return;

    // 单指旋转（上下左右 360°）
    if (Input.touchCount == 1)
    {
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Moved)
        {
            float deltaX = touch.deltaPosition.x;
            float deltaY = touch.deltaPosition.y;

            // 左右旋转（绕世界Y轴）
            model.Rotate(Vector3.up, -deltaX * rotationSpeed * 0.02f, Space.World);

            // 上下旋转（绕局部X轴）
            model.Rotate(Vector3.right, deltaY * rotationSpeed * 0.02f, Space.Self);
        }
    }

    // 双指缩放
    if (Input.touchCount == 2)
    {
        Touch t0 = Input.GetTouch(0);
        Touch t1 = Input.GetTouch(1);

        Vector2 prevPos0 = t0.position - t0.deltaPosition;
        Vector2 prevPos1 = t1.position - t1.deltaPosition;

        float prevDist = (prevPos0 - prevPos1).magnitude;
        float currDist = (t0.position - t1.position).magnitude;
        float delta = (currDist - prevDist) * zoomSpeed * 0.01f;

        if (Mathf.Abs(delta) > 0.001f)
        {
            Vector3 scale = model.localScale + Vector3.one * delta;
            scale = ClampScale(scale);

            float scaleFactor = scale.x / model.localScale.x;
            model.localScale = scale;

            //model.localPosition += Vector3.back * delta * 0.5f; // 缩小时靠近，放大时后移
        }
    }
}


    Vector3 ClampScale(Vector3 scale)
    {
        float min = 0.5f;
        float max = 1.5f;
        float clamped = Mathf.Clamp(scale.x, min, max);
        return new Vector3(clamped, clamped, clamped);
    }

    public void ResetModel()
    {
        model.localScale = initialScale;
        model.localRotation = initialRotation;
        model.localPosition = initialPosition;
    }

    public void ToggleAutoRotate()
    {
        autoRotate = !autoRotate;
    }
}
