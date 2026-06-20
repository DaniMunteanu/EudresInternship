using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Transform cup;

    public bool IsDragging { get; private set; } = false;

    private Camera mainCamera;
    private Rigidbody2D rb;
    private float spawnY;
    private float minY;
    private float offset;

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        spawnY = transform.position.y;

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

            float targetY = Mathf.Clamp(mousePosition.y + offset, minY, spawnY);
            rb.MovePosition(new Vector2(0, targetY));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsDragging = true;

        Vector2 mousePos2D = Mouse.current.position.ReadValue();
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePos2D.x, mousePos2D.y, 0));
        offset = transform.position.y - mousePosition.y;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsDragging = false;
    }
}
