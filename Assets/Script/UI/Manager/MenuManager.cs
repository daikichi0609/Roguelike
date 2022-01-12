using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.UI;

/// <summary>
/// 左上メニューと説明のUi
/// </summary>

public class MenuManager : SingletonMonoBehaviour<MenuManager>
{
    private MenuManager.Manager m_Manager = new Manager();
    public Manager GetManager
    {
        get => m_Manager;
    }

    public class Manager : UiManager
    {
        /// <summary>
        /// 選択肢の数
        /// </summary>
        protected override int OptionCount => 2;

        /// <summary>
        /// 選択肢のメソッド
        /// </summary>
        protected override List<Action> OptionMethods => new List<Action>
        {
            () => OpenBag(),
            () => CheckStatus(),
        };

        /// <summary>
        /// 操作するUi
        /// </summary>
        protected override GameObject Ui => UiHolder.Instance.MenuUi;

        /// <summary>
        /// 操作するテキストUi
        /// </summary>
        protected override List<Text> Texts => UiHolder.Instance.MenuText;


        protected override void Awake()
        {
            base.Awake();
        }

        /// <summary>
        /// バッグを開く
        /// </summary>
        private void OpenBag()
        {
            BagManager.Instance.GetManager.IsActive = true;
        }

        private void CheckStatus()
        {

        }
    }
}