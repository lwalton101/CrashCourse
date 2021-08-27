using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(SpriteRenderer))]
public class Car : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    public CarType carType;
    public Road.Lane currentLane;
    private SpriteRenderer sr;
    [SerializeField] private List<Sprite> carSprites = new List<Sprite>();
    [SerializeField] private GameObject selectObject;
    // Start is called before the first frame update
    void Awake()
    {
        int index = (int)carType;  
		sr = GetComponent<SpriteRenderer>();
        sr.sprite = carSprites.ElementAt(index);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int index = (int)carType;
        sr.sprite = carSprites.ElementAt(index);
        float changeBy = (GameManager.instance.rawSeconds / GameManager.instance.speedMultiplier);
        float realSpeed = speed / 1000 + changeBy + GameManager.instance.speedArrow;
        if (carType == CarType.Bomb || carType == CarType.SlowArrow || carType == CarType.SpeedArrrow)
        {
            realSpeed += 0.05f;
        }

        //Debug.Log("Raw Seconds: " + GameManager.instance.rawSeconds + " Speed Multiplier: " + GameManager.instance.speedMultiplier + " Change By: " + changeBy + " Divided Speed: " + speed);
        transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + realSpeed);

        
    }
	private void Update()
	{ 
        if (GameManager.instance.currentCar == gameObject)
        {
            selectObject.SetActive(true);
        }
        else
        {
            selectObject.SetActive(false);
        }
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Carpark") {
            Destroy(gameObject);
			switch (carType)
			{
				case CarType.Multi:
                    GameManager.instance.score += 5;
                    return;
                case CarType.SlowArrow:
                    return;
                case CarType.SpeedArrrow:
                    return;
                case CarType.Bomb:
                    GameManager.instance.GameOver();
					return;
			}
			if (collision.gameObject.name.StartsWith(carType.ToString()))
			{
                GameManager.instance.score += 5;
            }
			else
			{
                AudioManager.instance.Play("LifeLost");
                GameManager.instance.lives--;
			}
        }
	}

    public enum CarType
	{
        Red,
        Purple,
        Blue,
        Multi,
        Bomb,
        SpeedArrrow,
        SlowArrow
	}

    private void OnMouseDown()
    {
        GameManager.instance.currentCar = gameObject;
        Debug.Log("That tickles");
        if(carType == CarType.Bomb)
		{
            Destroy(gameObject);
        }
        if(carType == CarType.SpeedArrrow)
		{
            Destroy(gameObject);
            GameManager.instance.speedArrow += .01f;
        }
        if (carType == CarType.SlowArrow)
        {
            Destroy(gameObject);
            GameManager.instance.speedArrow -= .01f;
        }
    }
}
