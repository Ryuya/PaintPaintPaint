using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class GhostBlue : MonoBehaviour
{
    private Animator animator;
    public NavMeshAgent agent;
    public GameObject player;
    public GameObject ghost;
    
    public bool isTargetset = false;

    public float playerMoveSpeed = 0.5f;

    public GameObject bulletPrefab;

    public GameObject MuzzlePoint;

    private Vector3 defaultVector;
    
    private float timer = 0.5f;
    float moveTimer = 0.5f;
    private float moveThreshold = 2.3f;

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
        if(agent.pathStatus != NavMeshPathStatus.PathInvalid)
        agent.SetDestination(player.transform.position);
        animator.SetTrigger("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.pathStatus == NavMeshPathStatus.PathInvalid) return;
        //Timerを設定
        timer += Time.deltaTime;
        
        //タイマーが0.5秒以上経過したら
        if (timer >= 0.5f)
        {
            //タイマーを0にする
            timer = 0.0f;
            //ランダムに移動する
            DropPaint();
        }
        if (isTargetset == false)
        {
            //ランダムに地点を移動
            //Timerを設定
            timer += Time.deltaTime;
            moveTimer += Time.deltaTime;
            //タイマーが0.5秒以上経過したら
            if (moveTimer >= moveThreshold)
            {
                //タイマーを0にする
                moveTimer = 0.0f;
                //ランダムに移動する
                defaultVector = new Vector3(Random.Range(transform.position.x-10, transform.position.x+10), 0, Random.Range(transform.position.z-10, transform.position.z+10));
                agent.SetDestination(defaultVector);
            }

            animator.SetTrigger("isRanning");
        }
        else
        {
            agent.SetDestination(player.transform.position);
            animator.SetTrigger("isRanning");
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
    
    // void OnCollisionEnter(Collision other)
    // {
    //     if (other.gameObject.tag == "PlayerBullet")
    //     {
    //         Health -= 15;
    //     }
    // }

    public void DropPaint()
    {
        Instantiate(bulletPrefab, MuzzlePoint.transform.position, MuzzlePoint.transform.rotation);
    }
}
