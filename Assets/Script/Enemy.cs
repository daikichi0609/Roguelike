using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ICommand
{
	private EnemyData m_EnemyData;

	public Enemy(EnemyData data)
    {
		m_EnemyData = data;
    }

	public void OnAttack()
	{

	}
	public void Move(Vector3 vector3)
	{

	}
}