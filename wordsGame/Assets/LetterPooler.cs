using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterPooler: Singleton<LetterPooler>
{
    [SerializeField] private int size;
    [SerializeField] private GameObject letterPrefab;
    private Queue<GameObject> availableObjects=new Queue<GameObject>();


    protected override void Awake()
    {
        GrowPool();
    }
    
    public GameObject GetFromPool()
    {
        if (availableObjects.Count == 0)
        {
            GrowPool();
        }

        GameObject res = availableObjects.Dequeue();
        return res;
    }

    private void GrowPool()
    {
        for (int i = 0; i < size; i++)
        {
            GameObject added = Instantiate(letterPrefab);

            AddToPool(added);
        }
    }

    public void AddToPool(GameObject added)
    {
        added.transform.SetParent(transform);
        added.transform.localPosition=Vector3.zero;
        added.transform.localScale=Vector3.one;
        added.SetActive(false);
        availableObjects.Enqueue(added);
    }

    
}
