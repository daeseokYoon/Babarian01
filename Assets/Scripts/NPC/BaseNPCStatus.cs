using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[System.Serializable]
public class BaseNPCStatus
{
    public string name;

    public enum Rarity
    {
        COMMON,
        UNCOMMON,
        RARE,
        SUPERRARE,
    }

    public Rarity rarity;

    public float baseHP;
    public float curHP;

    public float baseAtk;
    public float curAtk;

    public float baseDef;
    public float curDef;


}
