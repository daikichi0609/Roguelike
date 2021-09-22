using UnityEngine;
using System.IO;

public class CharaDataManager : SingletonMonoBehaviour<CharaDataManager>
{
	private string m_Datapath;
	public string Datapath
    {
        get { return m_Datapath; }
    }

	private void Awake()
	{
		//初めに保存先を計算する　Application.dataPathで今開いているUnityプロジェクトのAssetsフォルダ直下を指定して、後ろに保存名を書く
		m_Datapath = Application.dataPath + "/Resources/TestJson.json";
	}

	//セーブのメソッド
	public void SaveTest(PlayerStatus data)
	{
		string jsonstr = JsonUtility.ToJson(data);//受け取ったPlayerDataをJSONに変換
		StreamWriter writer = new StreamWriter(m_Datapath, false);//初めに指定したデータの保存先を開く
		writer.WriteLine(jsonstr);//JSONデータを書き込み
		writer.Flush();//バッファをクリアする
		writer.Close();//ファイルをクローズする
	}

    public string LoadTest(string dataPath)
	{
		StreamReader reader = new StreamReader(dataPath); //受け取ったパスのファイルを読み込む
		string datastr = reader.ReadToEnd();//ファイルの中身をすべて読み込む
		reader.Close();//ファイルを閉じる

		return datastr;//読み込んだJSONファイルをstring型に変換して返す
	}

	public PlayerStatus LoadPlayerScriptableObject(BattleStatus.NAME name)
	{
		PlayerStatus status = new PlayerStatus();
		PlayerStatus constStatus = ChoosePlayerStatus(name) as PlayerStatus;

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

	public EnemyStatus LoadEnemyScriptableObject(BattleStatus.NAME name)
	{
		EnemyStatus status = new EnemyStatus();
		EnemyStatus constStatus = ChoosePlayerStatus(name) as EnemyStatus;

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

	public BattleStatus ChoosePlayerStatus(BattleStatus.NAME name)
    {
		switch(name)
        {
			case BattleStatus.NAME.BOXMAN:
				return CharaDataHolder.Instance.BoxmanStatus;

			case BattleStatus.NAME.MASHROOM:
				return CharaDataHolder.Instance.MashroomStatus;
        }
		return null;
    }
}