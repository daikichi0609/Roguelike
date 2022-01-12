using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.UI;

public class BagManager : SingletonMonoBehaviour<BagManager>
{
    private BagManager.Manager m_Manager = new Manager();
    public Manager GetManager
    {
        get => m_Manager;
    }

    public class Manager : UiManager
    {
        /// <summary>
        /// 参照しているバッグ
        /// </summary>
        private Bag m_Bag;

        public Bag Bag
        {
            get => m_Bag;
            set => m_Bag = value;
        }

        /// <summary>
        /// 選択肢の数
        /// </summary>
        protected override int OptionCount => Bag.ItemList.Count;

        /// <summary>
        /// 選択肢のメソッド
        /// </summary>
        protected override List<Action> OptionMethods => new List<Action>
        {
            () => SelectItem(Bag.ItemList[OptionId])
        };

        /// <summary>
        /// 操作するUi
        /// </summary>
        protected override GameObject Ui => UiHolder.Instance.BagUi;

        /// <summary>
        /// 操作するテキストUi
        /// </summary>
        protected override List<Text> Texts => UiHolder.Instance.ItemTexts;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void DetectInput()
        {
            //Ui表示中じゃないなら受け付けない
            if (IsActive == false)
            {
                return;
            }

            //決定ボタン 該当メソッド実行
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SelectItem(Bag.ItemList[OptionId]);
                return;
            }

            //上にカーソル移動
            if (Input.GetKeyDown(KeyCode.W))
            {
                OptionId--;
                if (OptionId < 0)
                {
                    OptionId = 0;
                }
            }

            //下にカーソル移動
            if (Input.GetKeyDown(KeyCode.S))
            {
                OptionId++;
                if (OptionId > OptionCount)
                {
                    OptionId = OptionCount;
                }
            }
        }

        public void SelectItem(Item item)
        {

        }

        public void PutAway(GameObject item)
        {
            bool success = Bag.PutAway(item);

            if (success == false)
            {

            }
        }
    }

}
