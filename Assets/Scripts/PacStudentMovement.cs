using System.Collections;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    public Transform[] movementPoints;
    public float movementSpeed = 2f;
    public AudioSource movementAudio; 
    public Animator characterAnimator;

    private int nextWaypointIndex = 0;
    private bool isCurrentlyMoving = false;
    private bool hasGameStarted = false;

    void Start()
    {
        StartCoroutine(StartWithDelay());
        if (movementPoints != null && movementPoints.Length > 0)
        {
            transform.position = movementPoints[nextWaypointIndex].position;
        }
        characterAnimator.enabled = false;
    }

    void Update()
    {
        if (!hasGameStarted) return;

        if (isCurrentlyMoving)
        {
            if (!movementAudio.isPlaying)
            {
                movementAudio.Play();
            }
            characterAnimator.SetBool("isMoving", true);
        }
        else
        {
            if (movementAudio.isPlaying)
            {
                movementAudio.Stop();
            }
            characterAnimator.SetBool("isMoving", false);
        }
    }

    IEnumerator StartWithDelay()
    {
        yield return new WaitForSeconds(3f);
        characterAnimator.enabled = true;
        hasGameStarted = true;
        StartCoroutine(CycleWaypoints());
    }

    IEnumerator CycleWaypoints()
    {
        while (true)
        {
            isCurrentlyMoving = true;
            nextWaypointIndex = (nextWaypointIndex + 1) % movementPoints.Length;
            Vector3 start = transform.position;
            Vector3 destination = movementPoints[nextWaypointIndex].position;
            float distance = Vector3.Distance(start, destination);
            float startTime = Time.time;

            while (Vector3.Distance(transform.position, destination) > 0.01f)
            {
                float coveredDistance = (Time.time - startTime) * movementSpeed;
                float journeyFraction = coveredDistance / distance;
                transform.position = Vector3.Lerp(start, destination, journeyFraction);
                yield return null;
            }

            transform.position = destination;
            isCurrentlyMoving = false;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
