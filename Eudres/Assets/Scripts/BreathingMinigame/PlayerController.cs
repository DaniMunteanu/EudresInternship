using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cup;

    [Header("Settings")]
    [Tooltip("Cât de sus față de centrul cănii se oprește plicul. Mărește valoarea dacă trece de cană.")]
    [SerializeField] private float stopOffset = 1.2f;

    [Header("Breathing Duration")]
    [SerializeField] private float breathDuration = 3.0f; // Fix 3 secunde

    public bool IsHeld { get; private set; } = false;
    public float NormalizedPosition { get; private set; } = 1f;

    private Rigidbody2D rb;
    private float spawnY;
    private float progress = 0f; // 0 = sus, 1 = în cană
    private bool touchscreenEnabled = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (Touchscreen.current != null)
        {
            touchscreenEnabled = true;
        }

        spawnY = transform.position.y;
    }

    void Update()
    {
        // 1. Detectare Input
        if (touchscreenEnabled && Touchscreen.current != null)
        {
            IsHeld = Touchscreen.current.primaryTouch.press.isPressed || (Mouse.current != null && Mouse.current.leftButton.isPressed);
        }
        else if (Mouse.current != null)
        {
            IsHeld = Mouse.current.leftButton.isPressed;
        }

        // 2. Progres fix în timp (exact 3 secunde)
        if (IsHeld)
        {
            progress = Mathf.MoveTowards(progress, 1f, Time.unscaledDeltaTime / breathDuration);
        }
        else
        {
            progress = Mathf.MoveTowards(progress, 0f, Time.unscaledDeltaTime / breathDuration);
        }

        // 3. Calcul punct de oprire cu offset
        float currentMinY = (cup != null) ? (cup.position.y + stopOffset) : (spawnY - 3f);
        float targetY = Mathf.Lerp(spawnY, currentMinY, progress);

        if (rb != null)
        {
            rb.MovePosition(new Vector2(transform.position.x, targetY));
        }
        else
        {
            transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
        }

        // 4. Progres normalizat pentru CupController
        NormalizedPosition = 1f - progress;
    }

    public bool IsAtBottom() => progress >= 0.95f;
    public bool IsAtTop() => progress <= 0.05f;
}