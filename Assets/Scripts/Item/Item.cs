using System;
using UnityEngine;
using System.Collections;
/// <summary>
/// 物品基础类
/// </summary>
public class Item
{
    public int ID { get; set; }
    public string Name { get; set; }
    public ItemType ItemType { get; set; }
    public Quality Quality { get; set; }
    public string Description { get; set; }
    public int Capacity { get; set; }
    public int BuyPrice { get; set; }
    public int SellPrice { get; set; }
    public string Sprite { get; set; }
    public Item()
    {
        this.ID = -1;
    }
    public Item(int id, string name, ItemType itemType, Quality quality, string des, int capacity, int buyPrice,
        int sellPrice, string sprite)
    {
        this.ID = id;
        this.Name = name;
        this.ItemType = itemType;
        this.Quality= quality;
        this.Description = des;
        this.Capacity = capacity;
        this.BuyPrice = buyPrice;
        this.SellPrice = sellPrice;
        this.Sprite = sprite;
    }
    /// <summary>
    /// 得到提示面板应该显示什么样的内容
    /// </summary>
    /// <returns></returns>
    public virtual string GetToolTipText()
    {
        string color = "";
        switch (Quality)
        {
            case Quality.Common:
                color = "white";
                break;
            case Quality.Uncommon:
                color = "lime";
                break;
            case Quality.Rare:
                color = "navy";
                break;
            case Quality.Epic:
                color = "magenta";
                break;
            case Quality.Legendary:
                color = "orange";
                break;
            case Quality.Artifact:
                color = "red";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        string text = string.Format("<color={4}>{0}</color>\n<size=15><color=green>购买价格：{1}出售价格：{2}</color></size>\n<color=yellow><size=10>{3}</size></color>", Name,BuyPrice,SellPrice,Description,color);
        
        return text; //TODO
    }
}


/// <summary>
/// 物品类型
/// </summary>
public enum ItemType
{
    Consumable,
    Equipment,
    Weapon,
    Material,
}

/// <summary>
/// 品质
/// </summary>
public enum Quality
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
    Artifact,
}