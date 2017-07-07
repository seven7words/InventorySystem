using UnityEngine;
using System.Collections;
using System.Xml.Schema;

public class Vendor : Inventory
{

    private static Vendor instance;

    public static Vendor Instance
    {
        get
        {
            if (instance == null)
            {
                //下面的代码只会执行一次
                instance = GameObject.Find("VendorPanel").GetComponent<Vendor>();
            }
            return instance;
        }

    }
    public int[] itemIdArray;
    private Player player;
    public override void Start()
    {
        base.Start();
        InitShop();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Hide();
    }

    private void InitShop()
    {
        foreach (int i in itemIdArray)
        {
            StoreItem(i);
        }
    }
    /// <summary>
    /// 主角购买
    /// </summary>
    /// <param name="item"></param>
    public void BuyItem(Item item)
    {
        bool isSuccess = player.Consume(item.BuyPrice);
        if (isSuccess)
        {
            Knapsack.Instance.StoreItem(item);
        }
    }
    /// <summary>
    /// 主角出售
    /// </summary>

    public void SellItem()
    {
        int sellAmount = 1;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            sellAmount = 1;
        }
        else
        {
            sellAmount = InventoryManager.Instance.PickedItem.Amount;
        }
        int coinAmount = InventoryManager.Instance.PickedItem.Item.SellPrice*sellAmount;
        player.EarnCoin(coinAmount);
        InventoryManager.Instance.PickedItem.ReduceAmount(sellAmount);
        if (InventoryManager.Instance.PickedItem.Amount <= 0)
        {
            InventoryManager.Instance.RemoveItem(sellAmount);
        }

    }

}
