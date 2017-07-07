using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class Inventory : MonoBehaviour
{
   
    protected Slot[] slotList;
    private float targetAlpha = 1;
    private float smoothing = 4;
    private CanvasGroup canvasGroup;
	// Use this for initialization
    public virtual void Start () {
	
	   slotList= GetComponentsInChildren<Slot>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if(canvasGroup.alpha  != targetAlpha)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, smoothing*Time.deltaTime);
            if (Mathf.Abs(canvasGroup.alpha - targetAlpha) < 0.01f)
            {
                canvasGroup.alpha = targetAlpha;
            }
        }
    }

    public bool StoreItem(int id)
    {
        Item item = InventoryManager.Instance.GetItemById(id);
        return StoreItem(item);
    }

    public bool StoreItem(Item item)
    {
        if (item == null)
        {
            Debug.LogWarning("要存储的物品的id不存在");
            return false;
        }
        if (item.Capacity == 1)
        {
            //TODO
            Slot slot = FindEmpeySlot();
            if (slot == null)
            {
                Debug.LogWarning("没有空的物品槽");
                return false;
            }
            else
            {
                slot.StoreItem(item);//把物品存储到空的物品槽里面
                return true;
            }
        }
        else
        {
            Slot slot = FindSameIdSlot(item);
            if (slot != null)
            {
                slot.StoreItem(item);
                return true;
            }
            else
            {
                Slot emptySlot = FindEmpeySlot();
                if (emptySlot != null)
                {
                    emptySlot.StoreItem(item);
                    return true;
                }
                else
                {
                    Debug.LogWarning("没有空的物品槽");
                    return false;
                }
            }
        }
    }
    /// <summary>
    /// 用来招到一个空的物品槽
    /// </summary>
    /// <returns></returns>
    private Slot FindEmpeySlot()
    {
        foreach (Slot slot in slotList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return null;
    }

    private Slot FindSameIdSlot(Item item)
    {
        foreach (Slot slot in slotList)
        {
            if (slot.transform.childCount >= 1&& slot.GetItemId()==item.ID&&slot.IsFilled()==false)
            {
                return slot;
            }
        }
        return null;
    }

    public void Show()
    {
        canvasGroup.blocksRaycasts = true;
        targetAlpha = 1f;
    }

    public void Hide()
    {
        canvasGroup.blocksRaycasts = false;
        targetAlpha = 0f;
        
    }

    public void DisplaySwitch()
    {
        if (targetAlpha == 0)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    #region save and load

    public void SaveInventory()
    {
        StringBuilder sb = new StringBuilder();
        foreach (Slot slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                ItemUI itemUI = slot.transform.GetChild(0).GetComponent<ItemUI>();
                sb.Append(itemUI.Item.ID + "," + itemUI.Amount + "-");
            }
            else
            {
                sb.Append("0-");
            }
        }
        PlayerPrefs.SetString(this.gameObject.name,sb.ToString());
    }

    public void LoadInventory()
    {
        if (PlayerPrefs.HasKey(this.gameObject.name)==false)
        {
            return;
        }
        string str = PlayerPrefs.GetString(this.gameObject.name);
        string[] itemArray = str.Split('-');
        for (int i = 0; i < itemArray.Length-1; i++)
        {
            string itemStr = itemArray[i];
            if (itemStr != "0")
            {
                string[] temp= itemStr.Split(',');
                int id;
                int.TryParse(temp[0], out id);
                Item item = InventoryManager.Instance.GetItemById(id);
                int amount;
                int.TryParse(temp[1], out amount);
                for (int j = 0; j < amount; j++)
                {
                    slotList[i].StoreItem(item);
                }

            }
        }
    }
#endregion
}
