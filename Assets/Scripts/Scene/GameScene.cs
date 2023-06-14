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
		Debug.Log("랜덤 맵 생성");
		yield return new WaitForSecondsRealtime(1f);
		progress = 0.3f;
		Debug.Log("랜덤 아이템 생성");
		yield return new WaitForSecondsRealtime(1f);
		progress = 0.7f;
		Debug.Log("플레이어 생성");
		player.transform.position = playerPosition.position;
		freeLookCamera.Follow = player.transform;
		freeLookCamera.LookAt = player.transform;
		yield return new WaitForSecondsRealtime(1f);
		progress = 1f;
	}
}
