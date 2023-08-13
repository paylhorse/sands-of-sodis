using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [Header("Vitality")]
    [SerializeField] private int maxVitality;
    private int currentVitality;

    public int CurrentVitality
    {
        get { return currentVitality; }
        set { currentVitality = Mathf.Clamp(value, 0, maxVitality); }
    }

    public VitBar healthBar;

    [Header("Determination")]
    [SerializeField] private int maxDet;
    private int currentDet;

    public int CurrentDet
    {
        get { return currentDet; }
        set { currentDet = Mathf.Clamp(value, 0, maxDet); }
    }

    public VitBar detBar;

    [Header("Fey")]
    [SerializeField] private int maxFey;
    private int currentFey;

    public int CurrentFey
    {
        get { return currentFey; }
        set { currentFey = Mathf.Clamp(value, 0, maxFey); }
    }

    public VitBar feyBar;

    public int Agility { get; set; }

    public int Strength { get; set; }

    public int Resilience { get; set; }

    public int MoveSpeed { get; set; }

    [Header("Inventory")]
    public List<Item> inventory = new List<Item>();

    public void Update()
    {
        // Update Faceplate
        healthBar.UpdateVitBar(currentVitality, maxVitality);
        detBar.UpdateVitBar(currentDet, maxDet);
        feyBar.UpdateVitBar(currentFey, maxFey);
    }

}
