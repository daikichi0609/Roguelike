using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.UI;

public class BagManager : SingletonMonoBehaviour<BagManager>
{
    private BagManager.BagUi m_Manager = new BagUi();
    public BagUi GetManager
    {
        get => m_Manager;
    }

    /// <summary>
    /// 参照しているバッグ
    /// </summary>
    private Bag m_Bag = new Bag();

    public Bag Bag
    {
        get => m_Bag;
        set => m_Bag = value;
    }

    /// <summary>
    /// バッグの要素数
    /// </summary>
    private int m_InventoryCount = 9;
    public int InventryCount
    {
        get => m_InventoryCount;
        set => m_InventoryCount = value;
    }

    protected override void Awake()
    {
        GameManager.Instance.GetUpdate
            .Subscribe(_ => GetManager.DetectInput());

        GetManager.IsActiveChanged.Subscribe(_ => GetManager.SwitchUi());

        GetManager.GetOptionId.Subscribe(_ => GetManager.UpdateText());
    }

    public class BagUi : UiBase
    {
        /// <summary>
        /// 選択肢の数
        /// </summary>
        protected override int OptionCount => BagManager.Instance.Bag.ItemList.Count;

        /// <summary>
        /// 選択肢のメソッド
        /// </summary>
        protected override List<Action> OptionMethods => new List<Action>
        {
            () => SelectItem(BagManager.Instance.Bag.ItemList[OptionId])
        };

        /// <summary>
        /// 操作するUi
        /// </summary>
        protected override GameObject Ui => UiHolder.Instance.BagUi;

        /// <summary>
        /// 操作するテキストUi
        /// </summary>
        protected override List<Text> Texts => UiHolder.Instance.ItemTexts;

        public override void DetectInput()
        {
            //Ui表示中じゃないなら受け付けない
            if (IsActive == false)
            {
                return;
            }

            if (InputManager.Instance.IsProhibitDuplicateInput == true)
            {
                InputManager.Instance.IsProhibitDuplicateInput = false;
                return;
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                CloseUi();
                return;
            }

            //ココが変更点
            //決定ボタン 該当メソッド実行
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SelectItem(BagManager.Instance.Bag.ItemList[OptionId]);
                return;
            }

            //上にカーソル移動
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (OptionId >= 1)
                {
                    OptionId--;
                }
            }

            //下にカーソル移動
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (OptionId <= Texts.Count - 2)
                {
                    OptionId++;
                }
            }
        }

        protected override void CloseUi()
        {
            base.CloseUi();
            MenuManager.Instance.GetManager.IsActive = true;
        }

        public void SelectItem(Item item)
        {

        }

        public void PutAway(GameObject item)
        {
            bool success = BagManager.Instance.Bag.PutAway(item);

            if (success == false)
            {

            }
        }
    }

}
