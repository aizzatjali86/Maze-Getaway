using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.GameFoundation;
using UnityEngine.SceneManagement;
using UnityEngine.GameFoundation.DataPersistence;

public class StartManager : MonoBehaviour
{
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

    public GameObject normalInfoPanel;
    public GameObject greedInfoPanel;
    public GameObject confirmPanel;
    public GameObject aboutPanel;

    IDataPersistence localPersistence;

    // Start is called before the first frame update
    private void Awake()
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

        GameFoundation.Load(localPersistence);

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

        try
        {
            Destroy(GameObject.Find("Level Select Manager"));
        }
        catch
        {

        }

        Advertisements.Instance.ShowBanner(BannerPosition.BOTTOM);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            //SceneManager.LoadSceneAsync("StartScene");

            Application.Quit();
        }
    }

    public void NormalGame()
    {
        SceneManager.LoadSceneAsync("NormalLevelSelectScene");
    }

    public void ConfirmGreed()
    {
        confirmPanel.transform.localPosition = new Vector3(0, confirmPanel.transform.localPosition.y, confirmPanel.transform.localPosition.z);
    }

    public void NewGreed()
    {
        SceneManager.LoadSceneAsync("GreedLevelSelectScene");
        fuelItem.quantity = 50; //TODO: Check with others
        greedEnemyItem.quantity = 1;
        greedBridgeItem.quantity = 5;
        greedSpecItem.quantity = 0;
        greedChopperItem.quantity = 0;
        level2Item.quantity = 0;
        level3Item.quantity = 0;
        level4Item.quantity = 0;
        greedPointsItem.quantity = 0;
        PlayerPrefs.SetFloat("cameraPos", -26);

        GameFoundation.Save(localPersistence);
    }

    public void ContinueGreed()
    {
        SceneManager.LoadSceneAsync("GreedLevelSelectScene");
    }

    public void NormalInfo()
    {
        normalInfoPanel.transform.localPosition = new Vector3(0, normalInfoPanel.transform.localPosition.y, normalInfoPanel.transform.localPosition.z);
    }

    public void GreedInfo()
    {
        greedInfoPanel.transform.localPosition = new Vector3(0, greedInfoPanel.transform.localPosition.y, greedInfoPanel.transform.localPosition.z);
    }

    public void HideInfo(string panel)
    {
        if (panel == "normal")
        {
            normalInfoPanel.transform.localPosition = new Vector3(10, normalInfoPanel.transform.localPosition.y, normalInfoPanel.transform.localPosition.z);
        }
        else if (panel == "greed")
        {
            greedInfoPanel.transform.localPosition = new Vector3(10, greedInfoPanel.transform.localPosition.y, greedInfoPanel.transform.localPosition.z);
        }
        else
        {
            confirmPanel.transform.localPosition = new Vector3(-20, confirmPanel.transform.localPosition.y, confirmPanel.transform.localPosition.z);
        }
    }

    public void OpenTextLink(string link)
    {
        Application.OpenURL(link);
    }

    public void ShowAbout()
    {
        aboutPanel.transform.localPosition = new Vector3(0, 0, aboutPanel.transform.localPosition.z);
    }

    public void HideAbout()
    {
        aboutPanel.transform.localPosition = new Vector3(0, -150, aboutPanel.transform.localPosition.z);
    }
}
