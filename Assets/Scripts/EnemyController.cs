using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
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
    public int moveCount = 0;
    public List<GameObject> neighbours;
    public List<string> neighboursD;
    public AnimationCurve MoveCurve;
    public bool isMoving;
    public bool finishedMoving;
    public GameObject player;
    public Vector3 _target;

    public List<List<string>> bridges;
    public int gameTimeBefore;
    public int testTime;
    public int t = 0;

    private GameObject Right1;
    private GameObject Right2;
    private GameObject Right3;
    private GameObject Left1;
    private GameObject Left2;
    private GameObject Left3;

    private GameObject SirenLeft;
    private GameObject SirenRight;

    public bool stop;

    // Start is called before the first frame update
    void Awake()
    {

        mapM = GameObject.Find("Map").GetComponent<MapManager>();
        gameM = GameObject.Find("Game Manager").GetComponent<GameManager>();
        pathf = GameObject.Find("Pathfinder").GetComponent<Pathfinding>();
        player = GameObject.Find("Player");

        try
        {
            Right1 = gameObject.transform.GetChild(0).transform.Find("Right1").gameObject;
            Left1 = gameObject.transform.GetChild(0).transform.Find("Left1").gameObject;
            Right2 = gameObject.transform.GetChild(0).transform.Find("Right2").gameObject;
            Left2 = gameObject.transform.GetChild(0).transform.Find("Left2").gameObject;
            Right3 = gameObject.transform.GetChild(0).transform.Find("Right3").gameObject;
            Left3 = gameObject.transform.GetChild(0).transform.Find("Left3").gameObject;
        }
        catch
        {

        }

        try
        {
            SirenLeft = gameObject.transform.GetChild(0).transform.Find("SirenLeft").gameObject;
            SirenRight = gameObject.transform.GetChild(0).transform.Find("SirenRight").gameObject;
        }
        catch
        {

        }

        SetTurnSignal();

        moveCount = 0;
        gameTimeBefore = gameM.gameTime;

        _target = new Vector3(0, 4.2f, 0);
    }

    private void Start()
    {
        gameObject.GetComponent<EnemyPathFind>().CreatePath(transform.parent.gameObject, player.transform.parent.gameObject);
        gameObject.GetComponent<EnemyPathFind>().CheckPlayerInView();

        SetTurnSignal();
        _target = new Vector3(0, 4.2f, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameM.gameTime > gameTimeBefore)
        {
            MoveTick();

            if (specialType)
            {
                gameObject.GetComponent<EnemyPathFind>().path.Clear();
                gameObject.GetComponent<EnemyPathFind>().pathDirection.Clear();
                gameObject.GetComponent<EnemyPathFind>().CreatePath(transform.parent.gameObject, GameObject.Find("Player").transform.parent.gameObject);

                if (gameObject.GetComponent<EnemyPathFind>().CheckPlayerInView() || player.GetComponent<PlayerController>().pursued)
                {
                    pursuitPath = gameObject.GetComponent<EnemyPathFind>().path.GetRange(0, gameObject.GetComponent<EnemyPathFind>().path.Count);
                    pursuitPathDirection = gameObject.GetComponent<EnemyPathFind>().pathDirection.GetRange(0, gameObject.GetComponent<EnemyPathFind>().pathDirection.Count);
                    inPursuit = true;
                    t = 0;
                }
            }
            

            NextMove(type);
            TargetDirection();
            finishedMoving = false;

            SetTurnSignal();

            if (specialType)
            {
                StartCoroutine(SetSiren());
            }
            _target = new Vector3(0, 3.7f, 0);
            for (int i = 9; i < transform.parent.childCount; i++)
            {
                if (transform.parent.GetChild(i).GetComponent<EnemyController>() != null)
                {
                    _target.y = _target.y + .5f;
                }
            }
        }

        Quaternion _targetR = rotation;

        if (stop)
        {
            _target = transform.localPosition;
            _targetR = transform.rotation;
        }

        if (_target != transform.localPosition)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _target, .2f);
            isMoving = true;
            mapM.enemyIsMoving = true;
        }
        else
        {
            isMoving = false;
            mapM.enemyIsMoving = false;
        }
        if (Vector3.Distance(_target, transform.localPosition) < .2f)
        {
            isMoving = false;
            mapM.enemyIsMoving = false;
        }
        
        if (_targetR != transform.rotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetR, .2f);
        }

        gameTimeBefore = gameM.gameTime;

        if (!isMoving && !finishedMoving)
        {

        }

    }

    void NextMove(int type)
    {
        if (specialType && inPursuit)
        {
            prevDirection = direction;
            next = pursuitPath[t + 1];
            direction = pursuitPathDirection[t];
            t++;
            if (next == pursuitPath[pursuitPath.Count - 1])
            {
                inPursuit = false;
                t = 0;
            }
        }
        else
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
        
    }

    void StraightMove()
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

    void TurnMove()
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

    void MoveTick()
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

    void TurnRight()
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

    void TurnLeft()
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

    void TurnBack()
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

    void TargetDirection()
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

    //private void OnTriggerEnter(Collider other)
    //{
    //    //Debug.Log("collide");
    //    if (other.tag == "Enemy")
    //    {
    //        inPursuit = false;
    //        next = previous;
    //        previous = current;
    //        current = next;
    //        direction = prevDirection;
    //        TurnBack();
    //        MoveTick();
    //        gameObject.GetComponent<EnemyPathFind>().path.Clear();
    //        gameObject.GetComponent<EnemyPathFind>().pathDirection.Clear();
    //        gameObject.GetComponent<EnemyPathFind>().CreatePath(transform.parent.gameObject, GameObject.Find("Player").transform.parent.gameObject);

    //        if (gameObject.GetComponent<EnemyPathFind>().CheckPlayerInView())
    //        {
    //            pursuitPath = gameObject.GetComponent<EnemyPathFind>().path.GetRange(0, gameObject.GetComponent<EnemyPathFind>().path.Count);
    //            pursuitPathDirection = gameObject.GetComponent<EnemyPathFind>().pathDirection.GetRange(0, gameObject.GetComponent<EnemyPathFind>().pathDirection.Count);
    //            inPursuit = true;
    //            t = 0;
    //        }

    //        NextMove(type);
    //        TargetDirection();
    //        //Debug.Log(current.name + " collide " + other.gameObject.transform.parent.name);
    //    }
    //}

    public void Restart()
    {
        direction = startDirection;

        prefTurn = startPrefTurn;

        current = start;

        SetTurnSignal();

        transform.parent = current.transform;
        transform.localPosition = new Vector3(0, 4.2f, 0);
        transform.localScale = new Vector3(1, 1, 1);
        //gameObject.GetComponent<MeshCollider>().isTrigger = true;

        neighbours = GetNeighbour(current, direction);

        NextMove(type);
        TargetDirection();
    }

    private void SetTurnSignal()
    {
        try
        {
            Right1.GetComponent<MeshRenderer>().material.color = Color.black;
            Left1.GetComponent<MeshRenderer>().material.color = Color.black;
            Right2.GetComponent<MeshRenderer>().material.color = Color.black;
            Left2.GetComponent<MeshRenderer>().material.color = Color.black;
            Right3.GetComponent<MeshRenderer>().material.color = Color.black;
            Left3.GetComponent<MeshRenderer>().material.color = Color.black;
        }
        catch
        {

        }

        if (prefTurn == "Left")
        {
            Left1.GetComponent<MeshRenderer>().material.color = Color.yellow;
        }
        else
        {
            Right1.GetComponent<MeshRenderer>().material.color = Color.yellow;
        }

        if (moveCount >= 1)
        {
            if (prefTurn == "Left")
            {
                Left2.GetComponent<MeshRenderer>().material.color = Color.yellow;
            }
            else
            {
                Right2.GetComponent<MeshRenderer>().material.color = Color.yellow;
            }
        }

        if (moveCount == 2)
        {
            if (prefTurn == "Left")
            {
                Left3.GetComponent<MeshRenderer>().material.color = Color.yellow;
            }
            else
            {
                Right3.GetComponent<MeshRenderer>().material.color = Color.yellow;
            }
        }
    }

    public IEnumerator SetSiren()
    {
        while (inPursuit || gameObject.GetComponent<EnemyPathFind>().CheckPlayerInView())
        {
            SirenLeft.GetComponent<MeshRenderer>().material.color = Color.red;
            SirenRight.GetComponent<MeshRenderer>().material.color = Color.blue;
            yield return new WaitForSeconds(0.5f);
            SirenLeft.GetComponent<MeshRenderer>().material.color = Color.clear;
            SirenRight.GetComponent<MeshRenderer>().material.color = Color.clear;
            yield return new WaitForSeconds(0.5f);
        }
    }

}
