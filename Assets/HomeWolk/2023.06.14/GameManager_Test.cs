using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Test : MonoBehaviour
{
	private static GameManager_Test instance;
	private static SceneManager_Test sceneManager;

	public static GameManager_Test Instance { get { return instance; } }
	public static SceneManager_Test Scene { get { return sceneManager; } }

	private void Awake()
	{
		if (instance != null)
		{
			Destroy(this);
			return;
		}

		instance = this;
		DontDestroyOnLoad(this);
		InitManagers();
	}

	private void OnDestroy()
	{
		if (instance == this)
			instance = null;
	}

	private void InitManagers()
	{
		GameObject sceneObj = new GameObject();
		sceneObj.name = "SceneManager";
		sceneObj.transform.parent = transform;
		sceneManager = sceneObj.AddComponent<SceneManager_Test>();
	}
}
