using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int currentX;
    public int currentY;
    public int type;
    public int fov;
    private MapManager mapM;
    private GameManager gameM;
    public int gameTimeBefore;
    public string direction;
    public List<GameObject> neighbours;
    public List<List<string>> bridges;
    public Button front, front2, back, back2, left, left2, right, right2;
    public GameObject frontSec, front2Sec, backSec, back2Sec, leftSec, left2Sec, rightSec, right2Sec;
    public Quaternion rotation;
    public bool pursued;

    bool oneD, oneU, oneL, oneR = false;
    public bool isMoving;
    public bool stop = false;
    public bool start = false;

    private void Awake()
    {
        GameObject playerModel = Instantiate(Resources.Load("Prefab/Car " + (type + 1).ToString(), typeof(GameObject))) as GameObject;
        playerModel.transform.parent = gameObject.transform;
        playerModel.transform.localScale = new Vector3(.16f, .16f, .16f);
        playerModel.transform.localPosition = new Vector3(0, -.98f, 0);

        try
        {
            LevelManager levelM = GameObject.Find("Level Select Manager").GetComponent<LevelManager>();
            type = levelM.car;
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
    }
    // Start is called before the first frame update
    void Start()
    {
        mapM = GameObject.Find("Map").GetComponent<MapManager>();
        gameM = GameObject.Find("Game Manager").GetComponent<GameManager>();

        transform.parent = GameObject.Find(mapM.startSector).transform;
        transform.localScale = new Vector3(1, 1, 1);

        currentX = transform.parent.gameObject.GetComponent<SectorManager>().sectorX;
        currentY = transform.parent.gameObject.GetComponent<SectorManager>().sectorY;

        ButtonFalse();

        switch (type)
        {
            case 0:
                front2.gameObject.SetActive(false);
                back2.gameObject.SetActive(false);
                left2.gameObject.SetActive(false);
                right2.gameObject.SetActive(false);
                break;
            case 1:
                back2.gameObject.SetActive(false);
                left2.gameObject.SetActive(false);
                right2.gameObject.SetActive(false);
                break;
            case 2:
                back2.gameObject.SetActive(false);             
                break;
            case 3:
                break;
        }

        //GameObject playerModel = Instantiate(Resources.Load("Prefab/Car " + (type +1).ToString(), typeof(GameObject))) as GameObject;
        //playerModel.transform.parent = gameObject.transform;
        //playerModel.transform.localScale = new Vector3(.16f, .16f, .16f);
        //playerModel.transform.localPosition = new Vector3(0, -.98f, 0);

        GetNeighbour(transform.parent.gameObject);
        TargetDirection();

        gameTimeBefore = gameM.gameTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameM.gameTime > gameTimeBefore)
        {
            if (!stop)
            {
                ButtonFalse();
                start = true;
                if (!mapM.enemyIsMoving)
                {
                    //GetNeighbour(transform.parent.gameObject);
                }
            }
            
            TargetDirection();

            currentX = transform.parent.gameObject.GetComponent<SectorManager>().sectorX;
            currentY = transform.parent.gameObject.GetComponent<SectorManager>().sectorY;

            FindObjectOfType<AudioManager>().PlaySound("Move");

        }

        if (!stop && start && !mapM.enemyIsMoving && !isMoving && gameTimeBefore == gameM.gameTime)
        {
            GetNeighbour(transform.parent.gameObject);
            start = false;
        }


        Vector3 _target = new Vector3(0, 1, 0);
        if (stop)
        {
            _target = transform.localPosition;
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

        Quaternion _targetR = rotation;
        if (_targetR != transform.rotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetR, .2f);
        }

        //Debug.Log(Vector3.Distance(transform.position, GameObject.Find(mapM.endSector).transform.position));
        if (fov == 3 || mapM.mapX <= 3)
        {
            if (!stop && Vector3.Distance(transform.position, GameObject.Find(mapM.endSector).transform.position) < 0.55f)
            {
                gameM.winLevel();
                //stop = true;
            }
        }
        else if(fov == 5 || mapM.mapX <= 5)
        {
            if (!stop && Vector3.Distance(transform.position, GameObject.Find(mapM.endSector).transform.position) < .4f)
            {
                gameM.winLevel();
                //stop = true;
            }
        }
        else if (fov == 7)
        {
            if (!stop && Vector3.Distance(transform.position, GameObject.Find(mapM.endSector).transform.position) < .32f)
            {
                gameM.winLevel();
                //stop = true;
            }
        }


        gameTimeBefore = gameM.gameTime;
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

    public void MoveFront(int moves)
    {
        switch (moves)
        {
            case 1:
                transform.parent = frontSec.transform;
                break;
            case 2:
                transform.parent = front2Sec.transform;
                break;
        }
        gameM.NextTick();
    }

    public void TurnLeft(int moves)
    {
        switch (moves)
        {
            case 1:
                transform.parent = leftSec.transform;
                break;
            case 2:
                transform.parent = left2Sec.transform;
                break;
        }
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
        gameM.NextTick();
    }

    public void TurnRight(int moves)
    {
        switch (moves)
        {
            case 1:
                transform.parent = rightSec.transform;
                break;
            case 2:
                transform.parent = right2Sec.transform;
                break;
        }
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
        gameM.NextTick();
    }

    public void TurnBack(int moves)
    {
        switch (moves)
        {
            case 1:
                //transform.parent = backSec.transform;
                break;
            case 2:
                transform.parent = backSec.transform;
                break;
        }
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
        gameM.NextTick();
    }

    public List<GameObject> GetNeighbour(GameObject current)
    {
        bridges = mapM.bridges;

        char[] separator = { '-' };
        string[] strList1 = current.name.Split(separator);
        List<GameObject> neighbours = new List<GameObject>();

        string neighbour1 = "Sector-" + strList1[1] + "-" + (System.Convert.ToInt32(strList1[2]) - 1).ToString();

        string neighbour2 = "Sector-" + strList1[1] + "-" + (System.Convert.ToInt32(strList1[2]) + 1).ToString();

        string neighbour3 = "Sector-" + (System.Convert.ToInt32(strList1[1]) - 1).ToString() + "-" + strList1[2];

        string neighbour4 = "Sector-" + (System.Convert.ToInt32(strList1[1]) + 1).ToString() + "-" + strList1[2];

        string neighbour5 = "Sector-" + strList1[1] + "-" + (System.Convert.ToInt32(strList1[2]) - 2).ToString();

        string neighbour6 = "Sector-" + strList1[1] + "-" + (System.Convert.ToInt32(strList1[2]) + 2).ToString();

        string neighbour7 = "Sector-" + (System.Convert.ToInt32(strList1[1]) - 2).ToString() + "-" + strList1[2];

        string neighbour8 = "Sector-" + (System.Convert.ToInt32(strList1[1]) + 2).ToString() + "-" + strList1[2];

        oneD = false;
        oneU = false;
        oneL = false;
        oneR = false;

        foreach (List<string> b in bridges)
        {
            //D
            if (b[1] == neighbour1 && b[2] == current.name)
            {
                SetButton(direction, "D", neighbour1);
                oneD = true;
            }
            //U
            if (b[1] == current.name && b[2] == neighbour2)
            {
                SetButton(direction, "U", neighbour2);
                oneU = true;
            }
            //L
            if (b[1] == neighbour3 && b[2] == current.name)
            {
                SetButton(direction, "L", neighbour3);
                oneL = true;
            }
            //R
            if (b[1] == current.name && b[2] == neighbour4)
            {
                SetButton(direction, "R", neighbour4);
                oneR = true;
            }
        }

        foreach (List<string> b in bridges)
        {
            if (b[1] == neighbour5 && b[2] == neighbour1 && oneD)
            {
                SetButton2(direction, "D", neighbour5);
            }
            if (b[1] == neighbour2 && b[2] == neighbour6 && oneU)
            {
                SetButton2(direction, "U", neighbour6);
            }
            if (b[1] == neighbour7 && b[2] == neighbour3 && oneL)
            {
                SetButton2(direction, "L", neighbour7);
            }
            if (b[1] == neighbour4 && b[2] == neighbour8 && oneR)
            {
                SetButton2(direction, "R", neighbour8);
            }
        }

        return neighbours;
    }

    public void SetButton(string playerDirection, string direction, string neighbour)
    {
        if (playerDirection == "U")
        {
            //front
            if (direction == "U")
            {
                front.interactable = true;
                frontSec = GameObject.Find(neighbour);
            }
            //back
            else if (direction == "D")
            {
                back.interactable = true;
                backSec = GameObject.Find(neighbour);
                back2.interactable = true;
                back2Sec = GameObject.Find(neighbour);
            }
            //left
            else if (direction == "L")
            {
                left.interactable = true;
                leftSec = GameObject.Find(neighbour);
            }
            //right
            else if (direction == "R")
            {
                right.interactable = true;
                rightSec = GameObject.Find(neighbour);
            }
        }
        else if (playerDirection == "D")
        {
            //front
            if (direction == "D")
            {
                front.interactable = true;
                frontSec = GameObject.Find(neighbour);
            }
            //back
            else if (direction == "U")
            {
                back.interactable = true;
                backSec = GameObject.Find(neighbour);
                back2.interactable = true;
                back2Sec = GameObject.Find(neighbour);
            }
            //left
            else if (direction == "R")
            {
                left.interactable = true;
                leftSec = GameObject.Find(neighbour);
            }
            //right
            else if (direction == "L")
            {
                right.interactable = true;
                rightSec = GameObject.Find(neighbour);
            }
        }
        else if (playerDirection == "L")
        {
            //front
            if (direction == "L")
            {
                front.interactable = true;
                frontSec = GameObject.Find(neighbour);
            }
            //back
            else if (direction == "R")
            {
                back.interactable = true;
                backSec = GameObject.Find(neighbour);
                back2.interactable = true;
                back2Sec = GameObject.Find(neighbour);
            }
            //left
            else if (direction == "D")
            {
                left.interactable = true;
                leftSec = GameObject.Find(neighbour);
            }
            //right
            else if (direction == "U")
            {
                right.interactable = true;
                rightSec = GameObject.Find(neighbour);
            }
        }
        else if (playerDirection == "R")
        {
            //front
            if (direction == "R")
            {
                front.interactable = true;
                frontSec = GameObject.Find(neighbour);
            }
            //back
            else if (direction == "L")
            {
                back.interactable = true;
                backSec = GameObject.Find(neighbour);
                back2.interactable = true;
                back2Sec = GameObject.Find(neighbour);
            }
            //left
            else if (direction == "U")
            {
                left.interactable = true;
                leftSec = GameObject.Find(neighbour);
            }
            //right
            else if (direction == "D")
            {
                right.interactable = true;
                rightSec = GameObject.Find(neighbour);
            }
        }
    }

    public void SetButton2(string playerDirection, string direction, string neighbour)
    {
        if (playerDirection == "U")
        {
            //front
            if (direction == "U")
            {
                front2.interactable = true;
                front2Sec = GameObject.Find(neighbour);
            }
            //back
            else if (direction == "D")
            {
                back2.interactable = true;
                back2Sec = GameObject.Find(neighbour);
            }
            //left
            else if (direction == "L")
            {
                left2.interactable = true;
                left2Sec = GameObject.Find(neighbour);
            }
            //right
            else if (direction == "R")
            {
                right2.interactable = true;
                right2Sec = GameObject.Find(neighbour);
            }
        }
        else if (playerDirection == "D")
        {
            //front
            if (direction == "D")
            {
                front2.interactable = true;
                front2Sec = GameObject.Find(neighbour);
            }
            //back
            else if (direction == "U")
            {
                back2.interactable = true;
                back2Sec = GameObject.Find(neighbour);
            }
            //left
            else if (direction == "R")
            {
                left2.interactable = true;
                left2Sec = GameObject.Find(neighbour);
            }
            //right
            else if (direction == "L")
            {
                right2.interactable = true;
                right2Sec = GameObject.Find(neighbour);
            }
        }
        else if (playerDirection == "L")
        {
            //front
            if (direction == "L")
            {
                front2.interactable = true;
                front2Sec = GameObject.Find(neighbour);
            }
            //back
            else if (direction == "R")
            {
                back2.interactable = true;
                back2Sec = GameObject.Find(neighbour);
            }
            //left
            else if (direction == "D")
            {
                left2.interactable = true;
                left2Sec = GameObject.Find(neighbour);
            }
            //right
            else if (direction == "U")
            {
                right2.interactable = true;
                right2Sec = GameObject.Find(neighbour);
            }
        }
        else if (playerDirection == "R")
        {
            //front
            if (direction == "R")
            {
                front2.interactable = true;
                front2Sec = GameObject.Find(neighbour);
            }
            //back
            else if (direction == "L")
            {
                back2.interactable = true;
                back2Sec = GameObject.Find(neighbour);
            }
            //left
            else if (direction == "U")
            {
                left2.interactable = true;
                left2Sec = GameObject.Find(neighbour);
            }
            //right
            else if (direction == "D")
            {
                right2.interactable = true;
                right2Sec = GameObject.Find(neighbour);
            }
        }
    }

    public void ButtonFalse()
    {
        front.interactable = false;
        back.interactable = false;
        left.interactable = false;
        right.interactable = false;
        front2.interactable = false;
        back2.interactable = false;
        left2.interactable = false;
        right2.interactable = false;
    }

    public void ButtonDisable()
    {
        front.enabled = false;
        back.enabled = false;
        left.enabled = false;
        right.enabled = false;
        front2.enabled = false;
        back2.enabled = false;
        left2.enabled = false;
        right2.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            gameM.GameOver();
            FindObjectOfType<AudioManager>().PlaySound("crash");
            //stop = true;
            other.GetComponent<EnemyController>().stop = true;
        }
        else if(other.tag == "Coin")
        {
            other.transform.parent.position = new Vector3(0, -10, 0);
            FindObjectOfType<AudioManager>().PlaySound("coin");
            // TODO: give points

            gameM.coinGet++;
        }
    }
}
