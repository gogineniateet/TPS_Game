using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectPool : MonoBehaviour
{
    public int number;
    public float spawnRadius;

    public static ObjectPool Instance;
    public GameObject enemyPrefab;
    private Vector3 result;
    
    public List<GameObject> pool = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        return;
    }

    // Start is called before the first frame update
    void Start()
    {
        AddToPool(number);
    }
    
    // Adding enemy to pool and setting to inactive
    public void AddToPool(int number)
    {
        for (int i = 0; i < number; i++)
        {
            Vector3 randomPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPosition, out hit, 10f, NavMesh.AllAreas))
            {
                result = hit.position;
                GameObject temp = Instantiate(enemyPrefab, result, Quaternion.identity);
                temp.SetActive(false);
                pool.Add(temp);
            }
            else
                i--;
        }
    }


    public GameObject GetEnemyFromPool(string name)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if(pool[i].gameObject.tag == name)
            {
                if(!pool[i].activeInHierarchy)
                {
                    return pool[i];
                }
            }
        }
        return null;
    }
}
