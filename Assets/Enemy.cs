using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [NonSerialized]
    public int Health = 100;

    [NonSerialized]
    public FPSActor fpsActor = null;
    [NonSerialized]
    public GameObject selfPrefab;
    [NonSerialized]
    public DropSystem dropSystem;
    [NonSerialized]
    public EnemySpwner enemySpwner;
    // Start is called before the first frame update
    void  Start()
    {
        Health = 100;

        dropSystem = GameObject.FindWithTag("DropSystem").GetComponent<DropSystem>();
        enemySpwner = GameObject.FindWithTag("EnemySpwner").GetComponent<EnemySpwner>();
        fpsActor = GameObject.FindWithTag("Player1").GetComponent<FPSActor>();
        // dropSystem.enemyCount += 1;
        // var prefab = Resources.Load("HealthCore");
        // Instantiate (prefab, transform.position, Quaternion.identity, transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0)
        {
            if (fpsActor.isIncreaseEnemy && dropSystem.enemyCount <= 120)
            {
                for(int i = 1; i <= 2; i++)
                {
                    var newPos = new Vector3(transform.position.x + Random.Range(-3, 3), 0f, transform.position.z + Random.Range(-3, 3));
                    Instantiate(selfPrefab, newPos, Quaternion.identity);
                }
            }
            else
            {
                enemySpwner.level += 3;
                fpsActor.increaseEnemyTime = 0F;
            }
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            Health -= 15;
        }
    }

    public void OnDestroy()
    {
        dropSystem.enemyCount -= 1;
        if (gameObject.tag == "Chest")
        {
            dropSystem.ChestDrop(this.transform.position); 
        }
        else
        {
            dropSystem.Drop(this.transform.position);    
        }
    }
}
