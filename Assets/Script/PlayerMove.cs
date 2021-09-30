using UnityEngine;

public abstract class CharaMove: MonoBehaviour
{
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

	public bool IsMoving
    {
	    get;
		set;
    }
	protected Vector3 m_DestinationPos;

	protected Animator m_CharaAnimator;
	public Animator CharaAnimator
    {
        get { return m_CharaAnimator; }
		set { m_CharaAnimator = value; }
    }

	public void InitializeBeforeMoveGrid()
    {
		int pos_x = (int)Position.x;
		int pos_z = (int)Position.z;
		Grid grid = DungeonTerrain.Instance.GetTerrainListObject(pos_x, pos_z).GetComponent<Grid>();
		grid.Initialize();
    }

	protected void ReloadAfterMoveGrid(bool isPlayer)
    {
		int pos_x = (int)Position.x;
		int pos_z = (int)Position.z;
		Grid grid = DungeonTerrain.Instance.GetTerrainListObject(pos_x, pos_z).GetComponent<Grid>();
		grid.IsOnObject = this.gameObject;
		switch(isPlayer)
        {
			case true:
				grid.IsOnId = Grid.ISON_ID.PLAYER;
				break;

			case false:
				grid.IsOnId = Grid.ISON_ID.ENEMY;
				break;
        }
	}

	protected void Moving(GameObject moveObject)
    {
		CharaAnimator.SetBool("IsRunning", true);

		moveObject.transform.position = Vector3.MoveTowards(moveObject.transform.position, m_DestinationPos, Time.deltaTime * 3f);
		if ((moveObject.transform.position - m_DestinationPos).magnitude <= 0.01f)
		{
			IsMoving = false;

			Position = m_DestinationPos;
			moveObject.transform.position = Position;
			ReloadAfterMoveGrid(true);
		}
	}

	protected void FinishTurn()
    {
		CharaBattle charaBattle = this.gameObject.GetComponent<CharaBattle>();
		charaBattle.Turn = false;
    }

	protected bool IsFinish()
    {
		return this.gameObject.GetComponent<CharaBattle>().Turn;
	}
}

public class PlayerMove : CharaMove
{
	[SerializeField] private GameObject m_ParentObject;

    private void Start()
    {
		Direction = new Vector3(0, 0, -1);
		Position = m_ParentObject.transform.position;
		CharaAnimator = this.gameObject.GetComponent<Animator>();
	}

    private void Update()
	{
		if(IsFinish() == true)
        {
			return;
        }

		if(IsMoving == true)
        {
			Moving(m_ParentObject);
			return;
		}

		if (Input.GetKey(KeyCode.W))
		{
			Move(new Vector3(0, 0, 1f));
		}
		if (Input.GetKey(KeyCode.A))
		{
			Move(new Vector3(-1f, 0, 0));
		}
		if (Input.GetKey(KeyCode.X))
		{
			Move(new Vector3(0, 0, -1f));
		}
		if (Input.GetKey(KeyCode.D))
		{
			Move(new Vector3(1f, 0, 0));
		}

		if (Input.GetKey(KeyCode.Q))
		{
			Move(new Vector3(-1f, 0, 1f));
		}
		if (Input.GetKey(KeyCode.E))
		{
			Move(new Vector3(1f, 0, 1f));
		}
		if (Input.GetKey(KeyCode.Z))
		{
			Move(new Vector3(-1f, 0, -1f));
		}
		if (Input.GetKey(KeyCode.C))
		{
			Move(new Vector3(1f, 0, -1f));
		}

		CharaAnimator.SetBool("IsRunning", false);
	}

	public void Move(Vector3 direction)
	{
		Face(direction);
		if (Input.GetKey(KeyCode.RightShift))
		{
			m_CharaAnimator.SetBool("IsRunning", false);
			return;
		}
		if (PositionManager.Instance.IsPossibleToMoveGrid(Position, direction) == false)
        {
			m_CharaAnimator.SetBool("IsRunning", false);
			return;
        }
		Vector3 destinationPos = Position + direction;
		if(PositionManager.Instance.EnemyIsOn(destinationPos) == true)
        {
			m_CharaAnimator.SetBool("IsRunning", false);
			return;
        }

		InitializeBeforeMoveGrid();
		m_DestinationPos = Position + direction;
		IsMoving = true;

		return;
	}

	public void Face(Vector3 direction)
    {
		Direction = direction;
		transform.rotation = Quaternion.LookRotation(direction);
	}
}