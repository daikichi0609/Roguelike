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

	public void InitializeBeforeMoveGrid()
    {
		int pos_x = (int)Position.x;
		int pos_z = (int)Position.z;
		Grid grid = DungeonTerrain.Instance.GetListObject(pos_x, pos_z).GetComponent<Grid>();
		grid.Initialize();
    }

	protected void ReloadAfterMoveGrid(bool isplayer)
    {
		int pos_x = (int)Position.x;
		int pos_z = (int)Position.z;
		Grid grid = DungeonTerrain.Instance.GetListObject(pos_x, pos_z).GetComponent<Grid>();
		grid.IsOnObject = this.gameObject;
		switch(isplayer)
        {
			case true:
				grid.IsOnId = Grid.ISON_ID.PLAYER;
				break;

			case false:
				grid.IsOnId = Grid.ISON_ID.ENEMY;
				break;
        }
	}
}

public class PlayerMove : CharaMove
{
    private void Start()
    {
		Direction = new Vector3(0, 0, -1);
		Position = this.transform.position;
		CameraManager.Instance.MainCamera.transform.position = Position + new Vector3(0, 5f, -1.5f);
		CameraManager.Instance.MainCamera.transform.eulerAngles = new Vector3(70, 0, 0);
	}

    private void Update()
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			Move(new Vector3(0, 0, 1));
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			Move(new Vector3(-1, 0, 0));
		}
		if (Input.GetKeyDown(KeyCode.X))
		{
			Move(new Vector3(0, 0, -1));
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			Move(new Vector3(1, 0, 0));
		}

		if (Input.GetKeyDown(KeyCode.Q))
		{
			Move(new Vector3(-1, 0, 1));
		}
		if (Input.GetKeyDown(KeyCode.E))
		{
			Move(new Vector3(1, 0, 1));
		}
		if (Input.GetKeyDown(KeyCode.Z))
		{
			Move(new Vector3(-1, 0, -1));
		}
		if (Input.GetKeyDown(KeyCode.C))
		{
			Move(new Vector3(1, 0, -1));
		}
	}

	public void Move(Vector3 direction)
	{
		Face(direction);
		if (Input.GetKey(KeyCode.RightShift))
		{
			return;
		}
		if (PositionManager.Instance.IsPossibleToMoveGrid(Position, direction) == false)
        {
			return;
        }
		Vector3 destinationPos = Position + direction;
		if(PositionManager.Instance.EnemyIsOn(destinationPos) == true)
        {
			return;
        }

		InitializeBeforeMoveGrid();
		transform.position += direction;
		Position = this.transform.position;
		ReloadAfterMoveGrid(true);
		CameraManager.Instance.MainCamera.transform.position += direction;

		//GameManager.Instance.SwitchTurn();
	}

	public void Face(Vector3 direction)
    {
		Direction = direction;
		transform.rotation = Quaternion.LookRotation(direction);
	}
}