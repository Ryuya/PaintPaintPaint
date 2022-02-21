﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Suriyun.MCS;
using UnityStandardAssets.Vehicles.Ball;

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
        

    protected virtual void Start()
    {
        Application.targetFrameRate = 60;
        ball = GetComponent<Ball>();
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
        }
        else
        {
            if (lerpStopping)
            {
                cachedInputMove = Vector3.Lerp(cachedInputMove, Vector3.zero, moveSpeed * Time.deltaTime);
            }
            else
            {
                cachedInputMove = Vector3.zero;
            }
        }

        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = cameraForward * cachedInputMove.z + Camera.main.transform.right * cachedInputMove.x;
        ball.Move(moveForward,false);



        // Move camera mount 
        fpsCameraMount.transform.position = transform.position + cameraOffset;
    }
}