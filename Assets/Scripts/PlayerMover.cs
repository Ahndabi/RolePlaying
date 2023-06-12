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
	float curSpeed;		// ����ӵ�
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
		// �ƹ��͵� ������ �ʾҴٸ�? (���� ���� 0�̶�� -> �Է°��� ���ٴ� ��)
		if (moveDir.magnitude == 0)
		{
			curSpeed = Mathf.Lerp(curSpeed, 0, 0.1f);
			anim.SetFloat("MoveSpeed", curSpeed);
			return;     // �ƹ��͵� ��������
		}

		// ĳ���� ���� (ī�޶�� ������ ���� ������. y�� �����ϰ�)
		// ���͸� �������� ������ ũ�Ⱑ 1�� ���͸� ������� �ʿ䰡 ���� (normalized -> �Լ��� ����)
		// *�����̸� normalize�� ������ֱ�*
		Vector3 forwardVec = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
		Vector3 rightVec = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;

		if (walk)
		{
			curSpeed = Mathf.Lerp(curSpeed, walkSpeed, 0.1f);
		}
		else    // �ٰ��ִ� ���
		{
			curSpeed = Mathf.Lerp(curSpeed, runSpeed, 0.1f);
		}

		// ĳ���� ����
		// z�Է°����� �չ������� �������ֱ�
		controller.Move(forwardVec * moveDir.z * curSpeed * Time.deltaTime);
		// x�Է°����� �����ʹ������� �������ֱ�
		controller.Move(rightVec * moveDir.x * curSpeed * Time.deltaTime);
		// MoveSpeed�� ���� �ӵ��� �����ֱ�
		anim.SetFloat("MoveSpeed", curSpeed);

		// ���� �ִ� ������ ĳ������ ����� ������ ���ֱ� (������ �ִ� ������ ĳ���Ͱ� �ٶ󺸵��� ��)
		Quaternion lookRotation = Quaternion.LookRotation(forwardVec * moveDir.z + rightVec * moveDir.x);
																 //		������ �ִ� ����
		// �ٷ� ȸ������ �ʰ� õõ�� ȸ�� (��������. Lerp�ε� ���ʹϾ𿡵� Lerp �� ����																 
		transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation,  0.1f);
											//    ���� ����	     �ٶ� ����    ������
	}

	void OnMove(InputValue value)
	{
		moveDir.x = value.Get<Vector2>().x;
		moveDir.z = value.Get<Vector2>().y;
	}

	void OnWalk(InputValue value)
	{
		walk = value.isPressed;		
		// isPressed : Pass Through���� ������ �� true, ���� �� false
	}

	void Fall()
	{
		ySpeed += Physics.gravity.y * Time.deltaTime;   // ���� �Ʒ��� �ӷ��� ����(�߷�)

		if (controller.isGrounded && ySpeed < 0)    // isGround ��������. ������ �׳� ���� �Ϸ��� ����
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
