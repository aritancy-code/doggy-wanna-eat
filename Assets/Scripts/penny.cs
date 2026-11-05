using System.Collections;
using UnityEngine;

/*penny will hiss when doggy gets close. when too close, they will fight and show an animation for 5 seconds. penny disappears after a fight.*/
public class penny : MonoBehaviour
{
    [Header("Doggy")]
    public doggy dog;                  // doggy_0 goes here

    [Header("Distances (select penny to see the circles)")]
    public float hissDistance = 3f;    // yellow circle: penny hisses inside this
    public float fightDistance = 1f;   // red circle: they fight inside this

    [Header("Hiss")]
    public Sprite angrySprite;         // penny angry hiss sprite
    public AudioClip hissSound;        // hiss sound

    [Header("Fight")]
    public float fightTime = 5f;       // duration of fight
    public SpriteRenderer dustCloud;   // DUST CLOUD OBJECT IS A CHILD OF PENNY'S SO THAT IT CAN FOLLOW (starts hidden)
    public Sprite[] dustFrames;        // the dust cloud images, played back and forth
    public float frameTime = 0.15f;    // seconds each dust image shows for

    private SpriteRenderer pennyRenderer;
    private Sprite calmSprite;         // regular penny
    private bool isHissing;
    private bool fighting;             // so the fight only happens once

    void Start()
    {
        pennyRenderer = GetComponent<SpriteRenderer>();
        calmSprite = pennyRenderer.sprite; // save calm penny
        if (dog == null) dog = FindAnyObjectByType<doggy>(); // no doggy dragged in? find him in the level automatically
        dustCloud.gameObject.SetActive(false);
    }

    void Update()
    {
        if (fighting || dog == null) return;

        float distance = Vector2.Distance(transform.position, dog.transform.position); // how far doggy is

        if (distance <= fightDistance) // too close = FIGHT
        {
            StartCoroutine(Fight());
            return;
        }

        bool shouldHiss = distance <= hissDistance;

        if (shouldHiss && !isHissing && hissSound != null) // just started hissing -> play the sound once
        {
            AudioSource.PlayClipAtPoint(hissSound, transform.position);
        }

        isHissing = shouldHiss;

        if (angrySprite != null)
        {
            pennyRenderer.sprite = isHissing ? angrySprite : calmSprite; // angry when hissing, calm otherwise
        }
    }

    IEnumerator Fight()
    {
        fighting = true;

        // freeze doggy and make them both invisible
        dog.enabled = false;                       // stops doggy's movement and controls
        dog.dogRB.linearVelocity = Vector2.zero;   // stop him sliding
        dog.dogRB.simulated = false;               // pause his physics (no falling, no eating)
        dog.doggyRederer.enabled = false;          // hide doggy
        pennyRenderer.enabled = false;             // hide penny

        // put the dust cloud in between them and show it
        dustCloud.transform.position = (transform.position + dog.transform.position) / 2f;
        dustCloud.gameObject.SetActive(true);

        // flip between the dust images until the fight time is up
        float timer = 0f;
        int frame = 0;
        while (timer < fightTime)
        {
            dustCloud.sprite = dustFrames[frame];
            frame = (frame + 1) % dustFrames.Length; // 0, 1, 0, 1...
            yield return new WaitForSeconds(frameTime);
            timer += frameTime;
        }

        // fight over: doggy comes back, penny is gone
        dustCloud.gameObject.SetActive(false);
        dog.doggyRederer.enabled = true;
        dog.dogRB.simulated = true;
        dog.enabled = true;

        Destroy(gameObject); // bye penny
    }

    // draws the hiss and fight circles in the Scene view when penny is selected (not in the game)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, hissDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fightDistance);
    }
}