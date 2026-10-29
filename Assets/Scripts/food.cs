using UnityEngine;
using UnityEngine.UI;

// A piece of food with its own freshness timer and freshness circle.
// Each food type (apple, ham, treat...) is its own prefab with its own settings,
// so different foods can rot at different speeds.
public class food : MonoBehaviour
{
    [Header("Freshness")]
    public float freshTime = 30f;          // seconds before this food rots
    public Sprite rottenSprite;            // drag the rotten version of this food here
    public SpriteRenderer foodRenderer;    // the food's own SpriteRenderer

    [Header("Freshness circle")]
    public Image clockImage;               // circle image (Filled, Radial 360) above this food
    public Color freshColor = Color.green; // circle colour at the start
    public Color rottenColor = Color.red;  // circle colour as it runs out

    private float timeLeft;
    public bool isRotten { get; private set; }      // other scripts can read it, only food can change it
    public Sprite freshSprite { get; private set; } // what this food looks like fresh (used to match cravings)

    void Start() // runs when the level starts
    {
        timeLeft = freshTime;
        if (foodRenderer == null) foodRenderer = GetComponent<SpriteRenderer>();
        freshSprite = foodRenderer.sprite; // remember the fresh look before it can rot
        UpdateClock();
    }

    void Update()
    {
        if (isRotten) return; // rotten food stops counting

        timeLeft -= Time.deltaTime; // counts down in real seconds (stops when game is paused)
        UpdateClock();

        if (timeLeft <= 0f)
        {
            Rot();
        }
    }

    void UpdateClock() // makes the circle match the time left
    {
        if (clockImage == null) return; // food without a circle still works

        float percentLeft = Mathf.Clamp01(timeLeft / freshTime); // 1 = full, 0 = empty
        clockImage.fillAmount = percentLeft;
        clockImage.color = Color.Lerp(rottenColor, freshColor, percentLeft); // green -> red
    }

    void Rot()
    {
        isRotten = true;
        timeLeft = 0f;
        foodRenderer.sprite = rottenSprite;

        if (clockImage != null) clockImage.gameObject.SetActive(false); // hide the circle once rotten
    }

    // Later: a replenish mechanic can go here, e.g. a public method that adds to timeLeft.
    // It only needs to check "if (isRotten) return;" first, so rotten food is never refreshed.
    // Eaten food is already destroyed, so it can't be refreshed either.
}