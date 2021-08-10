using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public int mapX;
    public int mapY;
    public float midY;
    public int bridgeNo;
    public int enemyType;
    public int enemyCount;
    public int specialEnemyCount;
    public int chopperCount;
    public Sprite sectorSprite;
    private string sectorType;
    private string nodeType;
    public List<GameObject> sectors = new List<GameObject>();
    public List<GameObject> enemyCurrent;
    public GameObject enemyLast;
    public List<string> enemyStart = new List<string>();
    public HashSet<string> enemyStartHS = new HashSet<string>();
    public string startSector;
    public string endSector;
    public int endSectorNo;
    public List<List<string>> bridges = new List<List<string>>();
    private List<List<string>> bridgesToRemove = new List<List<string>>();
    private List<List<string>> bridgesNotRemove = new List<List<string>>();
    private List<List<string>> bridgesRemoved = new List<List<string>>();
    public List<List<GameObject>> paths;
    public List<GameObject> path = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();
    public int pathNo;
    public Button Next;
    public Button Reset;
    private GameObject player;
    public bool enemyIsMoving;
    public int levelType;
    public int fov;

    private LevelManager levelM;

    // Start is called before the first frame update
    void Awake()
    {
        try
        {
            levelM = GameObject.Find("Level Select Manager").GetComponent<LevelManager>();
            mapX = levelM.mapX;
            mapY = levelM.mapY;
            levelType = levelM.mapType;
            enemyCount = levelM.enemyCount;
            specialEnemyCount = levelM.specCount;
            bridgeNo = levelM.bridge;
            chopperCount = levelM.chopper;
        }
        catch
        {

        }

        int fovC = PlayerPrefs.GetInt("FOV");
        switch (fovC)
        {
            case 0:
                fov = 3;
                break;
            case 1:
                fov = 5;
                break;
            case 2:
                fov = 7;
                break;
            default:
                fov = 5;
                break;
        }

        //select start and end sector
        int startSectorNo = Random.Range(0, mapX);
        startSector = "Sector-" + startSectorNo.ToString() + "-0";
        if (startSectorNo <= (int)mapX / 2)
        {
            endSectorNo = Random.Range((int)mapX/2, mapX);
        }
        else
        {
            endSectorNo = Random.Range(0, (int)mapX/2 + 1);
        }
        
        endSector = "Sector-" + endSectorNo.ToString() + "-" + (mapY - 1).ToString();

        //Debug.Log(startSector + ", " + endSector);

        for (int i = 0; i < mapX; i++)
        {
            for (int j = 0; j < mapY; j++)
            {
                if (j == 0 && i == startSectorNo)
                {
                    CreateSector(i, j, "start");
                }
                else if (j == mapY - 1 && i == endSectorNo)
                {
                    CreateSector(i, j, "end");
                }
                else
                {
                    CreateSector(i, j, "NA");
                }
            }
        }

        if (fov == 3 || mapX <= 3)
        {
            transform.localScale = new Vector3(1 / (.629f * 3), 1 / (0.629f * 3), 1 / (0.629f * 3));
        }
        else if (fov == 5 || mapX <= 5)
        {
            transform.localScale = new Vector3(1 / (.629f * 4), 1 / (0.629f * 4), 1 / (0.629f * 4));
        }
        else if (fov == 7)
        {
            transform.localScale = new Vector3(1 / (.629f * 5), 1 / (0.629f * 5), 1 / (0.629f * 5));
        }


        for (int i = 0; i < sectors.Count; i++)
        {
            for (int j = 0; j < sectors.Count; j++)
            {
                if (sectors[i].GetComponent<SectorManager>().sectorX == sectors[j].GetComponent<SectorManager>().sectorX - 1 && sectors[i].GetComponent<SectorManager>().sectorY == sectors[j].GetComponent<SectorManager>().sectorY)
                {
                    List<string> bridgeInfoH = new List<string>() { "H", sectors[i].name, sectors[j].name, "1" };
                    bridges.Add(bridgeInfoH);
                    string st1 = sectors[i].GetComponent<SectorManager>().sectorType;
                    string sp1 = sectors[i].GetComponent<SectorManager>().specialType;
                    string st2 = sectors[j].GetComponent<SectorManager>().sectorType;
                    string sp2 = sectors[j].GetComponent<SectorManager>().specialType;
                    if (st1 != "UR" && st1 != "UL" && st1 != "DR" && st1 != "DL" && sp1 != "ST" && sp1 != "ED" && st2 != "UR" && st2 != "UL" && st2 != "DR" && st2 != "DL" && sp2 != "ST" && sp2 != "ED")
                    {
                        bridgesToRemove.Add(bridgeInfoH);
                    }
                }
                if (sectors[i].GetComponent<SectorManager>().sectorX == sectors[j].GetComponent<SectorManager>().sectorX && sectors[i].GetComponent<SectorManager>().sectorY == sectors[j].GetComponent<SectorManager>().sectorY - 1)
                {
                    List<string> bridgeInfoV = new List<string>() { "V", sectors[i].name, sectors[j].name, "1" };
                    bridges.Add(bridgeInfoV);
                    string st1 = sectors[i].GetComponent<SectorManager>().sectorType;
                    string sp1 = sectors[i].GetComponent<SectorManager>().specialType;
                    string st2 = sectors[j].GetComponent<SectorManager>().sectorType;
                    string sp2 = sectors[j].GetComponent<SectorManager>().specialType;
                    if (st1 != "UR" && st1 != "UL" && st1 != "DR" && st1 != "DL" && sp1 != "ST" && sp1 != "ED" && st2 != "UR" && st2 != "UL" && st2 != "DR" && st2 != "DL" && sp2 != "ST" && sp2 != "ED")
                    {
                        bridgesToRemove.Add(bridgeInfoV);
                    }
                }
            }
        }
        bridgesToRemove.Shuffle();

        //Debug.Log(bridges.Count);       

        //TODO: Condition for removing bridge
        for (int i = 0; i < bridgeNo; i++)
        {
            foreach (List<string> bNR in bridgesNotRemove)
            {
                //Debug.Log(bNR[1] + " " + bNR[2]);
                bridgesToRemove.Remove(bNR);
            }
            //Debug.Log(bridgesToRemove.Count);
            if (bridgesToRemove.Count > 0)
            {
                List<string> bridgeRemoved = bridgesToRemove[Random.Range(0, bridgesToRemove.Count)];
                //Debug.Log("remove " + bridgeRemoved[0] + " " + bridgeRemoved[1] + " " + bridgeRemoved[2]);

                List<List<string>> sharedBridges1 = SharedBridge(bridgeRemoved, true);
                int noOfShared1 = 0;

                List<List<string>> sharedBridges2 = SharedBridge(bridgeRemoved, false);
                int noOfShared2 = 0;

                List<List<string>> parallelBridge = ParallelBridge(bridgeRemoved, mapX, mapY);
                int noOfParallel = 0;

                foreach (List<string> b in bridgesToRemove)
                {
                    if (((b[1] == sharedBridges1[0][1] && b[2] == sharedBridges1[0][2]) || (b[1] == sharedBridges1[1][1] && b[2] == sharedBridges1[1][2]) || (b[1] == sharedBridges1[2][1] && b[2] == sharedBridges1[2][2])) && noOfShared1 < 2)
                    {
                        //Debug.Log(b[1] + " " + b[2]);
                        bridgesNotRemove.Add(b);
                        noOfShared1++;
                    }
                    if (((b[1] == sharedBridges2[0][1] && b[2] == sharedBridges2[0][2]) || (b[1] == sharedBridges2[1][1] && b[2] == sharedBridges2[1][2]) || (b[1] == sharedBridges2[2][1] && b[2] == sharedBridges2[2][2])) && noOfShared2 < 2)
                    {
                        //Debug.Log(b[1] + " " + b[2]);
                        bridgesNotRemove.Add(b);
                        noOfShared2++;
                    }
                    if (((b[1] == parallelBridge[0][1] && b[2] == parallelBridge[0][2]) || (b[1] == parallelBridge[1][1] && b[2] == parallelBridge[1][2])) && noOfParallel < 1)
                    {
                        //Debug.Log("parallel " + b[1] + " " + b[2]);
                        bridgesNotRemove.Add(b);
                        noOfParallel++;
                    }
                }
                bridgesRemoved.Add(bridgeRemoved);
                bridgesToRemove.Remove(bridgeRemoved);
                bridges.Remove(bridgeRemoved);
            }
        }
        //Debug.Log(bridgesRemoved.Count);

        //Debug.Log(bridgesRemoved.Count);

        foreach (GameObject sector in sectors)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    bool isBridge = true;
                    //CreateNodes(sector, sector.GetComponent<SectorManager>().sectorType, i, j, false);
                    foreach (List<string> bR in bridgesRemoved)
                    {                        
                        if (bR[0] == "H" && sector.name == bR[1] && i == 2 && j == 1)
                        {
                            isBridge = false;
                        }
                        else if (bR[0] == "V" && sector.name == bR[1] && i == 1 && j == 2)
                        {
                            isBridge = false;
                        }
                        else if (bR[0] == "H" && sector.name == bR[2] && i == 0 && j == 1)
                        {
                            isBridge = false;
                        }
                        else if (bR[0] == "V" && sector.name == bR[2] && i == 1 && j == 0)
                        {
                            isBridge = false;
                        }
                    }
                    CreateNodes(sector, sector.GetComponent<SectorManager>().sectorType, sector.GetComponent<SectorManager>().specialType, i, j, isBridge);
                }
            }
            //sector.GetComponent<SectorManager>().CreateNodes(sector.GetComponent<SectorManager>().sectorType);
        }

        //GameObject.Find("Route").GetComponent<RouteManager>().CreateRoutes(startSector, endSector, bridges);

        //paths = GameObject.Find("Route").GetComponent<RouteManager>().CreateRoutes();

        path = GameObject.Find("Pathfinder").GetComponent<Pathfinding>().CreateLongestPath();

        enemyCurrent = sectors.GetRange(0, sectors.Count);
        
    }

    private void Start()
    {
        CheckEnemyPosition();
        //GameObject enemyCube = Instantiate(Resources.Load("Prefab/EnemyCube", typeof(GameObject))) as GameObject;
        //enemies.Add(enemyCube);
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        CreateEnemies();
        CreateChopper();

        //player = Instantiate(Resources.Load("Prefab/Player", typeof(GameObject))) as GameObject;
    }

    public void CheckEnemyPosition()
    {
        int moves;
        ResetTest();

        enemyCurrent.Remove(GameObject.Find(startSector));
        for (int i = 0; i < enemyCount; i++)
        {
            if (enemyCurrent.Count > 0 && enemyStart.Count < sectors.Count * 3)
            {
                enemyType = i % 3;
                enemyLast = Instantiate(Resources.Load("Prefab/EnemyCube", typeof(GameObject))) as GameObject;
                enemyLast.GetComponent<EnemyChecker>().type = enemyType;
                enemies.Add(enemyLast);
                enemyStart.Clear();
                enemyStartHS.Clear();
                do
                {
                    foreach (GameObject enemy in enemies)
                    {
                        enemy.GetComponent<EnemyChecker>().Reposition();
                    }
                    moves = 0;
                    for (int j = 0; j < path.Count; j++)
                    {
                        if (enemyStart.Count < sectors.Count * 3)
                        {
                            //Debug.Log(j);
                            gameObject.GetComponent<EnemyManager>().OneStep(j);
                            if (gameObject.GetComponent<EnemyManager>().overlap)
                            {
                                enemyStart.Add(enemyLast.GetComponent<EnemyChecker>().start.name + enemyLast.GetComponent<EnemyChecker>().direction);
                                enemyStartHS.Add(enemyLast.GetComponent<EnemyChecker>().start.name + enemyLast.GetComponent<EnemyChecker>().direction);
                                enemyLast.GetComponent<EnemyChecker>().Restart();
                                break;
                            }
                        }
                        moves++;
                    }
                    //Debug.Log("moves: " + moves);
                } while (moves != path.Count);
                enemyCurrent.Remove(enemyLast.GetComponent<EnemyChecker>().start);

                foreach (GameObject neighbour in enemyLast.GetComponent<EnemyChecker>().GetStartNeighbour(enemyLast.GetComponent<EnemyChecker>().start))
                {
                    enemyCurrent.Remove(neighbour);
                }

            }
            else
            {
                enemies.Remove(enemyLast);
                Destroy(enemyLast);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //when t == pathcount; reset
    }

    void CreateSector(int sectorX, int sectorY, string special)
    {
        GameObject sector = new GameObject("Sector" + "-" + sectorX.ToString() + "-" + sectorY.ToString());
        sector.AddComponent<SectorManager>();
        sector.GetComponent<SectorManager>().sectorX = sectorX;
        sector.GetComponent<SectorManager>().sectorY = sectorY;
        sector.tag = "Sector";
        if (special == "start")
        {
            sector.GetComponent<SectorManager>().specialType = "ST";
        }
        else if (special == "end")
        {
            sector.GetComponent<SectorManager>().specialType = "ED";
        }
        sector.GetComponent<SectorManager>().sectorType = GetSectorType(sectorX, sectorY);
        sector.transform.parent = gameObject.transform;
        sectors.Add(sector);
    }

    string GetSectorType(int sectorX, int sectorY)
    {

        if (sectorX == 0 && sectorY == 0)
        {
            sectorType = "DL";
        }
        else if (sectorX == 0 && sectorY == mapY - 1)
        {
            sectorType = "UL";
        }
        else if (sectorX == mapX - 1 && sectorY == 0)
        {
            sectorType = "DR";
        }
        else if (sectorX == mapX - 1 && sectorY == mapY - 1)
        {
            sectorType = "UR";
        }
        else if (sectorX == 0)
        {
            sectorType = "L";
        }
        else if (sectorX == mapX - 1)
        {
            sectorType = "R";
        }
        else if (sectorY == 0)
        {
            sectorType = "D";
        }
        else if (sectorY == mapY - 1)
        {
            sectorType = "U";
        }
        else
        {
            sectorType = "M";
        }

        return sectorType;
    }

    public void CreateNodes(GameObject sector, string sectorType, string specialType, int nodeX, int nodeY, bool isBridge)
    {
        nodeType = GetNodeType(sectorType, specialType, nodeX, nodeY, isBridge);

        GameObject node = new GameObject();
        node.name = "Node" + "-" + nodeX.ToString() + "-" + nodeY.ToString();
        node.AddComponent<NodeManager>();
        node.GetComponent<NodeManager>().nodeX = nodeX;
        node.GetComponent<NodeManager>().nodeY = nodeY;
        node.GetComponent<NodeManager>().nodeType = nodeType;
        node.GetComponent<NodeManager>().sectorType = sectorType;
        node.GetComponent<NodeManager>().isBridge = isBridge;
        node.GetComponent<NodeManager>().levelType = levelType;
        node.transform.parent = sector.transform;
    }

    void CreateEnemies()
    {
        for (int i = 0; i < (enemies.Count - specialEnemyCount); i++)
        {
            string objPath = null;
            switch (enemies[i].GetComponent<EnemyChecker>().type)
            {
                case 0:
                    objPath = "Prefab/EnemyBike";
                    break;
                case 1:
                    objPath = "Prefab/EnemyCar";
                    break;
                case 2:
                    objPath = "Prefab/EnemyTruck";
                    break;
            }

            GameObject enemy = Instantiate(Resources.Load(objPath, typeof(GameObject))) as GameObject;
            enemy.GetComponent<EnemyController>().start = enemies[i].GetComponent<EnemyChecker>().start;
            enemy.GetComponent<EnemyController>().startDirection = enemies[i].GetComponent<EnemyChecker>().startDirection;
            enemy.GetComponent<EnemyController>().startPrefTurn = enemies[i].GetComponent<EnemyChecker>().startPrefTurn;
            enemy.GetComponent<EnemyController>().type = enemies[i].GetComponent<EnemyChecker>().type;
            enemy.GetComponent<EnemyController>().Restart();
        }
        for (int i = (enemies.Count - specialEnemyCount); i < enemies.Count; i++)
        {
            string objPath = null;
            switch (enemies[i].GetComponent<EnemyChecker>().type)
            {
                case 0:
                    objPath = "Prefab/SpecBike";
                    break;
                case 1:
                    objPath = "Prefab/SpecCar";
                    break;
                case 2:
                    objPath = "Prefab/SpecTruck";
                    break;
            }

            GameObject enemy = Instantiate(Resources.Load(objPath, typeof(GameObject))) as GameObject;
            enemy.GetComponent<EnemyController>().start = enemies[i].GetComponent<EnemyChecker>().start;
            enemy.GetComponent<EnemyController>().startDirection = enemies[i].GetComponent<EnemyChecker>().startDirection;
            enemy.GetComponent<EnemyController>().startPrefTurn = enemies[i].GetComponent<EnemyChecker>().startPrefTurn;
            enemy.GetComponent<EnemyController>().type = enemies[i].GetComponent<EnemyChecker>().type;
            enemy.GetComponent<EnemyController>().specialType = true;
            //enemy.GetComponent<Renderer>().material.color = Color.red;
            enemy.GetComponent<EnemyController>().Restart();
        }
    }

    public void CreateChopper()
    {
        for (int i = 0; i < chopperCount; i++)
        {
            GameObject chopper = Instantiate(Resources.Load("Prefab/Chopper", typeof(GameObject))) as GameObject;
            chopper.GetComponent<ChopperManager>().distance = (i * 2) + 1;
        }
    }

    public void ResetTest()
    {
        Debug.Log("reset");
        enemyStart.Clear();
        enemyCurrent = sectors.GetRange(0, sectors.Count);
    }

    string GetNodeType(string sectorType, string specialType, int nodeX, int nodeY, bool isBridge)
    {
        nodeType = "E";

        if (isBridge)
        {
            if (nodeX == 1 && nodeY == 1)
            {
                nodeType = "M";
            }
            if (nodeX == 0 && nodeY == 1)
            {
                if (sectorType != "L" && sectorType != "UL" && sectorType != "DL")
                {
                    nodeType = "L";
                }
            }
            if (nodeX == 2 && nodeY == 1)
            {
                if (sectorType != "R" && sectorType != "UR" && sectorType != "DR")
                {
                    nodeType = "R";
                }
            }
            if (nodeX == 1 && nodeY == 2)
            {
                if (sectorType != "U" && sectorType != "UL" && sectorType != "UR" || specialType == "ED")
                {
                    nodeType = "U";
                }
            }
            if (nodeX == 1 && nodeY == 0)
            {
                if (sectorType != "D" && sectorType != "DL" && sectorType != "DR" || specialType == "ST")
                {
                    nodeType = "D";
                }
            }
        }        
        

        return nodeType;
    }

    List<List<string>> ParallelBridge(List<string> bridge, int mapX, int mapY)
    {
        List<List<string>> parallelBridges = new List<List<string>>();
        char[] separator = { '-' };

        if (bridge[0] == "H")
        {
            string[] strList1 = bridge[1].Split(separator);
            string[] strList2 = bridge[2].Split(separator);

            if (System.Convert.ToInt32(strList1[2]) == mapY - 2)
            {
                string parallelSector1 = "Sector-" + strList1[1] + "-" + (System.Convert.ToInt32(strList1[2]) + 1).ToString();
                string parallelSector2 = "Sector-" + strList2[1] + "-" + (System.Convert.ToInt32(strList2[2]) + 1).ToString();

                List<string> parallelBridge = new List<string>() { bridge[0], parallelSector1, parallelSector2 };
                parallelBridges.Add(parallelBridge);
                parallelBridges.Add(parallelBridge);
            }
            else if (System.Convert.ToInt32(strList1[2]) == 1)
            {
                string parallelSector1 = "Sector-" + strList1[1] + "-" + (System.Convert.ToInt32(strList1[2]) - 1).ToString();
                string parallelSector2 = "Sector-" + strList2[1] + "-" + (System.Convert.ToInt32(strList2[2]) - 1).ToString();

                List<string> parallelBridge = new List<string>() { bridge[0], parallelSector1, parallelSector2 };
                parallelBridges.Add(parallelBridge);
                parallelBridges.Add(parallelBridge);
            }
            else
            {
                string parallelSector11 = "Sector-" + strList1[1] + "-" + (System.Convert.ToInt32(strList1[2]) + 1).ToString();
                string parallelSector12 = "Sector-" + strList1[1] + "-" + (System.Convert.ToInt32(strList1[2]) - 1).ToString();

                string parallelSector21 = "Sector-" + strList2[1] + "-" + (System.Convert.ToInt32(strList2[2]) + 1).ToString();
                string parallelSector22 = "Sector-" + strList2[1] + "-" + (System.Convert.ToInt32(strList2[2]) - 1).ToString();

                List<string> parallelBridge1 = new List<string>() { bridge[0], parallelSector11, parallelSector21 };
                parallelBridges.Add(parallelBridge1);

                List<string> parallelBridge2 = new List<string>() { bridge[0], parallelSector12, parallelSector22 };
                parallelBridges.Add(parallelBridge2);
            }

        }
        else if (bridge[0] == "V")
        {

            string[] strList1 = bridge[1].Split(separator);
            string[] strList2 = bridge[2].Split(separator);

            if (System.Convert.ToInt32(strList1[1]) == mapX - 2)
            {
                string parallelSector1 = "Sector-" + (System.Convert.ToInt32(strList1[1]) + 1).ToString() + "-" + strList1[2];
                string parallelSector2 = "Sector-" + (System.Convert.ToInt32(strList2[1]) + 1).ToString() + "-" + strList2[2];

                List<string> parallelBridge = new List<string>() { bridge[0], parallelSector1, parallelSector2 };
                parallelBridges.Add(parallelBridge);
                parallelBridges.Add(parallelBridge);
            }
            else if (System.Convert.ToInt32(strList1[1]) == 1)
            {
                string parallelSector1 = "Sector-" + (System.Convert.ToInt32(strList1[1]) - 1).ToString() + "-" + strList1[2];
                string parallelSector2 = "Sector-" + (System.Convert.ToInt32(strList2[1]) - 1).ToString() + "-" + strList2[2];

                List<string> parallelBridge = new List<string>() { bridge[0], parallelSector1, parallelSector2 };
                parallelBridges.Add(parallelBridge);
                parallelBridges.Add(parallelBridge);
            }
            else
            {
                string parallelSector11 = "Sector-" + (System.Convert.ToInt32(strList1[1]) + 1).ToString() + "-" + strList1[2];
                string parallelSector12 = "Sector-" + (System.Convert.ToInt32(strList1[1]) - 1).ToString() + "-" + strList1[2];

                string parallelSector21 = "Sector-" + (System.Convert.ToInt32(strList2[1]) + 1).ToString() + "-" + strList2[2];
                string parallelSector22 = "Sector-" + (System.Convert.ToInt32(strList2[1]) - 1).ToString() + "-" + strList2[2];

                List<string> parallelBridge1 = new List<string>() { bridge[0], parallelSector11, parallelSector21 };
                parallelBridges.Add(parallelBridge1);

                List<string> parallelBridge2 = new List<string>() { bridge[0], parallelSector12, parallelSector22 };
                parallelBridges.Add(parallelBridge2);
            }
        }

        parallelBridges.Shuffle();

        return parallelBridges;
    }

    List<List<string>> SharedBridge(List<string> bridge, bool isFirst)
    {
        char[] separator = { '-' };
        List<List<string>> sharedBridges = new List<List<string>>();

        if (isFirst)
        {
            string[] strList1 = bridge[1].Split(separator);

            string sharedSector1 = "Sector-" + strList1[1] + "-" + (System.Convert.ToInt32(strList1[2]) - 1).ToString();
            List<string> sharedBridge1 = new List<string>() { "V", sharedSector1, bridge[1] };
            sharedBridges.Add(sharedBridge1);

            string sharedSector2 = "Sector-" + (System.Convert.ToInt32(strList1[1]) - 1).ToString() + "-" + strList1[2];
            List<string> sharedBridge2 = new List<string>() { "H", sharedSector2, bridge[1] };
            sharedBridges.Add(sharedBridge2);

            if (bridge[0] == "H")
            {
                string sharedSector3 = "Sector-" + strList1[1] + "-" + (System.Convert.ToInt32(strList1[2]) + 1).ToString();
                List<string> sharedBridge3 = new List<string>() { "V", bridge[1], sharedSector3 };
                sharedBridges.Add(sharedBridge3);
            }
            else
            {
                string sharedSector3 = "Sector-" + (System.Convert.ToInt32(strList1[1]) + 1).ToString() + "-" + strList1[2];
                List<string> sharedBridge3 = new List<string>() { "H", bridge[1], sharedSector3 };
                sharedBridges.Add(sharedBridge3);
            }

        }
        else
        {
            string[] strList1 = bridge[2].Split(separator);

            string sharedSector1 = "Sector-" + strList1[1] + "-" + (System.Convert.ToInt32(strList1[2]) + 1).ToString();
            List<string> sharedBridge1 = new List<string>() { "V", bridge[2], sharedSector1 };
            sharedBridges.Add(sharedBridge1);

            string sharedSector2 = "Sector-" + (System.Convert.ToInt32(strList1[1]) + 1).ToString() + "-" + strList1[2];
            List<string> sharedBridge2 = new List<string>() { "H", bridge[2], sharedSector2 };
            sharedBridges.Add(sharedBridge2);

            if (bridge[0] == "H")
            {
                string sharedSector3 = "Sector-" + strList1[1] + "-" + (System.Convert.ToInt32(strList1[2]) - 1).ToString();
                List<string> sharedBridge3 = new List<string>() { "V", sharedSector3, bridge[2] };
                sharedBridges.Add(sharedBridge3);
            }
            else
            {
                string sharedSector3 = "Sector-" + (System.Convert.ToInt32(strList1[1]) - 1).ToString() + "-" + strList1[2];
                List<string> sharedBridge3 = new List<string>() { "H", sharedSector3, bridge[2] };
                sharedBridges.Add(sharedBridge3);
            }
        }

        sharedBridges.Shuffle();

        return sharedBridges;
    }
}

public static class IListExtensions
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}


