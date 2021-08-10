using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPosition : MonoBehaviour
{

    public List<GameObject> enemies;
    private GameManager gameM;
    private Pathfinding pathf;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Test()
    {
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        gameM = GameObject.Find("Game Manager").GetComponent<GameManager>();
        pathf = GameObject.Find("Pathfinder").GetComponent<Pathfinding>();
        enemies[0].GetComponent<EnemyController>().testTime = pathf.longest.Count;
        for (int i = 0; i < pathf.longest.Count; i++)
        {
            Debug.Log(enemies[0].GetComponent<EnemyController>().current.name);
            if (enemies[0].GetComponent<EnemyController>().current == pathf.longest[i])
            {
                Debug.Log("No!!");
            }
        }
    }

    
}
