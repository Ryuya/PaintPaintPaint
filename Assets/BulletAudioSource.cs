using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAudioSource : MonoBehaviour
{
    // 効果音を再生するコンポーネント
    [SerializeField] public SEPlayer _sePlayer = default;
// 再生する音
    [SerializeField] public AudioClip _clip1 = default;
    [SerializeField] public AudioClip _clip2 = default;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
