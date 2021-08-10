using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapMenuManager : MonoBehaviour
{
    public Button left;
    public Text leftText;
    public Button right;
    public Text rightText;
    public Text midText;
    public GameObject cameraMain;
    public GameObject shopPanel;
    public GameObject optionsPanel;

    private Vector3 cameraPos;
    
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        cameraMain.transform.position = new Vector3(0, cameraMain.transform.position.y, cameraMain.transform.position.z);
        shopPanel.transform.position = new Vector3(36, shopPanel.transform.position.y, cameraMain.transform.position.z -8.5f + 25.7f);
        optionsPanel.transform.position = new Vector3(-36, optionsPanel.transform.position.y, cameraMain.transform.position.z - 8.5f + 25.7f);

        Advertisements.Instance.ShowBanner(BannerPosition.BOTTOM);
    }

    // Update is called once per frame
    void Update()
    {
        shopPanel.transform.position = new Vector3(36, shopPanel.transform.position.y, cameraMain.transform.position.z - 8.5f + 25.7f);
        optionsPanel.transform.position = new Vector3(-36, optionsPanel.transform.position.y, cameraMain.transform.position.z - 8.5f + 25.7f);
        if (cameraMain.transform.position.x < cameraPos.x)
        {
            cameraMain.transform.position = new Vector3(cameraMain.transform.position.x + 3f, cameraMain.transform.position.y, cameraMain.transform.position.z);
        }
        else if (cameraMain.transform.position.x > cameraPos.x)
        {
            cameraMain.transform.position = new Vector3(cameraMain.transform.position.x - 3f, cameraMain.transform.position.y, cameraMain.transform.position.z);
        }

        if (cameraMain.transform.position.x == 36)
        {
            right.interactable = false;
            left.interactable = true;
            rightText.text = "";
            leftText.text = "MAP";
            midText.text = "SHOP";
        }
        else if(cameraMain.transform.position.x == 0)
        {
            right.interactable = true;
            left.interactable = true;
            rightText.text = "SHOP";
            leftText.text = "OPTIONS";
            midText.text = "MAP";
        }
        else if (cameraMain.transform.position.x <= -36)
        {
            right.interactable = true;
            left.interactable = false;
            rightText.text = "MAP";
            leftText.text = "";
            midText.text = "OPTIONS";
        }
        else
        {
            right.interactable = false;
            left.interactable = false;
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadSceneAsync("StartScene");

            // OR Application.Quit();
        }
    }

    public void ToRight()
    {
        cameraPos = new Vector3(cameraMain.transform.position.x + 36, cameraMain.transform.position.y, cameraMain.transform.position.z);
    }

    public void ToLeft()
    {
        cameraPos = new Vector3(cameraMain.transform.position.x - 36, cameraMain.transform.position.y, cameraMain.transform.position.z);
    }
}
