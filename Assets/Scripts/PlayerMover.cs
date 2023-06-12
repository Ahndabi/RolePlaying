using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
	[SerializeField] float walkSpeed;
	[SerializeField] float runSpeed;
	[SerializeField] float jumpSpeed;

	CharacterController controller;
	Animator anim;
	Vector3 moveDir;
	bool walk;
	float curSpeed;		// 현재속도
	float ySpeed;

	private void Awake()
	{
		controller = GetComponent<CharacterController>();
		anim = GetComponent<Animator>();
	}

	private void Update()
	{
		Move();
		Fall();
	}

	void Move()
	{
		// 아무것도 누르지 않았다면? (벡터 값이 0이라면 -> 입력값이 없다는 뜻)
		if (moveDir.magnitude == 0)
		{
			curSpeed = Mathf.Lerp(curSpeed, 0, 0.1f);
			anim.SetFloat("MoveSpeed", curSpeed);
			return;     // 아무것도 하지마셈
		}

		// 캐릭터 시점 (카메라랑 시점이 같이 가야함. y만 제외하고)
		// 백터를 방향으로 쓰려면 크기가 1인 백터를 사용해줄 필요가 있음 (normalized -> 함수도 있음)
		// *방향이면 normalize를 사용해주기*
		Vector3 forwardVec = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
		Vector3 rightVec = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;

		if (walk)
		{
			curSpeed = Mathf.Lerp(curSpeed, walkSpeed, 0.1f);
		}
		else    // 뛰고있는 경우
		{
			curSpeed = Mathf.Lerp(curSpeed, runSpeed, 0.1f);
		}

		// 캐릭터 무브
		// z입력값으로 앞방향으로 움직여주기
		controller.Move(forwardVec * moveDir.z * curSpeed * Time.deltaTime);
		// x입력값으로 오른쪽방향으로 움직여주기
		controller.Move(rightVec * moveDir.x * curSpeed * Time.deltaTime);
		// MoveSpeed는 현재 속도로 정해주기
		anim.SetFloat("MoveSpeed", curSpeed);

		// 보고 있는 방향을 캐릭터의 방향과 같도록 해주기 (누르고 있는 방향을 캐릭터가 바라보도록 함)
		Quaternion lookRotation = Quaternion.LookRotation(forwardVec * moveDir.z + rightVec * moveDir.x);
																 //		누르고 있는 방향
		// 바로 회전하지 않고 천천히 회전 (선형보간. Lerp인데 쿼터니언에도 Lerp 가 있음																 
		transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation,  0.1f);
											//    현재 방향	     바라볼 방향    딜레이
	}

	void OnMove(InputValue value)
	{
		moveDir.x = value.Get<Vector2>().x;
		moveDir.z = value.Get<Vector2>().y;
	}

	void OnWalk(InputValue value)
	{
		walk = value.isPressed;		
		// isPressed : Pass Through에서 눌렀을 땐 true, 뗐을 땐 false
	}

	void Fall()
	{
		ySpeed += Physics.gravity.y * Time.deltaTime;   // 점점 아래로 속력을 받음(중력)

		if (controller.isGrounded && ySpeed < 0)    // isGround 쓰지마셈. 지금은 그냥 빨리 하려구 쓴거
			ySpeed = 0;

		controller.Move(Vector3.up * ySpeed * Time.deltaTime);
	}

	void Jump()
	{
		ySpeed = jumpSpeed;
	}

	void OnJump(InputValue value)
	{
		Jump();
	}
}
