using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSystem : MonoBehaviour
{
    public int EnemyMax = 70;
    public int enemyCount = 0;
    public List<GameObject> drops = new List<GameObject>();

    public int increaseEnemyItemMAX = 3;
    int currentIncreaseEnemyItem = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Drop(Vector3 pos,int dropRate = 10)
    {
        if (Random.Range(0, dropRate) == 0)
        {
            int index = Random.Range(0, drops.Count);
            //増殖アイテムは上限を設定する
            if (index == 0)
            {
                if (currentIncreaseEnemyItem < increaseEnemyItemMAX)
                {
                    currentIncreaseEnemyItem++;
                }
                else
                {
                    return;
                }    
            }
            
            
            Instantiate(drops[index], pos, Quaternion.identity);
        }

        if (Random.Range(0, dropRate) < 6)
        {
            Instantiate(drops[3], pos, Quaternion.identity);
        }
    }
    
    public void ChestDrop(Vector3 pos,int dropRate = 1)
    {
        var rand = Random.Range(0, 5);
        for (int i = 1; i <= 10 + rand; i++)
        {
            var newPos = new Vector3(pos.x+Random.Range(0f, 1.5f), pos.y+0.2f, pos.z + Random.Range(0f, 1.5f));
            int index = Random.Range(1, drops.Count);
            Instantiate(drops[index], newPos, Quaternion.identity);
        }
    }
}
