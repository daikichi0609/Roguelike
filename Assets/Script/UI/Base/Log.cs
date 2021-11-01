using System;

public abstract class LogInfo
{
    public int OptionId
    {
        get;
        set;
    } = 0;

    public virtual string Question
    {
        get;
    }

    public virtual int OptionNum
    {
        get;
        set;
    } = 0;

    public virtual string[] Option
    {
        get;
    }

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
