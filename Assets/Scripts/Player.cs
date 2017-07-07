using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	    coinText = GameObject.Find("Coin").GetComponentInChildren<Text>();

	    coinText.text = CoinAmount.ToString();
	}
#region 角色基础属性
    private int basicStrength = 10;
    private int basicIntellect = 10;
    private int basicAgility = 10;
    private int basicStamina = 10;
    private int basicDamage = 10;

    public int BasicStrength
    {
        get
        {
            return basicStrength;
        }

       
    }

    public int BasicIntellect
    {
        get
        {
            return basicIntellect;
        }

    }

    public int BasicAgility
    {
        get
        {
            return basicAgility;
        }

    }

    public int BasicStamina
    {
        get
        {
            return basicStamina;
        }

        
    }

    public int BasicDamage
    {
        get
        {
            return basicDamage;
        }

      
    }

    public int CoinAmount
    {
        get
        {
            return coinAmount;
        }

        set
        {
            coinAmount = value;
        }
    }


    #endregion

    private int coinAmount = 100;
    private Text coinText;
    // Update is called once per frame
    void Update () {
        //随机得到一个物品放到背包里面 G
	    if (Input.GetKeyDown(KeyCode.G))
	    {
	        int id = Random.Range(1, 18);
	        Knapsack.Instance.StoreItem(id);
            Debug.Log(id);
	    }
        //T 控制背包的显示和隐藏
	    if (Input.GetKeyDown(KeyCode.T))
	    {
	        Knapsack.Instance.DisplaySwitch();
	    }
        //Y控制箱子的显示和隐藏
	    if (Input.GetKeyDown(KeyCode.Y))
	    {
	        Chest.Instance.DisplaySwitch();
	    }
        //U 控制角色面板的显示和隐藏
	    if (Input.GetKeyDown(KeyCode.U))
	    {
	        CharacterPanel.Instance.DisplaySwitch();
	    }
        //I控制商店显示和隐藏
        if (Input.GetKeyDown(KeyCode.I))
        {
            Vendor.Instance.DisplaySwitch();
        }
        //O控制锻造界面显示和隐藏
        if (Input.GetKeyDown(KeyCode.O))
        {
            Forge.Instance.DisplaySwitch();
        }
    }
    /// <summary>
    /// 消费金币
    /// </summary>
    public bool Consume(int amount)
    {
        if (CoinAmount >= amount)
        {
            CoinAmount -= amount;
            coinText.text = CoinAmount.ToString();
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 赚取金币
    /// </summary>
    /// <param name="amount"></param>
    public void EarnCoin(int amount)
    {
        this.CoinAmount += amount;
        coinText.text = CoinAmount.ToString();
    }
}
