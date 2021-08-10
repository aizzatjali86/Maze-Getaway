using UnityEngine.Audio;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sounds[] sounds;
    string level;

    [Range(0f, 1f)]
    public float masterVolume;

    [Range(0f, 1f)]
    public float soundVolume;

    private static GameObject instance;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = gameObject;
        else
            Destroy(gameObject);

        masterVolume = PlayerPrefs.GetFloat("MusicVolume");
        soundVolume = PlayerPrefs.GetFloat("SoundFXVolume");

        for (int i = 0; i < 15; i++)
        {
            Sounds s = sounds[i];
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = masterVolume;
            s.source.pitch = s.pitch;
        }

        for (int i = 15; i < 20; i++)
        {
            Sounds s = sounds[i];
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = soundVolume;
            s.source.pitch = s.pitch;
        }
    }

    public void VolumeChange()
    {
        for (int i = 0; i < 15; i++)
        {
            AudioSource s = gameObject.GetComponents<AudioSource>()[i];
            s.volume = masterVolume;
        }
    }

    public void VolumeChange2()
    {
        for (int i = 15; i < 20; i++)
        {
            AudioSource s = gameObject.GetComponents<AudioSource>()[i];
            s.volume = soundVolume;
        }
    }

    private void Start()
    {
        StartCoroutine(BGMusic());
    }

    void Update()
    {

    }

    public void PlaySound(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
    }

    private void CheckLevel()
    {
        // Create a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        string sceneName = currentScene.name;

        switch (sceneName)
        {
            case "NormalGameScene":
                int levelType = FindObjectOfType<LevelManager>().mapType;
                level = levelType.ToString();
                break;
            case "NormalLevelSelectScene":
                float posZ = GameObject.FindObjectOfType<Camera>().transform.position.z;
                if (-posZ < 27) level = "0";
                else if (-posZ < 35) level = "1";
                else if (-posZ < 58) level = "2";
                else if (-posZ < 85) level = "3";
                else level = "4";
                break;
            case "GreedLevelSelectScene":
                float gposZ = GameObject.FindObjectOfType<Camera>().transform.position.z;
                if (-gposZ < 26) level = "0";
                else if (-gposZ < 31) level = "1";
                else if (-gposZ < 39) level = "2";
                else if (-gposZ < 45) level = "3";
                else level = "4";
                break;
            default:
                level = "0";
                break;
        }
    }

    private IEnumerator BGMusic()
    {
        while (true)
        {
            CheckLevel();
            Sounds s1 = Array.Find(sounds, sound => sound.name == level + "-1");
            s1.source.Play();
            yield return new WaitWhile(() => s1.source.isPlaying == true);
            CheckLevel();
            Sounds s2 = Array.Find(sounds, sound => sound.name == level + "-1");
            s2.source.Play();
            yield return new WaitWhile(() => s2.source.isPlaying == true);
            CheckLevel();
            Sounds s3 = Array.Find(sounds, sound => sound.name == level + "-2");
            s3.source.Play();
            yield return new WaitWhile(() => s3.source.isPlaying == true);
            CheckLevel();
            Sounds s4 = Array.Find(sounds, sound => sound.name == level + "-3");
            s4.source.Play();
            yield return new WaitWhile(() => s4.source.isPlaying == true);
            CheckLevel();
            Sounds s5 = Array.Find(sounds, sound => sound.name == level + "-3");
            s5.source.Play();
            yield return new WaitWhile(() => s5.source.isPlaying == true);
        }
    }
}
