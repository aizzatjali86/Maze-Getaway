using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NodeManager : MonoBehaviour
{
    public int nodeX;
    public int nodeY;
    public string sectorType;
    public string nodeType = "E";
    public bool isBridge = true;
    public int levelType;
    
    // Start is called before the first frame update
    void Start()
    {

        transform.localScale = new Vector3(1,1,1);

        //SpriteRenderer renderer = gameObject.AddComponent<SpriteRenderer>();
        //renderer.sprite = Resources.Load<Sprite>("Sprites/NodeBG");

        float coordX = (nodeX - 1) * 1;
        float coordY = (nodeY - 1) * 1;

        gameObject.transform.localPosition = new Vector3(coordX, 0, coordY);

        GetNodeSprite(nodeType);

        if ((sectorType == "L" && nodeX == 0 || sectorType == "R" && nodeX == 2 || sectorType == "U" && nodeY == 2 || sectorType == "D" && nodeY == 0 ||
            sectorType == "UL" && nodeX == 0 || sectorType == "UR" && nodeX == 2 || sectorType == "DL" && nodeX == 0 || sectorType == "DR" && nodeX == 2 ||
            sectorType == "UL" && nodeY == 2 || sectorType == "UR" && nodeY == 2 || sectorType == "DL" && nodeY == 0 || sectorType == "DR" && nodeY == 0) && nodeType == "E")
        {
            SetNodeObject();
        }

        if(sectorType == "L" && nodeX == 2 && nodeY == 0 || sectorType == "M" && nodeX == 2 && nodeY == 0 || sectorType == "UL" && nodeX == 2 && nodeY == 0 || sectorType == "U" && nodeX == 2 && nodeY == 0)
        {
            SetNodeMainObject();
        }

        if (!isBridge)
        {
            SetNodeBridgeObject(nodeX, nodeY);
        }

        if ((sectorType == "U" || sectorType == "UL" || sectorType == "UR") && nodeY == 2 && nodeType != "E")
        {
            GameObject tunnel = Instantiate(Resources.Load("Prefab/Tunnel", typeof(GameObject))) as GameObject;
            tunnel.transform.parent = gameObject.transform;
            tunnel.transform.localPosition = new Vector3(0, 0, 0);
            tunnel.transform.localScale = new Vector3(0.205f, 0.205f, 0.205f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetNodeSprite(string nodeType)
    {
        GameObject board = null;
        if (nodeType == "E")
        {
            if (levelType == 0)
            {
                board = Instantiate(Resources.Load("Sprites/Environment/road/env2", typeof(GameObject))) as GameObject;
            }
            else
            {
                board = Instantiate(Resources.Load("Sprites/Environment/road/env", typeof(GameObject))) as GameObject;
            }
        }
        else if(nodeType == "U" || nodeType == "D")
        {
            if (levelType == 1)
            {
                board = Instantiate(Resources.Load("Sprites/Environment/road/road2", typeof(GameObject))) as GameObject;
            }
            else if (levelType == 2)
            {
                board = Instantiate(Resources.Load("Sprites/Environment/road/road4", typeof(GameObject))) as GameObject;
            }
            else if (levelType == 3)
            {
                board = Instantiate(Resources.Load("Sprites/Environment/road/road3", typeof(GameObject))) as GameObject;
            }
            else if (levelType == 4)
            {
                board = Instantiate(Resources.Load("Sprites/Environment/road/road1", typeof(GameObject))) as GameObject;
            }
            else
            {
                board = Instantiate(Resources.Load("Sprites/Environment/road/road5", typeof(GameObject))) as GameObject;
            }
        }
        else if (nodeType == "L" || nodeType == "R")
        {
            if (levelType == 1)
            {
                board = Instantiate(Resources.Load("Sprites/Environment/road/road2", typeof(GameObject))) as GameObject;
            }
            else if (levelType == 2)
            {
                board = Instantiate(Resources.Load("Sprites/Environment/road/road4", typeof(GameObject))) as GameObject;
            }
            else if (levelType == 3)
            {
                board = Instantiate(Resources.Load("Sprites/Environment/road/road3", typeof(GameObject))) as GameObject;
            }
            else if (levelType == 4)
            {
                board = Instantiate(Resources.Load("Sprites/Environment/road/road1", typeof(GameObject))) as GameObject;
            }
            else
            {
                board = Instantiate(Resources.Load("Sprites/Environment/road/road5", typeof(GameObject))) as GameObject;
            }
            board.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            if (levelType == 1)
            {
                board = Instantiate(Resources.Load("Sprites/Environment/road/roadCenter2", typeof(GameObject))) as GameObject;
            }
            else if (levelType == 2)
            {
                board = Instantiate(Resources.Load("Sprites/Environment/road/roadCenter4", typeof(GameObject))) as GameObject;
            }
            else if (levelType == 3)
            {
                board = Instantiate(Resources.Load("Sprites/Environment/road/roadCenter3", typeof(GameObject))) as GameObject;
            }
            else if (levelType == 4)
            {
                board = Instantiate(Resources.Load("Sprites/Environment/road/roadCenter", typeof(GameObject))) as GameObject;
            }
            else
            {
                board = Instantiate(Resources.Load("Sprites/Environment/road/roadCenter5", typeof(GameObject))) as GameObject;
            }
        }
        board.transform.parent = gameObject.transform;
        board.transform.localPosition = new Vector3(0, 0, 0);
        board.transform.localScale = new Vector3(0.205f, 0.205f, 0.205f);
    }

    void SetNodeObject()
    {
        string objPath = null;
        switch (levelType)
        {
            case 1:
                objPath = ((int)Random.Range(3, 4)).ToString();
                break;
            case 2:
                objPath = ((int)Random.Range(2, 4)).ToString();
                break;
            case 3:
                objPath = ((int)Random.Range(1, 4)).ToString();
                break;
            case 4:
                objPath = ((int)Random.Range(0, 4)).ToString();
                break;
            default:
                objPath = ((int)Random.Range(20, 20)).ToString();
                break;
        }

        GameObject objectModel = null;
        objectModel = Instantiate(Resources.Load("Prefab/Tree/" + objPath, typeof(GameObject))) as GameObject;
        objectModel.transform.parent = gameObject.transform;
        objectModel.transform.localPosition = new Vector3(0, 0, 0);
        objectModel.transform.localScale = new Vector3(0.205f, 0.205f, 0.205f);
        objectModel.transform.rotation = Quaternion.Euler(0, Random.Range(0,180), 0);
    }

    void SetNodeMainObject()
    {
        string objPath = null;
        switch (levelType)
        {
            case 1:
                objPath = "Prefab/Village/" + ((int)Random.Range(0, 6)).ToString();
                break;
            case 2:
                objPath = "Prefab/Suburb/" + ((int)Random.Range(0, 6)).ToString();
                break;
            case 3:
                objPath = "Prefab/Town/" + ((int)Random.Range(0, 8)).ToString();
                break;
            case 4:
                objPath = "Prefab/City/" + ((int)Random.Range(0, 10)).ToString();
                break;
            default:
                objPath = "Prefab/School/" + ((int)Random.Range(0, 3)).ToString();
                break;
        }

        GameObject objectModel = null;
        objectModel = Instantiate(Resources.Load(objPath, typeof(GameObject))) as GameObject;
        objectModel.transform.parent = gameObject.transform;
        objectModel.transform.rotation = Quaternion.Euler(0, Random.Range(0, 4) * 90, 0);
        objectModel.transform.localPosition = new Vector3(0.5f, 0, -0.5f);
        objectModel.transform.localScale = new Vector3(1, 1, 1);
    }

    void SetNodeBridgeObject(int nodeX, int nodeY)
    {
        string objPath = null;
        switch (levelType)
        {
            case 1:
                objPath = "Prefab/Village/" + ((int)Random.Range(20, 22)).ToString();
                break;
            case 2:
                objPath = "Prefab/Suburb/" + ((int)Random.Range(20, 22)).ToString();
                break;
            case 3:
                objPath = "Prefab/Town/" + ((int)Random.Range(20, 23)).ToString();
                break;
            case 4:
                objPath = "Prefab/City/" + ((int)Random.Range(20, 24)).ToString();
                break;
            default:
                objPath = "Prefab/School/" + ((int)Random.Range(20, 20)).ToString();
                break;
        }

        GameObject objectModel = null;
        objectModel = Instantiate(Resources.Load(objPath, typeof(GameObject))) as GameObject;
        objectModel.transform.parent = gameObject.transform;
        if (nodeX == 1)
        {
            if (nodeY == 0)
            {
                objectModel.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                objectModel.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else if(nodeX == 0)
        {
            objectModel.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            objectModel.transform.rotation = Quaternion.Euler(0, 270, 0);
        }
        objectModel.transform.localPosition = new Vector3(0, 0, 0);
        objectModel.transform.localScale = new Vector3(1, 1, 1);
    }
}
