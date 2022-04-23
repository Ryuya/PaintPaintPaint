using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float currentTime = 0f;
    public float setTime = 60f;
    public float updateSpeed = 1f;

    private float nextUpdateTime = 0.0f;

    public Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = setTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance == false) return;
        if (currentTime > 0 && GameManager.Instance.isGameEnd == false)
        {
            currentTime -= Time.deltaTime;
            timerText.text = currentTime.ToString("F2");
        }
        else
        {
            currentTime = 0;
            timerText.text = currentTime.ToString("F2");
            if( GameManager.Instance.isGameEnd == false)GameManager.Instance.GameOver();
        }
        if (Time.realtimeSinceStartup < nextUpdateTime) return;
        nextUpdateTime = Time.realtimeSinceStartup + updateSpeed;
    }
}
