using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseEnemyPotion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player1")
        {
            other.gameObject.GetComponent<FPSActor>().IncreaseEnemyPotion();
            Destroy(transform.parent.gameObject);
        }
    }
}
