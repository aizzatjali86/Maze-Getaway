using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCheck : MonoBehaviour
{

    private MapManager mapM;
    private GameManager gameM;
    private Pathfinding pathf;
    public int gameTimeBefore;
    public int t = 0;

    // Start is called before the first frame update
    void Start()
    {

        mapM = GameObject.Find("Map").GetComponent<MapManager>();
        gameM = GameObject.Find("Game Manager").GetComponent<GameManager>();

        transform.parent = mapM.path[t].transform;
        transform.localScale = new Vector3(1, 1, 1);

        gameTimeBefore = gameM.gameTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameM.gameTime > gameTimeBefore)
        {
            t++;
            if (t < mapM.path.Count)
            {
                transform.parent = mapM.path[t].transform;
            }
        }
        Vector3 _target = new Vector3(0, 1, 0);
        if (_target != transform.localPosition)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _target, .1f);
        }

        gameTimeBefore = gameM.gameTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("delete");
            Destroy(other.gameObject);
        }
    }
}
