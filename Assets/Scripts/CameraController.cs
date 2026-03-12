using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("Instellingen")]
    public float speed = 5f;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        Vector3 beweging = Vector3.zero;

        if (kb.wKey.isPressed) beweging += Vector3.up;
        if (kb.sKey.isPressed) beweging += Vector3.down;
        if (kb.aKey.isPressed) beweging += Vector3.left;
        if (kb.dKey.isPressed) beweging += Vector3.right;

        transform.position += beweging * speed * Time.deltaTime;

        ClampAanWereld();
    }

    private void ClampAanWereld()
    {
        if (WorldManager.Instance == null) return;

        float maxX = WorldManager.Instance.MaxX;
        float maxY = WorldManager.Instance.MaxY;

        if (maxX <= 0 && maxY <= 0) return;

        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;

        float clampedX = Mathf.Clamp(transform.position.x, halfW, maxX - halfW);
        float clampedY = Mathf.Clamp(transform.position.y, halfH, maxY - halfH);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}
