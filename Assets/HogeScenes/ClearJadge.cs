using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearJadge : MonoBehaviour
{
    public float clearPersent = 2.0f;
    public Scores scores;
    // Start is called before the first frame update
    // Update is called once per frame

    private bool rush1 = false;
    private float rushTime = 0.0f;
    EnemySpwner spwner;
    void Start()
    {

        spwner = GameObject.FindWithTag("EnemySpwner").GetComponent<EnemySpwner>();
    }
    void Update()
    {
        rushTime += Time.deltaTime;
        //100%の割合なら
        if (scores != null )
            if (1.0f <=  scores.inProgress && !GameManager.Instance.isGameEnd)
            {
                GameManager.Instance.Clear();
                
                if (GameManager.Instance.isGameEnd)
                {
                    var perDisc = GameObject.Find("Persent").GetComponent<PerDisc>();
                    perDisc.setDisc(1f,"persent");
                    perDisc.enabled = false;
                }
            }

        if (0.1f >= scores.inProgress && !GameManager.Instance.isGameEnd)
        {
            if (rushTime > 6.0f)
            {
                spwner.BlueRush();
                rushTime = 0.0f;
            }
        }
    }
}
