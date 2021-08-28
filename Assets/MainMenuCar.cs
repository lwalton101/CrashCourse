using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class MainMenuCar : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    public Car.CarType carType;
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
        StartCoroutine(DestroyIn(6f));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int index = (int)carType;
        sr.sprite = carSprites.ElementAt(index);
        float realSpeed = speed / 1000;
        if (carType == Car.CarType.Bomb || carType == Car.CarType.SlowArrow || carType == Car.CarType.SpeedArrrow)
        {
            realSpeed += 0.05f;
        }

        //Debug.Log("Raw Seconds: " + GameManager.instance.rawSeconds + " Speed Multiplier: " + GameManager.instance.speedMultiplier + " Change By: " + changeBy + " Divided Speed: " + speed);
        transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + realSpeed);


    }

    private IEnumerator DestroyIn(float seconds)
	{
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
	}
}
