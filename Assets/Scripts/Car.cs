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
        float realSpeed = speed / 1000;
        transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + realSpeed);
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Carpark") {
            Destroy(gameObject);
            Debug.Log("Entered Carpark");
            //GameManager.instance.score += 5;
        }
	}

    public enum CarType
	{
        RED,
        PURPLE,
        BLUE,
        BOMB,
        MULTI,
        SPEEDARROW,
        SLOWARROW
	}

    private void OnMouseDown()
    {
        GameManager.instance.currentCar = gameObject;
    }
}
