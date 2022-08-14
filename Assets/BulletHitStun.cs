using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitStun : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit(GameObject player)
    {
        player.GetComponent<FPSActor>().isStunned = true;
        player.GetComponent<FPSActor>().stunTime = 2f;
    }
    
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player1")
        {
            Hit(collision.gameObject);
        }
    }
}
