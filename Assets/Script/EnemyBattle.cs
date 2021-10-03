using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattle : CharaBattle
{
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        CharaMove = this.gameObject.GetComponent<CharaMove>();
        Condition = this.gameObject.GetComponent<CharaCondition>();
        BattleStatus = CharaDataManager.Instance.LoadEnemyScriptableObject(m_CharaName);
    }
}
