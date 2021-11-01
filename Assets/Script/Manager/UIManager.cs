using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [SerializeField] private GameObject m_CharacterUi;
    public GameObject CharacterUi
    {
        get { return m_CharacterUi; }
        set { m_CharacterUi = value; }
    }

    [SerializeField] private List<GameObject> m_CharacterUiList = new List<GameObject>();
    public List<GameObject> CharacterUiList
    {
        get { return m_CharacterUiList; }
        set { m_CharacterUiList = value; }
    }

    [SerializeField] private GameObject m_Canvas;
    public GameObject Canvas
    {
        get { return m_Canvas; }
        set { m_Canvas = value; }
    }

    [SerializeField] private GameObject m_QuestionAndChoiceUi;
    public GameObject QuestionAndChoiceUi
    {
        get { return m_QuestionAndChoiceUi; }
        set { m_QuestionAndChoiceUi = value; }
    }

    [SerializeField] private Text m_QuestionText;
    public Text QuestionText
    {
        get { return m_QuestionText; }
        set { m_QuestionText = value; }
    }

    [SerializeField] private List<Text> m_OptionTextList = new List<Text>();
    public List<Text> OptionTextList
    {
        get { return m_OptionTextList; }
        set { m_OptionTextList = value; }
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
        GameObject ui = Instantiate(CharacterUi);
        ui.transform.SetParent(this.Canvas.transform, false);
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
        QuestionAndChoiceUi.SetActive(_switch);
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
        Debug.Log(LogInfo[CurentKey].OptionId);
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
        QuestionText.text = LogInfo[key].Question;
        for(int i = 0; i <= OptionTextList.Count - 1; i++)
        {
            OptionTextList[i].color = Color.white;
            OptionTextList[i].text = LogInfo[key].Option[i];
        }
        OptionTextList[LogInfo[key].OptionId].color = Color.yellow;
    }

}
