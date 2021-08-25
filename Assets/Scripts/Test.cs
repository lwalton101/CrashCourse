using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    /*public void SwitchLane(Road.Lane toSwitchTo)
    {
        if (currentCar == null) { return; }

        Car car = currentCar.GetComponent<Car>();
        float laneDifference = (int)toSwitchTo - (int)car.currentLane;

        if (car.currentLane.Equals(toSwitchTo)) { return; }

        Collider2D collider2D = Physics2D.OverlapPoint(new Vector2(currentCar.transform.position.x + (laneDifference * 8), currentCar.transform.position.y));
        if (collider2D != null && collider2D.gameObject.CompareTag("Car"))
        {
            Debug.Log("SOME OBSTRUCTION");
            collider2D = Physics2D.OverlapPoint(new Vector2(collider2D.gameObject.transform.position.x, collider2D.transform.position.y + 3));
            if (collider2D != null && collider2D.gameObject.CompareTag("Car"))
            {
                Debug.Log("Something behind");
            }
            else if (Physics2D.OverlapPoint(new Vector2(collider2D.gameObject.transform.position.x, currentCar.transform.position.y - movementGap)) == null)
            {
                currentCar.transform.position = new Vector2(currentCar.transform.position.x + (laneDifference * 8), currentCar.transform.position.y - movementGap);
                car.currentLane = toSwitchTo;
            }
            else
            {
                Debug.Log("SHIT PANIC THERE IS A CAR WE ALL GONNA DIE IM TOO YOUNG TO DIE");
                return;
            }
            return;
        }
        currentCar.transform.position = new Vector2(currentCar.transform.position.x + (laneDifference * 8), currentCar.transform.position.y);
        car.currentLane = toSwitchTo;
    }*/
}
