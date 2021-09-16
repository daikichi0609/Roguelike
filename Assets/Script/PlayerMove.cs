using UnityEngine;

public class PlayerMove : MonoBehaviour
{
	private Vector3 m_PlayerPos;
	public Vector3 PlayerPos
    {
		get { return m_PlayerPos; }
    }

    private void Update()
	{
		m_PlayerPos = this.transform.position;

		if (Input.GetKeyDown(KeyCode.W))
		{
			Move(new Vector3(0, 0, 1));
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			Move(new Vector3(-1, 0, 0));
		}
		if (Input.GetKeyDown(KeyCode.S))
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

	public void Move(Vector3 vector3)
	{
		if(PositionManager.Instance.IsPossibleToMove(m_PlayerPos, vector3) == false)
        {
			return;
        }

		transform.position += vector3;
		//GameManager.Instance.SwitchTurn();
	}
}