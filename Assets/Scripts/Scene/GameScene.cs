using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameScene : BaseScene
{
	public GameObject player;
	public Transform playerPosition;
	public CinemachineFreeLook freeLookCamera;

	protected override IEnumerator LoadingRoutine()
	{
		progress = 0f;
		Debug.Log("���� �� ����");
		yield return new WaitForSecondsRealtime(1f);
		progress = 0.3f;
		Debug.Log("���� ������ ����");
		yield return new WaitForSecondsRealtime(1f);
		progress = 0.7f;
		Debug.Log("�÷��̾� ����");
		player.transform.position = playerPosition.position;
		freeLookCamera.Follow = player.transform;
		freeLookCamera.LookAt = player.transform;
		yield return new WaitForSecondsRealtime(1f);
		progress = 1f;
	}
}
