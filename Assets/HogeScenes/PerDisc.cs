using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;

public class PerDisc : MonoBehaviour
{
    private Disc disc;
    [Range(0f,1f)]
    public float persent = 0.75f;
    
    public TextMesh text1, text2;
    // Start is called before the first frame update
    void Start()
    {
        persent = 0;
        disc = GetComponent<Disc>();
    }

    // Update is called once per frame
    void Update()
    {
        
        disc.AngRadiansEnd = Mathf.Deg2Rad * Mathf.Lerp( 90,-270, this.persent);
    }

    public void setDisc(float persent,string caseType)
    {
        if (caseType == "persent")
        {
            this.persent = persent;
            text1.text = (persent* 100).ToString() + "%";
            text2.text = (persent* 100).ToString() + "%";    
        }
        
        if (caseType == "string")
        {
            this.persent = persent;
            text1.text = (persent * 100).ToString();
            text2.text = (persent * 100).ToString();
        }
        
        disc.AngRadiansEnd = Mathf.Deg2Rad * Mathf.Lerp( 90,-270, this.persent);
    }
}
