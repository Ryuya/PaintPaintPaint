using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDetail : MonoBehaviour
{
    public void LoadScene(int stageNum)
    {
        SceneManager.LoadScene("CharacterStage"+stageNum);
    }

    public void UpgradeScene()
    {
        SceneManager.LoadScene("UpGrade");
    }
}
