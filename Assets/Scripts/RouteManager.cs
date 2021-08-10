using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteManager : MonoBehaviour
{
    public List<List<string>> bridges;
    public List<List<GameObject>> finalPathList = new List<List<GameObject>>();
    public List<List<GameObject>> pathList = new List<List<GameObject>>();
    public List<List<GameObject>> pathListToReplace = new List<List<GameObject>>();

    public List<GameObject> pathInit;
    public List<GameObject> longest;
    public List<GameObject> shortest;

    public GameObject start;
    public GameObject end;

    public int plCount;


    public List<List<GameObject>> CreateRoutes()
    {
        bridges = GameObject.Find("Map").GetComponent<MapManager>().bridges;

        pathInit.Add(start);
        pathList.Add(pathInit);
        //do
        for (int j = 0; j < 50; j++)
        {
            pathListToReplace.Clear();

            for (int p = 0; p < pathList.Count; p++)
            {
                GameObject current = pathList[p][pathList[p].Count - 1];

                if (current != end)
                {
                    List<GameObject> neighboursAll = GetNeighbour(current);
                    List<GameObject> neighbours = new List<GameObject>();
                    for (int i = 0; i < neighboursAll.Count; i++)
                    {
                        if (!pathList[p].Contains(neighboursAll[i]))
                        {
                            neighbours.Add(neighboursAll[i]);
                        }
                    }
                    //Debug.Log(current.name + ": " + neighbours.Count);
                    List<GameObject> newpath1 = pathList[p].GetRange(0, pathList[p].Count);
                    List<GameObject> newpath2 = pathList[p].GetRange(0, pathList[p].Count);
                    List<GameObject> newpath3 = pathList[p].GetRange(0, pathList[p].Count);
                    int randCount = 0;
                    if (neighbours.Count != 0 && j > 5)
                    {
                        randCount = Mathf.RoundToInt(Random.Range(1, neighbours.Count));
                    }
                    else
                    {
                        randCount = neighbours.Count;
                    }
                    for (int i = 0; i < randCount; i++)
                    {
                        if (!pathList[p].Contains(neighbours[i]))
                        {
                            if (i == 1)
                            {
                                newpath1.Add(neighbours[i]);
                                pathListToReplace.Add(newpath1);
                            }
                            else if (i == 2)
                            {
                                newpath2.Add(neighbours[i]);
                                pathListToReplace.Add(newpath2);
                            }
                            else if (i == 3)
                            {
                                newpath3.Add(neighbours[i]);
                                pathListToReplace.Add(newpath3);
                            }
                            else
                            {
                                pathList[p].Add(neighbours[i]);
                            }
                        }
                    }
                }
            }
            
            foreach(List<GameObject> pTR in pathListToReplace)
            {
                pathList.Add(pTR);
            }

            //Debug.Log(pathList.Count);
        } //while (plCount != pathList.Count);

        longest = pathList[0];
        shortest = pathList[0];
        for (int k = 0; k < pathList.Count; k++)
        {
            if (pathList[k][pathList[k].Count - 1] == end)
            {
                finalPathList.Add(pathList[k]);                
            }
        }
        plCount = finalPathList.Count;

        longest = finalPathList[0];
        shortest = finalPathList[finalPathList.Count/2 + 1];
        for (int k = 0; k < finalPathList.Count; k++)
        {
            if (k > 0 && longest.Count < finalPathList[k].Count)
            {
                longest = finalPathList[k];
            }
        }


            return finalPathList;
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
                //Debug.Log("add V1");
            }
            if (b[1] == current.name && b[2] == neighbour2)
            {
                neighbours.Add(GameObject.Find(neighbour2));
                //Debug.Log("add V2");
            }
            if (b[1] == neighbour3 && b[2] == current.name)
            {
                neighbours.Add(GameObject.Find(neighbour3));
                //Debug.Log("add H1");
            }
            if (b[1] == current.name && b[2] == neighbour4)
            {
                neighbours.Add(GameObject.Find(neighbour4));
                //Debug.Log("add H2");
            }
        }

        neighbours.Shuffle();

        return neighbours;
    }
}
