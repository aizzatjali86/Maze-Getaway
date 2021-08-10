using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorManager : MonoBehaviour
{
    public int sectorX;
    public int sectorY;
    public int mapX;
    public int mapY;
    public float midY;
    public string sectorType;
    public string nodeType = "E";
    public string specialType = "NA";
    public int gCost = 0;
    public int gCostEnemy;
    public int hCost = 0;
    public int hCostEnemy = 0;
    public int fCost = 0;
    public int fCostEnemy;
    public int penalty = 1;
    public string previous;
    public string previousD;

    private void Start()
    {
        GameObject Map = GameObject.Find("Map");
        MapManager map = Map.GetComponent<MapManager>();

        mapX = map.mapX;
        mapY = map.mapY;
        midY = map.midY;

        hCost = Mathf.Abs(sectorX - GameObject.Find("Map").GetComponent<MapManager>().endSectorNo) + Mathf.Abs(sectorY - (mapY - 1));

        //penalty = Mathf.RoundToInt(Random.Range(1, 20));

        //transform.localScale = new Vector3(1 / (.629f * mapX), 1 / (0.629f * mapX));

        //SpriteRenderer renderer = gameObject.AddComponent<SpriteRenderer>();
        //renderer.sprite = Resources.Load<Sprite>("Sprites/SectorBG");

        float coordX = (sectorX - Mathf.Round(mapX / 2)) * 3;
        float coordY = (sectorY - Mathf.Round(mapY / 2)) * 3 + midY;

        gameObject.transform.localPosition = new Vector3(coordX, 0, coordY);

        //sectorType = GetSectorType(sectorX, sectorY);

    }

    private void Update()
    {
        hCostEnemy = Mathf.Abs(sectorX - GameObject.Find("Player").GetComponent<PlayerController>().currentX) + Mathf.Abs(sectorY - GameObject.Find("Player").GetComponent<PlayerController>().currentY);
    }

}
