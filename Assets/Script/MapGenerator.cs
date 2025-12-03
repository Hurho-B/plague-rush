using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] mapChunks;
    public Transform spawnPoint; // Where the chunk will appear
    private GameObject oldMap;
    private GameObject newMap;

    void Start()
    {
        SpawnStartChunk();
    }

    void SpawnStartChunk()
    {
        int rand = Random.Range(0, mapChunks.Length);
        newMap = Instantiate(mapChunks[rand], spawnPoint.position, Quaternion.identity);     
    }
    public void SpawnRandomChunk(Transform spawnPointGiven)
    {
        if (newMap)
        {
            oldMap = newMap;
        }
        int rand = Random.Range(0, mapChunks.Length);
        newMap =Instantiate(mapChunks[rand], spawnPointGiven.position, Quaternion.identity);
    }

    public IEnumerator DeleteOldMap()
    {
        yield return new WaitForSeconds(4);
        Destroy(oldMap);
        Debug.Log("map3");
    }
    public void Delete()
    {
        Debug.Log("map2");
        StartCoroutine(DeleteOldMap());
    }

}
