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

	//Logのステート
	public enum LOG_STATE
	{
		STAIRS,
	}

	//Menuのステート
	public enum MENU_STATE
    {
		MENU,
		BAG
    }
}
