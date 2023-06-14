using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;		// �̸� ��ĥ��� �̷��� ����

public class SceneManager : MonoBehaviour
{
	// ������ ��� �̱������� �����ϸ� ������ ����
	// �� ������ �����ϴ� ���̽� ���� �ʿ���
	LoadingUI loadingUI;
	BaseScene curScene;

	// ���� ���� �������� �ʹٸ�!
	public BaseScene CurScene
	{
		get
		{
			// ���� ���� ������ ã�Ƽ� �ָ� �ǰ�
			if(curScene == null)
				curScene = GameObject.FindObjectOfType<BaseScene>();

			// ���� ���� �ִٸ� ã�Ƴ��� �� ����� �ָ� ��
			return curScene;
		}
	}

	private void Awake()
	{
		LoadingUI ui = Resources.Load<LoadingUI>("UI/LoadingUI");
		loadingUI = Instantiate(ui);
		loadingUI.transform.SetParent(transform);
	}

	public void LoadScene(string sceneName)
	{
		StartCoroutine(LoadingRoutine(sceneName));
	}


	IEnumerator LoadingRoutine(string sceneName)
	{
		loadingUI.FadeOut();
		loadingUI.SetProgress(0f);
		yield return new WaitForSeconds(0.5f);
		Time.timeScale = 0;

		AsyncOperation oper = UnitySceneManager.LoadSceneAsync(sceneName);
		oper.allowSceneActivation = false;

		// !oper.isDone = �� ������ �� ������ true
		while (!oper.isDone)
		{
			loadingUI.SetProgress(Mathf.Lerp(0f, 0.5f, oper.progress));
			yield return null;
		}

		// �߰����� ������ �غ��� �ε��� �����ϰ� �Ѿ��
		// ���� ������ġ
		loadingUI.SetProgress(0.6f);
		yield return new WaitForSecondsRealtime(0.5f);	  // timeScale������ ���� �ð�����!
		// Ǯ��
		loadingUI.SetProgress(0.7f);
		yield return new WaitForSecondsRealtime(0.5f);
		// ���ҽ� �ҷ�����
		loadingUI.SetProgress(0.8f);
		yield return new WaitForSecondsRealtime(0.5f);
		// ���� ������ ��ġ ��
		loadingUI.SetProgress(1f);
		yield return new WaitForSecondsRealtime(0.5f);

		CurScene.LoadAsync();
		while (curScene.progress < 1f)
		{
			loadingUI.SetProgress(Mathf.Lerp(0.5f, 1.0f, CurScene.progress));
			yield return null;
		}

		Time.timeScale = 1f;
		loadingUI.FadeIn();
		yield return new WaitForSeconds(0.5f);
	}
}
