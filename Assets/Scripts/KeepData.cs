using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class KeepData : MonoBehaviour
{

    public static KeepData instance;
    public int headingFrom;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
		{
            instance = this;
		}
		else
		{
            Destroy(gameObject);
		}
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
