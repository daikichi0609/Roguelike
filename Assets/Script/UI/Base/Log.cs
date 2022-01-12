using System;
using System.Collections.Generic;

public abstract class LogInfo
{
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
    public virtual List<string> Option
    {
        get;
    }

    //選択肢のメソッド
    public virtual List<Action> OptionMethod
    {
        get;
    }
} 