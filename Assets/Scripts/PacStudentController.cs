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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            HandleWallCollision();
        }
    }

    void HandleWallCollision()
    {
        if (wallCollisionEffectPrefab)
        {
            Instantiate(wallCollisionEffectPrefab, transform.position, Quaternion.identity);
        }

        if (wallCollisionSound && pelletAudioSource)
        {
            pelletAudioSource.PlayOneShot(wallCollisionSound);
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
        return false;
    }
}
