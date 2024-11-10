using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    public GameObject cherryPrefab;
    public float moveSpeed = 5f;
    public float spawnInterval = 10f;

    private float timer;
    private Vector2 screenCenter;

    void Start()
    {
        timer = spawnInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SpawnAndMoveCherry();
            timer = spawnInterval;
        }
    }

    void SpawnAndMoveCherry()
    {
        int spawnSide = Random.Range(0, 4);
        Vector2 spawnPosition = GetSpawnPosition(spawnSide);

        GameObject cherry = Instantiate(cherryPrefab, spawnPosition, Quaternion.identity);

        Vector2 exitPosition = GetExitPosition(spawnPosition);

        StartCoroutine(MoveCherry(cherry, exitPosition));
    }

    System.Collections.IEnumerator MoveCherry(GameObject cherry, Vector2 exitPosition)
    {
        while (Vector2.Distance(cherry.transform.position, exitPosition) > 0.1f)
        {
            cherry.transform.position = Vector2.MoveTowards(cherry.transform.position, exitPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        Destroy(cherry);
    }

    Vector2 GetSpawnPosition(int side)
    {
        float xPos = Camera.main.orthographicSize * Camera.main.aspect + 1;
        float yPos = Camera.main.orthographicSize + 1;

        switch (side)
        {
            case 0: return new Vector2(Random.Range(-xPos, xPos), yPos);
            case 1: return new Vector2(Random.Range(-xPos, xPos), -yPos);
            case 2: return new Vector2(-xPos, Random.Range(-yPos, yPos));
            case 3: return new Vector2(xPos, Random.Range(-yPos, yPos));
            default: return Vector2.zero;
        }
    }

    Vector2 GetExitPosition(Vector2 startPosition)
    {
        if (startPosition.x > 0) return new Vector2(-Camera.main.orthographicSize * Camera.main.aspect - 2, startPosition.y);
        else if (startPosition.x < 0) return new Vector2(Camera.main.orthographicSize * Camera.main.aspect + 2, startPosition.y);
        else if (startPosition.y > 0) return new Vector2(startPosition.x, -Camera.main.orthographicSize - 2);
        else return new Vector2(startPosition.x, Camera.main.orthographicSize + 2);
    }
}
