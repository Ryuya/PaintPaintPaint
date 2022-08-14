using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTONE : MonoBehaviour
{
    static public SingleTONE Instance;
    void Awake ()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);    

        DontDestroyOnLoad(gameObject);

    }
}
