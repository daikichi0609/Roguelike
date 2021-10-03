using UnityEngine;

public class CharaMove: MonoBehaviour
{
	[SerializeField] private GameObject m_MoveObject;
	public GameObject MoveObject
    {
        get { return m_MoveObject; }
    }

	[SerializeField] private GameObject m_CharaObject;
	public GameObject CharaObject
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

	protected Vector3 m_DestinationPos;

	protected Animator m_CharaAnimator;
	public Animator CharaAnimator
    {
        get { return m_CharaAnimator; }
		private set { m_CharaAnimator = value; }
    }

	public bool IsMoving
    {
		get;
		private set;
    }

	public void Initialize()
	{
		CharaAnimator = CharaObject.GetComponent<Animator>();
		Direction = new Vector3(0, 0, -1);
		Position = m_MoveObject.transform.position;
	}

	public bool Move(Vector3 direction)
	{
		Face(direction);

		if (PositionManager.Instance.IsPossibleToMoveGrid(Position, direction) == false)
		{
			return false;
		}
		Vector3 destinationPos = Position + direction;
		if (PositionManager.Instance.EnemyIsOn(destinationPos) == true)
		{
			return false;
		}

		m_DestinationPos = Position + direction;
		IsMoving = true;

		return true;
	}

	public void Face(Vector3 direction)
	{
		Direction = direction;
		CharaObject.transform.rotation = Quaternion.LookRotation(direction);
	}

	protected void Moving()
    {
		if(IsMoving == false)
        {
			return;
        }

		MoveObject.transform.position = Vector3.MoveTowards(MoveObject.transform.position, m_DestinationPos, Time.deltaTime * 3);

		if((MoveObject.transform.position - m_DestinationPos).magnitude <= 0.01f)
        {
			Position = m_DestinationPos;
			MoveObject.transform.position = Position;
			IsMoving = false;
		}
	}

    private void Update()
    {
		Moving();
    }
}