using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaDataHolder : SingletonMonoBehaviour<CharaDataHolder>
{
    [SerializeField, Label("ボックスマン")] private BattleStatus m_BoxmanStatus;
    public BattleStatus BoxmanStatus
    {
        get { return m_BoxmanStatus; }
    }

    [SerializeField, Label("マッシュルーム")] private BattleStatus m_MashroomStatus;
    public BattleStatus MashroomStatus
    {
        get { return m_MashroomStatus; }
    }
}
