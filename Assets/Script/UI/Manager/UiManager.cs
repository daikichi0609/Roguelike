using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : SingletonMonoBehaviour<UiManager>
{
    //各キャラUI格納用List
    [SerializeField] private List<GameObject> m_CharacterUiList = new List<GameObject>();
    public List<GameObject> CharacterUiList
    {
        get { return m_CharacterUiList; }
        set { m_CharacterUiList = value; }
    }

    public enum LOG_KEY
    {
        STAIRS
    }

    public LOG_KEY CurentKey
    {
        get;
        set;
    }

    [SerializeField] private Dictionary<LOG_KEY, LogInfo> m_LogInfo = new Dictionary<LOG_KEY, LogInfo>()
    {
        {LOG_KEY.STAIRS, new StairsLog() }
    };
    public Dictionary<LOG_KEY, LogInfo> LogInfo
    {
        get { return m_LogInfo; }
        set { m_LogInfo = value; }
    }

    public void GenerateCharacterUi(GameObject player)
    {
        GameObject ui = Instantiate(UiHolder.Instance.CharacterUi);
        ui.transform.SetParent(UiHolder.Instance.Canvas.transform, false);
        CharacterUiList.Add(ui);
        CharaUi charaUi = ui.GetComponent<CharaUi>();
        charaUi.Initialize(player);
    }

    public void UpdateCharaUi()
    {
        foreach(GameObject chara in CharacterUiList)
        {
            chara.GetComponent<CharaUi>().UpdateUi();
        }
    }

    public void ControlLogUi(LOG_KEY key, bool _switch)
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
        CurentKey = key;
        UpdateLog(key);
    }

    public void DetectInput()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            LogInfo[CurentKey].ExcuteMethod();
            return;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            LogInfo[CurentKey].OptionId--;
            if(LogInfo[CurentKey].OptionId < 0)
            {
                LogInfo[CurentKey].OptionId = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            LogInfo[CurentKey].OptionId++;
            if (LogInfo[CurentKey].OptionId > LogInfo[CurentKey].OptionNum)
            {
                LogInfo[CurentKey].OptionId = LogInfo[CurentKey].OptionNum;
            }
        }
        UpdateLog(CurentKey);
    }

    public void UpdateLog(LOG_KEY key)
    {
        UiHolder.Instance.QuestionText.text = LogInfo[key].Question;
        for(int i = 0; i <= UiHolder.Instance.OptionTextList.Count - 1; i++)
        {
            UiHolder.Instance.OptionTextList[i].color = Color.white;
            UiHolder.Instance.OptionTextList[i].text = LogInfo[key].Option[i];
        }
        UiHolder.Instance.OptionTextList[LogInfo[key].OptionId].color = Color.yellow;
    }
}