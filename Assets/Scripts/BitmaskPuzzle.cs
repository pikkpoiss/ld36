using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BitmaskPuzzle {
  public int targetValue;
  public int startValue;

  public enum Difficulty { Easy = 2, Medium = 5, Difficult = 10 };
  public enum Operation { ShiftLeft = 1, ShiftRight = 2, Negate = 4 };

  public static BitmaskPuzzle Get(Difficulty difficulty, params BitmaskPuzzle.Operation[] required) {
    return new BitmaskPuzzle(difficulty, new HashSet<BitmaskPuzzle.Operation>(required));
  }

  BitmaskPuzzle(Difficulty diff, HashSet<Operation> required) {
    startValue = Random.Range(1, 256);
    targetValue = Random.Range(1, 256); // TODO: Determine from applying operations.
  }
}
