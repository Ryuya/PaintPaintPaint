using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeManager : MonoBehaviour
{
    public List<GameObject> weaponList;
    public int currentWeaponNum = 0;

    public WeaponManager ScriptWeaponManager;
    // Start is called before the first frame update
    void Start()
    {
        ScriptWeaponManager = GameObject.Find("Scripts/WeaponManager").GetComponent<WeaponManager>();
        weaponList[ScriptWeaponManager.currentWeaponNum].SetActive(true);
        // weaponList[1].UnLock();
        // NextWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            NextWeapon();
        }
        
        if (Input.GetKeyUp(KeyCode.A))
        {
            PrevWeapon();
        }
    }
    
    public void CloseButton()
    {
        SceneManager.LoadScene("StageSelect");
    }

    public void NextWeapon()
    {
        weaponList[ScriptWeaponManager.currentWeaponNum].SetActive(false);
        if (ScriptWeaponManager.currentWeaponNum+1 >= weaponList.Count)
        {
            ScriptWeaponManager.CurrentWeapon = 0;
        }
        else
        {
            ScriptWeaponManager.CurrentWeapon++;
        }
        weaponList[ScriptWeaponManager.currentWeaponNum].SetActive(true);
    }
    
    public void PrevWeapon()
    {
        weaponList[ScriptWeaponManager.currentWeaponNum].SetActive(false);
        if (ScriptWeaponManager.currentWeaponNum-1 < 0)
        {
            ScriptWeaponManager.CurrentWeapon = weaponList.Count-1;
        }
        else
        {
            ScriptWeaponManager.CurrentWeapon--;
        }
        weaponList[ScriptWeaponManager.currentWeaponNum].SetActive(true);
    }
}
