using UnityEngine;
using System.Collections;

public class Knapsack : Inventory {

    #region 单例模式

    private static Knapsack instance;

    public static Knapsack Instance
    {
        get
        {
            if (instance == null)
            {
               instance = GameObject.Find("KnapsackPanel").GetComponent<Knapsack>();
            }
            return instance;
        }

    }

    #endregion
}
