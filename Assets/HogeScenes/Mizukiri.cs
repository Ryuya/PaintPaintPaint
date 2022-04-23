using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mizukiri : MonoBehaviour
{
    public Vector3 prevVelocity;
    public Rigidbody rb;
    public int count = 3; 
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("floor"))
        {
            count = count - 1;
            // transform.position = new Vector3(transform.position.x, transform.position.y+, transform.position.z);
            prevVelocity = prevVelocity +Vector3.up * 1.3f;
            rb.AddForce(prevVelocity,ForceMode.Impulse);
            Debug.Log(count + "count");
            if (count <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("floor"))
        {
            count = count - 1;
            // transform.position = new Vector3(transform.position.x, transform.position.y+1.2f, transform.position.z);
            prevVelocity = prevVelocity +Vector3.up * 1.3f;
            rb.AddForce(prevVelocity,ForceMode.Impulse);
            Debug.Log(count + "count");
            if (count <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
