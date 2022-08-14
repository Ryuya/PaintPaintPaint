using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HealthCore : MonoBehaviour
{
    public int HP = 100;

    public int MAXHP = 100;
    public DropSystem dropSystem;
    public EnemySpwner enemySpwner;
    [NonSerialized]
    public FPSActor fpsActor;
    // Start is called before the first frame update
    
    
    void Start()
    {
        
        dropSystem = GameObject.FindWithTag("DropSystem").GetComponent<DropSystem>();
        enemySpwner = GameObject.FindWithTag("EnemySpwner").GetComponent<EnemySpwner>();
        fpsActor = GameObject.FindWithTag("Player1").GetComponent<FPSActor>();
        HP = MAXHP;
        dropSystem.enemyCount += 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(HP > 0)
        {
            
        }
        else
        {
            if (fpsActor.isIncreaseEnemy && dropSystem.enemyCount <= 120)
            {
                for(int i = 1; i <= 2; i++)
                {
                    var newPos = new Vector3(transform.position.x + Random.Range(-3, 3), 0f, transform.position.z + Random.Range(-3, 3));
                    Instantiate(transform.parent.gameObject, newPos, Quaternion.identity);
                }
            }
            else
            {
                enemySpwner.level += 3;
                fpsActor.increaseEnemyTime = 0F;
            }    
            
            Destroy(transform.parent.gameObject); 
        }
    }
        
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerBullet")
        {
            if(HP > 0)
            {
                HP -= 15;
                Destroy(other.gameObject);
            }
            else
            {
                
            }
        }
    }
    
    public void SetMaxHealth(int MAXHP = 100)
    {
        HP = MAXHP;
    }
    
    public void OnDestroy()
    {
        
        dropSystem.enemyCount -= 1;
        
        if (gameObject.transform.parent.tag == "Chest")
        {
            dropSystem.ChestDrop(this.transform.position); 
        }
        else
        {
            dropSystem.Drop(this.transform.position);    
        }
        Destroy(gameObject);
    }
}
