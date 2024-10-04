using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentMovement : MonoBehaviour
{
    public Transform[] waypoints; 
    public float speed = 2f;        
    public AudioSource movingAudio; 
    public Animator animator;      

    private int currentWaypointIndex = 0;
    private bool isMoving = false;
    private bool gameStarted = false;

    void Start()
    {
        StartCoroutine(DelayGameStart());
        transform.position = waypoints[currentWaypointIndex].position;
        animator.enabled = false;
    }

    void Update()
    {
        if (!gameStarted)
        {
            return;
        }

        if (isMoving)
        {
            if (!movingAudio.isPlaying)
            {
                movingAudio.Play();
            }

            animator.SetBool("isMoving", true);
        }
        else
        {
            if (movingAudio.isPlaying)
            {
                movingAudio.Stop();
            }

            animator.SetBool("isMoving", false);
        }
    }

    IEnumerator DelayGameStart()
    {
        yield return new WaitForSeconds(3f);
        animator.enabled = true;
        gameStarted = true;
        StartCoroutine(MoveClockwise());
    }

    IEnumerator MoveClockwise()
    {
        while (true)
        {
            isMoving = true;
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = waypoints[currentWaypointIndex].position;
            float journeyLength = Vector3.Distance(startPosition, targetPosition);
            float startTime = Time.time;

            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                float distCovered = (Time.time - startTime) * speed;
                float fractionOfJourney = distCovered / journeyLength;
                transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
                yield return null; 
            }

            transform.position = targetPosition;
            isMoving = false;
            yield return new WaitForSeconds(0.5f);
        }
    }
}


