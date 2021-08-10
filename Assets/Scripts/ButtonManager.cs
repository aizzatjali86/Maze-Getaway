using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Button levelButton;

    // Start is called before the first frame update
    void Start()
    {
        levelButton = GetComponent<Button>();
        if (!PlayerPrefs.HasKey(gameObject.name))
        {
            PlayerPrefs.SetInt(gameObject.name, 0);
        }
        if (PlayerPrefs.GetInt(gameObject.name) == 1)
        {
            levelButton.interactable = true;
        }
        else
        {
            levelButton.interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
