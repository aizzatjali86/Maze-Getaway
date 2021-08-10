using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FovControl : MonoBehaviour
{
    public Slider fovSlider;

    public int sliderValue;
    public int fov;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("FOV"))
        {
            PlayerPrefs.SetInt("FOV", 0);
        }
        sliderValue = PlayerPrefs.GetInt("FOV");
        fovSlider.value = sliderValue;
        switch (sliderValue)
        {
            case 0:
                fov = 3;
                break;
            case 1:
                fov = 5;
                break;
            case 2:
                fov = 7;
                break;
            default:
                fov = 3;
                break;
        }
    }

    public void FOVController()
    {
        sliderValue = (int) fovSlider.value;
        switch (sliderValue)
        {
            case 0:
                fov = 3;
                break;
            case 1:
                fov = 5;
                break;
            case 2:
                fov = 7;
                break;
            default:
                fov = 5;
                break;
        }
        PlayerPrefs.SetInt("FOV", sliderValue);
    }
}
