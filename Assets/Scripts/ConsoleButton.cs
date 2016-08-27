using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConsoleButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

  private Text text_;
  private Color originalColor_;
  public Color highlightColor;

  private void Awake() {
    text_ = transform.Find("Text").GetComponent<Text>();
    originalColor_ = text_.color;
  }

  public void OnEnable() {
    text_.color = originalColor_;
  }

  public void OnPointerEnter(PointerEventData eventData) {
    text_.color = highlightColor;
  }

  public void OnPointerExit(PointerEventData eventData) {
    text_.color = originalColor_;
  }
}