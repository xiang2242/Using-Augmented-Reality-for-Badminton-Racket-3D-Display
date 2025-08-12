using UnityEngine;
using UnityEngine.EventSystems;

public class ModelRotator : MonoBehaviour
{
    public float rotationSpeed = 0.1f;
    public float zoomSpeed = 0.5f;
    public float minScale = 0.3f;
    public float maxScale = 3f;
    public bool autoRotate = false;
    public float autoRotateSpeed = 15f;

    private Vector3 initialRotation;
    private Vector3 initialScale;

    void Start()
    {
        initialRotation = transform.eulerAngles;
        initialScale = transform.localScale;
    }

    void Update()
    {
        #if UNITY_EDITOR || UNITY_STANDALONE
                HandleMouseInput();
        #else
             HandleTouchInput();
        #endif

        if (autoRotate)
        {
            transform.Rotate(Vector3.up, autoRotateSpeed * Time.deltaTime, Space.World);
        }
    }

    void HandleMouseInput()
    {

        Debug.Log("Touch Count: " + Input.touchCount);
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButton(0))
        {
            float rotateX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float rotateY = -Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            transform.Rotate(Vector3.up, -rotateX, Space.Self);       // 左右旋转
            transform.Rotate(Vector3.right, rotateY, Space.Self);     // 上下旋转
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            Zoom(scroll * 5f);
        }
    }

    void HandleTouchInput()
{
    if (Input.touchCount == 1)
    {
        Touch touch = Input.GetTouch(0);

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            return;

        if (touch.phase == TouchPhase.Moved)
        {
            float deltaX = touch.deltaPosition.x * rotationSpeed; // 去掉 Time.deltaTime
            float deltaY = touch.deltaPosition.y * rotationSpeed;

            transform.Rotate(Vector3.up, -deltaX, Space.World);
            transform.Rotate(Vector3.right, deltaY, Space.Self);
        }
    }

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
            Zoom(delta);
        }
    }
}


    void Zoom(float increment)
    {
        float currentScale = transform.localScale.x;
        float newScale = Mathf.Clamp(currentScale + increment, minScale, maxScale);
        transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    public void ResetModel()
    {
        transform.eulerAngles = initialRotation;
        transform.localScale = initialScale;
    }

    public void ToggleAutoRotate()
    {
        autoRotate = !autoRotate;
    }
}
