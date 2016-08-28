using UnityEngine;
using System.Collections;

public class CameraText : MonoBehaviour {
  private CRT crt_;
 
  void Start() {
    crt_ = GetComponent<CRT>();
  }

  void Update() {
    // Mathf.PingPong(Time.time, 1.0f)
    crt_.CurvatureSet1 = 1.0f + (Random.value / 2.0f);
    crt_.CurvatureSet2 = 3.0f + (Random.value / 2.0f);
  }
}
