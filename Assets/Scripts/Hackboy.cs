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
    DisableLEDs();
  }

  private void DisableLEDs() {
    SetLED(LED1, ledDisabledColor);
    SetLED(LED2, ledDisabledColor);
    SetLED(LED3, ledDisabledColor);
    SetLED(LED4, ledDisabledColor);
  }

  public void SetText(string text) {
    screenText.text = text;
  }

  public void SetEnabledOperations(HashSet<BitmaskOperation> operations) {
    DisableLEDs();
    screenTextBuilder.Length = 0;
    screenTextBuilder.AppendLine("ENABLED MODULES");
    screenTextBuilder.AppendLine("===============");
    foreach (BitmaskOperation op in operations) {
      if (op != null) {
        screenTextBuilder.AppendLine(op.Label());
        switch (op.LEDIndex()) {
          case 0:
            SetLED(LED1, ledEnabledColor);
            break;
          case 1:
            SetLED(LED2, ledEnabledColor);
            break;
          case 2:
            SetLED(LED3, ledEnabledColor);
            break;
          case 3:
            SetLED(LED4, ledEnabledColor);
            break;
        }
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
        break;
      case "$hackboy_root_readable":
        SetText("Path /usr/root is now readable!");
        break;
      case "$hackboy_dcrease_readable":
        SetText("Path /usr/dcrease is now readable!");
        break;
      case "$hackboy_won":
        screenTextBuilder.Length = 0;
        screenTextBuilder.AppendLine("==================");
        screenTextBuilder.AppendLine("SPACE PLANS COPIED");
        screenTextBuilder.AppendLine("==================");
        screenTextBuilder.AppendLine("Congrats! Wipe the system and exfiltrate the facility!");
        SetText(screenTextBuilder.ToString());
        break;
      case "$hackboy_lost":
        screenTextBuilder.Length = 0;
        screenTextBuilder.AppendLine("==================");
        screenTextBuilder.AppendLine("      FAILURE     ");
        screenTextBuilder.AppendLine("==================");
        screenTextBuilder.AppendLine("You have been detected! Exfiltrate the facility!");
        SetText(screenTextBuilder.ToString());
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
