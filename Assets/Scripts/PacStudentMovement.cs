using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentMovement : MonoBehaviour
{
    public Transform[] waypoints;    // Four waypoints (corners of the block)
    public float speed = 2f;         // Speed of movement
    public AudioSource movingAudio;  // Audio source for movement sound
    public Animator animator;        // Animator for movement animation

    private int currentWaypointIndex = 0;
    private bool isMoving = false;

    void Start()
    {
        // Ensure PacStudent starts at the first waypoint
        transform.position = waypoints[currentWaypointIndex].position;
        StartCoroutine(MoveClockwise());
    }

    void Update()
{
    if (isMoving)
    {
        // Play audio only if it's not already playing
        if (!movingAudio.isPlaying)
        {
            movingAudio.Play();
        }
    }
    else
    {
        // Stop audio if PacStudent stops moving or is colliding with a pellet
        if (movingAudio.isPlaying)
        {
            movingAudio.Stop();
        }
    }
}

    IEnumerator MoveClockwise()
    {
        while (true)
        {
            isMoving = true;

            // Get the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = waypoints[currentWaypointIndex].position;

            // Move linearly to the next waypoint
            float journeyLength = Vector3.Distance(startPosition, targetPosition);
            float startTime = Time.time;

            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                // Distance moved is constant, frame-rate independent
                float distCovered = (Time.time - startTime) * speed;
                float fractionOfJourney = distCovered / journeyLength;

                // Interpolate position between start and target
                transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
                yield return null;  // Wait for the next frame
            }

            // Ensure exact final position is set
            transform.position = targetPosition;

            isMoving = false;

            // Pause before moving to the next point, or adjust based on pellet collision
            yield return new WaitForSeconds(0.5f);
        }
    }
}

