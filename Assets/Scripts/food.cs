using UnityEngine;

public class food : MonoBehaviour
{
    public float freshness = 30f;          // seconds before it rots
    public Sprite rottenSprite;            // drag the rotten apple sprite here
    public SpriteRenderer foodRenderer;    // the food's own SpriteRenderer

    private float timeLeft;
    public bool isRotten { get; private set; } // a C# thing. doggy script can read it so it knows if food is rotten, only food can change rotten status

    void Start() // runs when the level starts
    {
        timeLeft = freshness;
        if (foodRenderer == null) foodRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isRotten) return; // already rotten, stop counting

        timeLeft -= Time.deltaTime; // counts down in real seconds
        if (timeLeft <= 0f)
        {
            Rot();
        }
    }

    void Rot()
    {
        isRotten = true;
        foodRenderer.sprite = rottenSprite;
    }
}