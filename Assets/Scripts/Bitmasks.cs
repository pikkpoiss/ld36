using UnityEngine;
using System.Collections;

public class Bitmasks : MonoBehaviour {
  public Bitmask targetBitmask;
  public Bitmask activeBitmask;

  public void SetPuzzle(BitmaskPuzzle puzzle) {
    activeBitmask.SetMask(puzzle.startValue);
    targetBitmask.SetMask(puzzle.targetValue);
  }
}