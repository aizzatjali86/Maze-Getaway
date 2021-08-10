using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    private MapManager mapM;
    private GameManager gameM;
    public PlayerController playerC;
    public List<GameObject> sectors;
    public List<GameObject> sectorsCoin;
    private int minE;
    private int minP;
    private int minC;
    public int coin;
    public GameObject firstCoin;
    public int coinsBefore;
    public bool greedMode;

    // Start is called before the first frame update
    void Start()
    {
        mapM = GameObject.Find("Map").GetComponent<MapManager>();
        gameM = GameObject.Find("Game Manager").GetComponent<GameManager>();
        playerC = GameObject.Find("Player").GetComponent<PlayerController>();

        sectors = mapM.sectors;
        minE = mapM.mapX - 1;
        minP = mapM.mapX - 1;
        minC = mapM.mapX - 1;

        try
        {
            LevelManager levelM = GameObject.Find("Level Select Manager").GetComponent<LevelManager>();
            coin = levelM.coin;
            greedMode = levelM.greed;
        }
        catch
        {

        }

        for (int i = 0; i < coin; i++)
        {
            CreateCoin(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (greedMode)
        {
            if (gameM.coinGet > coinsBefore)
            {
                CreateCoin(1);
            }
        }
        coinsBefore = gameM.coinGet;
    }

    public void CreateCoin(int coinNumber)
    {
        sectorsCoin = new List<GameObject>();
        foreach (GameObject sector in sectors)
        {
            SectorManager sm = sector.GetComponent<SectorManager>();
            int Xp = Mathf.Abs(playerC.currentX - sm.sectorX);
            int Yp = Mathf.Abs(playerC.currentY - sm.sectorY);
            int Xe = Mathf.Abs(mapM.endSectorNo - sm.sectorX);
            int Ye = Mathf.Abs(mapM.mapY - 1 - sm.sectorY);
            //Debug.Log("Xp: " + Xp + ", Yp: " + Yp + ", Xe: " + Xe + ", Ye: " + Ye + ", minE: " + minE + ", minP: " + minP);
            if (coinNumber == 0)
            {
                if (Xp + Yp >= minP && Xe + Ye >= minE)
                {
                    sectorsCoin.Add(sector);
                }
            }
            else
            {
                int Xc1 = Mathf.Abs(firstCoin.GetComponent<SectorManager>().sectorX - sm.sectorX);
                int Yc1 = Mathf.Abs(firstCoin.GetComponent<SectorManager>().sectorY - sm.sectorY);
                if (Xp + Yp >= minP && Xe + Ye >= minE && Xc1 + Yc1 >= minC)
                {                   
                    sectorsCoin.Add(sector);
                }
            }

        }
        GameObject coinLocation = sectorsCoin[Random.Range(0, sectorsCoin.Count)];
        firstCoin = coinLocation;
        GameObject coin = Instantiate(Resources.Load("Prefab/Coin", typeof(GameObject))) as GameObject;
        coin.transform.parent = coinLocation.transform;
        coin.transform.localPosition = new Vector3(0, 0, 0);
    }
}
