using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkChargeItem : MonoBehaviour
{
    [SerializeField]
    float Ammount = 80f;

    private PerDisc BulletAmountPerDisc;
    // Start is called before the first frame update
    void Start()
    {
        BulletAmountPerDisc = GameObject.Find("Camera/ShapeCanvas/BulletAmountDisc").GetComponent<PerDisc>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player1")
        {
            
            var fPSActor = collider.gameObject.GetComponent<FPSActor>();
            fPSActor.currentInk += Ammount;
            BulletAmountPerDisc.setDisc(fPSActor.currentInk / 500f,"string");
            if (BulletAmountPerDisc.text1 != null) BulletAmountPerDisc.text1.text = (fPSActor.currentInk).ToString("F0");
            if (BulletAmountPerDisc.text2 != null) BulletAmountPerDisc.text2.text = (fPSActor.currentInk).ToString("F0");
            Destroy(this.gameObject);
        }
    }
}
