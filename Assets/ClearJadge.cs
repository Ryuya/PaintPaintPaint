using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearJadge : MonoBehaviour
{
    public GameObject showGameCler;
    public TextMesh clearPersentage;
    public float clearPersent = 2.0f;
    public Scores scores;
    public bool isClear = false;
    // Start is called before the first frame update
    void Start()
    {
        showGameCler.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
		if (clearPersent <=  scores.inProgress && !isClear)
        {
            isClear = true;
            showGameCler.SetActive(true);
        }
    }
}
