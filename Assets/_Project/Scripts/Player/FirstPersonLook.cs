using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform playerBody; // Player 루트(좌우 회전)

    [Header("Settings")]
    [SerializeField] private float sensitivity = 200f;
    [SerializeField] private float clampUpDown = 80f;

    private float xRotation;

    private void Start()
    {
        LockCursor(true);
    }

    private void Update()
    {
        // ESC로 커서 해제(에디터에서 편하게)
        if (Input.GetKeyDown(KeyCode.Escape))
            LockCursor(false);

        // 마우스 클릭 시 다시 잠금
        if (Input.GetMouseButtonDown(0))
            LockCursor(true);

        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -clampUpDown, clampUpDown);

        // 상하: 카메라 로컬 회전
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 좌우: 플레이어 바디 회전
        if (playerBody != null)
            playerBody.Rotate(Vector3.up * mouseX);
    }

    private void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}
