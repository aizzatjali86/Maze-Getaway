using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraDrag : MonoBehaviour
{
    public float panSpeed = 4;
    private Vector3 panOrigin;
    private Vector3 oldPos;
    private Vector3 _target;
    private bool bDragging;

    public GameObject player;
    private MapManager mapM;

    private void Start()
    {
        player = GameObject.Find("Player");
        mapM = GameObject.Find("Map").GetComponent<MapManager>();
        _target = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z - 5);
    }

    void Update()
    {
        if (player.GetComponent<PlayerController>().isMoving)
        {
            _target = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z - 5);
        }

        if (Input.GetMouseButtonDown(0))
        {
            bDragging = true;
            oldPos = transform.position;
            panOrigin = Camera.main.ScreenToViewportPoint(Input.mousePosition);                    //Get the ScreenVector the mouse clicked
            //Debug.Log(panOrigin);
        }

        if (Input.GetMouseButton(0))
        {
            float posX = Camera.main.ScreenToViewportPoint(Input.mousePosition).x - panOrigin.x;
            float posZ = Camera.main.ScreenToViewportPoint(Input.mousePosition).y- panOrigin.y;
            Vector3 pos = new Vector3(posX*2, 0, posZ*4);   //Get the difference between where the mouse clicked and where it moved

            if ((transform.position.z >= -20 && posZ > 0 || transform.position.z <= 6 && posZ < 0) && (transform.position.x >= -10 && posX > 0 || transform.position.x <= 10 && posX < 0))
            {
                _target = oldPos + -pos * panSpeed;                                         //Move the position of the camera to simulate a drag, speed * 10 for screen to worldspace conversion
                                                                                                       //Debug.Log(pos);
            }

        }

        //if (Input.GetMouseButtonUp(0))
        //{
        //    bDragging = false;
        //    transform.position = oldPos;
        //}

        if (_target != transform.position)
        {
            transform.position = Vector3.Lerp(transform.localPosition, _target, .2f);
        }

    }

    void OnGUI()
    {
        if (Event.current.isMouse && Event.current.button == 0 && Event.current.clickCount > 1)
        {
            _target = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z - 5);
        }
    }
}
