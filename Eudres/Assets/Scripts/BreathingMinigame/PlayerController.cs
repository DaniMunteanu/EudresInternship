using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cup;
    [SerializeField] private float moveSpeed = 4f;

    public bool IsHeld { get; private set; } = false;
    public float NormalizedPosition { get; private set; } = 1f;

    private Rigidbody2D rb;
    private float spawnY;
    private float minY;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnY = transform.position.y;
        minY = cup != null ? cup.position.y : spawnY - 3f;
    }

    void Update()
    {
        IsHeld = Mouse.current.leftButton.isPressed;

        float targetY = IsHeld
            ? Mathf.Max(transform.position.y - moveSpeed * Time.deltaTime, minY)
            : Mathf.Min(transform.position.y + moveSpeed * Time.deltaTime, spawnY);

        rb.MovePosition(new Vector2(transform.position.x, targetY));

        NormalizedPosition = Mathf.InverseLerp(minY, spawnY, transform.position.y);
    }

    public bool IsAtBottom() => transform.position.y <= minY + 0.05f;
    public bool IsAtTop() => transform.position.y >= spawnY - 0.05f;
}
