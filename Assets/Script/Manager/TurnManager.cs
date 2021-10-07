using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : SingletonMonoBehaviour<TurnManager>
{
    public bool PlayerTurn
    {
        get;
        private set;
    }

    public void Initialize()
    {
        PlayerTurn = true;
    }

    public void UpdateTurn()
    {
        if (AllEnemyTurnIsOff() == true)
        {
            PlayerTurn = true;
            AllPlayerTurnOn();
        }

        if (AllPlayerTurnIsOff() == true)
        {
            EnemyAct();
            PlayerTurn = false;
            AllEnemyTurnOn();
            return;
        }
    }

    private void EnemyAct()
    {
        for (int i = 0; i <= ObjectManager.Instance.EnemyList.Count - 1; i++)
        {
            Debug.Log("a");
            Chara chara = ObjectManager.Instance.EnemyObject(i).GetComponent<Chara>();
            CharaBattle charaBattle = ObjectManager.Instance.EnemyObject(i).GetComponent<CharaBattle>();
            if (i == 0)
            {
                charaBattle.DecideAndExcuteAction();
                return;
            }

            if (chara.Turn == true && ObjectManager.Instance.EnemyObject(i - 1).GetComponent<Chara>().Turn == false)
            {
                charaBattle.DecideAndExcuteAction();
                return;
            }
        }
    }

    public bool AllPlayerTurnIsOff()
    {
        foreach (GameObject player in ObjectManager.Instance.PlayerList)
        {
            Chara chara = player.GetComponent<Chara>();
            if (chara.Turn == true)
            {
                return false;
            }
        }
        return true;
    }

    public void AllPlayerTurnOn()
    {
        foreach (GameObject player in ObjectManager.Instance.PlayerList)
        {
            Chara chara = player.GetComponent<Chara>();
            chara.Turn = true;
        }
    }

    public bool AllEnemyTurnIsOff()
    {
        foreach (GameObject enemy in ObjectManager.Instance.EnemyList)
        {
            Chara chara = enemy.GetComponent<Chara>();
            if (chara.Turn == true)
            {
                return false;
            }
        }
        return true;
    }

    public void AllEnemyTurnOn()
    {
        foreach (GameObject enemy in ObjectManager.Instance.EnemyList)
        {
            Chara chara = enemy.GetComponent<Chara>();
            chara.Turn = true;
        }
    }
}
