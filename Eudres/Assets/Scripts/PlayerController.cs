using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cup;

    public bool IsDragging { get; private set; } = false;

    private Vector3 offset;
    private Camera mainCamera;
    private Rigidbody2D rb;
    private float spawnY;
    private float minY;

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        spawnY = transform.position.y;

        // Limita de jos = centrul cupului (ca sa intre in el)
        if (cup != null)
            minY = cup.position.y;
        else
            minY = spawnY - 5f;
    }

    void Update()
    {
        if (IsDragging)
        {
            Vector2 mousePos2D = Mouse.current.position.ReadValue();
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePos2D.x, mousePos2D.y, 0));

            float targetY = Mathf.Clamp(mousePosition.y + offset.y, minY, spawnY);

            rb.MovePosition(new Vector2(0, targetY));
        }
    }

    private void OnMouseDown()
    {
        IsDragging = true;

        Vector2 mousePos2D = Mouse.current.position.ReadValue();
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePos2D.x, mousePos2D.y, 0));

        offset = transform.position - mousePosition;
    }

    private void OnMouseUp()
    {
        IsDragging = false;
    }
}
