using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class TurnManager : SingletonMonoBehaviour<TurnManager>
{
    /// <summary>
    /// 全キャラに行動を禁止させるフラグ
    /// </summary>
    [SerializeField]
    private bool m_IsCanAction => IsCanAction;
    public bool IsCanAction
    {
        get
        {
            foreach(GameObject obj in ObjectManager.Instance.m_PlayerList)
            {
                if(obj.GetComponent<CharaTurn>().CAN_ACTION == false)
                {
                    return false;
                }
            }

            foreach (GameObject obj in ObjectManager.Instance.m_EnemyList)
            {
                if (obj.GetComponent<CharaTurn>().CAN_ACTION == false)
                {
                    return false;
                }
            }

            return true;
        }
    }

    /// <summary>
    /// 全キャラに攻撃を禁止させるフラグ
    /// </summary>
    [SerializeField]
    private bool m_IsCanAttack => IsCanAttack;
    public bool IsCanAttack
    {
        get
        {
            foreach (GameObject obj in ObjectManager.Instance.m_PlayerList)
            {
                if (obj.GetComponent<CharaTurn>().CAN_ATTACK == false)
                {
                    return false;
                }
            }

            foreach (GameObject obj in ObjectManager.Instance.m_EnemyList)
            {
                if (obj.GetComponent<CharaTurn>().CAN_ATTACK == false)
                {
                    return false;
                }
            }

            return true;
        }
    }

    protected override void Awake()
    {
        MessageBroker.Default.Receive<Message.MFinishTurn>()
            .Subscribe(_ => NextAction()).AddTo(this);
    }

    /// <summary>
    /// 次の行動を促す
    /// </summary>
    private void NextAction()
    {
        //行動禁止中なら受け付けない
        if(IsCanAction == false)
        {
            Debug.Log("禁止中");
            return;
        }

        //プレイヤーから回す
        foreach(GameObject obj in ObjectManager.Instance.m_PlayerList)
        {
            if(obj.GetComponent<CharaTurn>().IsFinishTurn == false)
            {
                return;
            }
        }

        //敵
        foreach(GameObject obj in ObjectManager.Instance.m_EnemyList)
        {
            if (obj.GetComponent<CharaTurn>().IsFinishTurn == false)
            {
                EnemyBattle enemyBattle = obj.GetComponent<EnemyBattle>();
                enemyBattle.DecideAndExecuteAction();
                return;
            }
        }

        //全キャラ行動済みなら行動済みステータスをリセット
        AllCharaActionable();
    }

    public IEnumerator WaitForIsCanAttack()
    {
        yield return new WaitUntil(() => IsCanAttack == true);
    }

    public IEnumerator WaitForIsCanAttack(Action action)
    {
        yield return new WaitUntil(() => IsCanAttack == true);
        action?.Invoke();
    }

    public void AllCharaActionable()
    {
        AllPlayerActionable();
        AllEnemyActionable();
    }

    public void AllPlayerActionable()
    {
        foreach (GameObject player in ObjectManager.Instance.m_PlayerList)
        {
            CharaTurn chara = player.GetComponent<CharaTurn>();
            chara.StartTurn();
        }
    }

    public void AllEnemyActionable()
    {
        foreach (GameObject enemy in ObjectManager.Instance.m_EnemyList)
        {
            CharaTurn chara = enemy.GetComponent<CharaTurn>();
            chara.StartTurn();
        }
    }
}
