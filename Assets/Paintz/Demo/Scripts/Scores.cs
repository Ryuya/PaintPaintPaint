using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;
    
public class Scores : MonoBehaviour
{
    [Range(.1f,5f)]
    public float updateSpeed = 1;
    
    public float inProgress = 0;
    private float n;
    public TextMesh player1;
    public TextMesh player2;
    public PerDisc PerDisc;
    public ClearJadge clear_judge;
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

        n = PaintTarget.scores.x + PaintTarget.scores.y + PaintTarget.scores.z + PaintTarget.scores.w;
        inProgress = (PaintTarget.scores.x / n) * 100.0f / clear_judge.clearPersent;
        if (n == 0)
        {
            ClearScores();
            return;
        }

        if (player1 != null) player1.text = ((PaintTarget.scores.x / n) * 100.0f).ToString("F0") + "%";
        if (player2 != null) player2.text = ((PaintTarget.scores.x / n) * 100.0f).ToString("F0") + "%";
        if (clear_judge.clearPersent > 0)
        {
            PerDisc.setDisc(((PaintTarget.scores.x / n) * 100.0f) / clear_judge.clearPersent,"persent");
            Debug.LogWarning(((PaintTarget.scores.x / n) * 100.0f) / clear_judge.clearPersent);
        }
        
    }

    public float getProgress()
    {
        return  Mathf.RoundToInt(((PaintTarget.scores.x / n) * 100.0f));
    }
}
