using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    List<GameObject> open = new List<GameObject>();
    HashSet<GameObject> closed = new HashSet<GameObject>();

    public List<List<string>> bridges;
    public List<int> pBridges;
    public List<GameObject> path;
    public List<List<GameObject>> paths = new List<List<GameObject>>();

    public List<GameObject> longest;
    private List<GameObject> sectors = new List<GameObject>();

    public int rank = 1;
    int t;

    public GameObject start;
    public GameObject end;


    private void Start()
    {
        try
        {
            LevelManager levelM = GameObject.Find("Level Select Manager").GetComponent<LevelManager>();
            rank = levelM.pathRank;
        }
        catch
        {

        }
    }

    public List<GameObject> CreatePath()
    {
        start = GameObject.Find(GameObject.Find("Map").GetComponent<MapManager>().startSector);
        end = GameObject.Find(GameObject.Find("Map").GetComponent<MapManager>().endSector);
        bridges = GameObject.Find("Map").GetComponent<MapManager>().bridges;

        open.Add(start);

        SectorManager startSector = start.GetComponent<SectorManager>();
        SectorManager endSector = end.GetComponent<SectorManager>();

        GameObject current = null;
        GameObject before = null;

        do
        {
            if (t%3 < rank)
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
                        if (current.GetComponent<SectorManager>().fCost > open[i].GetComponent<SectorManager>().fCost)// < for shortest route
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

                List<GameObject> neighbours = GetNeighbour(current);
                //Debug.Log("cur: " + current.name  + ", nei: " + neighbours.Count);

                for (int i = 0; i < neighbours.Count; i++)
                {

                    if (!closed.Contains(neighbours[i]))
                    {
                        SectorManager neighSec = neighbours[i].GetComponent<SectorManager>();
                        if (current.GetComponent<SectorManager>().fCost + pBridges[i] < neighSec.fCost || !open.Contains(neighbours[i]))// < for shortest route
                        {
                            neighSec.gCost = current.GetComponent<SectorManager>().gCost + pBridges[i];
                            neighSec.fCost = neighSec.gCost + neighSec.hCost;
                            neighSec.previous = current.name;
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
            }
            else
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
                        if (current.GetComponent<SectorManager>().fCost < open[i].GetComponent<SectorManager>().fCost)// < for shortest route
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

                List<GameObject> neighbours = GetNeighbour(current);
                //Debug.Log("cur: " + current.name  + ", nei: " + neighbours.Count);

                for (int i = 0; i < neighbours.Count; i++)
                {

                    if (!closed.Contains(neighbours[i]))
                    {
                        SectorManager neighSec = neighbours[i].GetComponent<SectorManager>();
                        if (current.GetComponent<SectorManager>().fCost + pBridges[i] < neighSec.fCost || !open.Contains(neighbours[i]))// < for shortest route
                        {
                            neighSec.gCost = current.GetComponent<SectorManager>().gCost + pBridges[i];
                            neighSec.fCost = neighSec.gCost + neighSec.hCost;
                            neighSec.previous = current.name;
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
            }
            t++;
        } while (current != end);

        CheckPrevious(end);

        path.Reverse();

        open.Clear();
        closed.Clear();

        return path;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<GameObject> CreateLongestPath()
    {
        sectors = GameObject.Find("Map").GetComponent<MapManager>().sectors;

        for (int i = 0; i < 30; i++)
        {
            //path.Clear();

            foreach (GameObject sec in sectors)
            {
                sec.GetComponent<SectorManager>().gCost = 0;
                sec.GetComponent<SectorManager>().fCost = 0;
            }

            path = CreatePath();
            paths.Add(path.GetRange(0, path.Count));
            path.Clear();
        }

        //Debug.Log(paths.Count);

        longest = paths[0];
        for (int k = 0; k < paths.Count; k++)
        {
            if (k > 0 && longest.Count < paths[k].Count)
            {
                longest = paths[k];
                //Debug.Log(longest.Count);
            }
        }

        return longest;
    }

    void CheckPrevious(GameObject current)
    {
        path.Add(current);
        if (current.GetComponent<SectorManager>().previous != start.name)
        {
            CheckPrevious(GameObject.Find(current.GetComponent<SectorManager>().previous));
        }
        else
        {
            path.Add(start);
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
                //b[3] = (System.Convert.ToInt32(b[3]) + Mathf.RoundToInt(Random.Range(10, 10))).ToString();
                //Debug.Log("add V1");
            }
            if (b[1] == current.name && b[2] == neighbour2)
            {
                neighbours.Add(GameObject.Find(neighbour2));
                pBridges.Add(System.Convert.ToInt32(b[3]));
                //b[3] = (System.Convert.ToInt32(b[3]) + Mathf.RoundToInt(Random.Range(10, 10))).ToString();
                //Debug.Log("add V2");
            }
            if (b[1] == neighbour3 && b[2] == current.name)
            {
                neighbours.Add(GameObject.Find(neighbour3));
                pBridges.Add(System.Convert.ToInt32(b[3]));
                //b[3] = (System.Convert.ToInt32(b[3]) + Mathf.RoundToInt(Random.Range(10, 10))).ToString();
                //Debug.Log("add H1");
            }
            if (b[1] == current.name && b[2] == neighbour4)
            {
                neighbours.Add(GameObject.Find(neighbour4));
                pBridges.Add(System.Convert.ToInt32(b[3]));
                //b[3] = (System.Convert.ToInt32(b[3]) + Mathf.RoundToInt(Random.Range(10, 10))).ToString();
                //Debug.Log("add H2");
            }
        }
        neighbours.Shuffle();
        return neighbours;
    }
}

/*
OPEN //the set of nodes to be evaluated
CLOSED //the set of nodes already evaluated
add the start node to OPEN


loop
        current = node in OPEN with the lowest f_cost
        remove current from OPEN
        add current to CLOSED
 
        if current is the target node //path has been found
                return
 
        foreach neighbour of the current node
                if neighbour is not traversable or neighbour is in CLOSED
                        skip to the next neighbour
 
                if new path to neighbour is shorter OR neighbour is not in OPEN
                        set f_cost of neighbour
                        set parent of neighbour to current
                        if neighbour is not in OPEN
                                add neighbour to OPEN
                                */
