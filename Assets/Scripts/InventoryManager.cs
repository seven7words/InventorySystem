using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    private static InventoryManager instance;

    public static  InventoryManager Instance 
    {
        get
        {
            if (instance == null)
            {
                //下面的代码只会执行一次
                instance = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
            }
            return instance;
        }
     
    }
    #region ToolTip
    private bool isToolTipShow = false;
    private ToolTip toolTip;
            private Vector2 toolTipPositionOffset = new Vector2(10, -10);
#endregion

    private Canvas canvas;
#region PickedItem

    private bool isPickedItem = false;

    public bool IsPickedItem
    {
        get { return isPickedItem; }
        
    }
    private ItemUI pickedItem;//鼠标选中的物体
    /// <summary>
    /// 物品信息集合
    /// </summary>
    private List<Item> itemList;

    public ItemUI PickedItem
    {
        get { return pickedItem; }
    }
#endregion

    void Awake()
    {
        ParseItemJson();
    }
    void Start()
    {
        
       // toolTip = GameObject.FindObjectOfType<ToolTip>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        pickedItem = GameObject.Find("PickedItem").GetComponent<ItemUI>();
        toolTip = GameObject.Find("ToolTip").GetComponent<ToolTip>();
        pickedItem.Hide();
    }



    void Update()
    {
        if (isPickedItem)
        {
            //如果我们捡起了物品，我们就要让物品跟随鼠标
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                Input.mousePosition, null, out position);
            
            pickedItem.SetLocalPosition(position);
           
        }
        else  if (isToolTipShow)
        {
            //控制提示面板跟随鼠标
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                Input.mousePosition, null, out position);
            
            toolTip.SetLocalPosition(position+toolTipPositionOffset);
            
        }
        //物品丢弃的处理
        //鼠标上是否有UI
        if (isPickedItem && Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject(-1)==false)
        {
            isPickedItem = false;
            pickedItem.Hide();
        }

    }
    /// <summary>
    /// 解析物品信息
    /// </summary>
    /// 
    /// 
    void ParseItemJson()
    {
        itemList = new List<Item>();
        //AssetText文本在Unity是这个类型的
        TextAsset itemText = Resources.Load<TextAsset>("Items");
        string itemJson = itemText.text;//物品信息的json格式
        JSONObject json = new JSONObject(itemJson);
        foreach (JSONObject temp in json.list)
        {
            //解析这个对象共有的属性
            string typeStr = temp["type"].str;
            //根据名字尝试跟枚举比较
            ItemType type = (ItemType)System.Enum.Parse(typeof(ItemType), typeStr);
            int id = (int)(temp["id"].n);

            string name = temp["name"].str;
            Quality quality = (Quality)System.Enum.Parse(typeof(Quality), temp["quality"].str);
            string description = temp["description"].str;
            int capacity = (int)(temp["capacity"].n);

            int buyPrice = (int)(temp["buyPrice"].n);

            int sellPrice = (int)(temp["sellPrice"].n);

            string sprite = temp["sprite"].str;
            Item item = null;
            switch (type)
            {

                case ItemType.Consumable:
                    int hp = (int)(temp["hp"].n);

                    int mp = (int)(temp["mp"].n);

                    item = new Consumble(id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite, hp, mp);

                    break;
                case ItemType.Equipment:
                    //TODO
                    int strength = (int)temp["strength"].n;
                    int intellect = (int)temp["intellect"].n;
                    int stamina = (int)temp["stamina"].n;
                    int agility = (int)temp["agility"].n;
                    EquipmentType equipType =
                        (EquipmentType)System.Enum.Parse(typeof(EquipmentType), temp["equipType"].str);
                    item = new Equipment(id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite, strength, intellect, agility, stamina, equipType);
                    break;
                case ItemType.Weapon:
                    //TODO
                    int damage = (int)temp["damage"].n;
                    WeaponType wpType = (WeaponType)System.Enum.Parse(typeof(WeaponType), temp["weaponType"].str);
                    item = new Weapon(id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite, damage, wpType);
                    break;
                case ItemType.Material:
                    item = new Material(id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite);
                    //TODO
                    break;
            }
            itemList.Add(item);
            Debug.Log(item);
        }

    }




    public Item GetItemById(int id)
    {
        foreach (Item item in itemList)
        {
            if (item.ID == id)
            {
                return item;
            }
        }
        return null;
    }

    public void ShowToolTip(string content)
    {
        if(this.isPickedItem)
            return;
        isToolTipShow = true;
        toolTip.Show(content);
    }

    public void HideToolTip()
    {
        isToolTipShow = false;
        toolTip.Hide();
    }
    //捡起物品槽指定数量的物品
    public void PickupItem(Item item,int amount)
    {


       
        isPickedItem = true;
        this.toolTip.Hide();
        PickedItem.SetItem(item, amount);
        PickedItem.Show();
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
           Input.mousePosition, null, out position);
        pickedItem.SetLocalPosition(position);


    }
    /// <summary>
    /// 从手上拿掉一个物品放在物品槽里面
    /// </summary>


  
    /// <summary>
    /// 移除指定数量
    /// </summary>
    /// <param name="amount"></param>
    public void RemoveItem(int amount=1)
    {
        PickedItem.ReduceAmount(amount);
        if (PickedItem.Amount <= 0)
        {
            isPickedItem = false;
            PickedItem.Hide();
        }
    }

    public void SaveInventory()
    {
        Knapsack.Instance.SaveInventory();
        Chest.Instance.SaveInventory();
        CharacterPanel.Instance.SaveInventory();
      
        Forge.Instance.SaveInventory();
        PlayerPrefs.SetInt("CoinAmount",GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CoinAmount);

    }

    public void LoadInventory()
    {
        Knapsack.Instance.LoadInventory();
        Chest.Instance.LoadInventory();
        CharacterPanel.Instance.LoadInventory();
        
        Forge.Instance.LoadInventory();
        if (PlayerPrefs.HasKey("CoinAmount"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CoinAmount =
                PlayerPrefs.GetInt("CoinAmount");
        }
    }
    
}
