using UnityEngine;
using UnityEngine.UI;

public class BlackPanelManager : SingletonMonoBehaviour<BlackPanelManager>
{
    //暗転スピード
    private float BlackPanelSpeed
    {
        get;
    } = 0.1f;

    private float TextSpeed
    {
        get;
    } = 0.05f;

    //暗転用パネルの透明度
    private float PanelAlfa
    {
        get; set;
    } = 1f;

    //テキストの透明度
    private float TextAlfa
    {
        get; set;
    } = 0f;

    //暗転・明転完了フラグ
    public bool IsFinish
    {
        get; set;
    } = false;

    public void Indicate()
    {
        IsFinish = false;
        UiHolder.Instance.DungeonNameText.text = GameManager.Instance.DungeonName.ToString();
        UiHolder.Instance.FloorNumText.text = GameManager.Instance.FloorNum.ToString() + "F";

        UiHolder.Instance.BlackPanel.GetComponent<Image>().color = new Color(0, 0, 0, PanelAlfa);
        PanelAlfa += BlackPanelSpeed;
        if (PanelAlfa >= 1f)
        {
            UiHolder.Instance.DungeonNameText.color = new Color(255, 255, 255, TextAlfa);
            UiHolder.Instance.FloorNumText.color = new Color(255, 255, 255, TextAlfa);
            TextAlfa += TextSpeed;
            if (TextAlfa >= 1f)
            {
                IsFinish = true;
            }
        }
        CheckValue();
    }

    public void Hide()
    {
        IsFinish = false;
        UiHolder.Instance.DungeonNameText.color = new Color(255, 255, 255, TextAlfa);
        UiHolder.Instance.FloorNumText.color = new Color(255, 255, 255, TextAlfa);
        TextAlfa -= TextSpeed;
        if (TextAlfa <= 0)
        {
            UiHolder.Instance.BlackPanel.GetComponent<Image>().color = new Color(0, 0, 0, PanelAlfa);
            PanelAlfa -= BlackPanelSpeed;
            if (PanelAlfa <= 0)
            {
                IsFinish = true;
            }
        }
        CheckValue();
    }

    //色補正
    private void CheckValue()
    {
        if(PanelAlfa > 1f)
        {
            PanelAlfa = 1f;
        }

        if(PanelAlfa < 0f)
        {
            PanelAlfa = 0f;
        }

        if(TextAlfa > 1f)
        {
            TextAlfa = 1f;
        }

        if(TextAlfa < 0f)
        {
            TextAlfa = 0f;
        }
    }

    public void UiUpdate()
    {

    }
}
