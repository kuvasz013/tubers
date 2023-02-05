using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> playerPrefabs;
    [SerializeField] Vector3 startPosition;
    [SerializeField] Vector3 endPosition;
    [SerializeField] float spawnTimeInSeconds;

    private float _t;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPosition;
        SetDestination(endPosition, spawnTimeInSeconds);
        StartCoroutine(InstantiatePlayers(spawnTimeInSeconds / 4));
    }

    // Update is called once per frame
    void Update()
    {
        _t += Time.deltaTime / spawnTimeInSeconds;
        transform.position = Vector3.Lerp(startPosition, endPosition, _t);
    }

    private void SetDestination(Vector3 destination, float time)
    {
        _t = 0;
        startPosition = transform.position;
        spawnTimeInSeconds = time;
        endPosition = destination;
    }

    IEnumerator InstantiatePlayers(float seconds)
    {
        PlayerInput.Instantiate(playerPrefabs[(int)Mathf.Floor(Random.value * playerPrefabs.Count)], controlScheme: "wasd", pairWithDevice: Keyboard.current);
        yield return new WaitForSeconds(seconds);
        PlayerInput.Instantiate(playerPrefabs[(int)Mathf.Floor(Random.value * playerPrefabs.Count)], controlScheme: "arrows", pairWithDevice: Keyboard.current);
    }
}
