using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance = null;
    public List<Weapon> weaponList;
    public int currentWeaponNum = 0;
    public Weapon currentWeapon = null;
    public int CurrentWeapon {
        get
        {
            currentWeaponNum = ES3.Load<int>("currentWeapon");
            return currentWeaponNum;
        }
        set
        {
            currentWeaponNum = value;
            ES3.Save<int>("currentWeapon",currentWeaponNum);
            currentWeapon = weaponList[currentWeaponNum];
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (ES3.KeyExists("currentWeapon") == false)
        {
            CurrentWeapon = 0;
        }

        CurrentWeapon = ES3.Load<int>("currentWeapon");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
