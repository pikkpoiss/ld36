using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BitmaskPuzzle {
  private int currentValue_;
  private int maxBits = 8;

  private int targetValue_;
  public int targetValue { 
    get { return targetValue_; }
    private set { targetValue_ = cap(value); }
  }
   
  public int startValue { get; private set; }
  public int currentValue { 
    get { return currentValue_; }
    set { currentValue_ = cap(value); }
  }

  private int cap(int input) {
    int maxValue = (int)Mathf.Pow(2.0f, maxBits) - 1;
    return input & maxValue;
  }

  public bool solved {
    get { return currentValue == targetValue; }
  }

  public void Reset() {
    currentValue = startValue;
  }
    
  public enum Difficulty { Easy = 2, Medium = 5, Difficult = 10 };

  public static BitmaskPuzzle Get(Difficulty difficulty, params BitmaskOperation[] required) {
    return new BitmaskPuzzle(difficulty, new HashSet<BitmaskOperation>(required));
  }

  BitmaskPuzzle(Difficulty diff, HashSet<BitmaskOperation> required) {
    List<BitmaskOperation> operations = new List<BitmaskOperation>(required);
    int addl = (int)Mathf.Max((int)diff, (int)operations.Count) - operations.Count;
    while (addl > 0) {
      operations.Add(operations[Random.Range(0, operations.Count)]);
      addl--;
    }
    startValue = Random.Range(1, 256);
    currentValue = startValue;
    targetValue = startValue;
    foreach (BitmaskOperation operation in operations) {
      targetValue = operation.Act(targetValue);
    }
  }
}
