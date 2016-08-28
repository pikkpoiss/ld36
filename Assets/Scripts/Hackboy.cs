using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Hackboy : MonoBehaviour {
  public Text screenText;

  private StringBuilder screenTextBuilder = new StringBuilder();
  
  public void SetEnabledOperations(HashSet<BitmaskOperation> operations) {
    screenTextBuilder.Length = 0;
    screenTextBuilder.AppendLine("ENABLED MODULES");
    screenTextBuilder.AppendLine("===============");
    foreach (BitmaskOperation op in operations) {
      screenTextBuilder.AppendLine(op.Label());
    }
    screenText.text = screenTextBuilder.ToString();
  }
}
