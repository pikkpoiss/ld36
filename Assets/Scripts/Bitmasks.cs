using UnityEngine;
using System.Collections;

public class Bitmasks : MonoBehaviour {
  public Bitmask targetBitmask;
  public Bitmask activeBitmask;

  private BitmaskPuzzle puzzle_;

  public void SetPuzzle(BitmaskPuzzle puzzle) {
    puzzle_ = puzzle;
  }

  private void Update() {
    if (puzzle_ != null) {
      activeBitmask.SetMask(puzzle_.currentValue);
      targetBitmask.SetMask(puzzle_.targetValue);
    }
  }
}