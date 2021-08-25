using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject currentCar;
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private float movementGap = -3f;
    public List<Sprite> carSprites = new List<Sprite>();
    [SerializeField] private TextMeshPro stopWatchText;
    private float secondsCount;
    private float rawSeconds;
    private int minutesCount;
    private float lastTime = 0;
    [SerializeField] private float timeBetweenSpawns;
    private bool firstSpawn = false;
    public float score = 0;
    [SerializeField] private float divideBy = 1000;
    private Road.Lane lastLane;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        StopWatch();
        SpawnCar();
        if(currentCar != null)
		{
            Collider2D centerCol = Physics2D.OverlapPoint(new Vector2(currentCar.transform.position.x, currentCar.GetComponent<SpriteRenderer>().bounds.center.y - currentCar.GetComponent<SpriteRenderer>().bounds.extents.y));
            Collider2D topCol = Physics2D.OverlapPoint(new Vector2(currentCar.transform.position.x, currentCar.GetComponent<SpriteRenderer>().bounds.center.y - currentCar.GetComponent<SpriteRenderer>().bounds.extents.y));
            Collider2D bottomCol = Physics2D.OverlapPoint(new Vector2(currentCar.transform.position.x, currentCar.GetComponent<SpriteRenderer>().bounds.center.y - currentCar.GetComponent<SpriteRenderer>().bounds.extents.y));
            
            Debug.Log(centerCol);
            Debug.Log(topCol);
            Debug.Log(bottomCol);
        }
    }

    public void SpawnCar()
    {
        
        if((int)(secondsCount) == 0 && !firstSpawn)
		{
            firstSpawn = true;
            GameObject firstCar = GameObject.Instantiate(carPrefab, new Vector3(-10.75f, -9f, 0), Quaternion.identity);

            firstCar.GetComponent<Car>().currentLane = Road.Lane.RED;
            firstCar.GetComponent<Car>().carType = Car.CarType.RED;
            
        }
        float localtime = (int)lastTime + timeBetweenSpawns - (score / divideBy);
        if (rawSeconds < localtime)
		{
            //Debug.Log(rawSeconds + " is smaller than" + " " + localtime);
            return;
		}
        lastTime = (int)rawSeconds;
        float chance = Random.Range(0,101);


        Car.CarType carType = (Car.CarType)Random.Range(0, 3);
        Road.Lane lane = (Road.Lane)Random.Range(0, 3);
        if(lastLane != null && lane == lastLane)
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
            case Road.Lane.RED:
                x = -10f;
                break;
            case Road.Lane.PURPLE:
                x = -2f;
                break;
            case Road.Lane.BLUE:
                x = 6f;
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

        GameObject car = GameObject.Instantiate(carPrefab, new Vector3(x, y, 0), Quaternion.identity);

        car.GetComponent<Car>().currentLane = lane;
        car.GetComponent<Car>().carType = carType;
        lastLane = lane;
    }

    public void SwitchLane(Road.Lane toSwitchTo)
	{
		if (currentCar == null) { return; }

        Car car = currentCar.GetComponent<Car>();
        float laneDifference = (int)toSwitchTo - (int)car.currentLane;

		if (car.currentLane.Equals(toSwitchTo)) { return; }

        Collider2D collider2D = Physics2D.OverlapPoint(new Vector2(currentCar.transform.position.x + (laneDifference * 8), currentCar.transform.position.y));
        if(collider2D != null && collider2D.gameObject.CompareTag("Car"))
		{
            CheckToTeleport(collider2D.gameObject, currentCar, toSwitchTo);
            return;
		}
        currentCar.transform.position = new Vector2(currentCar.transform.position.x + (laneDifference * 8), currentCar.transform.position.y);
        car.currentLane = toSwitchTo;
        GameManager.instance.currentCar = null;
    }

    //TIME FOR RECURSION THIS WILL WORK I PROMISE
    public void CheckToTeleport(GameObject lastCar, GameObject currentCar, Road.Lane toSwitchTo)
	{
        
        Collider2D collider2 = Physics2D.OverlapPoint(new Vector2(lastCar.transform.position.x, lastCar.transform.position.y - movementGap));
        if (collider2 == null || !collider2.gameObject.CompareTag("Car"))
		{
            Debug.Log("I AM A TERRIBLE PIRATE YA HEAR EM I HAVE FAILED TO FIND THAT BINGING COLLIDER");
            currentCar.transform.position = new Vector2(lastCar.transform.position.x, lastCar.transform.position.y - movementGap);
            currentCar.GetComponent<Car>().currentLane = toSwitchTo;
            GameManager.instance.currentCar = null;
            return;
		}
        Debug.Log("I AM A FUCKING GOD");
        
		CheckToTeleport(collider2.gameObject, currentCar, toSwitchTo);
		
        return;
	}

    public void CheckBelow(Vector2 spawnpoint)
	{

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
    }
}
