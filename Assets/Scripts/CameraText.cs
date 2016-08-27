using UnityEngine;
using System.Collections;

public class CameraText : MonoBehaviour {
  private CRT crt_;
 
  void Start() {
    crt_ = GetComponent<CRT>();
  }

  void Update() {
    float v = Mathf.PingPong(Time.time, 2.0f);
    crt_.CurvatureSet1 = v;
  }
}
