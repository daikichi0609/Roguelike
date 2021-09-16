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


	public void InitializePlayerData(PlayerStatus playerStatus)
	{
		playerStatus.Name = "Louge";

		playerStatus.Hp = 20;
		playerStatus.Atk = 10;
		playerStatus.Def = 5;
		playerStatus.Agi = 1;
		playerStatus.Dex = 1f;
		playerStatus.Eva = 1f;
		playerStatus.CriticalRate = 1f;
		playerStatus.Res = 0.1f;

		playerStatus.Luk = 0.1f;
	}
}