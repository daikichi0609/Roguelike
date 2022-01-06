﻿using UnityEngine;

public abstract class Chara : MonoBehaviour
{
	public bool Turn
	{
		get;
		set;
	}

	public bool IsActing
    {
		get;
		set;
    }

	[SerializeField] private GameObject m_MoveObject;
	protected GameObject MoveObject
	{
		get { return m_MoveObject; }
	}

	[SerializeField] private GameObject m_CharaObject;
	protected GameObject CharaObject
	{
		get { return m_CharaObject; }
	}

	public Vector3 Position
	{
		get;
		set;
	}

	public Vector3 Direction
	{
		get;
		set;
	}

	public Bag Bag
	{
		get;
		set;
	}

	protected Animator m_CharaAnimator;
	public Animator CharaAnimator
	{
		get { return m_CharaAnimator; }
		protected set { m_CharaAnimator = value; }
	}

	public void Initialize(int inventoryCount)
	{
		Bag = new Bag(inventoryCount);
		CharaAnimator = CharaObject.GetComponent<Animator>();
		Direction = new Vector3(0, 0, -1);
		Position = MoveObject.transform.position;
	}

	public void FinishTurn()
    {
		Turn = false;
    }
}

public class CharaMove: Chara
{
	public Vector3 DestinationPos
    {
		get;
		private set;
    }

	public bool IsMoving
    {
		get;
	    set;
    }

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

		DestinationPos = Position + direction;
		Position = DestinationPos;
		IsMoving = true;
		IsActing = true;
		CharaAnimator.SetBool("IsRunning", true);

		FinishTurn();
		return true;
	}

	public void Face(Vector3 direction)
	{
		Direction = direction;
		CharaObject.transform.rotation = Quaternion.LookRotation(direction);
	}

	public void CheckCurrentGrid()
    {
		//メインプレイヤーなら
		if (this.gameObject == ObjectManager.Instance.PlayerList[0])
		{
			//階段チェック
			if (DungeonTerrain.Instance.GridID((int)Position.x, (int)Position.z) == (int)DungeonTerrain.GRID_ID.STAIRS)
			{
				LogManager.Instance.ControlLogUi(InternalDefine.LOG_STATE.STAIRS, true);
			}

			//アイテムチェック
			foreach(GameObject obj in ObjectManager.Instance.ItemList)
            {
				Vector3 pos = obj.GetComponent<Item>().Position;

				if(pos.x == Position.x && pos.z == Position.z)
                {
					Bag.PutAway(obj);
                }
            }

			//罠チェック

		}
    }

	protected void Moving()
    {
		if(IsMoving == false)
        {
			return;
        }
		MoveObject.transform.position = Vector3.MoveTowards(MoveObject.transform.position, DestinationPos, Time.deltaTime * 3);

		if ((MoveObject.transform.position - DestinationPos).magnitude <= 0.01f)
        {
			FinishMove();
		}
	}

	private void FinishMove()
    {
		IsMoving = false;
		IsActing = false;
		CharaAnimator.SetBool("IsRunning", false);
		MoveObject.transform.position = Position;

		CheckCurrentGrid();
	}

    private void Update()
    {
		Moving();
    }
}