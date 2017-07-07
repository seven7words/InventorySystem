using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// 物品槽
/// </summary>
public class Slot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler
{
    public GameObject itemPrefab;
    /// <summary>
    /// 把item放在自身下面，如果自身下面已经有了amount++，
    /// 如果没有，根据itemPrefab去实例化放在下面
    /// </summary>
    /// <param name="item"></param>
    public void StoreItem(Item item)
    {
        if (transform.childCount == 0)
        {
            GameObject itemGameObject =  Instantiate(itemPrefab) as GameObject;
            itemGameObject.transform.SetParent(this.transform);
            itemGameObject.transform.localScale = Vector3.one;
            itemGameObject.transform.localPosition = Vector3.zero;
            itemGameObject.GetComponent<ItemUI>().SetItem(item);

        }
        else
        {
            transform.GetChild(0).GetComponent<ItemUI>().AddAmount();
        }
    }

    
    /// <summary>
    /// 得到当前物品槽存储的物品类型
    /// </summary>
    /// <returns></returns>

    public ItemType GetItemType()
    {
        return transform.GetChild(0).GetComponent<ItemUI>().Item.ItemType;
    }

    public int GetItemId()
    {
        return transform.GetChild(0).GetComponent<ItemUI>().Item.ID;
    
}
    public bool IsFilled()
    {
        ItemUI  itemUI= transform.GetChild(0).GetComponent<ItemUI>();
        return itemUI.Amount >= itemUI.Item.Capacity;//当前数量大于容量
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (transform.childCount > 0)
        {
            string toolTipText = transform.GetChild(0).GetComponent<ItemUI>().Item.GetToolTipText();
            InventoryManager.Instance.ShowToolTip(toolTipText);
        }
       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (transform.childCount > 0) 
            InventoryManager.Instance.HideToolTip();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {

            if (InventoryManager.Instance.IsPickedItem==false&&transform.childCount > 0)
            {
                ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
                if (currentItemUI.Item is Equipment || currentItemUI.Item is Weapon)
                {
                    currentItemUI.ReduceAmount(1);
                    Item currentItem = currentItemUI.Item;
                    if (currentItemUI.Amount <= 0)
                    {
                        DestroyImmediate(currentItemUI.gameObject);
                        InventoryManager.Instance.HideToolTip();
                    }
                    CharacterPanel.Instance.PutOn(currentItem);
                   
                }
            }
        }

        if(eventData.button!=PointerEventData.InputButton.Left)
            return;
        //自身是空的1.IsPickedItem == true PickedItem放在这个位置
        //按下ctrol放置当前鼠标上的物品的一个
        //没有按下ctrol 放置当前鼠标上的物品的所有
        //2IsPickedItem == flase 不做任何处理

        //自身不是
        //IsPickedItem == true  PickedItem与当前物品交换进行交换
        //自身的id==pickedItem.id//
        //   //按下ctrol放置当前鼠标上的物品的一个
        //没有按下ctrol 放置当前鼠标上的物品的所有
        //可以完全放下
        //只能放下其中一部分

        //自身的id!=pickedItem.id 跟当前物体交换
        //2IsPickedItem == false把当前物品槽里的物品放在鼠标
        //ctrl按下，取得当前物品槽中物品的一半，
        //没有按下ctrl，把当前物品槽的物品放到鼠标上
        if (transform.childCount > 0)
        {
            ItemUI currentItem = transform.GetChild(0).GetComponent<ItemUI>();
            if (InventoryManager.Instance.IsPickedItem == false) //当前没有选中任何物品（当前受伤没有任何物品，火者鼠标山没有任何物品）
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    ///TODO
                    int amountPicked = (currentItem.Amount + 1)/2;
                    InventoryManager.Instance.PickupItem(currentItem.Item, amountPicked);
                    int amountRemained = currentItem.Amount - amountPicked;
                    if (amountRemained <= 0)
                    {
                        Destroy(currentItem.gameObject);

                    }
                    else
                    {
                        currentItem.SetAmount(amountRemained);
                    }
                }
                else
                {
                    //把当前物品槽的信息，赋值geiPickedItem(跟随鼠标移动)
                    InventoryManager.Instance.PickupItem(currentItem.Item, currentItem.Amount);
                    Destroy(currentItem.gameObject); //销毁当前物品
                }
            }
            else
            {

                //自身的id!=pickedItem.id 跟当前物体交换
                //2IsPickedItem == false把当前物品槽里的物品放在鼠标
                //ctrl按下，取得当前物品槽中物品的一半，
                //没有按下ctrl，把当前物品槽的物品放到鼠标上
                if (currentItem.Item.ID == InventoryManager.Instance.PickedItem.Item.ID)
                {
                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        if (currentItem.Item.Capacity > currentItem.Amount)
                        {
                            //当前物品槽还有容量
                            currentItem.AddAmount();
                            InventoryManager.Instance.RemoveItem();

                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (currentItem.Item.Capacity > currentItem.Amount)
                        {
                            //当前物品槽还有容量
                            //当前物品槽剩余的空间
                            int amountReamin = currentItem.Item.Capacity - currentItem.Amount;
                            if (amountReamin >= InventoryManager.Instance.PickedItem.Amount)
                            {
                                currentItem.SetAmount(currentItem.Amount + InventoryManager.Instance.PickedItem.Amount);
                                InventoryManager.Instance.RemoveItem(InventoryManager.Instance.PickedItem.Amount);
                            }
                            else
                            {
                                currentItem.SetAmount(amountReamin + currentItem.Amount);
                                InventoryManager.Instance.RemoveItem(amountReamin);
                            }

                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else
                {
                    Item item = currentItem.Item;
                    int amount = currentItem.Amount;
                    currentItem.SetItem(InventoryManager.Instance.PickedItem.Item,InventoryManager.Instance.PickedItem.Amount);
                    InventoryManager.Instance.PickedItem.SetItem(item,amount);
                }

            }
        }
        else
        {
            //自身是空的1.IsPickedItem == true PickedItem放在这个位置
            //按下ctrol放置当前鼠标上的物品的一个
            //没有按下ctrol 放置当前鼠标上的物品的所有数量
            //2IsPickedItem == flase 不做任何处理
            if (InventoryManager.Instance.IsPickedItem == true)
            {
                if (Input.GetKey((KeyCode.LeftControl)))
                {
                    this.StoreItem(InventoryManager.Instance.PickedItem.Item);
                    InventoryManager.Instance.RemoveItem();
                }
                else
                {
                    for (int i = 0; i < InventoryManager.Instance.PickedItem.Amount; i++)
                    {
                        this.StoreItem(InventoryManager.Instance.PickedItem.Item);
                    }
                    InventoryManager.Instance.RemoveItem(InventoryManager.Instance.PickedItem.Amount);
                }
            }
            else
            {
                return;
            }
           

        }
       
    }
}
