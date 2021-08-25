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
        float realSpeed = speed / 1000 + GameManager.instance.score / 5 / 3750;
        transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + realSpeed);
    }


	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Carpark") {
            Destroy(gameObject);
            if (collision.gameObject.name.StartsWith(carType.ToString()))
			{
                Debug.Log("YOU WIN");
                GameManager.instance.score += 5;
            }
			else
			{
                GameManager.instance.lives--;
			}
        }
	}

    public enum CarType
	{
        Red,
        Purple,
        Blue,
        Bomb,
        Multi,
        SpeedArrrow,
        SlowArrow
	}

    private void OnMouseDown()
    {
        GameManager.instance.currentCar = gameObject;
        Debug.Log("That tickles");
    }
}
