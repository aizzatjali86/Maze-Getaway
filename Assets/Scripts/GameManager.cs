using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.GameFoundation.DataPersistence;
using UnityEngine.GameFoundation;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int gameTime;
    public bool inProgress;
    public int maxFuel;
    public int fuel;
    public Slider fuelSlider;
    public Button next;
    public string nextLevel;
    public int multiplier;
    public int coins;
    public int coinGet;
    public bool greed;
    private bool win = false;
    private bool lose = false;

    public GameObject pausePanel;
    public GameObject winPanel;
    public GameObject greedWinPanel;
    public GameObject losePanel;
    public GameObject loadingPanel;
    public GameObject cameraControlPanel;
    public GameObject hintsPanel;
    public Button restartButton;
    public Button returnMapButton;
    public Button returnMenuButton;
    public Button restartButton2;
    public Button returnMapButton2;
    public Button returnMenuButton2;
    public Button rewardButton;

    public Text fuelLeft;
    public Text multi;
    public Text totalPointText;
    public GameObject coinPanel;
    public Text coinGetText;
    public Text coinPointsText;

    public Text greedCoin;
    public Text greedMulti;
    public Text greedTotal;
    public Text greedBonus;

    public int totalPoint;
    public int coinPoint;
    InventoryItem pointsItem;
    InventoryItem greedPointsItem;
    InventoryItem fuelItem;
    InventoryItem greedEnemyItem;
    InventoryItem greedBridgeItem;
    InventoryItem greedSpecItem;
    InventoryItem greedChopperItem;
    InventoryItem level2Item;
    InventoryItem level3Item;
    InventoryItem level4Item;

    public PlayerController playerC;
    private MapManager mapM;
    private Pathfinding pathf;
    private LevelManager levelM;

    IDataPersistence localPersistence;
    void Awake()
    {
        // choose what format you want to use
        JsonDataSerializer dataSerializer = new JsonDataSerializer();

        // choose where and how the data is stored
        localPersistence = new LocalPersistence(dataSerializer);

        // tell Game Foundation to initialize using this
        // persistence system. Only call Initialize once per session.
        if (!GameFoundation.IsInitialized)
        {
            GameFoundation.Initialize(localPersistence);
        }
    }

    void Start()
    {
        Application.targetFrameRate = 60;

        playerC = GameObject.Find("Player").GetComponent<PlayerController>();
        mapM = GameObject.Find("Map").GetComponent<MapManager>();
        pathf = GameObject.Find("Pathfinder").GetComponent<Pathfinding>();

        try
        {
            levelM = GameObject.Find("Level Select Manager").GetComponent<LevelManager>();
            nextLevel = levelM.nextLevel;
            multiplier = levelM.multiplier;
            coins = levelM.coin;
            greed = levelM.greed;

            if (greed)
            {
                maxFuel = levelM.fuel;
            }
            else
            {
                maxFuel = (int)(pathf.longest.Count * 1.5f);
            }            
        }
        catch
        {

        }

        inProgress = true;
        ShowCC();

        pointsItem = Wallet.GetItem("coin");
        greedPointsItem = Wallet.GetItem("greedCoin");
        fuelItem = Inventory.main.GetItem("greedFuel");
        greedEnemyItem = Inventory.main.GetItem("greedEnemy");
        greedBridgeItem = Inventory.main.GetItem("greedBridge");
        greedSpecItem = Inventory.main.GetItem("greedSpec");
        greedChopperItem = Inventory.main.GetItem("greedChopper");
        level2Item = Inventory.main.GetItem("greedLevel2");
        level3Item = Inventory.main.GetItem("greedLevel3");
        level4Item = Inventory.main.GetItem("greedLevel4");
        coinGet = 0;

        fuelSlider.maxValue = maxFuel;
        if (greed)
        {
            fuel = fuelItem.quantity;
        }
        else
        {
            fuel = maxFuel;
        }
        UpdateFuelSlider();

        pausePanel.SetActive(false);
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        rewardButton.interactable = true;

        if (greed)
        {
            restartButton.interactable = false;
            returnMapButton.interactable = false;
            returnMenuButton.interactable = false;

            restartButton2.interactable = false;
            returnMapButton2.interactable = false;
            returnMenuButton2.interactable = false;
        }

        Advertisements.Instance.ShowBanner(BannerPosition.BOTTOM);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (!pausePanel.activeSelf)
            {
                pausePanel.SetActive(true);
                pausePanel.transform.localPosition = new Vector3(0, 0, 0);
            }
            
            // OR Application.Quit();
        }

        if (win)
        {
            Vector3 _target = new Vector3(0, 100, 0);
            if (greed)
            {
                if (_target != greedWinPanel.transform.localPosition)
                {
                    greedWinPanel.transform.localPosition = Vector3.Lerp(greedWinPanel.transform.localPosition, _target, .1f);
                }
            }
            else
            {
                if (_target != winPanel.transform.localPosition)
                {
                    winPanel.transform.localPosition = Vector3.Lerp(winPanel.transform.localPosition, _target, .1f);
                }
            }
        }

        if (lose)
        {
            Vector3 _target = new Vector3(0, 100, 0);
            if (_target != losePanel.transform.localPosition)
            {
                losePanel.transform.localPosition = Vector3.Lerp(losePanel.transform.localPosition, _target, .1f);
            }
        }
    }

    public void NextTick()
    {

        if (inProgress)
        {
            gameTime++;
        }

        fuel--;
        UpdateFuelSlider();

        if (fuel < 0)
        {
            GameOver();
            FindObjectOfType<AudioManager>().PlaySound("lose");
        }
    }

    private void UpdateFuelSlider()
    {
        fuelSlider.value = fuel;
        if (fuel <= maxFuel * 1/3)
        {
            fuelSlider.fillRect.GetComponent<Image>().color = Color.red;
        }
        else if (fuel <= maxFuel * 2 / 3)
        {
            fuelSlider.fillRect.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            fuelSlider.fillRect.GetComponent<Image>().color = Color.green;
        }
    }

    public void GameOver()
    {
        Debug.Log("game over");
        playerC.stop = true;
        inProgress = false;
        //playerC.ButtonDisable();
        playerC.ButtonFalse();
        losePanel.SetActive(true);
        lose = true;
        //losePanel.transform.localPosition = new Vector3(0, 100, 0);

        if (greed)
        {
            fuelItem.quantity = 50; //TODO: Check with others
            greedEnemyItem.quantity = 1;
            greedBridgeItem.quantity = 5;
            level2Item.quantity = 0;
            level3Item.quantity = 0;
            level4Item.quantity = 0;
            greedPointsItem.quantity = 0;
            PlayerPrefs.SetInt("GreedPlayerCar", 0);
        }

        GameFoundation.Save(localPersistence);
    }

    public void winLevel()
    {
        Debug.Log("you win");
        FindObjectOfType<AudioManager>().PlaySound("Win");
        playerC.stop = true;
        inProgress = false;
        //playerC.ButtonDisable();
        playerC.ButtonFalse();
        winPanel.SetActive(true);
        win = true;
        
        // TODO: Give points
        Points();

        if (greed)
        {
            GreedLevelUp(levelM.mapType);
            fuelItem.quantity = fuel;
        }
        else
        {
            PlayerPrefs.SetInt(nextLevel, 1);
        }

        GameFoundation.Save(localPersistence);
    }

    public void ReturnToMap()
    {
        if (greed)
        {
            SceneManager.LoadScene("GreedLevelSelectScene");
        }
        else
        {
            SceneManager.LoadScene("NormalLevelSelectScene");
        }
        if (levelM.mapX <= 3)
        {

        }
        else
        {
            if (Random.Range(0,100) <= 80)
            {
                Advertisements.Instance.ShowInterstitial(InterstitialClosed);
                Debug.Log("Ads");
            }
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("StartScene");
        if (Random.Range(0, 100) <= 90)
        {
            Advertisements.Instance.ShowInterstitial(InterstitialClosed);
            Debug.Log("Ads");
        }
        Debug.Log("Ads");
    }

    public void Restart()
    {
        loadingPanel.transform.localPosition = new Vector3(0, 0, 0);
        SceneManager.LoadScene("NormalGameScene");
        if (levelM.mapX <= 3)
        {

        }
        else
        {
            if (Random.Range(0, 100) <= 70)
            {
                Advertisements.Instance.ShowInterstitial(InterstitialClosed);
                Debug.Log("Ads");
            }
        }
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
    }

    public void Points()
    {
        if (coins != 0)
        {
            coinPanel.SetActive(true);
        }
        else
        {
            coinPanel.SetActive(false);
        }

        fuelLeft.text = fuel.ToString();
        multi.text = multiplier.ToString();
        totalPoint = fuel * multiplier;
        totalPointText.text = totalPoint.ToString();

        coinGetText.text = coinGet.ToString();
        coinPoint = coinGet * 250;
        if (greed) coinPoint *= multiplier;
        coinPointsText.text = coinPoint.ToString();
        int greedBonusPoint = (50 * ((coinGet - 1) * coinGet) / 2);

        if (greed)
        {
            greedCoin.text = coinGet.ToString();
            greedMulti.text = (250 * multiplier).ToString();
            greedTotal.text = coinPoint.ToString();
            greedBonus.text = greedBonusPoint.ToString();
            //greedPointsItem.quantity += totalPoint;
            greedPointsItem.quantity += coinPoint;
            greedPointsItem.quantity += greedBonusPoint;
        }
        else
        {
            pointsItem.quantity += totalPoint;
            pointsItem.quantity += coinPoint;
        }
    }

    private void GreedLevelUp(int type)
    {
        if (!PlayerPrefs.HasKey("GreedUp"))
        {
            PlayerPrefs.SetInt("GreedUp", 0);
        }
        int upType = PlayerPrefs.GetInt("GreedUp");

        switch (type)
        {
            case 0:
                if (levelM.bridge <= 6 && upType%2 == 0)
                {
                    levelM.bridge += 5;
                }
                if (levelM.enemyCount <= 3 && upType % 2 == 1)
                {
                    levelM.enemyCount += 1;
                }
                break;
            case 1:
                if (levelM.bridge <= 100 && upType % 2 == 0)
                {
                    levelM.bridge += 10;
                }
                if (levelM.enemyCount < 10 && upType % 2 == 1)
                {
                    levelM.enemyCount += 1;
                }
                break;
            case 2:
                if (levelM.bridge <= 200 && upType % 2 == 0)
                {
                    levelM.bridge += 20;
                }
                if (levelM.enemyCount < 20 && upType % 2 == 1)
                {
                    levelM.enemyCount += 1;
                }
                break;
            case 3:
                if (levelM.bridge <= 300 && upType % 2 == 0)
                {
                    levelM.bridge += 30;
                }
                if (levelM.enemyCount < 30 && upType % 2 == 1)
                {
                    levelM.enemyCount += 1;
                }
                break;
            case 4:
                if (upType % 2 == 0)
                {
                    levelM.bridge += 40;
                }
                if (upType % 2 == 1)
                {
                    levelM.enemyCount += 1;
                }
                break;
        }
        upType += 1;
        PlayerPrefs.SetInt("GreedUp", upType);

        if (levelM.enemyCount >= 10)
        {
            levelM.specCount = (levelM.enemyCount - 5) % 5;
        }
        if (levelM.enemyCount > 20)
        {
            levelM.chopper = (levelM.enemyCount - 10) % 10;
        }

        greedEnemyItem.quantity = levelM.enemyCount;
        greedBridgeItem.quantity = levelM.bridge;
        greedSpecItem.quantity = levelM.specCount;
        greedChopperItem.quantity = levelM.chopper;

    }

    private void InterstitialClosed()
    {
        Debug.Log("Interstitial closed -> Resume Game ");
    }

    private void CompleteMethod(bool completed)
    {
        if (completed)
        {
            Debug.Log("Give reward");
            pointsItem.quantity += 150;
            GameFoundation.Save(localPersistence);
        }
        else
        {
            Debug.Log("Ad skipped -> no reward for you");
        }
        rewardButton.interactable = false;
    }

    public void ShowHints()
    {
        hintsPanel.SetActive(true);
        cameraControlPanel.SetActive(false);
    }

    public void ShowCC()
    {
        hintsPanel.SetActive(false);
        cameraControlPanel.SetActive(true);
    }

    public void WatchRewardedVideo()
    {
        Advertisements.Instance.ShowRewardedVideo(CompleteMethod);
    }
}
