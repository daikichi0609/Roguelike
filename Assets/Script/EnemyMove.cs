using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : CharaMove
{
	private void Start()
	{
		Direction = new Vector3(0, 0, -1);
		Position = this.transform.position;
	}
}