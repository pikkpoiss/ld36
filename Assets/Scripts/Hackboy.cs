using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Hackboy : MonoBehaviour {
  public Text screenText;
  public GameObject LED1;
  public GameObject LED2;
  public GameObject LED3;
  public GameObject LED4;
  public Color ledEnabledColor;
  public Color ledDisabledColor;

  private StringBuilder screenTextBuilder = new StringBuilder();

  public void Awake() {
    SetLED(LED1, ledDisabledColor);
    SetLED(LED2, ledDisabledColor);
    SetLED(LED3, ledDisabledColor);
    SetLED(LED4, ledDisabledColor);
  }

  public void SetText(string text) {
    screenText.text = text;
  }

  public void SetEnabledOperations(HashSet<BitmaskOperation> operations) {
    screenTextBuilder.Length = 0;
    screenTextBuilder.AppendLine("ENABLED MODULES");
    screenTextBuilder.AppendLine("===============");
    foreach (BitmaskOperation op in operations) {
      if (op != null) {
        screenTextBuilder.AppendLine(op.Label());
      }
    }
    screenText.text = screenTextBuilder.ToString();
  }

  public void SetEnabledPuzzle(BitmaskPuzzle puzzle, HashSet<BitmaskOperation> operations) {
    SetEnabledOperations(operations);
  }

  public void SetValue(string key, bool value) {
    switch (key) {
      case "$hackboy_hack_module":
        SetText("Located hack program on $PATH!");
        SetLED(LED1, ledEnabledColor);
        break;
    }
  }

  private void SetLED(GameObject led, Color color) {
    Renderer renderer = led.GetComponent<Renderer>();
    Material mat = renderer.material;
    mat.SetColor("_Color", color);
    mat.SetColor("_EmissionColor", color);
  }
}
