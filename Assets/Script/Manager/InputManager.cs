using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingletonMonoBehaviour<InputManager>
{
	//移動方向
	private Vector3 Direction
    {
		get;
		set;
    }

	//追加入力受付用タイマー
	private float Timer
    {
		get;
		set;
    }

	//追加入力受付用フラグ
	private bool IsWaitingAdditionalInput
    {
		get;
		set;
    }

	//入力受付メソッド
    public void DetectInput()
    {
		GameObject chara = ObjectManager.Instance.PlayerObject(0);

		if (TurnManager.Instance.CurrentState == TurnManager.STATE.UI_POPUPING)
        {
			DetectUiInput();
			return;
        }

		CharaMove playerMove = chara.GetComponent<CharaMove>();
		PlayerBattle playerBattle = chara.GetComponent<PlayerBattle>();

		if (playerMove.Turn == false || playerMove.IsActing == true)
        {
			return;
        }

		if (IsWaitingAdditionalInput == true)
        {
			DetectAdditionalInput(chara);
			return;
        }

		if (Input.GetKey(KeyCode.W))
		{
			Direction = new Vector3(0f, 0f, 1);
		}
		if (Input.GetKey(KeyCode.A))
		{
			Direction = new Vector3(-1f, 0f, 0f);
		}
		if (Input.GetKey(KeyCode.S))
		{
			Direction = new Vector3(0f, 0f, -1);
		}
		if (Input.GetKey(KeyCode.D))
		{
			Direction = new Vector3(1f, 0f, 0f);
		}

		if(Direction != new Vector3(0f, 0f, 0f))
        {
			IsWaitingAdditionalInput = true;
			return;
        }

		if (playerMove.CharaAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") == false)
		{
			return;
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			playerBattle.Act(CharaBattle.ACTION.ATTACK);
		}		
	}

	public void DetectUiInput()
    {
		UiManager.Instance.DetectInput();
	}

	private void DetectAdditionalInput(GameObject chara)
    {
		if (Input.GetKey(KeyCode.W) && Direction.z == 0)
		{
			Direction += new Vector3(0f, 0f, 1f);
		}
		if (Input.GetKey(KeyCode.A) && Direction.x == 0)
		{
			Direction += new Vector3(-1f, 0f, 0f);
		}
		if (Input.GetKey(KeyCode.S) && Direction.z == 0)
		{
			Direction += new Vector3(0f, 0f, -1);
		}
		if (Input.GetKey(KeyCode.D) && Direction.x == 0)
		{
			Direction += new Vector3(1f, 0f, 0f);
		}

		Timer += Time.deltaTime;
		if(Timer >= 0.01f || JudgeDirectionDiagonal(Direction) == true)
        {
			CharaMove playerMove = chara.GetComponent<CharaMove>();
			playerMove.Move(Direction);
			Direction = new Vector3(0f, 0f, 0f);
			Timer = 0f;
			IsWaitingAdditionalInput = false;
		}
    }

	private bool JudgeDirectionDiagonal(Vector3 direction)
    {
		if(direction == new Vector3(1f, 0f, 1f))
        {
			return true;
        }
		if (direction == new Vector3(1f, 0f, -1f))
		{
			return true;
		}
		if (direction == new Vector3(-1f, 0f, 1f))
		{
			return true;
		}
		if (direction == new Vector3(-1f, 0f, -1f))
		{
			return true;
		}

		return false;
	}
}
