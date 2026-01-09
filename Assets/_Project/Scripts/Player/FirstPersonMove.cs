using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonMove : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 4.5f;
    [SerializeField] private float gravity = -15f;

    private CharacterController cc;
    private Vector3 velocity;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal"); // A/D
        float z = Input.GetAxisRaw("Vertical");   // W/S

        Vector3 move = (transform.right * x + transform.forward * z);
        move = Vector3.ClampMagnitude(move, 1f);

        cc.Move(move * moveSpeed * Time.deltaTime);

        // 중력
        if (cc.isGrounded && velocity.y < 0f)
            velocity.y = -2f; // 바닥에 붙게 하는 작은 값

        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }
}
