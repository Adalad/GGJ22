using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    public float[] XLimits;
    public float[] ZLimits;
    public float[] CamSizes;
    public float CameraSpeed;

    private Vector2 Movement = Vector2.zero;
    private Camera CameraComponent;

    private void Update()
    {
        CameraComponent = GetComponent<Camera>();
        Vector3 newPos = transform.position;
        newPos.x = Mathf.Clamp(transform.position.x + Movement.x * CameraSpeed * Time.deltaTime, XLimits[0], XLimits[1]);
        newPos.z = Mathf.Clamp(transform.position.z + Movement.y * CameraSpeed * Time.deltaTime, ZLimits[0], ZLimits[1]);
        transform.position = newPos;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Movement = context.ReadValue<Vector2>();
    }

    public void OnScroll(InputAction.CallbackContext context)
    {
        Vector2 scroll = context.ReadValue<Vector2>();
        if (scroll.y > 0)
        {
            CameraComponent.orthographicSize = Mathf.Clamp(CameraComponent.orthographicSize - 1, CamSizes[0], CamSizes[1]);
        }
        else if (scroll.y < 0)
        {
            CameraComponent.orthographicSize = Mathf.Clamp(CameraComponent.orthographicSize + 1, CamSizes[0], CamSizes[1]);
        }
    }
}
