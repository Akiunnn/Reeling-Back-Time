using UnityEngine;

public class fishingBarScript : MonoBehaviour
{
    public float targetTime = 4.0f;

    public GameObject p1;
    public GameObject p2;
    public GameObject p3;
    public GameObject p4;
    public GameObject p5;
    public GameObject p6;
    public GameObject p7;

    [HideInInspector] public bool onFish;
    public PlayerMovement playerMovement;
    public GameObject bobber;

    void Start()
    {
        // optionally hide the bar at start (if you spawn active in scene)
        // gameObject.SetActive(false);
    }

    void Update()
    {
        if (onFish)
            targetTime += Time.deltaTime;
        else
            targetTime -= Time.deltaTime;

        targetTime = Mathf.Clamp(targetTime, 0f, 7f);

        if (onFish && targetTime >= 3f && targetTime <= 5f)
        {
            if (playerMovement != null) playerMovement.reelingBackTimeWon();
            FinishFishing(true);
            DestroyBobberIfExists();
            targetTime = 4.0f;
        }
        else if (targetTime <= 0f || targetTime >= 7f)
        {
            if (playerMovement != null) playerMovement.reelingBackTimeLossed();
            FinishFishing(false);
            DestroyBobberIfExists();
            targetTime = 4.0f;
        }

        if (p1 != null) p1.SetActive(targetTime >= 1f);
        if (p2 != null) p2.SetActive(targetTime >= 2f);
        if (p3 != null) p3.SetActive(targetTime >= 3f);
        if (p4 != null) p4.SetActive(targetTime >= 4f);
        if (p5 != null) p5.SetActive(targetTime >= 5f);
        if (p6 != null) p6.SetActive(targetTime >= 6f);
        if (p7 != null) p7.SetActive(targetTime >= 7f);
    }

    private void DestroyBobberIfExists()
    {
        var bob = GameObject.Find("bobber(Clone)");
        if (bob != null) Destroy(bob);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("fish"))
        {
            onFish = true;
            // ensure bar is visible when entering fishing area
            gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("fish"))
        {
            onFish = false;
        }
    }

    public void FinishFishing(bool win)
    {
        onFish = false;
        targetTime = 4f;

        if (p1 != null) p1.SetActive(false);
        if (p2 != null) p2.SetActive(false);
        if (p3 != null) p3.SetActive(false);
        if (p4 != null) p4.SetActive(false);
        if (p5 != null) p5.SetActive(false);
        if (p6 != null) p6.SetActive(false);
        if (p7 != null) p7.SetActive(false);

        // hide the bar on finish
        gameObject.SetActive(false);

        if (playerMovement != null && playerMovement.fishBar != null)
            playerMovement.fishBar.SetActive(false);
    }
}
