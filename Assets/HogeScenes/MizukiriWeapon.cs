using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class MizukiriWeapon :  Weapon
{
    // MizukiriWeapon instance = null;
    // Start is called before the first frame update
    public bool isOne = false;
    public string saveKey;
    void Start()
    {
        // ES3.Save("Money",100000);
        saveKey = this.GetType().Name +"Level";
        // ES3.Save(saveKey,1);
        // ES3.DeleteKey("MizukiriWeapon");
        Load();
        // Buy();
        // Debug.Log(currentLevel);
        
        
        // Debug.Log(inkScaleList[currentLevel - 1]);
    }

    void OnEnable()
    {
        isOne = false;
    }

    // Update is called once per frame
    [SerializeField]
    public int BuyCost
    {
        get; set;
    }

    public bool IsUnLock;
    public int currentLevel;
    public int currentLevelIndex
    {
        get { return currentLevel - 1; }
    }
    public int CurrentLevel
    {
        get
        {
            if (ES3.KeyExists(saveKey))
            {
                currentLevel = ES3.Load<int>(this.GetType().Name +"Level");    
            }
            else
            {
                UnLock();
            }
            
            return currentLevel;

        }
        set
        {
            currentLevel = value;
            ES3.Save<int>(this.GetType().Name +"Level",currentLevel);
        }
    }

    public List<int> levelList;
    public List<int> costList;
    public Image image;
    public List<float> inkScaleList;
    public List<int> inkAmoList;
    public List<float> spitFireList;
    public List<int> AbilityLevel;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            ES3.DeleteKey(this.GetType().Name +"Level");
        }
        if (isOne == false && SceneManager.GetActiveScene().name == "UpGrade") 
        {
            if (CurrentLevel > costList.Count)
            {
                GameObject.Find("LevelPanel/Lv").GetComponent<Text>().text = "Lv" + levelList[currentLevelIndex].ToString();
                GameObject.Find("LevelPanel/LvNextVal").GetComponent<Text>().text = "";
                GameObject.Find("InkScale/Lv").GetComponent<Text>().text = inkScaleList[currentLevelIndex].ToString();
                GameObject.Find("InkScale/LvNextVal").GetComponent<Text>().text = "";
                GameObject.Find("InkAmo/Lv").GetComponent<Text>().text = inkAmoList[currentLevelIndex].ToString();
                GameObject.Find("InkAmo/LvNextVal").GetComponent<Text>().text = "";
                GameObject.Find("SpitFire/Lv").GetComponent<Text>().text = spitFireList[currentLevelIndex].ToString();
                GameObject.Find("SpitFire/LvNextVal").GetComponent<Text>().text = "";
                GameObject.Find("AbilityLevel/Lv").GetComponent<Text>().text =
                    "Lv" + AbilityLevel[currentLevelIndex].ToString();
                GameObject.Find("AbilityLevel/LvNextVal").GetComponent<Text>().text =
                    "";
                isOne = true;
                GameObject.Find("UpgradeCostText").transform.parent.GetComponent<Button>().interactable = false;
                return;
            }
            
            GameObject.Find("HaveMoneyText").GetComponent<Text>().text = ES3.Load<int>("Money").ToString()+"$";
            GameObject.Find("/Canvas/AllFramePanel/L_Panel/L_InnerFramePanel/Button/UpgradeCostText").GetComponent<Text>().text = "Upgrade!!\n"+costList[currentLevelIndex].ToString()+"$";
            CheckMax();
            GameObject.Find("UpgradeCostText").transform.parent.GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("UpgradeCostText").transform.parent.GetComponent<Button>().onClick.AddListener(Upgrade);
            if (costList[currentLevel - 1] <= ES3.Load<int>("Money") && currentLevelIndex < costList.Count)
            {
                GameObject.Find("UpgradeCostText").transform.parent.GetComponent<Button>().interactable = true;
            }
            else
            {
                GameObject.Find("UpgradeCostText").transform.parent.GetComponent<Button>().interactable = false;
            }
            UpdateUI();
            
            
            isOne = true;
        }

        if (isOne == false && SceneManager.GetActiveScene().name != "UpGrade")
        {
            var actor = GetComponent<FPSActor>();
            actor.currentInk = inkAmoList[CurrentLevel - 1];
            actor.splatScale = inkScaleList[CurrentLevel - 1];
            actor.spitFire = spitFireList[CurrentLevel - 1];
            actor.abilityLevel = AbilityLevel[CurrentLevel - 1];
            actor.UIUpdate();
            isOne = true;
        }
    }
    public override void UnLock()
    {
        Save();
    }

    public override void Upgrade()
    {
        var haveMoney = ES3.Load<int>("Money");
        
        if (haveMoney >= costList[currentLevel-1] && currentLevel <= costList.Count)
        {
            haveMoney -= costList[currentLevel-1];
            ES3.Save<int>("Money", haveMoney);
            GameObject.Find("HaveMoneyText").GetComponent<Text>().text = haveMoney.ToString()+"$";
            CheckMax();
            if (CurrentLevel <= costList.Count)
            {
                CurrentLevel += 1;
                UpdateUI();
                GameObject.Find("UpgradeCostText").GetComponent<Text>().text = "Upgrade!!\n"+costList[currentLevel-1].ToString()+"$";
            } else if (currentLevel == costList.Count)
            {
                GameObject.Find("LevelPanel/Lv").GetComponent<Text>().text = "Lv" + levelList[currentLevel].ToString();
                GameObject.Find("LevelPanel/LvNextVal").GetComponent<Text>().text = "";
                GameObject.Find("InkScale/Lv").GetComponent<Text>().text = inkScaleList[currentLevel].ToString();
                GameObject.Find("InkScale/LvNextVal").GetComponent<Text>().text = "";
                GameObject.Find("InkAmo/Lv").GetComponent<Text>().text = inkAmoList[currentLevel].ToString();
                GameObject.Find("InkAmo/LvNextVal").GetComponent<Text>().text = "";
                GameObject.Find("SpitFire/Lv").GetComponent<Text>().text = spitFireList[currentLevel].ToString();
                GameObject.Find("SpitFire/LvNextVal").GetComponent<Text>().text = "";
                GameObject.Find("AbilityLevel/Lv").GetComponent<Text>().text =
                    "Lv" + AbilityLevel[currentLevel].ToString();
                GameObject.Find("AbilityLevel/LvNextVal").GetComponent<Text>().text =
                    "";
            }
        }
        if (costList[currentLevel - 1] <= ES3.Load<int>("Money") && currentLevelIndex < costList.Count)
        {
            GameObject.Find("UpgradeCostText").transform.parent.GetComponent<Button>().interactable = true;
        }
        else
        {
            GameObject.Find("UpgradeCostText").transform.parent.GetComponent<Button>().interactable = false;
        }
        
    }

    public void UpdateUI()
    {
        if (CurrentLevel <= costList.Count)
        {
            GameObject.Find("LevelPanel/Lv").GetComponent<Text>().text = "Lv" + levelList[CurrentLevel-1].ToString();
            GameObject.Find("LevelPanel/LvNextVal").GetComponent<Text>().text =
                "Lv" + levelList[CurrentLevel].ToString();
            GameObject.Find("InkScale/Lv").GetComponent<Text>().text = inkScaleList[CurrentLevel - 1].ToString();
            GameObject.Find("InkScale/LvNextVal").GetComponent<Text>().text = inkScaleList[CurrentLevel].ToString();
            GameObject.Find("InkAmo/Lv").GetComponent<Text>().text = inkAmoList[CurrentLevel - 1].ToString();
            GameObject.Find("InkAmo/LvNextVal").GetComponent<Text>().text = inkAmoList[CurrentLevel].ToString();
            GameObject.Find("SpitFire/Lv").GetComponent<Text>().text = spitFireList[CurrentLevel - 1].ToString();
            GameObject.Find("SpitFire/LvNextVal").GetComponent<Text>().text = spitFireList[CurrentLevel].ToString();
            GameObject.Find("AbilityLevel/Lv").GetComponent<Text>().text =
                "Lv" + AbilityLevel[CurrentLevel - 1].ToString();
            GameObject.Find("AbilityLevel/LvNextVal").GetComponent<Text>().text =
                "Lv" + AbilityLevel[CurrentLevel].ToString();
        }
    }

    public  void CheckMax()
    {
        if (currentLevel == costList.Count)
        {
            GameObject.Find("UpgradeCostText").transform.parent.GetComponent<Button>().interactable = false;
            GameObject.Find("UpgradeCostText").GetComponent<Text>().text = "MAX LEVEL";
        }
    }

    public override void Save()
    {
        ES3.Save(saveKey,currentLevel);
    }

    public override void Load()
    {
        Setup();
        if (ES3.KeyExists(saveKey))
        {
            CurrentLevel = (int)ES3.Load(saveKey);
        }
        else
        {
            ES3.Save(saveKey,1);
        }
    }
    /// <summary>
    /// ????????????????????????Load???????????????????????????????????????
    /// </summary>
    public override void Setup()
    {
        IsUnLock = false;
        currentLevel = 1;
        levelList = new List<int>{1,2,3,4,5,6};
        costList = new List<int> { 1*10*2,2*20*2,3*120*2,4*430*2,5*650*2};
        image = null;
        inkScaleList = new List<float>{0.8f,1.6f,2.2f,2.5f,2.8f,5.5f};
        inkAmoList = new List<int> { 45,90,180,300,400,700};
        spitFireList = new List<float> { 0.18f,0.15f,0.13f,0.08f,0.05f,0.03f};
        AbilityLevel = new List<int> { 3, 4, 5, 6,7,8 };
    }
    
    public class SaveData
    {
        public int currentLevel;
    }
}
