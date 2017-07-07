using System;
using UnityEngine;
using System.Collections;
/// <summary>
/// 武器类
/// </summary>
public class Weapon : Item {
    public Weapon(int id, string name, ItemType itemType, Quality quality, string des, int capacity, int buyPrice, int sellPrice, string sprite, int damage,WeaponType weaponType) : base(id, name, itemType, quality, des, capacity, buyPrice, sellPrice,sprite)
    {
        this.Damage = damage;
        this.WeaponType = weaponType;
    }

    public int Damage { get; set; }


    public WeaponType WeaponType { get; set; }

    public override string GetToolTipText()
    {
        string text = base.GetToolTipText();
        string wpTypeText = "";
        switch (WeaponType)
        {
            case WeaponType.OffHand:
                wpTypeText = "副手";
                break;
            case WeaponType.MainHand:
                wpTypeText = "主手";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        string newText = string.Format("{0}\n\n<color=blue>武器类型:{1}\n攻击力：{2}</color>", text, wpTypeText,Damage);
        return newText;
    }
}

public enum WeaponType
{None,
    OffHand,
    MainHand
}


