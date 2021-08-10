using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public int mapType;
    public int mapX;
    public int mapY;
    public int bridge;
    public int enemyCount;
    public int specCount;
    public int pathRank;
    public int coin;
    public bool greed;
    public int chopper;
    public string nextLevel;
    public int multiplier;
    public int fuel;

    public GameObject loadingPanel;

    public LevelManager levelM;
    
    // Start is called before the first frame update
    void Start()
    {
        levelM = GameObject.Find("Level Select Manager").GetComponent<LevelManager>();
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

        if (mapType == 4)
        {
            if (levelM.enemyCount > 40 && levelM.enemyCount%10 == 0)
            {
                mapX += 2;
                mapY = mapX + 2;
            }
        }
        levelM.coin = coin;

        if (!greed)
        {
            levelM.bridge = bridge;
            levelM.enemyCount = enemyCount;
            levelM.specCount = specCount;
            levelM.pathRank = pathRank;
            levelM.chopper = chopper;
            levelM.nextLevel = nextLevel;
            //levelM.fuel = fuel;
        }
        else
        {
            levelM.greed = greed;
        }
        levelM.multiplier = multiplier;
        SceneManager.LoadSceneAsync("NormalGameScene");
    }
}
