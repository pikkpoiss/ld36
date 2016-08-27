using UnityEngine;
using System.Collections;
using Yarn.Unity;

public class DoomClockBehaviorScript : MonoBehaviour {

	private int seconds_left = -1;

	[YarnCommand("start")]
	public void StartClock(string seconds) {
		seconds_left = int.Parse (seconds);
	}

	[YarnCommand("stop")]
	public void StopClock() {
		seconds_left = -1;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
