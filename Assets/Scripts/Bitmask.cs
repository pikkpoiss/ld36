using UnityEngine;
using System.Collections;

public class Bitmask : MonoBehaviour {
  public Material enabledMaterial;
  public Material disabledMaterial;

  public Renderer bit1;
  public Renderer bit2;
  public Renderer bit3;
  public Renderer bit4;
  public Renderer bit5;
  public Renderer bit6;
  public Renderer bit7;
  public Renderer bit8;

  void Start() {
    SetMask(93);
  }

  private Material GetMaterial(bool enabled) {
    return enabled ? enabledMaterial : disabledMaterial;
  }

  public void SetMask(uint mask) {
    bit1.sharedMaterial = GetMaterial((mask & 1) == 1);
    bit2.sharedMaterial = GetMaterial((mask & 2) == 2);
    bit3.sharedMaterial = GetMaterial((mask & 4) == 4);
    bit4.sharedMaterial = GetMaterial((mask & 8) == 8);
    bit5.sharedMaterial = GetMaterial((mask & 16) == 16);
    bit6.sharedMaterial = GetMaterial((mask & 32) == 32);
    bit7.sharedMaterial = GetMaterial((mask & 64) == 64);
    bit8.sharedMaterial = GetMaterial((mask & 128) == 128);
  }
}