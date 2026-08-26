using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private RectTransform cup;
    [SerializeField] private float moveSpeed = 4f;

    public bool IsHeld { get; private set; } = false;
    public float NormalizedPosition { get; private set; } = 1f;

    private Rigidbody2D rb;
    private float spawnY;
    private float minY;
    private bool touchscreenEnabled = false;

    void Start()
    {
        if (Touchscreen.current != null)
        {
            touchscreenEnabled = true;
        }

        rb = GetComponent<Rigidbody2D>();
        spawnY = GetComponent<RectTransform>().position.y;
        minY = cup != null ? cup.position.y : spawnY - 3f;
    }

    void Update()
    {
        if (touchscreenEnabled)
        {
            IsHeld = Touchscreen.current.primaryTouch.press.isPressed || Mouse.current.leftButton.isPressed;
        }
        else
        {
            IsHeld = Mouse.current.leftButton.isPressed;
        }

        float targetY = IsHeld
            ? Mathf.Max(GetComponent<RectTransform>().position.y - moveSpeed * Time.deltaTime, minY)
            : Mathf.Min(GetComponent<RectTransform>().position.y + moveSpeed * Time.deltaTime, spawnY);

        rb.MovePosition(new Vector2(GetComponent<RectTransform>().position.x, targetY));

        NormalizedPosition = Mathf.InverseLerp(minY, spawnY, GetComponent<RectTransform>().position.y);
    }

    public bool IsAtBottom() => GetComponent<RectTransform>().position.y <= minY + 0.05f;
    public bool IsAtTop() => GetComponent<RectTransform>().position.y >= spawnY - 0.05f;
}
