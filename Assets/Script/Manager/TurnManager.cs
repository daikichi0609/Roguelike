﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TurnManager : SingletonMonoBehaviour<TurnManager>
{
    /// <summary>
    /// 全キャラに行動を禁止させるフラグ
    /// </summary>
    [SerializeField]
    private bool m_IsCanAction = true;
    public bool IsCanAction
    {
        get { return m_IsCanAction; }
        set { m_IsCanAction = value; }
    }

    /// <summary>
    /// 全キャラに攻撃を禁止させるフラグ
    /// </summary>
    [SerializeField]
    private bool m_IsCanAttack = true;
    public bool IsCanAttack
    {
        get { return m_IsCanAction; }
        set { m_IsCanAction = value; }
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
        foreach(GameObject obj in ObjectManager.Instance.m_PlayerList)
        {
            if(obj.GetComponent<CharaTurn>().IsFinishTurn == false)
            {
                Debug.Log("自分の行動まち");
                return;
            }
        }

        foreach(GameObject obj in ObjectManager.Instance.m_EnemyList)
        {
            if (obj.GetComponent<CharaTurn>().IsFinishTurn == false)
            {
                Debug.Log("敵の行動");
                EnemyAct(obj);
                return;
            }
        }

        //全キャラ行動済みなら行動済みステータスをリセット
        AllCharaActionable();
    }

    private void EnemyAct(GameObject enemy)
    {
        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
        enemyAI.DecideAndExcuteAction();
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
            chara.IsFinishTurn = false;
        }
    }

    public void AllEnemyActionable()
    {
        foreach (GameObject enemy in ObjectManager.Instance.m_EnemyList)
        {
            Debug.Log("再起処理");
            CharaTurn chara = enemy.GetComponent<CharaTurn>();
            chara.IsFinishTurn = false;
        }
    }
}
