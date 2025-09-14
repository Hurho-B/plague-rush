using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CreateWires : MonoBehaviour
{
    public GameObject[] wires;
    public GameObject[] wiresEnd;
    public Transform[] wireParent;
    public Transform[] wireParentEnd;
    private List<int> availableIndices;
    private List<int> availableIndices2;

    private void Start()
    {
        // Initialize and shuffle indices
        availableIndices = Enumerable.Range(0, wireParent.Length).ToList();
        Shuffle(availableIndices);

        SpawnWires();
    }

    void SpawnWires()
    {
        foreach (GameObject wirePrefab in wires)
        {
            int index = GetNextParentIndex();
            Instantiate(wirePrefab, wireParent[index]);
        }

     /*   foreach (GameObject wireEndPrefab in wiresEnd)
        {
            int index = GetNextParentIndex();
            Instantiate(wireEndPrefab, wireParent[index]);
        }*/
    }

    int GetNextParentIndex()
    {
        if (availableIndices.Count > 1)
        {
            int index = availableIndices[0];
            availableIndices.RemoveAt(0);
            return index;
        }
        else
        {
            // Only one left—reuse it
            return availableIndices[0];
        }
    }

    void Shuffle(List<int> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            int value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
