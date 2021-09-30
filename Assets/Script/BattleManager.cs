using UnityEngine;
using System.Collections.Generic;

public class BattleManager : SingletonMonoBehaviour<BattleManager>
{
    private delegate bool CheckFinishTurn();
    private CheckFinishTurn m_ChechFinishTurn;

    private List<GameObject> m_PlayerList = new List<GameObject>();
    public List<GameObject> PlayerList
    {
        get { return m_PlayerList; }
    }
    public GameObject PlayerObject(int i)
    {
        return m_PlayerList[i];
    }

    private List<GameObject> m_EnemyList = new List<GameObject>();
    public List<GameObject> EnemyList
    {
        get { return m_EnemyList; }
    }
    public GameObject EnemyObject(int i)
    {
        return m_EnemyList[i];
    }
    private int IsActingEnemyID;

    private bool m_PlayerTurn = true;
    public bool PlayerTurn
    {
        get { return m_PlayerTurn; }
        set { m_PlayerTurn = value; }
    }

    private void Update()
    {
        if (PlayerTurn == false) //キャラ毎にターン切り替えを行う
        {
            SwitchEnemyTurn();
        }

        SubstitutionCheckFinishMethod(); //delegate代入

        if (m_ChechFinishTurn != null)
        {
            return;
        }

        if(m_ChechFinishTurn() == true) //ターン切り替え
        {
            PlayerTurn = !PlayerTurn;
        }
    }

    private void SubstitutionCheckFinishMethod()
    {
        if(PlayerTurn == true)
        {
            m_ChechFinishTurn = FinishPlayerTurn;
        }
        else
        {
            m_ChechFinishTurn = FinishEnemyTurn;
        }
    }

    private bool FinishPlayerTurn()
    {
        int num = 0;
        foreach (GameObject players in PlayerList)
        {
            if (players.GetComponent<CharaBattle>().Turn == false)
            {
                num++;
            }
        }
        if (PlayerList.Count == num)
        {
            return true;
        }
        return false;
    }

    private bool FinishEnemyTurn()
    {
        int num = 0;
        foreach (GameObject enemys in EnemyList)
        {
            if (enemys.GetComponent<CharaBattle>().Turn == false)
            {
                num++;
            }
        }
        if (EnemyList.Count == num)
        {
            return true;
        }
        return false;
    }

    private void SwitchEnemyTurn()
    {
        
    }

    public int CalculatePower(int atk, float mag)
    {
        return (int)(atk * mag);
    }

    public int CalculateDamage(int power, int def)
    {
        int damage = power - def;
        if (damage < 1)
        {
            damage = 1;
        }
        return damage;
    }

    public int CalculateRemainingHp(int hp, int damage)
    {
        int remainingHp = hp - damage;
        if (remainingHp < 0)
        {
            remainingHp = 0;
        }
        return remainingHp;
    }
}