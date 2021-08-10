using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopperManager : MonoBehaviour
{
    public int startX;
    public int startY;
    public int currentX;
    public int currentY;
    public Quaternion rotation;
    public GameObject currentSector;
    public string currentLane;
    public string direction;
    public string currentMove;
    public int distance;
    public MapManager mapM;
    public GameManager gameM;
    public PlayerController playerC;
    public bool playerInView;
    public int gameTimeBefore;

    // Start is called before the first frame update
    void Start()
    {
        mapM = GameObject.Find("Map").GetComponent<MapManager>();
        gameM = GameObject.Find("Game Manager").GetComponent<GameManager>();
        playerC = GameObject.Find("Player").GetComponent<PlayerController>();

        playerInView = false;

        int XY = Random.Range(0, 2);
        int HL = Random.Range(0, 2);
        int CW = Random.Range(0, 2);
        if (CW == 0)
        {
            direction = "CW";
        }
        else
        {
            direction = "ACW";
        }
        if (XY == 0)
        {
            currentLane = "Y";
            if (HL == 0)
            {
                currentMove = "L";
                startX = distance;
                startY = Random.Range(distance + 1, mapM.mapY - distance - 2);
                if (direction == "CW")
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    rotation = Quaternion.Euler(0, 0, 0);
                }
            }
            else
            {
                currentMove = "H";
                startX = mapM.mapX - distance - 1;
                startY = Random.Range(distance + 1, mapM.mapY - distance - 2);
                if (direction == "CW")
                {
                    rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
            }
        }
        else
        {
            currentLane = "X";
            if (HL == 0)
            {
                currentMove = "L";
                startY = distance;
                startX = Random.Range(distance + 1, mapM.mapX - distance - 2);
                if (direction == "CW")
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else
                {
                    rotation = Quaternion.Euler(0, -90, 0);
                }
            }
            else
            {
                currentMove = "H";
                startY = mapM.mapY - distance - 1;
                startX = Random.Range(distance + 1, mapM.mapX - distance - 2);
                if (direction == "CW")
                {
                    rotation = Quaternion.Euler(0, -90, 0);
                }
                else
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
            }
        }

        currentX = startX;
        currentY = startY;

        gameTimeBefore = gameM.gameTime;

        currentSector = GameObject.Find("Sector-" + currentX + "-" + currentY);
        transform.parent = currentSector.transform;
        transform.localPosition = new Vector3(0, 3, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameM.gameTime > gameTimeBefore)
        {
            MoveNext();
        }
        gameTimeBefore = gameM.gameTime;

        Vector3 _target = new Vector3(0, 3, 0);
        Quaternion _targetR = rotation;

        if (_target != transform.localPosition)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _target, .1f);
        }
        if (_targetR != transform.rotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetR, .2f);
        }

        if (playerC.currentX >= currentX - 1 && playerC.currentX <= currentX + 1 && playerC.currentY >= currentY - 1 && playerC.currentY <= currentY + 1)
        {
            playerInView = true;
            playerC.pursued = true;
        }
        else
        {
            playerInView = false;
            playerC.pursued = false;
        }
    }

    public void MoveNext()
    {
        if (direction == "CW")
        {
            if (currentLane == "X")
            {
                if (currentMove == "L")
                {                    
                    currentX--;
                    if (currentX == distance)
                    {
                        //turn
                        rotation = Quaternion.Euler(0, 180, 0);
                        currentLane = "Y";
                    }
                }
                else
                {
                    currentX++;
                    if (currentX == mapM.mapX - distance - 1)
                    {
                        //turn
                        rotation = Quaternion.Euler(0, 0, 0);
                        currentLane = "Y";
                    }
                }
            }
            else
            {
                if (currentMove == "L")
                {
                    currentY++;
                    if (currentY == mapM.mapY - distance - 1)
                    {
                        //turn
                        rotation = Quaternion.Euler(0, -90, 0);
                        currentLane = "X";
                        currentMove = "H";
                    }
                }
                else
                {
                    currentY--;
                    if (currentY == distance)
                    {
                        //turn
                        rotation = Quaternion.Euler(0, 90, 0);
                        currentLane = "X";
                        currentMove = "L";
                    }
                }
            }
        }
        else
        {
            if (currentLane == "Y")
            {
                if (currentMove == "L")
                {
                    currentY--;                   
                    if (currentY == distance)
                    {
                        //turn
                        rotation = Quaternion.Euler(0, -90, 0);
                        currentLane = "X";
                    }
                }
                else
                {
                    currentY++;
                    if (currentY == mapM.mapY - distance - 1)
                    {
                        //turn
                        rotation = Quaternion.Euler(0, 90, 0);
                        currentLane = "X";
                    }
                }
            }
            else
            {
                if (currentMove == "L")
                {
                    currentX++;
                    if (currentX == mapM.mapX - distance - 1)
                    {
                        //turn
                        rotation = Quaternion.Euler(0, 180, 0);
                        currentLane = "Y";
                        currentMove = "H";
                    }
                }
                else
                {
                    currentX--;
                    if (currentX == distance)
                    {
                        //turn
                        rotation = Quaternion.Euler(0, 0, 0);
                        currentLane = "Y";
                        currentMove = "L";
                    }
                }
            }
        }
        currentSector = GameObject.Find("Sector-" + currentX + "-" + currentY);
        transform.parent = currentSector.transform;
    }

}
