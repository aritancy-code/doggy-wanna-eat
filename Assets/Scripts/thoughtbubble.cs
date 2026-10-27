using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable] // lets this show up in the Inspector
public class craving
{
    public Sprite foodSprite;
    public int amount = 1;
}

// At the start of the level:
// 1. the little tail bubbles appear one by one
// 2. then the big bubble appears, showing what doggy wants to eat CRAVINGS
// The whole thought bubble follows a marker on doggy's head, so it works wherever doggy starts. HELLYEA
public class thoughtbubble : MonoBehaviour
{
    [Header("What doggy wants this level (max 3)")]
    public craving[] cravings;
    public GameObject rowPrefab;   // one row: food picture + "Xnumber" text
    public GameObject bigBubble;   // the bubble that holds the rows

    [Header("Tail animation")]
    public GameObject[] tailBubbles;   // the little bubbles, in the order they appear (smallest first)
    public float stepDelay = 0.4f;     // seconds between each bubble

    [Header("Follow doggy")]
    public Transform head;             // the marker on doggy's head
    public Vector3 offset;             // how far the bubble sits a fixed distance from the marker

    IEnumerator Start() // start can wait between steps when it's an IEnumerator
    {
        FollowHead(); // jump to doggy straight away

        bigBubble.SetActive(false); // hidden until the tail finishes

        // build the rows now, while the bubble is hidden
        foreach (craving c in cravings)
        {
            GameObject row = Instantiate(rowPrefab, bigBubble.transform);
            row.GetComponentInChildren<Image>().sprite = c.foodSprite;
            row.GetComponentInChildren<TMP_Text>().text = "x" + c.amount;
        }

        // tail animation: hide all the little bubbles, then show them one by one
        foreach (GameObject bubble in tailBubbles)
        {
            bubble.SetActive(false);
        }
        foreach (GameObject bubble in tailBubbles)
        {
            yield return new WaitForSeconds(stepDelay);
            bubble.SetActive(true);
        }

        yield return new WaitForSeconds(stepDelay);
        ShowBubble();
    }

    void LateUpdate() // runs after doggy moves each frame, so the bubble keeps up
    {
        FollowHead();
    }

    void FollowHead()
    {
        if (head == null) return;
        transform.position = head.position + offset;
    }

    public void ShowBubble() // public, so an Animation Event could also call it
    {
        bigBubble.SetActive(true);
    }

    // right-click the thoughtbubble component's title in the Inspector and pick this.
    // remembers where the bubble is right now compared to doggy's head.
    [ContextMenu("Save current position as offset")]
    void SaveOffset()
    {
        if (head == null) return;
        offset = transform.position - head.position;
    }
}