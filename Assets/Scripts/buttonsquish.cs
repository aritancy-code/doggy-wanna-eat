using UnityEngine;
using UnityEngine.EventSystems; // gives us the mouse hover/click events for UI

// put on any UI Button to make it squish when mouse hovers over it,
// and squish a bit more when clicked.
// uses unscaled time, so it still animates when the game is paused (Time.timeScale = 0).
public class buttonsquish : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Vector3 hoverScale = new Vector3(1.05f, 0.9f, 1f); // a bit wider, a bit shorter = squished
    public Vector3 pressScale = new Vector3(1.1f, 0.8f, 1f);  // squished even more while clicking
    public float speed = 15f;                                 // how fast it squishes/unsquishes

    private Vector3 normalScale;  // the button's original size
    private Vector3 targetScale;  // squished size
    private bool isHovering;

    void Awake()
    {
        normalScale = transform.localScale;
        targetScale = normalScale;
    }

    void OnEnable()
    {
        // reset every time the screen shows up, so it never gets stuck squished
        transform.localScale = normalScale;
        targetScale = normalScale;
        isHovering = false;
    }

    void Update()
    {
        // smoothly move towards the target size
        // unscaledDeltaTime ignores Time.timeScale, so this works on the paused screen
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, speed * Time.unscaledDeltaTime);
    }

    public void OnPointerEnter(PointerEventData eventData) // mouse moves onto the button
    {
        isHovering = true;
        targetScale = Vector3.Scale(normalScale, hoverScale);
    }

    public void OnPointerExit(PointerEventData eventData) // mouse leaves the button
    {
        isHovering = false;
        targetScale = normalScale;
    }

    public void OnPointerDown(PointerEventData eventData) // mouse button pressed on it
    {
        targetScale = Vector3.Scale(normalScale, pressScale);
    }

    public void OnPointerUp(PointerEventData eventData) // mouse button released
    {
        targetScale = isHovering ? Vector3.Scale(normalScale, hoverScale) : normalScale;
    }
}