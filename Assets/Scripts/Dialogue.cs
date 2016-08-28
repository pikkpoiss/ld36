using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Yarn.Unity;

public class Dialogue : MonoBehaviour {
  public DialogueStorage storage;
  public DialogueUI ui;

  private Dictionary<string, BitmaskPuzzle> puzzles_;

  private void Awake() {
    puzzles_ = new Dictionary<string, BitmaskPuzzle>() {
      { "intro", BitmaskPuzzle.Get(BitmaskPuzzle.Difficulty.Easy, BitmaskOperation.shiftLeft) }
    };
  }
    
  [YarnCommand("enable")]
  public void Enable(string name) {
    storage.EnableOperation(name);
  }

  [YarnCommand("clearpuzzle")]
  public void ClearPuzzle() {
    ui.ClearPuzzle();
    storage.SetValue("$puzzle_target", Yarn.Value.NULL);
    storage.SetValue("$puzzle_active", Yarn.Value.NULL);
  }

  [YarnCommand("setpuzzle")]
  public void SetPuzzle(string key) {
    BitmaskPuzzle puzzle;
    if (!puzzles_.TryGetValue(key, out puzzle)) {
      Debug.LogErrorFormat("Could not find puzzle with key {0}", key);
      return;
    }
    storage.SetValue("$puzzle_target", GetPuzzleValue(puzzle.targetValue));
    storage.SetValue("$puzzle_active", GetPuzzleValue(puzzle.startValue));
    ui.SetPuzzle(ref puzzle);
  }


    
  private string GetPuzzleValue(int value) {
    return value.ToString("X4");
  }
}
