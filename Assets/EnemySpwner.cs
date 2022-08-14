using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class EnemySpwner : MonoBehaviour
{
    public List<GameObject> enemyList;
    [NonSerialized]
    public float SpawnTime = 2.8f;
    public float timer = 0;
    public int level = 0;
    public DropSystem dropSystem;
    // Start is called before the first frame update
    void Start()
    {
        dropSystem = GameObject.FindWithTag("DropSystem").GetComponent<DropSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= SpawnTime)
        {
            timer = 0;
            Spawn();
        }
    }

    public void Spawn()
    {
        
        string name = SceneManager.GetActiveScene().name;
        switch (name)
        {
            case "CharacterStage1":
                level = 1;
                break;
            case "CharacterStage2":
                level = 2;
                break;
            case "CharacterStage3":
                level = 3;
                break;
            case "CharacterStage4":
                level = 4;
                break;
            default:
                level = 7;
                break;
        }
        for (int i = 0; i < level; i++)
        {
            float x = 0;
            float z = 0;
            if (Random.Range(0, 2) == 0)
            {
                x = transform.position.x + Random.Range(8f, 15f);
            }
            else
            {
                x = transform.position.x - Random.Range(8f, 15f);
            }


            if (Random.Range(0, 2) == 0)
            {
                z = transform.position.z + Random.Range(8f, 15f);
            }
            else
            {
                z = transform.position.z - Random.Range(8f, 15f);
            }

            var newPos = new Vector3(x, 0, z);
            if (dropSystem.enemyCount <= 70)
            {
                Instantiate(enemyList[Random.Range(0, enemyList.Count)], newPos, Quaternion.identity);    
            }
            
        }
    }

    public void BlueRush()
    {
        for (int i = 0; i < 20; i++)
        {
            float x = 0;
            float z = 0;
            if (Random.Range(0, 2) == 0)
            {
                x = transform.position.x + Random.Range(8f, 15f);
            }
            else
            {
                x = transform.position.x - Random.Range(8f, 15f);
            }


            if (Random.Range(0, 2) == 0)
            {
                z = transform.position.z + Random.Range(8f, 15f);
            }
            else
            {
                z = transform.position.z - Random.Range(8f, 15f);
            }

            var newPos = new Vector3(x, 0, z);
            
            Instantiate(enemyList[2], newPos, Quaternion.identity);
        }
    }
}
