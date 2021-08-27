using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isOver = false;
    public GameObject currentCar;
    public GameObject gameOverPanel;
    [SerializeField] private GameObject carPrefab = null;
    [SerializeField] private List<Sprite> lifeSprites = new List<Sprite>();
    [SerializeField] private TextMeshPro stopWatchText = null;
    [SerializeField] private TextMeshPro pointsText = null;
    private float secondsCount;
    public float rawSeconds;
    private int minutesCount;
    private float lastTime = 0;
    [SerializeField] private float timeBetweenSpawns = 0f;
    private bool firstSpawn = false;
    public float score = 0;
    [SerializeField] private float divideBy = 1000;
    private Road.Lane lastLane;
    public int lives = 3;
    [SerializeField] private GameObject livesObject;
    [Range(0, 50)]
    public float timeScale = 0f;
    public float speedMultiplier = 0.05f;
    public float spawnMultiplier = 0.05f;
    public float speedArrow;
    public float numberOf5s = 0f;
    private int lastNumber;

    [Header("Debugs")]
    [SerializeField] private bool debugMode = false;
    [SerializeField] private int redSpawns = 0;
    [SerializeField] private int purpleSpawns = 0;
    [SerializeField] private int blueSpawns = 0;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        lastLane = Road.Lane.Red;
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = timeScale;
		if (!isOver)
		{
            StopWatch();
            SpawnCar();
        }

        if(currentCar != null && debugMode)
		{
            Collider2D centerCol = Physics2D.OverlapPoint(new Vector2(currentCar.transform.position.x + 8, currentCar.transform.position.y));
            Collider2D topCol = Physics2D.OverlapPoint(new Vector2(currentCar.transform.position.x + 8, currentCar.transform.position.y + 0.75f));
            Collider2D bottomCol = Physics2D.OverlapPoint(new Vector2(currentCar.transform.position.x + 8, currentCar.transform.position.y - 0.7f));

            Debug.Log("Top Collider: " + topCol.name);
            Debug.Log("Center Collider: " + centerCol.name);
            Debug.Log("Bottom Collider: " + bottomCol.name);

            Car car = currentCar.GetComponent<Car>();

            //Debug.Log("Current Lane: " + car.currentLane + ", Car Type: " + car.carType + ", Car Sprite: " + car.carSprites.ElementAt((int)car.carType));
        }

        livesObject.GetComponent<SpriteRenderer>().sprite = lifeSprites.ElementAt(lives);

        if(lives == 0 && !isOver)
		{
            GameOver();
		}

        ListenToInput();

        pointsText.text = "Points: " + score.ToString();
    }


    public void ListenToInput()
	{
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchLane((Road.Lane)0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchLane((Road.Lane)1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchLane((Road.Lane)2);
        }
    }
	public void GameOver()
	{
        lives = 0;
        gameOverPanel.SetActive(true);
        TextMeshProUGUI scoreText = GameObject.Find("GameOverScore").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI highScoreText = GameObject.Find("GameOverHighScore").GetComponent<TextMeshProUGUI>();
        scoreText.text = "Score: " + score;
        highScoreText.text = "Highscore: " + "not implemented";
        isOver = true;
        foreach(GameObject gameObject in GameObject.FindGameObjectsWithTag("Car"))
		{
            Destroy(gameObject);
		}
	}

    public void Retry()
	{
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

    public void MainMenu()
	{
        SceneManager.LoadScene(0);
	}

	public void SpawnCar()
    {
        if((int)(secondsCount) == 0 && !firstSpawn)
		{
            firstSpawn = true;
            GameObject firstCar = GameObject.Instantiate(carPrefab, new Vector3(-10.75f, -9f, 0), Quaternion.identity);

            firstCar.GetComponent<Car>().currentLane = Road.Lane.Red;
            firstCar.GetComponent<Car>().carType = Car.CarType.Purple;
            
        }
        float changeBy = 3 - (rawSeconds / spawnMultiplier);
        float localtime = lastTime + changeBy;
        if(changeBy <= .5f)
		{
            changeBy = .5f;
            localtime = lastTime + .5f;
		}
        //Debug.Log(changeBy);
        if (rawSeconds < localtime)
		{
            //Debug.Log(rawSeconds + " is smaller than" + " " + localtime);
            return;
		}
        lastTime = localtime;

        //Debug.Log("Change By: " + changeBy + " Last Time : " + lastTime + " Local Time: " + localtime + " To be subtracted: " + (rawSeconds / spawnMultiplier) + " Raw Seconds: " + rawSeconds + " is smaller than" + " " + localtime);

        Car.CarType carType = (Car.CarType)Random.Range(0, 3);
        if(Random.Range(0,20) == 0)
		{
            carType = Car.CarType.Multi;
		}
        Road.Lane lane = (Road.Lane)Random.Range(0, 3);
        if(lane == lastLane)
		{
            
            List<Road.Lane> lanes = new List<Road.Lane>();
            for(int i = 1; i > 4; i++)
			{
                lanes.Add((Road.Lane)i);
			}
            lanes.Remove(lastLane);
            lane = (Road.Lane)Random.Range(0, 2);
            
		}
        float x = 0f;
        float y = -9.1f;

		switch (lane)
		{
            case Road.Lane.Red:
                x = -10f;
                redSpawns++;
                break;
            case Road.Lane.Purple:
                x = -2f;
                purpleSpawns++;
                break;
            case Road.Lane.Blue:
                x = 6f;
                blueSpawns++;
                break;
		}

        if((int)carType > 3)
		{
            x = x + .75f;
		}
		else
		{
            x = x - .75f;
		}

        CheckIfSafe(new Vector2(x, y), lane, carType);

        float chance = Random.Range(0, 101);

        if(chance > 5)
		{
            return;
		}

        Car.CarType specialCarType = (Car.CarType)Random.Range(4, 7);
        float specialX = x + 1.5f;
        GameObject car = Instantiate(carPrefab, new Vector2(specialX, y + -2), Quaternion.identity);
        car.name = lane.ToString();
        car.GetComponent<Car>().currentLane = lane;
        car.GetComponent<Car>().carType = specialCarType;
        //car.GetComponent<Car>().GetComponent<SpriteRenderer>().sprite = car.GetComponent<Car>().carSprites.ElementAt((int)specialCarType);
        lastLane = lane;
    }

    public void SwitchLane(Road.Lane toSwitchTo)
	{
		if (currentCar == null) { return; }

        Car car = currentCar.GetComponent<Car>();
        float laneDifference = (int)toSwitchTo - (int)car.currentLane;

		if (car.currentLane.Equals(toSwitchTo)) { return; }

        CheckIfSafe(new Vector2(currentCar.transform.position.x + (laneDifference * 8), currentCar.transform.position.y), toSwitchTo);

    }

    //TIME FOR RECURSION THIS WILL WORK I PROMISE
    public void CheckIfSafe(Vector2 center, Road.Lane toSwitchTo)
	{
        Collider2D centerCol = Physics2D.OverlapPoint(new Vector2(center.x, center.y));
        Collider2D topCol = Physics2D.OverlapPoint(new Vector2(center.x, center.y + 0.75f));
        Collider2D bottomCol = Physics2D.OverlapPoint(new Vector2(center.x, center.y - 0.7f));
        
        Car car = currentCar.GetComponent<Car>();

        if (centerCol != null && centerCol.gameObject.CompareTag("Car") && topCol != null && !topCol.gameObject.CompareTag("Car") && bottomCol != null && bottomCol.gameObject.CompareTag("Car"))
		{
            centerCol = Physics2D.OverlapPoint(new Vector2(center.x, center.y + 1f)); 
            topCol = Physics2D.OverlapPoint(new Vector2(center.x, center.y + 1.75f));
            bottomCol = Physics2D.OverlapPoint(new Vector2(center.x, center.y + .3f));

            if (centerCol != null && !centerCol.gameObject.CompareTag("Car") && topCol != null && !topCol.gameObject.CompareTag("Car") && bottomCol != null && !bottomCol.gameObject.CompareTag("Car"))
            {
                currentCar.transform.position = (new Vector2(center.x, center.y + 1f));
                car.currentLane = toSwitchTo;
                currentCar = null;
                return;
            }
        }

        if (centerCol != null && centerCol.gameObject.CompareTag("Car") || topCol != null && topCol.gameObject.CompareTag("Car") || bottomCol != null && bottomCol.gameObject.CompareTag("Car"))
		{
            CheckIfSafe(new Vector2(center.x, center.y - .5f), toSwitchTo);
            return;
		}
        currentCar.transform.position = center;
        car.currentLane = toSwitchTo;
        currentCar = null;
        return;
    }

    public void CheckIfSafe(Vector2 center, Road.Lane lane, Car.CarType carType)
	{
        Collider2D centerCol = Physics2D.OverlapPoint(new Vector2(center.x, center.y));
        Collider2D topCol = Physics2D.OverlapPoint(new Vector2(center.x, center.y + 0.75f));
        Collider2D bottomCol = Physics2D.OverlapPoint(new Vector2(center.x, center.y - 0.7f));
        
        if (centerCol != null && centerCol.gameObject.CompareTag("Car") || topCol != null && topCol.gameObject.CompareTag("Car") || bottomCol != null && bottomCol.gameObject.CompareTag("Car"))
        {
            CheckIfSafe(new Vector2(center.x, center.y - .5f), lane, carType);
            return;
        }

        GameObject car = Instantiate(carPrefab, center, Quaternion.identity);
        //car.GetComponent<Car>().GetComponent<SpriteRenderer>().sprite = car.GetComponent<Car>().carSprites.ElementAt((int)carType);
        car.GetComponent<Car>().currentLane = lane;
        car.GetComponent<Car>().carType = carType;
        lastLane = lane;
        return;
    }

    public void StopWatch()
	{
        secondsCount += Time.deltaTime;
        rawSeconds += Time.deltaTime;
        string minutesText;
        string secondsText;

        if (minutesCount < 10)
            minutesText = "0" + minutesCount;
        else
            minutesText = minutesCount + "";
        if ((int)secondsCount < 10)
            secondsText = "0" + (int)secondsCount;
        else
            secondsText = (int)secondsCount + "";

        stopWatchText.text = minutesText + ":" + secondsText;

        if(secondsCount >= 60)
		{
            minutesCount++;
            secondsCount = 0;
		}
        if((int)rawSeconds % 5 == 0)
		{
            if(lastNumber == (int)rawSeconds) { return; }
            numberOf5s++;
            lastNumber = (int)rawSeconds;
		}
    }
}
