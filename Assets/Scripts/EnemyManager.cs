using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private MapManager mapM;
    private GameManager gameM;
    private List<GameObject> others;
    public int gameTimeBefore;
    public bool overlap;

    // Start is called before the first frame update
    void Start()
    {
        mapM = GameObject.Find("Map").GetComponent<MapManager>();
        gameM = GameObject.Find("Game Manager").GetComponent<GameManager>();

        gameTimeBefore = gameM.gameTime;

    }

    public void OneStep(int move)
    {
        //others = mapM.enemies;
        mapM = GameObject.Find("Map").GetComponent<MapManager>();
        overlap = false;

        foreach (GameObject other in mapM.enemies)
        {
            foreach (GameObject other2 in mapM.enemies)
            {
                if (other != other2)
                {
                    if (other.GetComponent<EnemyChecker>().previous == other2.GetComponent<EnemyChecker>().current && other.GetComponent<EnemyChecker>().current == other2.GetComponent<EnemyChecker>().previous)
                    {
                        //Debug.Log("Collide");
                        //other.GetComponent<EnemyChecker>().Collide2();
                        //other2.GetComponent<EnemyChecker>().Collide2();
                    }
                    else if (other.GetComponent<EnemyChecker>().current == other2.GetComponent<EnemyChecker>().current)
                    {
                        //Debug.Log("Collide2");
                        //other.GetComponent<EnemyChecker>().Collide2();
                        //other2.GetComponent<EnemyChecker>().Collide2();
                    }
                }
            }
        }

        foreach (GameObject other in mapM.enemies)
        {
            if (other.GetComponent<EnemyChecker>().current == mapM.path[move])
            {
                //Debug.Log("Delete");
                overlap = true;
            }
            if (move > 0 )
            {
                if (other.GetComponent<EnemyChecker>().current == mapM.path[move - 1] && other.GetComponent<EnemyChecker>().previous == mapM.path[move])
                {
                    //Debug.Log("Delete");
                    overlap = true;
                }
            }
        }

        foreach (GameObject other in mapM.enemies)
        {
            other.GetComponent<EnemyChecker>().MoveTick();
            other.GetComponent<EnemyChecker>().NextMove(other.GetComponent<EnemyChecker>().type);
            other.GetComponent<EnemyChecker>().TargetDirection();
            //Debug.Log(other.GetComponent<EnemyChecker>().current.name);
        }
    }

    public void Reposition(GameObject enemy)
    {
        enemy.GetComponent<EnemyChecker>().Restart();
    }


}
