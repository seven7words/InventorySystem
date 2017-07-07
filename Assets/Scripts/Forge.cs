using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Forge : Inventory
{
    private static Forge instance;

    public static Forge Instance
    {
        get
        {
            if (instance == null)
            {
                //下面的代码只会执行一次
                instance = GameObject.Find("ForgePanel").GetComponent<Forge>();
            }
            return instance;
        }

    }
    private List<Formula> formulaList;
    public override void Start()
    {
        base.Start();
        ParseFormulaJson();
    }

    void ParseFormulaJson()
    {
        formulaList  = new List<Formula>();
        TextAsset formulaText = Resources.Load<TextAsset>("Formulas");
        string formulasJson = formulaText.text;//配方信息的Json数据
     
        JSONObject jo = new JSONObject(formulasJson);
        foreach (JSONObject temp in jo.list)
        {
            int item1ID = (int)temp["Item1ID"].n;
            int item1Amount = (int)temp["Item1Amount"].n;
            int item2ID = (int)temp["Item2ID"].n;
            int item2Amount = (int)temp["Item2Amount"].n;
            int resID = (int) temp["ResID"].n;
            Formula formula = new Formula(item1ID,item1Amount,item2ID,item2Amount,resID);
            formulaList.Add(formula);
        }
        Debug.Log(formulaList[1].ResID);
    }

    public void ForgeItem()
    {
        //得到当前有哪些材料
        //判断满足哪一个秘籍的要求

        List<int> haveMaterialIDList = new List<int>();//存储当前拥有的材料的id
        foreach (Slot slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                ItemUI currentItemUI = slot.transform.GetChild(0).GetComponent<ItemUI>();
                for (int i = 0; i < currentItemUI.Amount; i++)
                {
                    haveMaterialIDList.Add(currentItemUI.Item.ID);//这个隔离子liam有多少个物体，就存在多少个id
                }
            }
        }
        Formula matchedFormula = null;
        foreach (Formula formula in formulaList)
        {
           bool isMatch =  formula.Mathch(haveMaterialIDList);
            if (isMatch)
            {
                matchedFormula = formula;
                break;
            }
        }
        if (matchedFormula != null)
        {
            Knapsack.Instance.StoreItem(matchedFormula.ResID);
            //去掉消耗的材料
            foreach (int id in matchedFormula.NeedIdList)
            {
                foreach (Slot slot in slotList  )
                {
                    if (slot.transform.childCount > 0)
                    {
                        ItemUI itemUI = slot.transform.GetChild(0).GetComponent<ItemUI>();
                        if (itemUI.Item.ID == id&& itemUI.Amount > 0)
                        {
                            
                                itemUI.ReduceAmount();
                                if (itemUI.Amount <= 0)
                                {
                                    DestroyImmediate(itemUI.gameObject);
                                }
                                break;
                            
                        }
                    }
                }
            }
        }
        
    }
}
