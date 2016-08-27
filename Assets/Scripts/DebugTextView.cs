using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class DebugTextView : MonoBehaviour {
  private Text text_;

  private void Awake() {
    text_ = GetComponent<Text>();
  }

  [YarnCommand("toggle")]
  public void Toggle() {
    text_.enabled = !text_.enabled;
  }
}
