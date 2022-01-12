public static class Define
{
	// キャラ名
	public enum CHARA_NAME
	{
		BOXMAN,
		RAGON,
		ICHIGO,
		WIZARD,
		ARCHER,

		MASHROOM
	}

	// アイテム名
	public enum ITEM_NAME
    {
		APPLE
    }

	//ダンジョンテーマ
	public enum DUNGEON_THEME
	{
		GRASS,
		ROCK,
		CRYSTAL,
		WHITE
	}

	//ダンジョン名
	public enum DUNGEON_NAME
	{
		始まりの森,
		岩場,
		クリスタル,
		白
	}
}

public static class InternalDefine
{
	//ゲーム全体のステート
	public enum GAME_STATE
	{
		LOADING,
		PLAYING
	}
}

public static class Message
{
	/// <summary>
    /// 暗転・明転のリクエスト用
    /// </summary>
	public struct MRequestBlackPanel
	{
		public bool IsDark
		{
			get;
			set;
		}
	}

	/// <summary>
    /// 明転・暗転終了通知
    /// </summary>
	public struct MFinishBlackPanel
    {
		public bool IsDark
        {
			get; set;
        }
    }

	/// <summary>
    /// FloorText表示処理終了通知
    /// 実質ダンジョン再構築完了通知
    /// </summary>
	public struct MFinishFloorText
    {

    }

	/// <summary>
    /// ターン終了通知
    /// </summary>
	public struct MFinishTurn
    {
		
    }

	/// <summary>
    /// プレイヤーのListに変更があった通知
    /// </summary>
	public struct MChangedPlayerList
    {

    }
}