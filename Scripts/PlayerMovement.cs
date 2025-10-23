using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    [SerializeField] private Animator animator;
    [SerializeField] private Vector3 fishBarOffset = new Vector3(1.5f, 0.5f, 0f);

    public bool isFishing;
    public bool poleBack;
    public bool throwBobber;
    public Transform fishingPoint;
    public GameObject bobber;

    public float timeTillCatch = 0.0f;
    public GameObject fishBar;        // assign the fishBar GameObject in Inspector

    private Vector2 facing = Vector2.down;
    public bool winnerAnim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isFishing = false;
        if (fishBar != null) fishBar.SetActive(false);
        throwBobber = false;
        timeTillCatch = 0.0f;
    }

    void Update()
    {
        // Movement application
        rb.linearVelocity = moveInput * moveSpeed;

        // Update facing when moving
        if (moveInput != Vector2.zero)
        {
            facing = moveInput.normalized;
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
            // Keep LastInput as facing to maintain orientation when idle/fishing
            animator.SetFloat("LastInputX", facing.x);
            animator.SetFloat("LastInputY", facing.y);
        }

        // Start "swing back" (charging throw) with Space
        if (Input.GetKeyDown(KeyCode.Space) && !isFishing && !winnerAnim)
        {
            poleBack = true;
            // store facing into animator for use by the swing animation
            animator.SetFloat("SavedFacingX", facing.x);
            animator.SetFloat("SavedFacingY", facing.y);
        }

        // While charging, increment targetTime (if you use it visually)
        if (poleBack)
        {
            animator.Play("playerSwingBack");
            // if you track some targetTime visually for how far to throw, do it here
        }

        // release to cast
        if (Input.GetKeyUp(KeyCode.Space) && !isFishing && !winnerAnim)
        {
            poleBack = false;
            isFishing = true;
            throwBobber = true;

            // instantiate bobber at an offset based on facing direction
            Vector3 dir = new Vector3(Mathf.Round(facing.x), Mathf.Round(facing.y), 0f);
            if (dir == Vector3.zero) dir = Vector3.up * -1f; // default down if ambiguous

            float throwDistance = 1.5f; // tweak as needed
            Vector3 temp = dir * throwDistance;

            if (throwBobber && bobber != null && fishingPoint != null)
            {
                Instantiate(bobber, fishingPoint.position + temp, fishingPoint.rotation, null);
                throwBobber = false;
            }

            // ensure fishBar becomes visible and positioned
            if (fishBar != null)
            {
                fishBar.SetActive(true);
                fishBar.transform.position = transform.position + fishBarOffset;
            }

            timeTillCatch = 0f; // reset catch timer
        }

        // Fishing state
        if (isFishing)
        {
            timeTillCatch += Time.deltaTime;
            if (timeTillCatch >= 3f)
            {
                if (fishBar != null) fishBar.SetActive(true);
            }

            // play fishing animation - choose direction using facing values
            animator.SetBool("isFishing", true);
            animator.SetFloat("FishingDirX", facing.x);
            animator.SetFloat("FishingDirY", facing.y);
        }
        else
        {
            animator.SetBool("isFishing", false);
        }

        // cancel fishing (P key in your previous code)
        if (Input.GetKeyDown(KeyCode.P) && timeTillCatch <= 3f)
        {
            animator.Play("playerStill");
            poleBack = false;
            throwBobber = false;
            isFishing = false;
            timeTillCatch = 0f;
            if (fishBar != null) fishBar.SetActive(false);
        }
    }

    void LateUpdate()
    {
        // Keep the fishBar positioned over the player (if it's world-space)
        if (fishBar != null && fishBar.activeSelf)
        {
            fishBar.transform.position = transform.position + fishBarOffset;
        }
    }

    public void reelingBackTimeWon()
    {
        animator.Play("playerWonFish");
        if (fishBar != null) fishBar.SetActive(false);
        poleBack = false;
        throwBobber = false;
        isFishing = false;
        timeTillCatch = 0f;
    }

    public void reelingBackTimeLossed()
    {
        animator.Play("playerStill");
        if (fishBar != null) fishBar.SetActive(false);
        poleBack = false;
        throwBobber = false;
        isFishing = false;
        timeTillCatch = 0f;
    }

    // Input system move method (if you're using the new Input System)
    public void Move(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }

        moveInput = context.ReadValue<Vector2>();
        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);
    }
}
