using UnityEngine;
using System.Collections;

public class Computer : MonoBehaviour {
  public Color ledEnabledColor;
  public Color ledDisabledColor;

  public GameObject monitorLED1;
  public GameObject monitorLED2;
  public GameObject monitorLED3;

  public void SetValue(string key, bool value) {
    Debug.Log("Computer setting " + key + " to " + value);
    Color target = (value) ? ledEnabledColor : ledDisabledColor;
    switch (key) {
      case "$computer_led_1":
        SetLED(monitorLED1, target);
        break;
      case "$computer_led_2":
        SetLED(monitorLED2, target);
        break;
      case "$computer_led_3":
        SetLED(monitorLED3, target);
        break;
    }
  }

  private void SetLED(GameObject led, Color color) {
    Renderer renderer = led.GetComponent<Renderer>();
    Material mat = renderer.material;
    mat.SetColor("_Color", color);
    mat.SetColor("_EmissionColor", color);
  }
    
  void Update() {
    //float emission = Mathf.PingPong (Time.time, 1.0f);
    //Color baseColor = Color.yellow;
    //Color finalColor = baseColor * Mathf.LinearToGammaSpace (emission);
  }
}
