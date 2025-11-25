using System;
using System.Drawing;
using UnityEngine;

[System.Serializable]
public class Party
{
    public string name;
    public string partyColor; // bude jse používat k reprezentacy strany
    public string ideology;
    public string[] secundery_ideology;
    public int power; // sila v porocen tech ve volbách
    public int seats; //poèet realných pozic v parlamentu
    public string[] goals;       
}
