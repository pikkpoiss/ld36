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
      { 
        "hack_joke",
        BitmaskPuzzle.Get(
          BitmaskPuzzle.Difficulty.Easy,
          BitmaskOperation.shiftLeft
        ) 
      },
      { 
        "hack_xor5",
        BitmaskPuzzle.Get(
          BitmaskPuzzle.Difficulty.Medium,
          BitmaskOperation.add32,
          BitmaskOperation.shiftLeft
        ) 
      },
      { 
        "hack_email",
        BitmaskPuzzle.Get(
          BitmaskPuzzle.Difficulty.Difficult, 
          BitmaskOperation.shiftLeft
        ) 
      },
      {
        "hack_root",
        BitmaskPuzzle.Get(
          BitmaskPuzzle.Difficulty.Difficult,
          BitmaskOperation.shiftLeft
        ) 
      }
    };
  }

  [YarnCommand("exit")]
  public void Exit() {
    Application.Quit();
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
    storage.SetValue("$puzzle_active", GetPuzzleValue(puzzle.currentValue));
    ui.SetPuzzle(ref puzzle);
  }
    
  private string GetPuzzleValue(int value) {
    return value.ToString("X4");
  }
}
