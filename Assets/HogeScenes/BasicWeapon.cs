using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class BasicWeapon :  Weapon
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

    public int CurrentLevel
    {
        get
        {
            if (ES3.KeyExists(saveKey))
            {
                currentLevel = ES3.Load<int>(this.GetType().Name + "Level");
            }
            else
            {
                ES3.Save<int>(this.GetType().Name +"Level",1);
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
            ES3.Save("Money",100000);
        }
        
        if (isOne == false && SceneManager.GetActiveScene().name == "UpGrade") 
        {
            GameObject.Find("HaveMoneyText").GetComponent<Text>().text = ES3.Load<int>("Money").ToString()+"$";
            GameObject.Find("/Canvas/AllFramePanel/L_Panel/L_InnerFramePanel/Button/UpgradeCostText").GetComponent<Text>().text = "Upgrade!!\n"+costList[currentLevel-1].ToString()+"$";
            CheckMax();
            GameObject.Find("UpgradeCostText").transform.parent.GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("UpgradeCostText").transform.parent.GetComponent<Button>().onClick.AddListener(Upgrade);
            if (costList[currentLevel - 1] <= ES3.Load<int>("Money") && currentLevel <= costList.Count)
            {
                GameObject.Find("UpgradeCostText").transform.parent.GetComponent<Button>().interactable = true;
            }
            else
            {
                GameObject.Find("UpgradeCostText").transform.parent.GetComponent<Button>().interactable = false;
            }
            UpdateUI();
            if (currentLevel == costList.Count)
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
            isOne = true;
        }
        
        if (isOne == false && SceneManager.GetActiveScene().name != "UpGrade")
        {
            var actor = GetComponent<FPSActor>();
            actor.currentInk = inkAmoList[CurrentLevel-1];
            actor.splatScale = inkScaleList[CurrentLevel-1];
            actor.spitFire = spitFireList[CurrentLevel-1];
            actor.abilityLevel = AbilityLevel[CurrentLevel-1];
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
        
        if (haveMoney >= costList[currentLevel-1] && currentLevel-1 <= costList.Count)
        {
            haveMoney -= costList[currentLevel-1];
            ES3.Save<int>("Money", haveMoney);
            GameObject.Find("HaveMoneyText").GetComponent<Text>().text = haveMoney.ToString()+"$";
            CheckMax();
            if (CurrentLevel < costList.Count)
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

            Save();    
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
        if (ES3.KeyExists(saveKey)){
            CurrentLevel = (int)ES3.Load(saveKey);
        } else {
            ES3.Save(saveKey,1);
        }
    }
    /// <summary>
    /// ファイルがなくてLoadが出来なかったら実行される
    /// </summary>
    public override void Setup()
    {
        IsUnLock = false;
        currentLevel = 0;
        levelList = new List<int>{1,2,3,4,5,6};
        costList = new List<int> { 1*10*2,2*20*2,3*120*2,4*430*2,5*650*2};
        image = null;
        inkScaleList = new List<float>{0.8f,1.6f,2.2f,2.5f,4.8f,9.5f};
        inkAmoList = new List<int> { 220, 360,500,1050,2200,3600};
        spitFireList = new List<float> { 0.05f,0.04f,0.03f,0.02f,0.01f,0.005f};
        AbilityLevel = new List<int> { 3, 4, 5, 6,7,8};
    }
}
