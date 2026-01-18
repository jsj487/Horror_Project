using UnityEngine;

public class FootstepController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform playerRoot;   // Player Transform
    [SerializeField] private CharacterController controller;
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips")]
    [SerializeField] private AudioClip[] clips;

    [Header("Tuning")]
    [SerializeField] private float minSpeed = 0.1f;
    [SerializeField] private float stepIntervalWalk = 0.48f;
    [SerializeField] private float stepIntervalRun = 0.32f;
    [SerializeField] private KeyCode runKey = KeyCode.LeftShift;

    private float timer;
    private Vector3 lastPos;

    private void Awake()
    {
        if (playerRoot == null) playerRoot = transform; // 못 찾으면 자기 자신
        if (controller == null) controller = GetComponentInParent<CharacterController>();
        if (sfxSource == null) sfxSource = GetComponent<AudioSource>();

        lastPos = playerRoot.position;
    }

    private void Update()
    {
        if (playerRoot == null || sfxSource == null || clips == null || clips.Length == 0) return;

        // 프레임 이동거리 기반 속도 계산
        Vector3 pos = playerRoot.position;
        float speed = (pos - lastPos).magnitude / Mathf.Max(Time.deltaTime, 0.0001f);
        lastPos = pos;

        bool grounded = controller == null ? true : controller.isGrounded;
        bool moving = grounded && speed >= minSpeed;

        if (!moving)
        {
            timer = 0f;
            return;
        }

        float interval = Input.GetKey(runKey) ? stepIntervalRun : stepIntervalWalk;
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0f;
            var clip = clips[Random.Range(0, clips.Length)];
            sfxSource.PlayOneShot(clip);
        }
    }
}
