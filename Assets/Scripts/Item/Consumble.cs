using UnityEngine;
using System.Collections;
/// <summary>
/// 消耗品类
/// </summary>
public class Consumble : Item {
    public Consumble(int id, string name, ItemType itemType, Quality quality, string des, int capacity, int buyPrice, int sellPrice, string sprite, int hp,int mp) : base(id, name, itemType, quality, des, capacity, buyPrice, sellPrice, sprite)
    {
        this.HP = hp;
        this.MP = mp;
    }

    public override string ToString()
    {
        string s = "";
        s += ID;
        s += ItemType;
        s += Quality;
        s += Description;
        s += Capacity.ToString();
        s += BuyPrice.ToString();
        s += SellPrice.ToString();
        s += HP.ToString();
        s += MP.ToString();
        return s;
    }

    public int HP { get; set; }
    public int MP { get; set; }

    public override string GetToolTipText()
    {
        string text = base.GetToolTipText();
        string newText = string.Format("{0}\n\n<color=blue>加血:{1}</color>\n<color=blue>加蓝:{2}</color>", text, HP, MP);
        return newText;
    }
}
