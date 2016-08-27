using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Yarn.Unity;

public class DoomClockBehaviorScript : MonoBehaviour {

  private Text text;
  private float seconds_left = -1f;

  [YarnCommand("start")]
  public void StartClock(string seconds) {
    seconds_left = float.Parse(seconds);
    enabled = true;
  }

  [YarnCommand("stop")]
  public void StopClock() {
    seconds_left = -1;
    text.text = "";
    enabled = false;
  }

  // Use this for initialization
  void Start() {
    enabled = false;
    text = GetComponent<Text>();
    text.text = "";
  }
	
  // Update is called once per frame
  void Update() {
    if (enabled) {
      seconds_left -= Time.deltaTime;
      text.text = seconds_left.ToString("#.00");
    }
  }
}
