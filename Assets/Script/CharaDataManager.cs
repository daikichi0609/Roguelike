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
	public void SaveTest(PlayerData data)
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


	public void InitializePlayerData(PlayerData playerData)
	{
		playerData.Name = "Louge";
		playerData.Gender = BattleParameter.GENDER.MALE;
		playerData.Race = BattleParameter.RACE.HUMAN;

		playerData.Lv = 1;
		playerData.Hp = 20;
		playerData.Atk = 10;
		playerData.Def = 5;
		playerData.Agi = 1;
		playerData.Dex = 1f;
		playerData.Eva = 1f;
		playerData.CriticalRate = 1f;
		playerData.Res = 0.1f;

		playerData.Ex = 0;
		playerData.Satiety = 100f;
		playerData.Luk = 0.1f;
	}
}