using UnityEngine;

public class Player : MonoBehaviour, ICommand
{
	private PlayerData m_Data;

    private void Start()
    {
		m_Data = new PlayerData();
		JsonUtility.FromJsonOverwrite(CharaDataManager.Instance.LoadTest(CharaDataManager.Instance.Datapath), m_Data);
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
		if (Input.GetKeyDown(KeyCode.S))
		{
			Move(new Vector3(0, 0, -1));
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			Move(new Vector3(1, 0, 0));
		}
	}

	public void OnAttack()
	{
		
	}
	public void Move(Vector3 vector3)
	{
		if (GameManager.Instance.PlayerTurn == false)
        {
			//return;
        }
		transform.position += vector3;
		GameManager.Instance.SwitchTurn();
	}
}