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
  public Camera screenCamera;
  public Color[] screenColors;
  private int screenColorIndex;

  private StringBuilder screenTextBuilder = new StringBuilder();

  public void Awake() {
    DisableLEDs();
    screenColorIndex = -1;
    SetText("HACKBOY OS\n(C)1982");
  }

  private void DisableLEDs() {
    SetLED(LED1, ledDisabledColor);
    SetLED(LED2, ledDisabledColor);
    SetLED(LED3, ledDisabledColor);
    SetLED(LED4, ledDisabledColor);
  }

  public void SetText(string text) {
    if (screenText.text == text) {
      return;
    }
    screenColorIndex = (screenColorIndex + 1) % screenColors.Length;
    screenCamera.backgroundColor = screenColors[screenColorIndex];
    screenText.text = text;
  }

  public void SetEnabledOperations(HashSet<BitmaskOperation> operations) {
    screenTextBuilder.Length = 0;
    screenTextBuilder.AppendLine("ENABLED MODULES");
    screenTextBuilder.AppendLine("===============");
    foreach (BitmaskOperation op in operations) {
      if (op != null) {
        screenTextBuilder.AppendLine(op.Label());
        switch (op.LEDIndex()) {
          case 0:
            // Enabled by default so we won't add an LED.
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
    SetText(screenTextBuilder.ToString());
  }

  public void SetEnabledPuzzle(BitmaskPuzzle puzzle, HashSet<BitmaskOperation> enabled) {
    if (puzzle.HasRequiredModules(enabled)) {
      SetEnabledOperations(enabled);
    } else {
      screenTextBuilder.Length = 0;
      screenTextBuilder.AppendLine("==================");
      screenTextBuilder.AppendLine("      WARNING     ");
      screenTextBuilder.AppendLine("==================");
      screenTextBuilder.AppendLine("Required module missing! Hack may not be possible!");
      SetText(screenTextBuilder.ToString());
    }
  }

  public void SetValue(string key, bool value) {
    switch (key) {
      case "$hackboy_hack_module":
        SetText("Located hack program on $PATH!");
        SetLED(LED1, ledEnabledColor);
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
