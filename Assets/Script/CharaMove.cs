using UnityEngine;

public abstract class Chara : MonoBehaviour
{
	public bool Turn
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

	protected Animator m_CharaAnimator;
	public Animator CharaAnimator
	{
		get { return m_CharaAnimator; }
		protected set { m_CharaAnimator = value; }
	}

	public void Initialize()
	{
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
	public Vector3 m_DestinationPos;

	public bool IsMoving
    {
		get;
	    set;
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
		FinishTurn();

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