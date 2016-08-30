using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BitmaskPuzzle {
  private int currentValue_;
  private int maxBits = 8;
  private HashSet<BitmaskOperation> required_;

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
    
  public enum Difficulty { Easy = 2, Medium = 4, Difficult = 6 };

  public static BitmaskPuzzle Get(Difficulty difficulty, params BitmaskOperation[] required) {
    return new BitmaskPuzzle(difficulty, new HashSet<BitmaskOperation>(required));
  }

  BitmaskPuzzle(Difficulty diff, HashSet<BitmaskOperation> required) {
    required_ = required;
    List<BitmaskOperation> operations = new List<BitmaskOperation>(required);
    int addl = (int)Mathf.Max((int)diff, (int)operations.Count) - operations.Count;
    while (addl > 0) {
      operations.Add(operations[Random.Range(0, operations.Count)]);
      addl--;
    }
    int attempts = 0;
    do {
      startValue = Random.Range(1, 256);
      currentValue = startValue;
      targetValue = startValue;
      foreach (BitmaskOperation operation in operations) {
        targetValue = operation.Act(targetValue);
      }
      attempts++;
    } while (targetValue == startValue && attempts < 10);
    if (attempts == 10) {
      Debug.LogFormat("Couldn't generate a unique target != start value puzzle!  Operations were {0}", required);
    }
  }

  public bool HasRequiredModules(HashSet<BitmaskOperation> enabled) {
    Debug.LogFormat("HasRequiredModules {0} of {1} is {2}", required_.ToString(), enabled.ToString(), required_.IsSubsetOf(enabled));
    return required_.IsSubsetOf(enabled);
  }
}
