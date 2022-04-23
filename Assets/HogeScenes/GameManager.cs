using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Ball;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isGameEnd = false;
    public bool isStageClear = false;
    public GameObject showGameClear;

    public GameObject GameResult;
    
    public int SpeedLevel = 0;
    public int InkScaleLevel = 0;
    
    public Text speedLevelText,InkScaleLevelText,MoneyText;

    public int Money = 0;
    public GameObject ball;

    public GameObject NextStageButton;

    public int currentMoney = 0;
    // Start is called before the first frame update
    void Start()
    {
        // if (Instance == null)
        Instance = this;
        // StageSelectCanvas.SetActive(false);
        
        ball = GameObject.Find("RollerBall");
        // else if (Instance != this)
        // {
        //     Destroy(gameObject);
        // }
        //     
        // DontDestroyOnLoad(gameObject);
        // Reset();
        BallSetup(ball);
        Init();
        
    }

    public void Reset()
    {
        ES3.Save<int> ("SpeedLevel",1);
        ES3.Save<int> ("InkScaleLevel",1);
        ES3.Save<int> ("Money",0);
    }

    public void BallSetup(GameObject ball)
    {
        if (ES3.KeyExists("Money") == false)
        {
            ES3.Save<int> ("Money",0);
        }
        
        if (ES3.KeyExists("SpeedLevel") == false)
        {
            ES3.Save<int> ("SpeedLevel",1);
        }
        if (ES3.KeyExists("InkScaleLevel") == false)
        {
            ES3.Save<int> ("InkScaleLevel",1);
        }
        Money = ES3.Load<int> ("Money");
        int speedLevel = ES3.Load<int> ("SpeedLevel");
        int inkScaleLevel = ES3.Load<int> ("InkScaleLevel");
        
        
        // InkScaleLevelText.text =  "Lv" + inkScaleLevel +"$" + Mathf.RoundToInt(inkScaleLevel * inkScaleLevel).ToString();
        // speedLevelText.text = "Lv" + speedLevel +"$" + Mathf.RoundToInt(speedLevel * speedLevel).ToString();
        
        Debug.Log("(speed:" + speedLevel + ",inkScale:" + inkScaleLevel + ")");
        // ball.GetComponent<Ball>().m_MovePower =  ball.GetComponent<Ball>().m_MovePower * speedLevel;
        // ball.GetComponent<Ball>().m_MaxAngularVelocity =  ball.GetComponent<Ball>().m_MaxAngularVelocity * speedLevel;
        // ball.GetComponent<Rigidbody>().mass = Mathf.RoundToInt(ball.GetComponent<Ball>().m_MaxAngularVelocity * 0.01f);
        // ball.GetComponent<CollisionPainter>().brush.splatScale = inkScaleLevel;
    }

    public void BallLevelUpSpeed()
    {
        if (Money >= Mathf.RoundToInt(ES3.Load<int> ("SpeedLevel") * ES3.Load<int> ("SpeedLevel")))
        {
            Money -= Mathf.RoundToInt(ES3.Load<int> ("SpeedLevel") * ES3.Load<int> ("SpeedLevel"));
            int SpeedLevel = ES3.Load<int>("SpeedLevel");
            ES3.Save<int> ("SpeedLevel",++SpeedLevel);
            ES3.Save<int> ("Money",Money);
            speedLevelText.text = "Lv" + ES3.Load<int> ("SpeedLevel") +"$" + Mathf.RoundToInt(ES3.Load<int> ("SpeedLevel") * ES3.Load<int> ("SpeedLevel")).ToString();
            MoneyText.text = Money.ToString();
        }
    }

    public void BallLevelUpInkScale()
    {
        int inkScaleLevel = ES3.Load<int> ("InkScaleLevel");
        if (Money >= Mathf.RoundToInt(ES3.Load<int> ("InkScaleLevel") * ES3.Load<int> ("InkScaleLevel")))
        {
            Money -= Mathf.RoundToInt(ES3.Load<int> ("InkScaleLevel") * ES3.Load<int> ("InkScaleLevel"));
            ES3.Save<int> ("InkScaleLevel",++inkScaleLevel);
            InkScaleLevelText.text = "Lv" + ES3.Load<int> ("InkScaleLevel") +"$" + Mathf.RoundToInt(ES3.Load<int> ("InkScaleLevel") * ES3.Load<int> ("InkScaleLevel")).ToString();
            ES3.Save<int> ("Money",Money);
            MoneyText.text = Money.ToString();
        }
        
        
    }

    void OnEnable()
    {
        
    }

    public void Init()
    {
        isGameEnd = false;
        isStageClear = false;
        showGameClear = GameObject.Find("/ResultCanvas/Result/ClearText/");
        GameResult = GameObject.Find("/ResultCanvas");
        GameResult.SetActive(false);
        currentMoney = 0;
        
        // StageSelectCanvas = GameObject.Find("/StageSelectCanvas").GetComponent<GameObject>();
        // StageSelectCanvas.SetActive(false);
    }

    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Clear()
    {
        float time = GameObject.Find("Timer").GetComponent<Timer>().currentTime;
        // ES3.Save<int> ("Money", ES3.Load<int>("Money")+Mathf.RoundToInt(time *0.1f));
        // Money = ES3.Load<int>("Money");
        // MoneyText.text = ES3.Load<int>("Money").ToString();
        isGameEnd = true;
        isStageClear = true;
        showGameClear.GetComponent<Text>().text = "Clear" ;
        showGameClear.SetActive(true);
        
        GameObject.Find("Scores 1").GetComponent<Scores>().enabled = false;
        // StageSelectCanvas.SetActive(true);
        
        //リザルト表示
        GameResult.SetActive(true);
        
        GameObject.Find("/ResultCanvas/Result/RetryButton/Text").GetComponent<Text>().text = "Retry?:"+SceneManager.GetActiveScene().name;
        SetResultText();
    }

    
    public void  GameOver()
    {
        isGameEnd = true;
        isStageClear = false;
        showGameClear.GetComponent<Text>().text = "GAME OVER" + SceneManager.GetActiveScene().name;;
        showGameClear.GetComponent<Text>().color = Color.red;
        showGameClear.GetComponent<Text>().fontSize = 69;
        showGameClear.SetActive(true);
        GameResult.SetActive(true);
        
        GameObject.Find("/ResultCanvas/Result/NextStageButton").GetComponent<Button>().interactable = false;
        GameObject.Find("/ResultCanvas/Result/RetryButton/Text").GetComponent<Text>().text = "Retry?:"+SceneManager.GetActiveScene().name;
        GameObject.Find("Scores 1").GetComponent<Scores>().enabled = false;
        SetResultText();
        // StageSelectCanvas.SetActive(true);
    }

    public void  SceneLoad(int level)
    {
        
        SceneManager.LoadScene("Stage"+level);
        Init();
        
    }

    public void UpGrade(string param)
    {
        switch (param)
        {
            case "speed":
                break;
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene().name);
    }

    public void GoStageSelect()
    {
        SceneManager.LoadScene ("StageSelect");
    }

    public void NextStage(int nextLevel)
    {
        SceneManager.LoadScene ("CharacterStage"+nextLevel);
    }

    public void SetResultText()
    {
        //残り時間
        GameObject.Find("/ResultCanvas/Result/Detail/TimeNumber").GetComponent<Text>().text = GameObject.Find("/Mobile Controller/Timer/").GetComponent<Timer>().currentTime.ToString();
        //現在の取得金額
        GameObject.Find("/ResultCanvas/Result/Detail/MoneyNumber").GetComponent<Text>().text =
            currentMoney.ToString() + "$";
        //獲得金額
        var getMoney =currentMoney+ currentMoney * Mathf.RoundToInt((GameObject.Find("/Mobile Controller/Timer/").GetComponent<Timer>().currentTime) * 0.2f);
        GameObject.Find("/ResultCanvas/Result/Detail/TotalNumber").GetComponent<Text>().text =(currentMoney*Mathf.RoundToInt((GameObject.Find("/Mobile Controller/Timer/").GetComponent<Timer>().currentTime) * 0.2f)).ToString() + "$ = "+ currentMoney +"x" + Mathf.RoundToInt((GameObject.Find("/Mobile Controller/Timer/").GetComponent<Timer>().currentTime) * 0.2f);
        ES3.Save<int>("Money",ES3.Load<int>("Money") + getMoney);
        GameObject.Find("/ResultCanvas/Result/Detail/TotalMoneyNumber").GetComponent<Text>().text =
            (ES3.Load<int>("Money")).ToString() + "$";
    }
}
