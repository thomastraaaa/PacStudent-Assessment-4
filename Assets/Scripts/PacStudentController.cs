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
    public Animator animator;
    public AudioSource pelletAudioSource;   
    public AudioSource movementAudioSource;
    public AudioClip eatingPelletClip;
    public AudioClip movementClip;
    public ParticleSystem dustParticles;

    void Start()
    {
        gridPosition = transform.position;
        targetPosition = gridPosition;
    }

    void Update()
    {
        // checks the last input
        if (Input.GetKeyDown(KeyCode.W)) lastInput = "up";
        else if (Input.GetKeyDown(KeyCode.A)) lastInput = "left";
        else if (Input.GetKeyDown(KeyCode.S)) lastInput = "down";
        else if (Input.GetKeyDown(KeyCode.D)) lastInput = "right";

        // animator checks 
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

        if (!isLerping)
        {
            ProcessMovement();
        }
        else
        {
            LerpMovement();
        }
    }

    void ProcessMovement()
    {
        Vector2 newPosition = gridPosition;

        // determine movement based on the pervious input
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
            newPosition = gridPosition;
            if (currentInput == "up") newPosition += Vector2.up;
            else if (currentInput == "down") newPosition += Vector2.down;
            else if (currentInput == "left") newPosition += Vector2.left;
            else if (currentInput == "right") newPosition += Vector2.right;

            if (IsWalkable(newPosition))
            {
                SetTargetPosition(newPosition);
            }
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

    bool IsWalkable(Vector2 position)
    {
        return true;
    }
    bool IsPellet(Vector2 position)
    {
        return false;
    }
}
