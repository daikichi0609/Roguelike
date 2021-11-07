using UnityEngine;
using System.IO;
using UnityEditor;

public static class CharaDataManager
{
	private static string m_Datapath = Application.dataPath + "/Resources/TestJson.json";
	public static string Datapath
    {
        get { return m_Datapath; }
    } 

	//セーブのメソッド
	public static void SaveTest(PlayerStatus data)
	{
		string jsonstr = JsonUtility.ToJson(data);//受け取ったPlayerDataをJSONに変換
		StreamWriter writer = new StreamWriter(m_Datapath, false);//初めに指定したデータの保存先を開く
		writer.WriteLine(jsonstr);//JSONデータを書き込み
		writer.Flush();//バッファをクリアする
		writer.Close();//ファイルをクローズする
	}

    public static string LoadTest(string dataPath)
	{
		StreamReader reader = new StreamReader(dataPath); //受け取ったパスのファイルを読み込む
		string datastr = reader.ReadToEnd();//ファイルの中身をすべて読み込む
		reader.Close();//ファイルを閉じる

		return datastr;//読み込んだJSONファイルをstring型に変換して返す
	}

	public static PlayerStatus LoadPlayerScriptableObject(BattleStatus.NAME name)
	{
		PlayerStatus status = ScriptableObject.CreateInstance<PlayerStatus>();
		PlayerStatus constStatus = LoadCharaStatus(name) as PlayerStatus;

		status.Name = constStatus.Name;
		status.Hp = constStatus.Hp;
		status.Atk = constStatus.Atk;
		status.Def = constStatus.Def;
		status.Agi = constStatus.Agi;
		status.Dex = constStatus.Dex;
		status.Eva = constStatus.Eva;
		status.CriticalRate = constStatus.CriticalRate;
		status.Res = constStatus.Res;

		status.Luk = constStatus.Luk;

		return status;
    }

	public static EnemyStatus LoadEnemyScriptableObject(BattleStatus.NAME name)
	{
		EnemyStatus status = ScriptableObject.CreateInstance<EnemyStatus>();
		EnemyStatus constStatus = LoadCharaStatus(name) as EnemyStatus;

		status.Name = constStatus.Name;
		status.Hp = constStatus.Hp;
		status.Atk = constStatus.Atk;
		status.Def = constStatus.Def;
		status.Agi = constStatus.Agi;
		status.Dex = constStatus.Dex;
		status.Eva = constStatus.Eva;
		status.CriticalRate = constStatus.CriticalRate;
		status.Res = constStatus.Res;

		status.Ex = constStatus.Ex;

		return status;
	}

	public static void SaveScriptableObject(BattleStatus status)
    {
		EditorUtility.SetDirty(status);
		AssetDatabase.SaveAssets();
	}

	public static BattleStatus LoadCharaStatus(BattleStatus.NAME name)
    {
		return Resources.Load<BattleStatus>(name.ToString());
    }
}