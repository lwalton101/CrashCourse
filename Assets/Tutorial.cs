using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space))
		{
            PlayerPrefs.SetInt(PlayerPrefsHandler.hasTutorialKey, 1);
            PlayerPrefs.Save();
            StartCoroutine(LoadGame());
		}
    }

    IEnumerator LoadGame()
	{
        animator.SetTrigger("start");
        KeepData.instance.headingFrom = 1;
        yield return new WaitForSeconds(.9f);

        SceneManager.LoadSceneAsync(2);
	}
}
