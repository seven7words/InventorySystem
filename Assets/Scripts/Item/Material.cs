using UnityEngine;
using System.Collections;
/// <summary>
/// 材料类
/// </summary>
public class Material : Item
{
    public Material(int id, string name, ItemType itemType, Quality quality, string des, int capacity, int buyPrice, int sellPrice, string sprite) : base(id, name, itemType, quality, des, capacity, buyPrice, sellPrice,sprite)
    {
       
    }

}
