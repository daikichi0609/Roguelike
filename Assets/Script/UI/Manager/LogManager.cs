using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 選択肢と質問文のUi
/// </summary>

public class LogManager : SingletonMonoBehaviour<LogManager>
{
    //何のLog表示中か示す
    public InternalDefine.LOG_STATE CurentState
    {
        get;
        set;
    }

    [SerializeField] private Dictionary<InternalDefine.LOG_STATE, LogInfo> m_LogInfo = new Dictionary<InternalDefine.LOG_STATE, LogInfo>()
    {
        {InternalDefine.LOG_STATE.STAIRS, new StairsLog() }
    };
    public Dictionary<InternalDefine.LOG_STATE, LogInfo> LogInfo
    {
        get { return m_LogInfo; }
        set { m_LogInfo = value; }
    }

    /// <summary>
    /// Ui表示と非表示
    /// </summary>
    /// <param name="state">どのUiを操作するか</param>
    /// <param name="_switch">オンオフフラグ</param>
    
    public void ControlLogUi(InternalDefine.LOG_STATE state, bool _switch)
    {
        if(_switch == true)
        {
            TurnManager.Instance.CurrentState = TurnManager.STATE.UI_POPUPING;
        }
        else if(_switch == false)
        {
            TurnManager.Instance.CurrentState = TurnManager.STATE.NONE;
        }
        UiHolder.Instance.QuestionAndChoiceUi.SetActive(_switch);
        CurentState = state;
        UpdateLog(state);
    }

    public void DetectInput()
    {
        //決定ボタン 該当メソッド実行
        if (Input.GetKeyDown(KeyCode.Return))
        {
            LogInfo[CurentState].ExcuteMethod();
            return;
        }

        //上にカーソル移動
        if (Input.GetKeyDown(KeyCode.W))
        {
            LogInfo[CurentState].OptionId--;
            if(LogInfo[CurentState].OptionId < 0)
            {
                LogInfo[CurentState].OptionId = 0;
            }
        }

        //下にカーソル移動
        if (Input.GetKeyDown(KeyCode.S))
        {
            LogInfo[CurentState].OptionId++;
            if (LogInfo[CurentState].OptionId > LogInfo[CurentState].OptionNum)
            {
                LogInfo[CurentState].OptionId = LogInfo[CurentState].OptionNum;
            }
        }

        UpdateLog(CurentState);
    }

    public void UpdateLog(InternalDefine.LOG_STATE key)
    {
        //質問文の更新
        UiHolder.Instance.QuestionText.text = LogInfo[key].Question;

        //選択肢の文字色更新
        for(int i = 0; i <= UiHolder.Instance.OptionTextList.Count - 1; i++)
        {
            UiHolder.Instance.OptionTextList[i].color = Color.white;
            UiHolder.Instance.OptionTextList[i].text = LogInfo[key].Option[i];
        }

        //選択中の文字色更新
        UiHolder.Instance.OptionTextList[LogInfo[key].OptionId].color = Color.yellow;
    }
}