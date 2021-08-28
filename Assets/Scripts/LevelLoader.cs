using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelLoader : MonoBehaviour
{

    public Animator transition;

    public float transitionTime = 1f;

	public void Awake()
	{
		if(KeepData.instance.headingFrom == 1) { return; }
		transition.SetTrigger("End");
	}
	public void LoadGame()
	{
		if(PlayerPrefs.GetInt(PlayerPrefsHandler.hasTutorialKey) == 1)
		{
			StartCoroutine(LoadLevel(2));
		}
		else
		{
			StartCoroutine(LoadLevel(1));
		}

	}

	public IEnumerator LoadLevel(int buildIndex)
	{
		transition.SetTrigger("Start");

		yield return new WaitForSeconds(transitionTime);

		SceneManager.LoadScene(buildIndex);
	}

	public void LoadSpecificLevel(int buildIndex)
	{
		StartCoroutine(LoadLevel(buildIndex));
	}
}
