﻿using UnityEngine;
public abstract class BattleParameter: ScriptableObject //キャラ共通で必要なパラメタまとめ
{
	// 名前
	[SerializeField, Label("名前")] private string m_GivenName;
	public string Name
    {
		get { return m_GivenName; }
		set { m_GivenName = value; }
    }
	// 性別enum
	public enum GENDER
    {
		MALE,
		FEMALE,
		UNKNOWN
    }
	//性別
	[SerializeField, Label("性別")] private GENDER m_Gender;
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
		GOBLIN,
		GOD,
		UNKNOWN
    }
	//種族
	[SerializeField, Label("種族")] private RACE m_Race;
	public RACE Race
    {
		get { return m_Race; }
		set { m_Race = value; }
    }
	// ヒットポイント
	[SerializeField, Label("体力")] private int m_Hp;
	public int Hp
    {
		get { return m_Hp; }
		set { m_Hp = value; }
    }
	// 攻撃力
	[SerializeField, Label("攻撃力")] private int m_Atk;
	public int Atk
    {
		get { return m_Atk; }
		set { m_Atk = value; }
    }
	// 防御力
	[SerializeField, Label("防御力")] private int m_Def;
	public int Def
    {
		get { return m_Def; }
		set { m_Def = value; }
    }
	// 速さ
	[SerializeField, Label("速さ")] private int m_Agi;
	public int Agi
    {
		get { return m_Agi; }
		set { m_Agi = value; }
    }
	// 命中率補正
	[SerializeField, Label("命中率補正")] private float m_Dex;
	public float Dex
    {
		get { return m_Dex; }
		set { m_Dex = value; }
    }
	// 回避率補正
	[SerializeField, Label("回避率補正")] private float m_Eva;
	public float Eva
    {
        get { return m_Eva; }
		set { m_Eva = value; }
    }
	// 会心率補正
	[SerializeField, Label("会心率補正")] private float m_CriticalRate;
	public float CriticalRate
    {
        get { return m_CriticalRate; }
		set { m_CriticalRate = value; }
    }
	// 抵抗率
	[SerializeField, Label("抵抗率")] private float m_Res;
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

[CreateAssetMenu(menuName = "MyScriptable/Create PlayerData")]
[System.Serializable] //定義したクラスをJSONデータに変換できるようにする
public class PlayerData: BattleParameter
{
	//運
	[SerializeField, Label("運")] private float m_Luk;
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
	[SerializeField, Label("倒すともらえる経験値")] private int m_Ex;
	public int Ex
    {
		get { return m_Ex; }
		set { m_Ex = value; }
    }
}