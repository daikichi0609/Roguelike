using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

/// <summary>
/// 選択肢と質問文のUi
/// </summary>

public class LogManager :SingletonMonoBehaviour<LogManager>
{
    private LogManager.Manager m_Manager = new Manager();
    public Manager GetManager
    {
        get => m_Manager;
    }

    public class Manager : UiManager
    {
        /// <summary>
        /// Logテキスト情報
        /// </summary>
        private ReactiveProperty<LogInfo> m_Log = new ReactiveProperty<LogInfo>();

        public IObservable<LogInfo> LogChanged
        {
            get { return m_Log.Skip(1); }
        }

        public LogInfo Log
        {
            private get => m_Log.Value;
            set => m_Log.Value = value;
        }

        /// <summary>
        /// 選択肢の数
        /// </summary>
        protected override int OptionCount => Log.OptionNum;

        /// <summary>
        /// 選択肢のメソッド
        /// </summary>
        protected override List<Action> OptionMethods => Log.OptionMethod;

        /// <summary>
        /// 操作するUi
        /// </summary>
        protected override GameObject Ui => UiHolder.Instance.QuestionAndChoiceUi;

        /// <summary>
        /// 操作するテキストUi
        /// </summary>
        protected override List<Text> Texts => UiHolder.Instance.OptionTextList;

        protected override void Awake()
        {
            base.Awake();
        }

        /// <summary>
        /// Subscribeする 一回読み込み
        /// </summary>
        protected override void UpdateText()
        {
            base.UpdateText();

            //質問文の更新
            UiHolder.Instance.QuestionText.text = Log.Question;
        }
    }


}