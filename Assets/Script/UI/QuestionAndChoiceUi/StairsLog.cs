﻿using System;
using System.Collections.Generic;

public class StairsLog : LogInfo
{
    public override string Question
    {
        get => "先に進みますか？";
    }

    public override int OptionNum
    {
        get => 2;
        set => OptionNum = value;
    }

    public override List<string> Option
    {
        get => new List<string> { "はい", "いいえ" };
    }

    public override List<Action> OptionMethod
    {
        get => new List<Action> { () => Yes(), () => No() };
    }

    private void Yes()
    {
        GameManager.Instance.UpToNextFloor();
    }

    private void No()
    {
        
    }
}
