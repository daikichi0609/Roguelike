using UnityEngine;
public abstract class BattleParameter: ScriptableObject //キャラ共通で必要なパラメタまとめ
{
	// 名前
	[SerializeField] private string m_Name;
	public string Name
    {
		get { return m_Name; }
		set { m_Name = value; }
    }

	// 性別enum
	public enum GENDER
    {
		MALE,
		FEMALE,
		UNKNOWN
    }
	//性別
	[SerializeField] private GENDER m_Gender;
	public GENDER Gender
    {
		get { return m_Gender; }
		set { m_Gender = value; }
    }

	// 種族enum
	public enum RACE
    {
		HUMAN,
		ELF,
		GOBLIN
    }
	//種族
	[SerializeField] private RACE m_Race;
	public RACE Race
    {
		get { return m_Race; }
		set { m_Race = value; }
    }

	//レベル
	[SerializeField] private int m_Lv;
	public int Lv
    {
        get { return m_Lv; }
		set { m_Lv = value; }
    }
	// ヒットポイント
	[SerializeField] private int m_Hp;
	public int Hp
    {
		get { return m_Hp; }
		set { m_Hp = value; }
    }
	// 攻撃力
	[SerializeField] private int m_Atk;
	public int Atk
    {
		get { return m_Atk; }
		set { m_Atk = value; }
    }
	// 防御力
	[SerializeField] private int m_Def;
	public int Def
    {
		get { return m_Def; }
		set { m_Def = value; }
    }
	// 速さ
	[SerializeField] private int m_Agi;
	public int Agi
    {
		get { return m_Agi; }
		set { m_Agi = value; }
    }
	// 命中率補正
	[SerializeField] private float m_Dex;
	public float Dex
    {
		get { return m_Dex; }
		set { m_Dex = value; }
    }
	// 回避率補正
	[SerializeField] private float m_Eva;
	public float Eva
    {
        get { return m_Eva; }
		set { m_Eva = value; }
    }
	// 会心率補正
	[SerializeField] private float m_CriticalRate;
	public float CriticalRate
    {
        get { return m_CriticalRate; }
		set { m_CriticalRate = value; }
    }
	// 抵抗率
	[SerializeField] private float m_Res;
	public float Res
    {
		get { return m_Res; }
		set { m_Res = value; }
    }
}

interface ICommand //キャラ共通で必要なメソッドまとめ
{
	// 攻撃
	void OnAttack();
	//移動
	void Move(Vector3 vector3);
}

[System.Serializable] //定義したクラスをJSONデータに変換できるようにする
public class PlayerData: BattleParameter
{
	//所持している経験値
	[SerializeField] private int m_Ex;
	public int Ex
    {
		get { return m_Ex; }
        set { m_Ex = value; }
    }
	//満腹度
	[SerializeField] private float m_Satiety;
	public float Satiety
    {
        get { return m_Satiety; }
		set { m_Satiety = value; }
    }
	//運
	[SerializeField] private float m_Luk;
	public float Luk
    {
		get { return m_Luk; }
		set { m_Luk = value; }
    }
}

[CreateAssetMenu(menuName = "MyScriptable/Create EnemyData")]
public class EnemyData : BattleParameter
{
	//倒されるともらえる経験値
	[SerializeField] private int m_Ex;
	public int Ex
    {
		get { return m_Ex; }
		set { m_Ex = value; }
    }
}