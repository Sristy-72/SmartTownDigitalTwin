
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 12f;
    public float lookSpeed = 2f;
    public float zoomSpeed = 10f;

    private float yaw;
    private float pitch = 35f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void Update()
    {
        // Keyboard movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement = transform.right * x + transform.forward * z;
        transform.position += movement * moveSpeed * Time.deltaTime;

        // Hold right mouse button to rotate
        if (Input.GetMouseButton(1))
        {
            yaw += Input.GetAxis("Mouse X") * lookSpeed;
            pitch -= Input.GetAxis("Mouse Y") * lookSpeed;
            pitch = Mathf.Clamp(pitch, -80f, 80f);

            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }

        // Mouse wheel zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.position += transform.forward * scroll * zoomSpeed;
    }
}
