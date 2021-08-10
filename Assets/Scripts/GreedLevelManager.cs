using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GreedLevelManager : MonoBehaviour
{
    public int mapType;
    public int mapX;
    public int mapY;
    public int bridge;
    public int enemyCount;
    public int specCount;
    public int pathRank;
    public int coin;
    public int chopper;
    public string nextLevel;
    public int car;
    public int multiplier;
    public int fuel;

    private static GameObject instance;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = gameObject;
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
