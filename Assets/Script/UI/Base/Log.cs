using System;

public abstract class LogInfo
{
    //選択肢Id
    public int OptionId
    {
        get;
        set;
    } = 0;

    //質問文
    public virtual string Question
    {
        get;
    }

    //選択肢の数
    public virtual int OptionNum
    {
        get;
        set;
    } = 0;

    //選択肢文
    public virtual string[] Option
    {
        get;
    }

    //選択肢のメソッド
    public virtual Action[] OptionMethod
    {
        get;
    }

    //実行用メソッド
    public void ExcuteMethod()
    {
        OptionMethod[OptionId]();
    }
} 
