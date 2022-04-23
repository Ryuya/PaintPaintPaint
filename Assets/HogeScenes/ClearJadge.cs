using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearJadge : MonoBehaviour
{
    public float clearPersent = 2.0f;
    public Scores scores;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
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
    }
}
