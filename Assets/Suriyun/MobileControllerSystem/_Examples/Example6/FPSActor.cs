using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Suriyun.MCS;
using UnityStandardAssets.Vehicles.Ball;
using Random = UnityEngine.Random;

public class FPSActor : MonoBehaviour
{
    public UniversalButton inputMove;
    public TouchArea inputAimArea;
    public TouchArea inputAimBtn;

    public bool lerpStopping = false;
    public float moveSpeed;
    public float aimSpeed;

    protected Vector3 cachedInputMove;
    protected Vector3 cachedInputAim;

    public GameObject fpsCameraMount;
    public Vector3 cameraOffset;
    protected Quaternion camRotation;

    public Ball ball;

    public GameObject OrangeBulletPrefab;

    public float spitFire = 0.04f;
    public float shootTime;
    public float MAX_INK = 400f;
    public float currentInk;
    public float splatScale;
    public float inkCost = 1f;
    public Rigidbody rb;
    float speed = 1;
    private Animator anim;
    
    private Vector3 prev;
    
    private Vector2 startPos, currentPos, differenceDisVector2;

    public Transform Muzzle;

    public PerDisc BulletAmountPerDisc;

    public int abilityLevel = 0;
    bool One = false;

    public Mizukiri mizukiri;
    
    //インクを補充する際は200~400 4~8個取得
    protected virtual void Start()
    {
        Application.targetFrameRate = 60;
        ball = GetComponent<Ball>();
        // currentInk = MAX_INK;
        
        anim = GetComponent<Animator>();
        prev = transform.position;

        LoadWeapon();
    }

    protected virtual void OnEnable()
    {
        inputAimArea.onDrag.AddListener(OnDrag);
        inputAimBtn.onDrag.AddListener(OnDrag);
    }

    protected virtual void OnDisable()
    {
        inputAimArea.onDrag.RemoveListener(OnDrag);
        inputAimBtn.onDrag.RemoveListener(OnDrag);
    }

    protected virtual void OnDrag(int btnId)
    {
        switch (btnId)
        {
            case 0:
                cachedInputAim += inputAimArea.deltaFingerPositionInchesYX * aimSpeed;
                break;
            case 1:
                cachedInputAim += inputAimBtn.deltaFingerPositionInchesYX * aimSpeed;
                break;
        }
        cachedInputAim.x *= -1f;
        cachedInputAim.z = 0f;
    }

    protected Vector3 horizontalAim;
    protected Vector3 verticalAim;
    protected Quaternion tmpQuaternion;

    protected virtual void Update()
    {


        if (One == false)
        {
            BulletAmountPerDisc = GameObject.Find("Camera/ShapeCanvas/BulletAmountDisc").GetComponent<PerDisc>();
            BulletAmountPerDisc.setDisc(currentInk / 500f,"string");
            if (BulletAmountPerDisc.text1 != null) BulletAmountPerDisc.text1.text = (currentInk).ToString("F0");
            if (BulletAmountPerDisc.text2 != null) BulletAmountPerDisc.text2.text = (currentInk).ToString("F0");
            One = true;
        }
        
        horizontalAim.y = cachedInputAim.y;
        verticalAim.x = cachedInputAim.x;

        tmpQuaternion = fpsCameraMount.transform.rotation;

        // Handle vertical aim input.
        fpsCameraMount.transform.eulerAngles = fpsCameraMount.transform.eulerAngles + verticalAim;

        // Limit look up/down rotation.
        if (fpsCameraMount.transform.up.y < 0)
        {
            fpsCameraMount.transform.rotation = tmpQuaternion;
        }

        // Handle horizontal aim input.
        fpsCameraMount.transform.eulerAngles = fpsCameraMount.transform.eulerAngles + horizontalAim;

        // Rotate actor to match camera rotation.
        cachedInputAim.x = 0f;
        transform.eulerAngles = transform.eulerAngles + cachedInputAim;
        cachedInputAim = Vector3.zero;

       
        // Moving the actor
        if (inputMove.isFingerDown)
        {
            cachedInputMove = inputMove.directionXZ;
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

            // 方向キーの入力値とカメラの向きから、移動方向を決定
            Vector3 moveForward = cameraForward * cachedInputMove.z + Camera.main.transform.right * cachedInputMove.x;
            // Vector3 moveForward = cameraForward;
            // ball.Move(moveForward,false);
            this.transform.position += moveForward *0.1f *speed;
            // this.transform.rotation = Quaternion.Euler(this.transform.rotation.x,moveForward.y,this.transform.rotation.x);
            anim.SetFloat("moveSpeed",1);
            
            // transform.rotation = Quaternion.Euler(0,Camera.main.transform.rotation.eulerAngles.y,0);
        }
        else
        {
            anim.SetFloat("moveSpeed",0);
            if (lerpStopping)
            {
                cachedInputMove = Vector3.Lerp(cachedInputMove, Vector3.zero, moveSpeed * Time.deltaTime);
            }
            else
            {
                cachedInputMove = Vector3.zero;
            }
        }
        
        if (inputAimArea.isFingerDown)
        {
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

            // 方向キーの入力値とカメラの向きから、移動方向を決定
            Vector3 moveForward = cameraForward * cachedInputMove.z + Camera.main.transform.right * cachedInputMove.x;
            this.Shoot();
        }


        // var diff = transform.position - prev;
        // if (diff.magnitude > 0.01) {
        //     transform.rotation = Quaternion.LookRotation (diff);
        // }
        // prev = transform.position;
        // Move camera mount 
        fpsCameraMount.transform.position = transform.position + cameraOffset;
    }

    public void Shoot()
    {
        BulletAmountPerDisc.setDisc(currentInk / 500f,"string");
        if (BulletAmountPerDisc.text1 != null) BulletAmountPerDisc.text1.text = (currentInk).ToString("F0");
        if (BulletAmountPerDisc.text2 != null) BulletAmountPerDisc.text2.text = (currentInk).ToString("F0");
        shootTime += Time.deltaTime;
        if (spitFire <= shootTime && currentInk >= inkCost)
        {
            currentInk -= inkCost;
            GameObject go = Instantiate(OrangeBulletPrefab,new Vector3(Muzzle.transform.position.x,Muzzle.transform.position.y,Muzzle.transform.position.z),Quaternion.Euler(transform.localEulerAngles));
            var rb = go.GetComponent<Rigidbody>();
            go.GetComponent<CollisionPainter>().brush.splatScale = splatScale;
            var buletForce = transform.forward * (Random.Range(0,4) * 40f +(Random.Range(1f,12f)));
            buletForce += transform.right * Random.Range(-0.3f, 0.3f);
            var mizukiri = go.GetComponent<Mizukiri>();
            if (mizukiri != null)
            {
                mizukiri.prevVelocity = buletForce;
                mizukiri.count = abilityLevel;
            }
            rb.AddForce(buletForce,ForceMode.Impulse);
            shootTime = 0;
        }
        
    }

    public void UIUpdate()
    {
        BulletAmountPerDisc = GameObject.Find("Camera/ShapeCanvas/BulletAmountDisc").GetComponent<PerDisc>();
        BulletAmountPerDisc.setDisc(currentInk / 500f,"string");
        if (BulletAmountPerDisc.text1 != null) BulletAmountPerDisc.text1.text = (currentInk).ToString("F0");
        if (BulletAmountPerDisc.text2 != null) BulletAmountPerDisc.text2.text = (currentInk).ToString("F0");
    }

    public void LoadWeapon()
    {
        var weaponComponent =  GameObject.Find("/Scripts/WeaponManager").GetComponent<WeaponManager>().currentWeapon;
        Weapon weapon = (Weapon)this.gameObject.AddComponent(weaponComponent.GetType());
        var mizukiriWeapon = GetComponent<MizukiriWeapon>();
        if (mizukiriWeapon != null) OrangeBulletPrefab = Resources.Load("MizukiriBullet") as GameObject;
    }

    public void ChangeMizukiriBullet()
    {
        OrangeBulletPrefab = Resources.Load("MizukiriBullet") as GameObject;
    }

    
}
