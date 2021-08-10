using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.GameFoundation;
using UnityEngine.GameFoundation.DataPersistence;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int mapType;
    public int mapX;
    public int mapY;
    public int bridge;
    public int enemyCount;
    public int specCount;
    public int pathRank;
    public bool greed;
    public int coin;
    public int chopper;
    public string nextLevel;
    public int car;
    public int multiplier;
    public int fuel;

    GreedInventoryManager gIM;
    InventoryItem greedEnemyItem;
    InventoryItem greedBridgeItem;
    InventoryItem greedSpecItem;
    InventoryItem greedChopperItem;

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

    private static GameObject instance;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = gameObject;
        else
            Destroy(gameObject);

        gIM = GameObject.Find("InventoryManager").GetComponent<GreedInventoryManager>();

        greedEnemyItem = Inventory.main.GetItem("greedEnemy");
        greedBridgeItem = Inventory.main.GetItem("greedBridge");
        greedSpecItem = Inventory.main.GetItem("greedSpec");
        greedChopperItem = Inventory.main.GetItem("greedChopper");

        enemyCount = greedEnemyItem.quantity;
        bridge = greedBridgeItem.quantity;
        specCount = greedSpecItem.quantity;
        chopper = greedChopperItem.quantity;

        if (greed)
        {
            car = PlayerPrefs.GetInt("GreedPlayerCar");
        }
        else
        {
            car = PlayerPrefs.GetInt("PlayerCar");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(greed) fuel = gIM.maxFuel;
    }
}
