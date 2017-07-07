using UnityEngine;
using System.Collections;

public class Chest : Inventory {

    #region 单例模式

    private static Chest instance;

    public static Chest Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.Find("ChestPanel").GetComponent<Chest>();
            }
            return instance;
        }

    }

    #endregion
}

