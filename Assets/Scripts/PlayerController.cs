/* Author: E. Nathan Lee
 * Date: 12/7/2025
 * Description: The main player controller script. This handles movement, animations, "abilities",
 *              death sequences, win sequences, and sound.
 */
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2d; // Rigidbody2D reference

    //==========================
    // Public movement variables
    //==========================
    [Header("Movement Settings")] [Tooltip("Movement speed of the player")]
    public float moveSpeed = 5f;
    public float gravityScale;

    //===========================
    // Private movement variables
    //===========================
    private float movementInput; // Movement input variable

    //===========================
    // Public Animation variables
    //===========================
    [Header("Animation Settings")] [Tooltip("Minimum time for idle fidget animations")] [Range(1f, 10f)]
    public float minTime = 3f;

    [Tooltip("Maximum time for idle fidget animations")] [Range(5f, 15f)]
    public float maxTime = 10f;

    [Tooltip("Animator assignment")]
    public Animator anim;

    //============================
    // Private animation variables
    //============================
    private float timer; // idle fidget anim timer
    float randomNum; // idle fidget anim random number
    private SpriteRenderer sr; // SpriteRenderer reference for flipping

    //============================
    // Public dodge roll variables
    //============================
    [Header("Dodge Roll Settings")] [Tooltip("Dodge roll force")] [Range(5f, 500f)]
    public float dodgeForce = 12f;

    [Tooltip("Dodge roll duration")] [Range(0.1f, 2f)]
    public float dodgeDuration = 0.5f;

    [Tooltip("Top collider assignment to disable during dodge roll")]
    public BoxCollider2D topCollider;

    //=============================
    // Private dodge roll variables
    //=============================
    private bool dodgeLock = false; // Dodge lock to disable movement during dodge roll
    private int dodgeDirection = 1; // Dodge direction based on last faced direction
    private bool isDodging = false; // Dodge state check

    //======================
    // Public jump variables
    //======================
    [Header("Jump Settings")] [Range(5f, 500f)] 
    public float jumpForce = 25f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    //=======================
    // Private jump variables
    //=======================
    private bool jumpPressed = false;
    private bool isGrounded = true;
    //private bool airControlLocked = false;
    private float lockedAirXVelocity;
    private int dashJumpAmount = 1;

    //=======================
    // Public death variables
    //=======================
    [Header("Death Settings")]
    public LayerMask deathLayer;
    public Image fadeOverlay;
    public float fadeDuration = 1f;
    public float deathAnimDelay = 1f;
    public TextMeshProUGUI deathMessage;
    [HideInInspector]
    public bool isDead = false;

    //========================
    // Private Win variables
    //========================
    private bool isWin = false;

    //========================
    // Private Sound variables
    //========================
    [SerializeField] private AudioSource running;
    [SerializeField] private float moveThreshold = 0.1f;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip victorySound;

    // Start is called before the first frame update
    void Start()
    {
        // Game object component references
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        topCollider.enabled = true; // Top collider enabled on start
        gravityScale = rb2d.gravityScale; // Default gravity scale

        // Sound handling
        running.Play();
        running.Pause();
        running.loop = true;

        // Death state
        isDead = false;
    }

    //=======
    // UPDATE
    //=======
    void Update()
    {
        if (isDead || isWin) // Exit for control prevention on condition
        {
            movementInput = 0f;
            return; // Early out if dead
        }
        //==============================
        // Movement Input
        //==============================
        movementInput = Input.GetAxisRaw("Horizontal");

        //===============
        // Movement Sound
        //===============
        bool isMoving = Mathf.Abs(movementInput) > moveThreshold;
        bool shouldPlay = isGrounded && isMoving;

        if (shouldPlay && !isDead) // Is moving and not dead, play sound
        {
            running.UnPause();
        }
        else
        {
            running.Pause();
        }


        //==============================
        // Animation Trigger and Timing
        //==============================
        anim.SetFloat("Speed", Mathf.Abs(movementInput)); // Speed paramater for running animation

        timer -= Time.deltaTime; // Timer for idle fidget animations

        if (timer <= 0f)
        {
            randomNum = Random.Range(0f, 11f); // Random number for fidget anim selection
            anim.SetFloat("IdlePoint", randomNum); // Setting the IdlePoint param
            ResetTimer(); // Resetting timer
        }

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0); // Testing current animation state

        if (!stateInfo.IsName("Idle")) // Resetting IdlePoint param if not in idle default for fidget loop prevention
        {
            anim.SetFloat("IdlePoint", 0f); // Set to 0 for loop prevention
        }

        //==============================
        // Dodge Roll Functionality
        //==============================
        if (!dodgeLock) // Dodge lock check
        {
            if (movementInput > 0) dodgeDirection = 1; // (Right) Setting dodge direction based on last direction faced
            if (movementInput < 0) dodgeDirection = -1; //  (Left) Setting dodge direction based on last direction faced
        }
        
        if (Input.GetButtonDown("Dodge") && !isDodging )  // Dodge input / state check for animation and action kickoff
        {
            anim.SetBool("dodge", true);
            StartCoroutine(DodgeRoll());
            
        }

        //=================== 
        // Jump Functionality
        //===================
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpPressed = true;
        }
        //======================== 
        // Dash-Jump Functionality
        //========================
        if (Input.GetButtonDown("DodgeJump") && !isDodging && dashJumpAmount == 1) 
        {
            jumpPressed = true;
            anim.SetBool("dodge", true);
            StartCoroutine(DodgeRoll());
            dashJumpAmount--; // Added to limit dash-jump to once per air time
        }

        if (dashJumpAmount == 0 && isGrounded) // Reset dash jump amount when grounded
        {
            dashJumpAmount = 1;
        }
    }

    void FixedUpdate()
    {
        //=========================
        // Dodge roll movement lock
        //=========================
        if (!dodgeLock) // Movement disabled during dodge roll
        {
            rb2d.velocity = new Vector2(movementInput * moveSpeed, rb2d.velocity.y);
        }

        //======================
        // Sprite direction flip
        //======================
        if (movementInput > 0) // Sprite flip facing right
        {
            sr.flipX = false;
        } 
        else if (movementInput < 0) // Else if sprite flip facing left
        {
            sr.flipX = true;
        }

        //=====================
        // Jump Grounding Check
        //=====================
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (!isDodging)
        {            
            rb2d.velocity = new Vector2(movementInput * moveSpeed, rb2d.velocity.y);
        }

        if (jumpPressed)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);

            lockedAirXVelocity = rb2d.velocity.x;
            //airControlLocked = true;

            jumpPressed = false;
        }
    }

    void ResetTimer() // Reset the timer for idle fidget animations
    {
        timer = Random.Range(minTime, maxTime);
    }

    //===============================================================
    // Dodge Roll Coroutine for proper timing and collider management
    //===============================================================
    private System.Collections.IEnumerator DodgeRoll() 
    {
        isDodging = true;
        dodgeLock = true;
        topCollider.enabled = false;
        float verticalVelocity = rb2d.velocity.y;

        if (isGrounded)
        {
            verticalVelocity = 0f;
            rb2d.gravityScale = 0f;
        } 
        else
        {
            verticalVelocity *= 0.70f; // Reduce velocity by 30% for air dodge
        }

            //rb2d.gravityScale = 0f; // Disable gravity during dodge roll
            float dodgeY = rb2d.position.y;

        rb2d.velocity = new Vector2(dodgeDirection * dodgeForce, 0f);
        rb2d.position = new Vector2(rb2d.position.x, dodgeY); // Lock Y position

        yield return new WaitForSeconds(dodgeDuration);
        

        isDodging = false; // Clear dodge state
        dodgeLock = false; // Unlock movement after dodge
        anim.SetBool("dodge", false); // Turn off dodge animation
        rb2d.gravityScale = gravityScale; // Re-enable gravity after dodge roll

        yield return new WaitUntil(CanStandUp);

        topCollider.enabled = true; // Re-enable top collider after dodge roll

        if (!isGrounded) // Lock air control if not grounded after dodge roll
        {
            lockedAirXVelocity = rb2d.velocity.x;
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
            //airControlLocked = true;
        }
    }

    // Function for preventing top collider clipping when standing up after dodge roll
    bool CanStandUp()
    {
        Vector2 size = topCollider.bounds.size;
        Vector2 center = topCollider.bounds.center;
        Collider2D hit = Physics2D.OverlapBox(
            center,
            size,
            0f,
            groundLayer
        );

        return hit == null;
    }

    // Death detection
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Death"))
        {
            Die();
        }
    }

    // Death function
    void Die()
    {
        if (isDead || isWin) return; // Early out if already dead
        running.Pause();
        isDead = true;
        movementInput = 0f;
        anim.SetBool("dead", true);

        StartCoroutine(DeathSequence());

    }

    // Timed death sequence
    private IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(deathAnimDelay);

        float t = 0f;
        Color c = fadeOverlay.color;
        Color c1 = deathMessage.color;
        MusicManager.Instance.PlaySfx(deathSound);
        MusicManager.Instance.musicSource.Pause();
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            c1.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadeOverlay.color = c;
            deathMessage.color = c1;
            yield return null;
        }

        c.a = 1f;
        c1.a = 1f;
        fadeOverlay.color = c;
        deathMessage.color = c1;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterDeath();
        }

        yield return new WaitForSeconds(2f);

        MusicManager.Instance.musicSource.UnPause();
        isDead = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Victory function
    public void EnterVictoryState()
    {
        isWin = true;
        rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
        anim.SetTrigger("Victory");
        MusicManager.Instance.PlaySfx(victorySound);
    }

}
