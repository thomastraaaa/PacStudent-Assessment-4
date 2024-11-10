using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public Vector2 gridPosition;
    private Vector2 targetPosition;
    private string lastInput = "";
    private string currentInput = "";
    private bool isLerping = false;
    private float lerpProgress = 0f;

    public ScoreManager scoreManager;

    // Animator, audio, particles, and wall collision
    public Animator animator;
    public AudioSource pelletAudioSource;   
    public AudioSource movementAudioSource;
    public AudioClip eatingPelletClip;
    public AudioClip movementClip;
    public ParticleSystem dustParticles;
    public GameObject wallCollisionEffectPrefab;
    public AudioClip wallCollisionSound;

    void Start()
    {
        gridPosition = transform.position;
        targetPosition = gridPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) lastInput = "up";
        else if (Input.GetKeyDown(KeyCode.A)) lastInput = "left";
        else if (Input.GetKeyDown(KeyCode.S)) lastInput = "down";
        else if (Input.GetKeyDown(KeyCode.D)) lastInput = "right";

        UpdateAnimatorParameters();

        if (!isLerping)
        {
            ProcessMovement();
        }
        else
        {
            LerpMovement();
        }
    }

    // update the movement animation based on the last input
    void UpdateAnimatorParameters()
    {
        if (lastInput == "up")
        {
            animator.SetFloat("Vertical", 1);
            animator.SetFloat("Horizontal", 0);
        }
        else if (lastInput == "down")
        {
            animator.SetFloat("Vertical", -1);
            animator.SetFloat("Horizontal", 0);
        }
        else if (lastInput == "left")
        {
            animator.SetFloat("Horizontal", -1);
            animator.SetFloat("Vertical", 0);
        }
        else if (lastInput == "right")
        {
            animator.SetFloat("Horizontal", 1);
            animator.SetFloat("Vertical", 0);
        }
    }

    // determining movement based on the last input
    void ProcessMovement()
    {
        Vector2 newPosition = gridPosition;

        if (lastInput == "up") newPosition += Vector2.up;
        else if (lastInput == "down") newPosition += Vector2.down;
        else if (lastInput == "left") newPosition += Vector2.left;
        else if (lastInput == "right") newPosition += Vector2.right;

        if (IsWalkable(newPosition))
        {
            currentInput = lastInput;
            SetTargetPosition(newPosition);

            if (IsPellet(newPosition))
            {
                pelletAudioSource.clip = eatingPelletClip;
                pelletAudioSource.Play();
                DestroyPellet(newPosition); 
            }
            else
            {
                movementAudioSource.clip = movementClip;
                movementAudioSource.Play();
            }
        }
        else
        {
            HandleWallCollision();
        }
    }

    void SetTargetPosition(Vector2 newPosition)
    {
        targetPosition = newPosition;
        isLerping = true;
        lerpProgress = 0f;
    }

    void LerpMovement()
    {
        if (!animator.GetBool("isMoving"))
        {
            animator.SetBool("isMoving", true);
            dustParticles.Play();
        }

        lerpProgress += (Time.deltaTime * moveSpeed);
        transform.position = Vector2.Lerp(gridPosition, targetPosition, lerpProgress);

        if (lerpProgress >= 1f)
        {
            isLerping = false;
            gridPosition = targetPosition;

            animator.SetBool("isMoving", false);
            dustParticles.Stop();
        }
    }

    // collision triggers with tags
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            HandleWallCollision();
        }

        else if (other.CompareTag("Pellet"))
        {
            DestroyPellet(other.transform.position);
        }

        else if (other.CompareTag("Cherry"))
        {
            DestroyCherry(other.gameObject);
        }
    }

    // play wall collisions effects, sound clips
    void HandleWallCollision()
    {
        if (wallCollisionEffectPrefab)
        {
            Instantiate(wallCollisionEffectPrefab, transform.position, Quaternion.identity);
        }

        if (wallCollisionSound && pelletAudioSource)
        {
            pelletAudioSource.PlayOneShot(wallCollisionSound, 0.3f);
        }

        targetPosition = gridPosition;
        isLerping = false;
    }

    bool IsWalkable(Vector2 position)
    {
        Collider2D hitCollider = Physics2D.OverlapPoint(position);
        if (hitCollider != null && hitCollider.CompareTag("Wall"))
        {
            return false;
        }
        return true;
    }

    bool IsPellet(Vector2 position)
    {
        return Physics2D.OverlapPoint(position) != null && 
               Physics2D.OverlapPoint(position).CompareTag("Pellet");
    }

    void DestroyPellet(Vector2 position)
    {
        // if tag pellet is found, destroy it and add 10 to score
        Collider2D pelletCollider = Physics2D.OverlapPoint(position);
        if (pelletCollider != null && pelletCollider.CompareTag("Pellet"))
        {
            Destroy(pelletCollider.gameObject);
            scoreManager.AddScore(10);
        }
    }

    void DestroyCherry(GameObject cherry)
    {
        Destroy(cherry);
        scoreManager.AddScore(100); 
    }
}
