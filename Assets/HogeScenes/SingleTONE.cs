using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTONE : MonoBehaviour
{
    static public SingleTONE instance;
    void Awake ()
    {
        if (instance == null) {
 
            instance = this;
            DontDestroyOnLoad (gameObject);
        }
        else {
 
            Destroy (gameObject);
        }
    }
}
