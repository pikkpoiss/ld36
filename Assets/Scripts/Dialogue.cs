using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Yarn.Unity;

public class Dialogue : MonoBehaviour {
  public DialogueStorage storage;
  public DialogueUI ui;
  public DialogueRunner runner;

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
          BitmaskOperation.shiftLeft,
          BitmaskOperation.xor5,
          BitmaskOperation.invert
        ) 
      },
      {
        "hack_spaceplans",
        BitmaskPuzzle.Get(
          BitmaskPuzzle.Difficulty.Difficult,
          BitmaskOperation.shiftLeft,
          BitmaskOperation.xor5,
          BitmaskOperation.add32,
          BitmaskOperation.invert
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
    storage.ClearPuzzle();
  }

  [YarnCommand("setpuzzle")]
  public void SetPuzzle(string key) {
    BitmaskPuzzle puzzle;
    if (!puzzles_.TryGetValue(key, out puzzle)) {
      Debug.LogErrorFormat("Could not find puzzle with key {0}", key);
      return;
    }
    ui.SetPuzzle(ref puzzle);
    storage.EnablePuzzle(ref puzzle);
  }

  public void OnDoomClockExpired() {
    ui.ResetAll();
    runner.startNode = "Fail";
    runner.ResetDialogue();
  }
}
