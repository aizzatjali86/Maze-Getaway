using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GreedLevelSelect : MonoBehaviour
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
    public int multiplier;
    public int fuel;

    public GameObject loadingPanel;

    public GreedLevelManager levelM;

    // Start is called before the first frame update
    void Start()
    {
        levelM = GameObject.Find("Level Select Manager").GetComponent<GreedLevelManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LevelButton()
    {
        loadingPanel.transform.localPosition = new Vector3(0, 0, 0);
        levelM.mapType = mapType;
        levelM.mapX = mapX;
        levelM.mapY = mapY;
        levelM.bridge = bridge;
        levelM.enemyCount = enemyCount;
        levelM.specCount = specCount;
        levelM.pathRank = pathRank;
        levelM.coin = coin;
        levelM.chopper = chopper;
        levelM.nextLevel = nextLevel;
        levelM.multiplier = multiplier;
        levelM.fuel = fuel;
        SceneManager.LoadSceneAsync("GreedGameScene");
    }
}
