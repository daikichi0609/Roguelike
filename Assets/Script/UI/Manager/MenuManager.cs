using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 選択肢と説明のUi
/// </summary>

public class MenuManager : SingletonMonoBehaviour<MenuManager>
{
    //何のMenu表示中か示す
    public InternalDefine.MENU_STATE CurentKey
    {
        get;
        set;
    }

    [SerializeField]
    private Dictionary<InternalDefine.MENU_STATE, LogInfo> m_LogInfo = new Dictionary<InternalDefine.MENU_STATE, LogInfo>()
    {
        {InternalDefine.MENU_STATE.MENU, new StairsLog() }
    };
    public Dictionary<InternalDefine.MENU_STATE, LogInfo> LogInfo
    {
        get { return m_LogInfo; }
        set { m_LogInfo = value; }
    }

    public void Indicate()
    {
        UiHolder.Instance.MenuUi.SetActive(true);
    }
}