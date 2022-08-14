using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class GhostRed : MonoBehaviour
{
    private Animator animator;
    public NavMeshAgent agent;
    public GameObject player;
    public GameObject ghost;
    
    public bool isTargetset = false;
    public bool isAttack = false;
    
    public float attackRange = 5f;
    public float attackTime = 1f;
    public float attackTimer = 0f;
    public float attackDelay = 1f;
    
    public float stopTime = 2.3f;

    public float playerMoveSpeed = 0.5f;

    public GameObject bulletPrefab;

    public GameObject MuzzlePoint;

    private Vector3 defaultVector;
    [NonSerialized]
    public FPSActor fpsActor = null;

    public GameObject selfPrefab;
    [NonSerialized]
    public DropSystem dropSystem; 
    [NonSerialized]
    public EnemySpwner enemySpwner;
    
    // Start is called before the first frame update
    void Awake()
    { 
        
    }
    void Start()
    {
        transform.Find("HealthCore").GetComponent<HealthCore>().SetMaxHealth(100);
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player1");
        ghost = gameObject;
        if(agent.pathStatus != NavMeshPathStatus.PathInvalid) agent.SetDestination(player.transform.position);
        animator.SetTrigger("Idle");
        
        bulletPrefab = Resources.Load("vfx_Projectile_Trail03_Red_Mobile") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
       
        if (agent.pathStatus == NavMeshPathStatus.PathInvalid) return;
        if (stopTime > 0)
        {
            stopTime -= Time.deltaTime;
            agent.SetDestination(ghost.transform.position);
            return;
        }
        if (isTargetset == false)
        {
            agent.SetDestination(ghost.transform.position);
            animator.SetTrigger("Idle");
        }
        else
        {
            if (isAttack == true)
            {
                attackTimer += Time.deltaTime;
                if (attackTimer >= attackTime)
                {
                    animator.SetTrigger("isAttack");
                    playerMoveSpeed = player.GetComponent<FPSActor>().moveSpeed;
                    player.GetComponent<FPSActor>().moveSpeed = 0f;
                    attackTimer = 0f;
                }
                else
                {
                    animator.SetTrigger("Idle");
                    return;
                }
            }
            else
            {
                agent.SetDestination(player.transform.position);
                animator.SetTrigger("isRanning");    
            }
            
        }
        
        
    }

    public void OnTriggerEnter(Collider other)
    {   
        if(other.gameObject.tag == "Player1")
        {
            isTargetset = true;
        }
    }
    
    public void OnTriggerExit(Collider other)
    {   
        if(other.gameObject.tag == "Player1")
        {
            isTargetset = false;
        }
    }
    
    public void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player1")
        {   
            
            isAttack = true;
            
        }
    }

    public void OnCollisionExit(Collision other)
    {
        if(other.gameObject.tag == "Player1")
        {
            isAttack = false;
        }   
    }

    public void Attack()
    {
        Shoot(Vector3.zero);
        Shoot(Vector3.right * -2.5f);
        Shoot(Vector3.right * 2.5f);
        Stop();
    }

    public void Shoot(Vector3 offset)
    {
        transform.LookAt(player.transform.position);
        var bullet = Instantiate(bulletPrefab, MuzzlePoint.transform.position, Quaternion.identity);
        bullet.transform.LookAt(player.transform.position + offset);
        // bullet.GetComponent<Rigidbody>().AddForce(transform.forward * 1000);
    }

    // void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.tag == "PlayerBullet")
    //     {
    //         Health -= 15;
    //     }
    // }

    
    public void Stop()
    {
        stopTime = 5f;
    }
}
