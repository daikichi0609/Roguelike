using UnityEngine;
using UniRx;

public abstract class Chara : MonoBehaviour
{
	/// <summary>
    /// 実際に動かすオブジェクト
    /// </summary>
	[SerializeField] private GameObject m_MoveObject;
	protected GameObject MoveObject
	{
		get { return m_MoveObject; }
	}

	/// <summary>
    /// キャラのオブジェクト
    /// </summary>
	[SerializeField] private GameObject m_CharaObject;
	protected GameObject CharaObject
	{
		get { return m_CharaObject; }
	}

	protected CharaTurn CharaTurn
    {
		get;
		set;
    }

	/// <summary>
    /// 位置
    /// </summary>
	public Vector3 Position
	{
		get;
		set;
	}

	/// <summary>
    /// 向いている方向
    /// </summary>
	public Vector3 Direction
	{
		get;
		set;
	}

	/// <summary>
    /// キャラが持つアニメーター
    /// </summary>
	protected Animator m_CharaAnimator;
	public Animator CharaAnimator
	{
		get { return m_CharaAnimator; }
		protected set { m_CharaAnimator = value; }
	}

	/// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="inventoryCount"></param>
	public virtual void Initialize(int inventoryCount)
	{
		CharaAnimator = CharaObject.GetComponent<Animator>();
		Direction = new Vector3(0, 0, -1);
		Position = MoveObject.transform.position;
		CharaTurn = GetComponent<CharaTurn>();
	}
}

public class CharaMove: Chara
{
	/// <summary>
    /// 移動目標座標
    /// </summary>
	public Vector3 DestinationPos
    {
		get;
		private set;
    }

	/// <summary>
    /// 移動中かどうか
    /// </summary>
	private bool IsMoving
    {
		get;
		set;
    }

    public override void Initialize(int inventoryCount)
    {
        base.Initialize(inventoryCount);

		GameManager.Instance.GetUpdate.Subscribe(_ => Moving());
    }

	/// <summary>
    /// 向きを変える
    /// </summary>
    /// <param name="direction"></param>
	public void Face(Vector3 direction)
	{
		Direction = direction;
		CharaObject.transform.rotation = Quaternion.LookRotation(direction);
	}

	/// <summary>
    /// 移動
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
	public bool Move(Vector3 direction)
	{
		Face(direction);

		if (Positional.IsPossibleToMoveGrid(Position, direction) == false)
		{
			return false;
		}
		Vector3 destinationPos = Position + direction;
		if (Positional.IsEnemyOn(destinationPos) == true)
		{
			return false;
		}

		//移動開始
		CharaAnimator.SetBool("IsRunning", true);
		IsMoving = true;
		TurnManager.Instance.IsCanAttack = false;

		//目標座標設定
		DestinationPos = destinationPos;

		//内部的には先に移動しとく
		Position = DestinationPos;

		//移動終わる前に現在マスチェックを済ませる
		CheckCurrentGrid();

		//ターンを返す
		FinishTurn();

		return true;
	}

	/// <summary>
    /// 移動中処理
    /// </summary>
	private void Moving()
	{
		if(IsMoving == false)
        {
			return;
        }

		TurnManager.Instance.IsCanAttack = false;
		MoveObject.transform.position = Vector3.MoveTowards(MoveObject.transform.position, DestinationPos, Time.deltaTime * 3);

		if ((MoveObject.transform.position - DestinationPos).magnitude <= 0.01f)
		{
			FinishMove();
		}
	}

	/// <summary>
    /// 移動終わり
    /// </summary>
	private void FinishMove()
	{
		Debug.Log("移動終了");
		TurnManager.Instance.IsCanAttack = true;
		IsMoving = false;
		CharaAnimator.SetBool("IsRunning", false);
		MoveObject.transform.position = Position;
	}

	/// <summary>
    /// 現在地マスのイベント処理
    /// </summary>
	public void CheckCurrentGrid()
    {
		//メインプレイヤーなら
		if (this.gameObject == ObjectManager.Instance.m_PlayerList[0])
		{
			//階段チェック
			if (DungeonTerrain.Instance.GridID((int)Position.x, (int)Position.z) == (int)DungeonTerrain.GRID_ID.STAIRS)
			{
				LogManager.Instance.GetManager.Log = new StairsLog();
				LogManager.Instance.GetManager.IsActive = true;
				return;
			}

			//アイテムチェック
			foreach(GameObject obj in ObjectManager.Instance.ItemList)
            {
				Vector3 pos = obj.GetComponent<Item>().Position;

				if(pos.x == Position.x && pos.z == Position.z)
                {
					BagManager.Instance.GetManager.PutAway(obj);
                }
				return;
            }

			//罠チェック

		}
    }

	/// <summary>
    /// 行動済み状態になる
    /// </summary>
	public void FinishTurn()
    {
		Debug.Log("ターン終了");
		CharaTurn.FinishTurn();
		MessageBroker.Default.Publish(new Message.MFinishTurn());
    }
}