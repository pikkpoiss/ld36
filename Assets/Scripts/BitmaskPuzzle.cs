using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BitmaskPuzzle {
  public int targetValue { get; private set; }
  public int startValue { get; private set; }
  public int currentValue { get; set; }

  public void Reset() {
    currentValue = startValue;
  }
    
  public enum Difficulty { Easy = 2, Medium = 5, Difficult = 10 };

  public static BitmaskPuzzle Get(Difficulty difficulty, params BitmaskOperation[] required) {
    return new BitmaskPuzzle(difficulty, new HashSet<BitmaskOperation>(required));
  }

  BitmaskPuzzle(Difficulty diff, HashSet<BitmaskOperation> required) {
    startValue = Random.Range(1, 256);
    currentValue = startValue;
    targetValue = Random.Range(1, 256); // TODO: Determine from applying operations.
  }
}
