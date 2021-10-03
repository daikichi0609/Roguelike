﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingletonMonoBehaviour<InputManager>
{
	private Vector3 Direction
    {
		get;
		set;
    }

	private float Timer
    {
		get;
		set;
    }

	private bool IsWaitingAddInput
    {
		get;
		set;
    }

    public void DetectInput(GameObject chara)
    {
		CharaMove charaMove = chara.GetComponent<CharaMove>();
		CharaBattle charaBattle = chara.GetComponent<CharaBattle>();

		if (charaMove.IsMoving == true)
        {
			charaMove.CharaAnimator.SetBool("IsRunning", true);
			return;
        }

		if(charaBattle.IsAttacking == true)
        {
			return;
        }

		if (IsWaitingAddInput == true)
        {
			DetectAdditionalInput(chara);
			return;
        }

		if (Input.GetKey(KeyCode.W))
		{
			Direction = new Vector3(0, 0, 1);
		}
		if (Input.GetKey(KeyCode.A))
		{
			Direction = new Vector3(-1f, 0, 0);
		}
		if (Input.GetKey(KeyCode.S))
		{
			Direction = new Vector3(0, 0, -1);
		}
		if (Input.GetKey(KeyCode.D))
		{
			Direction = new Vector3(1f, 0, 0);
		}

		if(Direction != new Vector3(0, 0, 0))
        {
			IsWaitingAddInput = true;
			return;
        }

		charaMove.CharaAnimator.SetBool("IsRunning", false);

		if (charaMove.CharaAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") == false)
		{
			return;
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			charaBattle.Attack();
		}		
	}

	private void DetectAdditionalInput(GameObject chara)
    {
		if (Input.GetKey(KeyCode.W) && Direction.z == 0)
		{
			Direction += new Vector3(0, 0, 1);
		}
		if (Input.GetKey(KeyCode.A) && Direction.x == 0)
		{
			Direction += new Vector3(-1f, 0, 0);
		}
		if (Input.GetKey(KeyCode.S) && Direction.z == 0)
		{
			Direction += new Vector3(0, 0, -1);
		}
		if (Input.GetKey(KeyCode.D) && Direction.x == 0)
		{
			Direction += new Vector3(1f, 0, 0);
		}

		Timer += Time.deltaTime;
		if(Timer >= 0.01f || JudgeDirectionDiagonal(Direction) == true)
        {
			CharaMove charaMove = chara.GetComponent<CharaMove>();
			if(charaMove.Move(Direction) == false)
            {
				charaMove.CharaAnimator.SetBool("IsRunning", false);
			}
			Direction = new Vector3(0, 0, 0);
			Timer = 0f;
			IsWaitingAddInput = false;
		}
    }

	private bool JudgeDirectionDiagonal(Vector3 direction)
    {
		if(direction == new Vector3(1f, 0, 1f))
        {
			return true;
        }
		if (direction == new Vector3(1f, 0, -1f))
		{
			return true;
		}
		if (direction == new Vector3(-1f, 0, 1f))
		{
			return true;
		}
		if (direction == new Vector3(-1f, 0, -1f))
		{
			return true;
		}

		return false;
	}
}