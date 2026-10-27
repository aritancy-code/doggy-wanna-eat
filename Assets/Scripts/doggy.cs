using UnityEngine;

public class doggy : MonoBehaviour
{
    public Rigidbody2D dogRB; //reference to rigid body which means needs physics to keep the gravity
    public float speed;    // how fast to move left/right
    public float direction;    // moving left/right
    public SpriteRenderer doggyRederer;    // ref to SpriteRenderer so that itll flip it along x
    public float jumpForce;

    public LayerMask groundLayer; // lets us label things as ground. we want doggy only to jump when in contact with ground
    private bool isGround;

    public GameObject badEndingScreen; // drag the rottenend panel here

    //Update called once per frame
    void Update()
    {
        direction = Input.GetAxisRaw("Horizontal"); // -1 is left, 1 is right
        if (direction < 0)
        {
            doggyRederer.flipX = true; // go into sprite renderer component and look for the thingy to flip it which is flipx
        }
        else if (direction > 0)
        {
            doggyRederer.flipX = false;
        }

        if (isGround == true && Input.GetButton("Jump")) //default jump in unity is space
        {
            dogRB.linearVelocity = Vector2.up * jumpForce;
        }
    }

    /* 1 << (bitshift) collision.gameObject.layer means 1 bitshift left by 6.
     * the layer index i called Ground which has index 6 is now converted into a bit index
     * this is important because type LayerMask is saved as a 32 bit thing
     * so it's the safest way to check if it's actually correct because i could have a layer mask that is ground but is like a moving platform
     * aka multiple layermasks
     * i prolly wont do that but this is robust i guess
    */

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGround = false;
        }
    }

    //FixedUpdate runs at 50frames/second
    void FixedUpdate() // doesnt needd to be private actually
    {
        //put player movement here because we only want player to move at one speed only always
        dogRB.linearVelocity = new Vector2(direction * speed, dogRB.linearVelocity.y);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Food")) // if player collide with object that is tagged food go to food script
        {
            food eaten = other.gameObject.GetComponent<food>(); // add other things like sfx and other types of food later

            if (eaten != null && eaten.isRotten) // ate rotten food = bad ending
            {
                ShowBadEnding();
            }

            Destroy(other.gameObject);
        }
    }

    void ShowBadEnding()
    {
        badEndingScreen.SetActive(true); // turns on the ending screen and its buttons
        Time.timeScale = 0f; // pauses the game so doggy stops moving
    }
}