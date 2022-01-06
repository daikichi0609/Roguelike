using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    //現在のゲームステート
    [SerializeField] private InternalDefine.GAME_STATE m_CurrentGameState;
    public InternalDefine.GAME_STATE CurrentGameState
    {
        get { return m_CurrentGameState; }
        set { m_CurrentGameState = value; }
    }

    //現在のダンジョンテーマ
    [SerializeField] private Define.DUNGEON_THEME m_DungeonTheme;
    public Define.DUNGEON_THEME DungeonTheme
    {
        get { return m_DungeonTheme; }
        set { m_DungeonTheme = value; }
    }

    //現在のダンジョン名
    public Define.DUNGEON_NAME DungeonName
    {
        get; set;
    }

    //現在のフロア階数
    [SerializeField] private int m_FloorNum = 1;
    public int FloorNum
    {
        get { return m_FloorNum; }
        set { m_FloorNum = value; }
    }

    //初回ダンジョン生成かどうか
    private bool IsFirstTime
    {
        get; set;
    } = true;

    //ダンジョン生成済みかどうか
    private bool IsFinishToBuildDungeon
    {
        get; set;
    } = false;

    //リーダーキャラの名前
    [SerializeField] private Define.CHARA_NAME m_LeaderName;
    public Define.CHARA_NAME LeaderName
    {
        get { return m_LeaderName; }
    }

    //初期化処理呼び出し
    private void Awake()
    {
        Initialize();
    }

    //初期化処理
    private void Initialize()
    {
        m_LeaderName = Define.CHARA_NAME.BOXMAN;

        SoundManager.Instance.BlueCrossBGM.Play();
        //SoundManager.Instance.BossBGM.Play();
        //SoundManager.Instance.KD.Play();

        DungeonTerrain.Instance.DeployDungeon();
        DungeonContents.Instance.DeployDungeonContents();
    }

    //次の階に行くときの初期化
    private void ReInitialize()
    {
        Debug.Log("再初期化");
        RemoveDungeon();
        RebuildDungeon();
        TurnManager.Instance.InitializeTurn();
    }

    //Update関数は基本的にここだけ
    private void Update()
    {
        //暗転・明転時に呼ばれる
        if(CurrentGameState == InternalDefine.GAME_STATE.LOADING)
        {
            switch(IsFinishToBuildDungeon)
            {
                case false:
                    IndicateNextFloorUi();
                    break;

                case true:
                    HideNextFloorUi();
                    break;
            }
            
            return;
        }

        TurnManager.Instance.UpdateTurn();
        InputManager.Instance.DetectInput();
        CharaUiManager.Instance.UpdateCharaUi();
    }

    //ダンジョン再構築
    private void RebuildDungeon()
    {
        Debug.Log("Rebuild");
        DungeonTerrain.Instance.DeployDungeon();
        DungeonContents.Instance.RedeployDungeonContents();
        TurnManager.Instance.InitializeTurn();
    }

    //ダンジョン破壊
    private void RemoveDungeon()
    {
        DungeonTerrain.Instance.RemoveDungeon();
        DungeonContents.Instance.RemoveDungeonContents();
    }

    //次の階にいくときの処理
    public void UpToNextFloor()
    {
        FloorNum++;
        CurrentGameState = InternalDefine.GAME_STATE.LOADING;
        IsFinishToBuildDungeon = false;
    }

    //暗転してダンジョン生成
    private void IndicateNextFloorUi()
    {
        BlackPanelManager.Instance.Indicate();
        if(BlackPanelManager.Instance.IsFinish == true)
        {
            IsFinishToBuildDungeon = true;

            //初回は既にダンジョン生成済みなので再初期化はスキップ
            if (IsFirstTime == true)
            {
                IsFirstTime = false;
                return;
            }
            
            ReInitialize();
        }
    }

    //明転
    private void HideNextFloorUi()
    {
        BlackPanelManager.Instance.Hide();
        if (BlackPanelManager.Instance.IsFinish == true)
        {
            CurrentGameState = InternalDefine.GAME_STATE.PLAYING;
        }
    }
}
