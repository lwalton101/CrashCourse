using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
	private float secondsCount;
	private float rawSeconds;
	private int minutesCount;
    private int lastSecond;
    [SerializeField] private GameObject uiCarPrefab;

	// Start is called before the first frame update
	void Update()
    {
        StopWatch();
        if((int)rawSeconds % 3 == 0 && lastSecond != (int)rawSeconds)
		{
            lastSecond = (int)rawSeconds;
            SpawnUICar();
            
		}
    }

	private void SpawnUICar()
	{
        Car.CarType carType = (Car.CarType)Random.Range(0, 3);
        Road.Lane lane = (Road.Lane)Random.Range(0, 3);

        float x = 0f;
        float y = -9.1f;

        switch (lane)
        {
            case Road.Lane.Red:
                carType = Car.CarType.Red;
                x = -9f;
                break;
            case Road.Lane.Purple:
                carType = Car.CarType.Purple;
                x = -0f;
                break;
            case Road.Lane.Blue:
                carType = Car.CarType.Blue;
                x = 9f;
                break;
        }

        if ((int)carType > 3)
        {
            x = x + .75f;
        }
        else
        {
            x = x - .75f;
        }

        GameObject car = Instantiate(uiCarPrefab, new Vector2(x,y), Quaternion.identity);
        car.GetComponent<MainMenuCar>().currentLane = lane;
        car.GetComponent<MainMenuCar>().carType = carType;

	}

	// Update is called once per frame
	public void Play()
    {
        SceneManager.LoadScene(1);
    }

	public void Exit()
	{
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    public void StopWatch()
    {
        secondsCount += Time.deltaTime;
        rawSeconds += Time.deltaTime;
        string minutesText;
        string secondsText;
        Debug.Log(rawSeconds);
        if (minutesCount < 10)
            minutesText = "0" + minutesCount;
        else
            minutesText = minutesCount + "";
        if ((int)secondsCount < 10)
            secondsText = "0" + (int)secondsCount;
        else
            secondsText = (int)secondsCount + "";


        if (secondsCount >= 60)
        {
            minutesCount++;
            secondsCount = 0;
        }
    }

}
