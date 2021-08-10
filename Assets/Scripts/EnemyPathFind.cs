using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyPathFind : MonoBehaviour
{
    List<GameObject> open = new List<GameObject>();
    HashSet<GameObject> closed = new HashSet<GameObject>();

    public GameObject player;
    public bool playerInView;

    public List<string> neighboursD = new List<string>();
    public List<List<string>> bridges;
    public List<int> pBridges;
    public List<GameObject> path;
    public List<List<GameObject>> paths = new List<List<GameObject>>();

    public List<string> pathDirection;
    private List<GameObject> sectors = new List<GameObject>();

    public int rank = 1;
    int t;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    public List<GameObject> CreatePath(GameObject start, GameObject end)
    {
        bridges = GameObject.Find("Map").GetComponent<MapManager>().bridges;

        open.Add(start);

        GameObject current;
        GameObject before = null;

        do
        {
            if (open.Count == 1)
            {
                current = open[0];
            }
            else
            {
                current = open[0];
                for (int i = 1; i < open.Count; i++)
                {
                    if (current.GetComponent<SectorManager>().fCostEnemy < open[i].GetComponent<SectorManager>().fCostEnemy)// < for shortest route
                    {

                    }
                    else
                    {
                        current = open[i];
                    }
                }
                foreach (List<string> b in bridges)
                {
                    if ((b[1] == current.name && b[2] == before.name) || (b[2] == current.name && b[1] == before.name))
                    {
                        b[3] = (System.Convert.ToInt32(b[3]) + Mathf.RoundToInt(Random.Range(5, 10))).ToString();
                    }
                }
            }

            open.Remove(current);
            closed.Add(current);

            neighboursD.Clear();
            List<GameObject> neighbours = GetNeighbour(current);
            //Debug.Log("cur: " + current.name  + ", nei: " + neighbours.Count);

            for (int i = 0; i < neighbours.Count; i++)
            {

                if (!closed.Contains(neighbours[i]))
                {
                    SectorManager neighSec = neighbours[i].GetComponent<SectorManager>();
                    if (current.GetComponent<SectorManager>().fCostEnemy + 1 < neighSec.fCostEnemy || !open.Contains(neighbours[i]))// < for shortest route
                    {
                        neighSec.gCostEnemy = current.GetComponent<SectorManager>().gCostEnemy + 1;
                        neighSec.fCostEnemy = neighSec.gCostEnemy + neighSec.hCostEnemy;
                        neighSec.previous = current.name;
                        neighSec.previousD = neighboursD[i];
                        if (!open.Contains(neighbours[i]))
                        {
                            open.Add(neighbours[i]);
                        }
                    }
                }
            }
            before = current;
            pBridges.Clear();
            //Debug.Log(current.name);
        } while (current != end);

        CheckPrevious(end);

        path.Reverse();
        pathDirection.Reverse();

        open.Clear();
        closed.Clear();

        return path;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool CheckPlayerInView()
    {
        if (pathDirection.All(o => o == pathDirection[0]))
        {
            playerInView = true;
        }
        else
        {
            playerInView = false;
        }

        return playerInView;
    }


    void CheckPrevious(GameObject current)
    {
        path.Add(current);
        pathDirection.Add(current.GetComponent<SectorManager>().previousD);
        if (current.GetComponent<SectorManager>().previous != transform.parent.name)
        {
            CheckPrevious(GameObject.Find(current.GetComponent<SectorManager>().previous));
        }
        else
        {
            path.Add(transform.parent.gameObject);
            return;
        }
    }

    List<GameObject> GetNeighbour(GameObject current)
    {
        char[] separator = { '-' };
        string[] strList1 = current.name.Split(separator);
        List<GameObject> neighbours = new List<GameObject>();

        string neighbour1 = "Sector-" + strList1[1] + "-" + (System.Convert.ToInt32(strList1[2]) - 1).ToString();

        string neighbour2 = "Sector-" + strList1[1] + "-" + (System.Convert.ToInt32(strList1[2]) + 1).ToString();

        string neighbour3 = "Sector-" + (System.Convert.ToInt32(strList1[1]) - 1).ToString() + "-" + strList1[2];

        string neighbour4 = "Sector-" + (System.Convert.ToInt32(strList1[1]) + 1).ToString() + "-" + strList1[2];

        foreach (List<string> b in bridges)
        {
            if (b[1] == neighbour1 && b[2] == current.name)
            {
                neighbours.Add(GameObject.Find(neighbour1));
                pBridges.Add(System.Convert.ToInt32(b[3]));
                neighboursD.Add("D");
                //b[3] = (System.Convert.ToInt32(b[3]) + Mathf.RoundToInt(Random.Range(10, 10))).ToString();
                //Debug.Log("add V1");
            }
            if (b[1] == current.name && b[2] == neighbour2)
            {
                neighbours.Add(GameObject.Find(neighbour2));
                pBridges.Add(System.Convert.ToInt32(b[3]));
                neighboursD.Add("U");
                //b[3] = (System.Convert.ToInt32(b[3]) + Mathf.RoundToInt(Random.Range(10, 10))).ToString();
                //Debug.Log("add V2");
            }
            if (b[1] == neighbour3 && b[2] == current.name)
            {
                neighbours.Add(GameObject.Find(neighbour3));
                pBridges.Add(System.Convert.ToInt32(b[3]));
                neighboursD.Add("L");
                //b[3] = (System.Convert.ToInt32(b[3]) + Mathf.RoundToInt(Random.Range(10, 10))).ToString();
                //Debug.Log("add H1");
            }
            if (b[1] == current.name && b[2] == neighbour4)
            {
                neighbours.Add(GameObject.Find(neighbour4));
                pBridges.Add(System.Convert.ToInt32(b[3]));
                neighboursD.Add("R");
                //b[3] = (System.Convert.ToInt32(b[3]) + Mathf.RoundToInt(Random.Range(10, 10))).ToString();
                //Debug.Log("add H2");
            }
        }
        //neighbours.Shuffle();
        return neighbours;
    }
}
