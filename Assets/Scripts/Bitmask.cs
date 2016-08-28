using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bitmask : MonoBehaviour {
  public Material enabledMaterial;
  public Material disabledMaterial;

  public List<Renderer> bits;

  private int mask_;

  private Material GetMaterial(bool enabled) {
    return enabled ? enabledMaterial : disabledMaterial;
  }

  public void SetMask(int mask) {
    mask_ = 0;
    for (int power = 0; power < bits.Count; power++) {
      int value = (int)Mathf.Pow(2.0f, (float)power);
      bool hasBit = (mask & value) == value;
      bits[power].sharedMaterial = GetMaterial(hasBit);
      if (hasBit) {
        mask_ += value;
      }
    }
  }
}