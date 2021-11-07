using System;

public class StairsLog : LogInfo
{
    public override string Question
    {
        get => "先に進みますか？";
    }

    public override int OptionNum
    {
        get => 2;
        set => base.OptionNum = value;
    }

    public override string[] Option
    {
        get => new string[2] { "はい", "いいえ" };
    }

    public override Action[] OptionMethod
    {
        get => new Action[2] { () => Yes(), () => No() };
    }

    private void Yes()
    {
        UiManager.Instance.ControlLogUi(UiManager.LOG_KEY.STAIRS, false);
        GameManager.Instance.UpToNextFloor();
    }

    private void No()
    {
        UiManager.Instance.ControlLogUi(UiManager.LOG_KEY.STAIRS, false);
        TurnManager.Instance.CurrentState = TurnManager.STATE.NONE;
    }
}
