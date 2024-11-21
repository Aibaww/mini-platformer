using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    public LayerMask groundLayer; 

    private bool isGrounded;
    private bool isTouchingWall;

    private AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip hitSound;
    public AudioClip winSound;

    public Camera mainCamera;
    public Transform spawnPoint;

    public Rigidbody2D rb;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        jumpSound = Resources.Load<AudioClip>("jump");
        hitSound = Resources.Load<AudioClip>("hit");
        winSound = Resources.Load<AudioClip>("win");
    }

    void Update() {
        rb = GetComponent<Rigidbody2D>();

        if (isTouchingWall) {
            MaybeClimb();
        }
        Move();
        MaybeJump();
        CheckOffScreen();
    }

    void Move() {
        float x = Input.GetAxisRaw("Horizontal");

        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        if (viewportPosition.x <= 0.01 && x < 0) {
            x = 0;
        }

        if (!isTouchingWall) {
            rb.velocity = new Vector2(x * speed, rb.velocity.y);
        }
    }

    void MaybeJump() {
        float jump = Input.GetAxis("Jump");
        if (jump > 0 && isGrounded && !isTouchingWall) {
            audioSource.PlayOneShot(jumpSound);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
        } 
    }

    void MaybeClimb() {
        float climb = Input.GetAxisRaw("Climb");
        float y = Input.GetAxisRaw("Vertical");

        if (climb > 0 && y != 0) {
            rb.velocity = new Vector2(rb.velocity.x, y * speed);

            // scan for ground within a small distance
            RaycastHit2D groundCheck = Physics2D.Raycast(transform.position + Vector3.up * 0.1f, Vector2.right * Mathf.Sign(rb.velocity.x), 0.1f, groundLayer);
            if (groundCheck.collider != null) {
                isTouchingWall = false;
                isGrounded = true;
            } else {
                rb.velocity = new Vector2(rb.velocity.x, y * speed); // keep climbing
            }
        } else if (climb > 0) {
            rb.velocity = new Vector2(0, 0); 
        }
    }

    void CheckOffScreen() {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
        if (viewportPosition.y < 0 || viewportPosition.y > 1) {
            Respawn();
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Platform")) {
            isGrounded = true;
        } else if (other.gameObject.CompareTag("Wall")) {
            isTouchingWall = true;
        } else if (other.gameObject.CompareTag("Spikes")) {
            Respawn();
        } else if (other.gameObject.name == "WinZone") {
            Win();
        }
    }

    void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.CompareTag("Wall")) {
            isTouchingWall = false;
        }
    }

    void Respawn() {
        audioSource.PlayOneShot(hitSound);
        DeathCounter cnt = FindObjectOfType<DeathCounter>();
        cnt.AddDeath();
        transform.position = spawnPoint.position;
        rb.velocity = Vector2.zero;
    }

    void Win() {
        WinScreen winScreen = FindObjectOfType<WinScreen>();
        winScreen.Display();
        MusicPlayer musicPlayer = FindObjectOfType<MusicPlayer>();
        musicPlayer.Stop();
        audioSource.PlayOneShot(winSound);
        Time.timeScale = 0f;
    }
}
