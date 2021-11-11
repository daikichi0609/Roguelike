using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiHolder : SingletonMonoBehaviour<UiHolder>
{
    //キャンバス
    [SerializeField] private GameObject m_Canvas;
    public GameObject Canvas
    {
        get { return m_Canvas; }
    }

    //暗転用パネル
    [SerializeField] private GameObject m_BlackPanel;
    public GameObject BlackPanel
    {
        get { return m_BlackPanel; }
    }

    [SerializeField] private Text m_DungeonNameText;
    public Text DungeonNameText
    {
        get { return m_DungeonNameText; }
    }

    [SerializeField] private Text m_FloorNumText;
    public Text FloorNumText
    {
        get { return m_FloorNumText; }
    }

    //キャラクターUIプレハブ
    [SerializeField] private GameObject m_CharacterUi;
    public GameObject CharacterUi
    {
        get { return m_CharacterUi; }
    }

    //質問文と選択肢だけの汎用UI（シーンに存在）
    [SerializeField] private GameObject m_QuestionAndChoiceUi;
    public GameObject QuestionAndChoiceUi
    {
        get { return m_QuestionAndChoiceUi; }
    }

    //質問文
    [SerializeField] private Text m_QuestionText;
    public Text QuestionText
    {
        get { return m_QuestionText; }
    }

    //選択肢の文格納List
    [SerializeField] private List<Text> m_OptionTextList;
    public List<Text> OptionTextList
    {
        get { return m_OptionTextList; }
        set { m_OptionTextList = value; }
    }
}
