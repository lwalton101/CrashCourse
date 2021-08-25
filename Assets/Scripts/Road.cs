using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField] private Lane lane;
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void OnMouseDown()
    {
        GameManager.instance.SwitchLane(lane);
    }

    public enum Lane
	{
        RED,
        PURPLE,
        BLUE
	}
}
