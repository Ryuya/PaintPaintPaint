using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class GhostYellow : MonoBehaviour
{
    private Animator animator;
    public NavMeshAgent agent;
    public GameObject player;
    public GameObject ghost;
    
    public bool isTargetset = false;
    public bool isAttack = false;
    
    [NonSerialized]
    public float attackRange = 0.5f;
    public float attackTime = 3f;
    public float attackTimer = 0f;
    public float attackDelay = 1f;
    
    public float stopTime = 5f;

    public float playerMoveSpeed = 0.5f;
    [NonSerialized]
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
    
    // void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.tag == "PlayerBullet")
    //     {
    //         Health -= 15;
    //     }
    // }

    

    public void OnCollisionExit(Collision other)
    {
        if(other.gameObject.tag == "Player1")
        {
            isAttack = false;
        }   
    }

    public void Attack()
    {
        if(Vector3.Distance(transform.position,player.transform.position) <= attackRange)
            player.GetComponent<FPSActor>().isStunned = true;
        player.GetComponent<FPSActor>().stunTime = 2f;
        Stop();
    }
    

    public void Stop()
    {
        stopTime = 5f;
    }
}
