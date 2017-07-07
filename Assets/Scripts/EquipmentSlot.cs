using UnityEngine;
using System.Collections;
using System.Xml.Linq;
using UnityEngine.EventSystems;

public class EquipmentSlot : Slot
{

    public EquipmentType equipmentType;
    public WeaponType wpType;
    public override void OnPointerDown(PointerEventData eventData)

    {


        if (eventData.button == PointerEventData.InputButton.Right)
        {

            if (InventoryManager.Instance.IsPickedItem == false && transform.childCount > 0)
            {
                ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();

                Item itemTemp = currentItemUI.Item;
                //脱掉放到背包里面
                DestroyImmediate(currentItemUI.gameObject);
                transform.parent.SendMessage("PutOff",itemTemp);
               
                InventoryManager.Instance.HideToolTip();

                
            }
        }
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        //手上有东西
                    //当前装备槽有装备
                    //无装备
        //手上没东西
                    //当前装备槽有装备
                    //无装备 不做处理
        bool isUpdateProperty = false;
        if (InventoryManager.Instance.IsPickedItem == true)
        {
            //手上有东西
            ItemUI pickedItem = InventoryManager.Instance.PickedItem;
            if (transform.childCount > 0)
            {
                ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
                transform.GetChild(0).GetComponent<ItemUI>();
            
                if (IsRightItem(pickedItem.Item))
                {
                    InventoryManager.Instance.PickedItem.Exchange(currentItemUI);
                    isUpdateProperty = true;

                }
            }
            else
            {
                if (IsRightItem(pickedItem.Item))
                {
                    this.StoreItem((InventoryManager.Instance.PickedItem.Item));
                    InventoryManager.Instance.RemoveItem(1);
                    isUpdateProperty = true;
                }

               
            }
        }
        else
        {
            //手上没东西的情况
            if (transform.childCount > 0)
            {
                ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
                InventoryManager.Instance.PickupItem(currentItemUI.Item,currentItemUI.Amount);
                Destroy(transform.GetChild(0).gameObject);
                isUpdateProperty = true;
            }
            
        }
        if (isUpdateProperty)
        {
            transform.parent.SendMessage("UpdatePropertyText");
        }

    }
    public bool IsRightItem(Item item)
    {
        if ((item is Equipment && ((Equipment)(item)).EquipmentType == this.equipmentType) ||
                    (item is Weapon && ((Weapon)(item)).WeaponType == this.wpType))
        {
            return true;
        }
        return false;
    }
}
