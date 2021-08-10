using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour
{
    private float x;
    private float sp;
    
    // Start is called before the first frame update
    void Start()
    {
        x = Random.Range(-2, 2);
        sp = Random.Range(.01f, .02f);
        if ((int)Random.Range(0,1) == 0)
        {
            sp *= (-1);
        }        
    }

    // Update is called once per frame
    void Update()
    {
        x += sp;
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
        if (x > 2 || x < -2)
        {
            sp *= -1;
        }
    }
}
