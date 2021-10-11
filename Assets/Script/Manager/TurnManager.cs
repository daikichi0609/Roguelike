using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : SingletonMonoBehaviour<TurnManager>
{
    public enum STATE
    {
        NONE,
        PLAYER_TURN,
        ENEMY_TURN,
    }

    [SerializeField] private STATE m_CurrentState;
    [SerializeField] private bool m_IsActing;
    public bool IsActing
    {
        get { return m_IsActing; }
        set { m_IsActing = value; }
    }

    public STATE CurrentState
    {
        get { return m_CurrentState; }
        set { m_CurrentState = value; }
    }

    public void InitializeTurn()
    {
        TurnOnAllPlayer();
    }

    public void UpdateTurn()
    {
        IsActingCheck();
        switch(CurrentState)
        {
            case STATE.NONE:
                if (IsAllEnemyTurnOff() == false)
                {
                    CurrentState = STATE.ENEMY_TURN;
                }
                else if (IsAllPlayerTurnOff() == false)
                {
                    CurrentState = STATE.PLAYER_TURN;
                }
                else
                {
                    CurrentState = STATE.PLAYER_TURN;
                }
                break;

            case STATE.PLAYER_TURN:
                if (IsAllPlayerTurnOff() == true)
                {
                    CurrentState = STATE.ENEMY_TURN;
                    TurnOnAllEnemy();
                    return;
                }
                break;

            case STATE.ENEMY_TURN:
                if (IsAllEnemyTurnOff() == true)
                {
                    CurrentState = STATE.PLAYER_TURN;
                    TurnOnAllPlayer();
                    return;
                }
                EnemyAct();
                break;
        }
    }

    private void IsActingCheck()
    {
        foreach(GameObject player in ObjectManager.Instance.PlayerList)
        {
            CharaMove playerMove = player.GetComponent<CharaMove>();
            if(playerMove.IsActing == true)
            {
                IsActing = true;
                return;
            }
        }

        foreach (GameObject enemy in ObjectManager.Instance.EnemyList)
        {
            CharaMove enemyMove = enemy.GetComponent<CharaMove>();
            if (enemyMove.IsActing == true)
            {
                IsActing = true;
                return;
            }
        }
        IsActing = false;
    }

    private void EnemyAct()
    {
        for (int i = 0; i <= ObjectManager.Instance.EnemyList.Count - 1; i++)
        {
            Chara chara = ObjectManager.Instance.EnemyObject(i).GetComponent<Chara>();
            EnemyAI enemyAI = ObjectManager.Instance.EnemyObject(i).GetComponent<EnemyAI>();
            if (chara.Turn == true)
            {
                Debug.Log(i);
                enemyAI.DecideAndExcuteAction();
                break;
            }
        }
    }

    public bool IsAllPlayerTurnOff()
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

    public void TurnOnAllPlayer()
    {
        foreach (GameObject player in ObjectManager.Instance.PlayerList)
        {
            Chara chara = player.GetComponent<Chara>();
            chara.Turn = true;
        }
    }

    public bool IsAllEnemyTurnOff()
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

    public void TurnOnAllEnemy()
    {
        foreach (GameObject enemy in ObjectManager.Instance.EnemyList)
        {
            Chara chara = enemy.GetComponent<Chara>();
            chara.Turn = true;
        }
    }
}
