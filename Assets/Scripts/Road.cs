using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField] private Lane lane = Lane.Blue;

    // Update is called once per frame
    void OnMouseDown()
    {
        GameManager.instance.SwitchLane(lane);
    }

    public enum Lane
	{
        Red,
        Purple,
        Blue
	}
}
