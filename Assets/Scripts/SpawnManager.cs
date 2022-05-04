using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //public GameObject enemyPrefab;
    private float timer;    

    // Update is called once per frame
    void Update()
    {
        // spawning enemy and setting active 
        timer = timer + Time.deltaTime;
        if(timer > 1f)
        {
            //Instantiate(enemyPrefab, this.transform.position, Quaternion.identity);
            GameObject fromPool = ObjectPool.Instance.GetEnemyFromPool("Enemy");
            if (fromPool != null)
            {
                fromPool.SetActive(true);
                timer = 0f;
            }
            //Debug.Log(fromPool);            
        }       
    }
}
