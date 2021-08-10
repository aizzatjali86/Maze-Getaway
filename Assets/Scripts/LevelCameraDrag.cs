using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCameraDrag : MonoBehaviour
{
    public float panSpeed = 2;
    public bool greed;
    private Vector3 panOrigin;
    private Vector3 oldPos;
    private float zStart;

    private void Start()
    {
        if (greed)
        {
            if (!PlayerPrefs.HasKey("GreedCameraPos"))
            {
                PlayerPrefs.SetFloat("GreedCameraPos", transform.position.z);
            }
            zStart = PlayerPrefs.GetFloat("GreedCameraPos");
            transform.position = new Vector3(transform.position.x, transform.position.y, zStart);
        }
        else
        {
            if (!PlayerPrefs.HasKey("cameraPos"))
            {
                PlayerPrefs.SetFloat("cameraPos", transform.position.z);
            }
            zStart = PlayerPrefs.GetFloat("cameraPos");
            transform.position = new Vector3(transform.position.x, transform.position.y, zStart);
        }
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            oldPos = transform.position;
            panOrigin = Camera.main.ScreenToViewportPoint(Input.mousePosition);                    //Get the ScreenVector the mouse clicked
            //Debug.Log(panOrigin);
        }

        if (Input.GetMouseButton(0))
        {
            float posX = Camera.main.ScreenToViewportPoint(Input.mousePosition).x - panOrigin.x;
            float posZ = Camera.main.ScreenToViewportPoint(Input.mousePosition).y - panOrigin.y;
            Vector3 pos = new Vector3(0, 0, posZ*20);   //Get the difference between where the mouse clicked and where it moved

            if (greed)
            {
                if (transform.position.z >= -50 && posZ > 0 || transform.position.z <= -26 && posZ < 0)
                {
                    transform.position = oldPos + -pos * panSpeed;                                         //Move the position of the camera to simulate a drag, speed * 10 for screen to worldspace conversion
                                                                                                           //Debug.Log(pos);
                }
            }
            else
            {
                if (transform.position.z >= -117 && posZ > 0 || transform.position.z <= -26 && posZ < 0)
                {
                    transform.position = oldPos + -pos * panSpeed;                                         //Move the position of the camera to simulate a drag, speed * 10 for screen to worldspace conversion
                                                                                                           //Debug.Log(pos);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (greed)
            {
                PlayerPrefs.SetFloat("GreedCameraPos", transform.position.z);
            }
            else
            {
                PlayerPrefs.SetFloat("cameraPos", transform.position.z);
            }
        }

    }
}
