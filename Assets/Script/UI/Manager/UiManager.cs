using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.UI;

public abstract class UiManager : MonoBehaviour
{
    /// <summary>
    /// Ui表示中かどうか
    /// </summary>
    private ReactiveProperty<bool> m_IsActive = new ReactiveProperty<bool>(false);

    public IObservable<bool> IsActiveChanged
    {
        get { return m_IsActive.Skip(1); }
    }

    public bool IsActive
    {
        get => m_IsActive.Value;
        set => m_IsActive.Value = value;
    }

    /// <summary>
    /// 選択肢Id
    /// </summary>
    private ReactiveProperty<int> m_OptionId;

    public IObservable<int> GetOptionId
    {
        get => m_OptionId;
    }

    public int OptionId
    {
        get => m_OptionId.Value;
        set => m_OptionId.Value = value;
    }

    /// <summary>
    /// 選択肢の数
    /// </summary>
    protected virtual int OptionCount
    {
        get;
        set;
    }

    /// <summary>
    /// 選択肢のメソッド
    /// </summary>
    protected virtual List<Action> OptionMethods
    {
        get;
    }

    /// <summary>
    /// 操作するUi
    /// </summary>
    protected virtual GameObject Ui
    {
        get;
    }

    /// <summary>
    /// 操作するテキストUi
    /// </summary>
    protected virtual List<Text> Texts
    {
        get;
    }

    protected virtual void Awake()
    {
        GameManager.Instance.GetUpdate
            .Subscribe(_ => DetectInput());

        IsActiveChanged.Subscribe(_ => SwitchLog()).AddTo(this);

        GetOptionId.Subscribe(_ => UpdateText()).AddTo(this);
    }

    /// <summary>
    /// Ui表示・非表示操作
    /// </summary>
    public void SwitchLog()
    {
        Ui.SetActive(IsActive);

        if(IsActive == true)
        {
            UpdateText();
        }
    }

    /// <summary>
    /// 入力検知
    /// </summary>
    protected virtual void DetectInput()
    {
        //Ui表示中じゃないなら受け付けない
        if (IsActive == false)
        {
            return;
        }

        //決定ボタン 該当メソッド実行
        if (Input.GetKeyDown(KeyCode.Return))
        {
            IsActive = false;
            OptionMethods[OptionId]();
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

    /// <summary>
    /// Subscribeする 一回読み込み
    /// </summary>
    protected virtual void UpdateText()
    {
        //選択肢の文字色更新
        for (int i = 0; i <= OptionCount - 1; i++)
        {
            Texts[i].color = Color.white;
        }

        //選択中の文字色更新
        Texts[OptionId].color = Color.yellow;
    }
}
