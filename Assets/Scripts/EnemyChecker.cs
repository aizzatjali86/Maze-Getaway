using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChecker : MonoBehaviour
{
    public int type;
    public bool specialType;
    public bool inPursuit;
    public List<GameObject> pursuitPath;
    public List<string> pursuitPathDirection;
    private MapManager mapM;
    private GameManager gameM;
    private Pathfinding pathf;
    public GameObject start;
    public GameObject current;
    public GameObject previous;
    public GameObject next;
    public string prefTurn;
    public string startPrefTurn;
    public string direction;
    public string startDirection;
    public string prevDirection;
    public Quaternion rotation;
    public int moveCount;
    public List<GameObject> neighbours;
    public List<string> neighboursD;
    public AnimationCurve MoveCurve;
    public bool isMoving;

    public List<List<string>> bridges;
    public int gameTimeBefore;
    public int testTime;
    int t = 0;

    // Start is called before the first frame update
    void Awake()
    {

        Restart();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NextMove(int type)
    {
        int turnCheck = type + 1;

        if (moveCount < turnCheck - 1)
        {
            StraightMove();
        }
        else
        {
            TurnMove();
        }
    }

    public void StraightMove()
    {
        prevDirection = direction;
        if (neighboursD.Contains(direction))
        {
            next = neighbours[neighboursD.IndexOf(direction)];
            moveCount++;
        }
        else
        {
            if (neighbours.Count == 1)
            {
                next = neighbours[0];
                direction = neighboursD[0];
                moveCount++;
            }
            else
            {
                TurnMove();
            }
        }
    }

    public void TurnMove()
    {
        prevDirection = direction;
        if (neighbours.Count == 1)
        {
            next = neighbours[0];
            if (neighboursD[0] == direction)
            {

            }
            else
            {
                direction = neighboursD[0];
            }
        }
        else
        {
            if (prefTurn == "Right")
            {
                TurnRight();
                prefTurn = "Left";
                if (neighboursD.Contains(direction))
                {

                }
                else
                {
                    TurnLeft();
                }
                //turn and change turn
            }
            else
            {
                //turn and change turn
                TurnLeft();
                prefTurn = "Right";
                if (neighboursD.Contains(direction))
                {

                }
                else
                {
                    TurnRight();
                }
            }
            next = neighbours[neighboursD.IndexOf(direction)];
        }
        moveCount = 0;
    }

    public void MoveTick()
    {
        previous = current;
        current = next;
        transform.parent = next.transform;
        //transform.localPosition = new Vector3(0, 1, 0);
        transform.localScale = new Vector3(1, 1, 1);

        neighbours = GetNeighbour(current, direction);
    }

    public List<GameObject> GetNeighbour(GameObject current, string direction)
    {
        bridges = GameObject.Find("Map").GetComponent<MapManager>().bridges;
        neighboursD = new List<string>();

        char[] separator = { '-' };
        string[] strList1 = current.name.Split(separator);
        List<GameObject> neighbours = new List<GameObject>();

        string neighbour1 = "Sector-" + strList1[1] + "-" + (System.Convert.ToInt32(strList1[2]) - 1).ToString();

        string neighbour2 = "Sector-" + strList1[1] + "-" + (System.Convert.ToInt32(strList1[2]) + 1).ToString();

        string neighbour3 = "Sector-" + (System.Convert.ToInt32(strList1[1]) - 1).ToString() + "-" + strList1[2];

        string neighbour4 = "Sector-" + (System.Convert.ToInt32(strList1[1]) + 1).ToString() + "-" + strList1[2];

        foreach (List<string> b in bridges)
        {
            if (b[1] == neighbour1 && b[2] == current.name && direction != "U")
            {
                neighbours.Add(GameObject.Find(neighbour1));
                neighboursD.Add("D");
                //Debug.Log("add V1");
            }
            if (b[1] == current.name && b[2] == neighbour2 && direction != "D")
            {
                neighbours.Add(GameObject.Find(neighbour2));
                neighboursD.Add("U");
                //Debug.Log("add V2");
            }
            if (b[1] == neighbour3 && b[2] == current.name && direction != "R")
            {
                neighbours.Add(GameObject.Find(neighbour3));
                neighboursD.Add("L");
                //Debug.Log("add H1");
            }
            if (b[1] == current.name && b[2] == neighbour4 && direction != "L")
            {
                neighbours.Add(GameObject.Find(neighbour4));
                neighboursD.Add("R");
                //Debug.Log("add H2");
            }
        }

        return neighbours;
    }

    public List<GameObject> GetStartNeighbour(GameObject current)
    {
        bridges = GameObject.Find("Map").GetComponent<MapManager>().bridges;
        neighboursD = new List<string>();

        char[] separator = { '-' };
        string[] strList1 = current.name.Split(separator);
        List<GameObject> neighbours = new List<GameObject>();

        string neighbour1 = "Sector-" + strList1[1] + "-" + (System.Convert.ToInt32(strList1[2]) - 1).ToString();

        string neighbour2 = "Sector-" + strList1[1] + "-" + (System.Convert.ToInt32(strList1[2]) + 1).ToString();

        string neighbour3 = "Sector-" + (System.Convert.ToInt32(strList1[1]) - 1).ToString() + "-" + strList1[2];

        string neighbour4 = "Sector-" + (System.Convert.ToInt32(strList1[1]) + 1).ToString() + "-" + strList1[2];

        string neighbour5 = "Sector-" + (System.Convert.ToInt32(strList1[1]) - 1).ToString() + "-" + (System.Convert.ToInt32(strList1[2]) - 1).ToString();

        string neighbour6 = "Sector-" + (System.Convert.ToInt32(strList1[1]) - 1).ToString() + "-" + (System.Convert.ToInt32(strList1[2]) + 1).ToString();

        string neighbour7 = "Sector-" + (System.Convert.ToInt32(strList1[1]) + 1).ToString() + "-" + (System.Convert.ToInt32(strList1[2]) - 1).ToString();

        string neighbour8 = "Sector-" + (System.Convert.ToInt32(strList1[1]) + 1).ToString() + "-" + (System.Convert.ToInt32(strList1[2]) + 1).ToString();

        neighbours.Add(GameObject.Find(neighbour1));
        neighbours.Add(GameObject.Find(neighbour2));
        neighbours.Add(GameObject.Find(neighbour3));
        neighbours.Add(GameObject.Find(neighbour4));
        neighbours.Add(GameObject.Find(neighbour5));
        neighbours.Add(GameObject.Find(neighbour6));
        neighbours.Add(GameObject.Find(neighbour7));
        neighbours.Add(GameObject.Find(neighbour8));

        return neighbours;
    }

    public void TurnRight()
    {
        switch (direction)
        {
            case "U":
                direction = "R";
                break;
            case "R":
                direction = "D";
                break;
            case "D":
                direction = "L";
                break;
            case "L":
                direction = "U";
                break;
        }
    }

    public void TurnLeft()
    {
        switch (direction)
        {
            case "U":
                direction = "L";
                break;
            case "L":
                direction = "D";
                break;
            case "D":
                direction = "R";
                break;
            case "R":
                direction = "U";
                break;
        }
    }

    public void TurnBack()
    {
        switch (direction)
        {
            case "U":
                direction = "D";
                break;
            case "L":
                direction = "R";
                break;
            case "D":
                direction = "U";
                break;
            case "R":
                direction = "L";
                break;
        }
    }

    public void TargetDirection()
    {
        switch (direction)
        {
            case "U":
                rotation = Quaternion.Euler(0, 0, 0);
                break;
            case "L":
                rotation = Quaternion.Euler(0, -90, 0);
                break;
            case "D":
                rotation = Quaternion.Euler(0, 180, 0);
                break;
            case "R":
                rotation = Quaternion.Euler(0, 90, 0);
                break;
        }
    }

    public void Collide2()
    {
        //Debug.Log("collide");
        next = previous;
        previous = current;
        current = next;
        direction = prevDirection;
        TurnBack();
        MoveTick();
        NextMove(type);
        TargetDirection();
        //Debug.Log(current.name + " collide " + other.gameObject.transform.parent.name);
    }

    public void Collide()
    {
        //Debug.Log("collide");
        next = previous;
        previous = current;
        current = next;
        direction = prevDirection;
        TurnBack();
        //MoveTick();
        //NextMove(type);
        TargetDirection();
        //Debug.Log(current.name + " collide " + other.gameObject.transform.parent.name);
    }

    public void Restart()
    {
        mapM = GameObject.Find("Map").GetComponent<MapManager>();
        gameM = GameObject.Find("Game Manager").GetComponent<GameManager>();
        pathf = GameObject.Find("Pathfinder").GetComponent<Pathfinding>();

        do
        {
            List<string> directions = new List<string>() { "U", "D", "L", "R" };
            direction = directions[Random.Range(0, directions.Count)];
            startDirection = direction;

            List<string> turns = new List<string>() { "Left", "Right" };
            prefTurn = turns[Random.Range(0, turns.Count)];
            startPrefTurn = prefTurn;

            start = mapM.enemyCurrent[Random.Range(0, mapM.enemyCurrent.Count)];

        } while (mapM.enemyStartHS.Contains(start.name + direction));

        current = start;

        transform.parent = current.transform;
        transform.localPosition = new Vector3(0, 4.2f, 0);
        transform.localScale = new Vector3(1, 1, 1);
        //gameObject.GetComponent<MeshCollider>().isTrigger = true;

        neighbours = GetNeighbour(current, direction);

        NextMove(type);
        TargetDirection();
    }

    public void Reposition()
    {
        direction = startDirection;
        current = start;
        prefTurn = startPrefTurn;
        moveCount = 0;

        transform.parent = current.transform;
        transform.localPosition = new Vector3(0, 4.2f, 0);
        transform.localScale = new Vector3(1, 1, 1);

        neighbours = GetNeighbour(current, direction);

        NextMove(type);
        TargetDirection();
    }
}
