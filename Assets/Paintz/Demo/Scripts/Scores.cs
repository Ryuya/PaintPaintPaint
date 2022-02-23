using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
    
public class Scores : MonoBehaviour
{
    [Range(.1f,5f)]
    public float updateSpeed = 1;
    
    public float inProgress = 0;

    public TextMesh player1;
    public TextMesh player2;

	void Start ()
    {
	}

    void ClearScores()
    {
        if (player1 != null) player1.text = "0%";
        if (player2 != null) player2.text = "0%";
    }

    private float nextUpdateTime = 0;
	void Update ()
    {
        
        if (Time.realtimeSinceStartup < nextUpdateTime) return;
        nextUpdateTime = Time.realtimeSinceStartup + updateSpeed;

        PaintTarget.TallyScore();

        float n = PaintTarget.scores.x + PaintTarget.scores.y + PaintTarget.scores.z + PaintTarget.scores.w;
        inProgress = Mathf.RoundToInt(((PaintTarget.scores.x / n) * 100.0f));
        if (n == 0)
        {
            ClearScores();
            return;
        }

        if (player1 != null) player1.text = Mathf.RoundToInt(((PaintTarget.scores.x / n) * 100.0f)).ToString() + "%";
        if (player2 != null) player2.text = Mathf.RoundToInt(((PaintTarget.scores.x / n) * 100.0f)).ToString() + "%";
       
    }
}
