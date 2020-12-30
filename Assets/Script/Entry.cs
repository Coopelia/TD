using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntryData
{
    public Sprite icon;
    public string _name;
    public float number;

    public EntryData()
    {
        icon = null;
        _name = "";
        number = 0;
    }
}

public class Entry: MonoBehaviour
{
    public Image icon;
    public Text _name;
    public Text number;
}
