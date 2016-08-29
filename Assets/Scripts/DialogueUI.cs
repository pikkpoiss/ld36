using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : Yarn.Unity.DialogueUIBehaviour {
  public DialogueStorage storage;
  public GameObject dialogueContainer;
  public GameObject continuePrompt;
  public Text lineText;
  public List<Button> optionButtons;
  public RectTransform gameControlsContainer;
  public Bitmasks bitmasks;

  private BitmaskPuzzle puzzle_;
  private Yarn.OptionChooser SetSelectedOption;
  private Dictionary<int, SelectedOptionAction> SetSelectedOptionMap = new Dictionary<int, SelectedOptionAction>();

  private class SelectedOptionAction {
    public string label_;
    private BitmaskOperation operation_ = null;
    private BitmaskPuzzle puzzle_ = null;
    private int normalOption_;
    private int successOption_;
    private int failureOption_;

    public SelectedOptionAction(int option, string label) {
      normalOption_ = option;
      label_ = label;
    }

    public SelectedOptionAction(BitmaskOperation op, ref BitmaskPuzzle puzzle, int option, int success, int failure) {
      operation_ = op;
      puzzle_ = puzzle;
      label_ = op.Label();
      normalOption_ = option;
      successOption_ = success;
      failureOption_ = failure;
    }

    public void Act(Yarn.OptionChooser chooser) {
      if (operation_ != null) {
        operation_.Apply(ref puzzle_);
        if (puzzle_.solved) {
          Debug.Log("Puzzle was solved");
          chooser(successOption_);
          return;
        }
        // TODO: Failure mode.
      }
      chooser(normalOption_);
    }

    public string label {
      get { return label_; }
    }
  }

  [Tooltip("How quickly to show the text, in seconds per character")]
  public float textSpeed = 0.025f;

  private void SetDialogueContainerActive(bool value) {
    if (dialogueContainer != null) {
      dialogueContainer.SetActive(value);
    }
  }

  private void SetContinuePromptActive(bool value) {
    if (continuePrompt != null) {
      continuePrompt.SetActive(value);
    }
  }

  void Awake() {
    lineText.text = "";
    SetDialogueContainerActive(false);
    SetContinuePromptActive(false);
    foreach (var button in optionButtons) {
      button.gameObject.SetActive(false);
    }
    lineText.gameObject.SetActive(false);
    bitmasks.gameObject.SetActive(false);
  }

  private string CheckVars(string input) {
    string output = string.Empty;
    bool checkingVar = false;
    string currentVar = string.Empty;
    int index = 0;
    while (index < input.Length) {
      if (input[index] == '[') {
        checkingVar = true;
        currentVar = string.Empty;
      } else if (input[index] == ']') {
        checkingVar = false;
        output += ParseVariable(currentVar);
        currentVar = string.Empty;
      } else if (checkingVar) {
        currentVar += input[index];
      } else {
        output += input[index];
      }
      index += 1;
    }
    return output;
  }

  string ParseVariable(string varName) {
    if (storage.GetValue(varName) != Yarn.Value.NULL) {
      return storage.GetValue(varName).AsString;
    }
    if (varName == "$time") {
      return Time.time.ToString();
    }
    return varName;
  }

  public override IEnumerator RunLine(Yarn.Line line) {
    lineText.gameObject.SetActive(true);
    var renderedText = "\n" + CheckVars(line.text);
    if (textSpeed > 0.0f) {
      var stringBuilder = new StringBuilder();
      stringBuilder.Append(lineText.text);
      foreach (char c in renderedText) {
        stringBuilder.Append(c);
        lineText.text = stringBuilder.ToString();
        yield return new WaitForSeconds(textSpeed);
      }
    } else {
      lineText.text = lineText.text + renderedText;
    }
  }

  public override IEnumerator RunOptions(Yarn.Options optionsCollection, Yarn.OptionChooser optionChooser) {
    if (optionsCollection.options.Count > optionButtons.Count) {
      Debug.LogWarning("There are more options to present than there are" +
      "buttons to present them in. This will cause problems.");
    }
    List<SelectedOptionAction> actions = new List<SelectedOptionAction>();
    int optionIndex = 0;
    int successIndex = -1;
    int failureIndex = -1;
    foreach (var optionString in optionsCollection.options) {
      switch (optionString.Trim()) {
        case "SUCCESS":
          successIndex = optionIndex;
          break;
        case "FAILURE":
          failureIndex = optionIndex;
          break;
      }
      optionIndex++;
    }
    optionIndex = 0;
    foreach (var optionString in optionsCollection.options) {
      switch (optionString.Trim()) {
        case "COMMANDS":
          foreach (BitmaskOperation op in storage.EnabledOperations()) {
            actions.Add(new SelectedOptionAction(op, ref puzzle_, optionIndex, successIndex, failureIndex));
          }
          break;
        case "ABORT":
          actions.Add(new SelectedOptionAction(optionIndex, "Abort"));
          break;
        case "SUCCESS":
          break;
        case "FAILURE":
          break;
        default:
          actions.Add(new SelectedOptionAction(optionIndex, optionString));
          break;
      }
      optionIndex++;
    }
    if (actions.Count > 0) {
      int buttonIndex = 0;
      if (actions.Count > optionButtons.Count) {
        Debug.LogWarning("Attempting to add options but count is greater than number of buttons!");
      }
      foreach (SelectedOptionAction action in actions) {
        optionButtons[buttonIndex].gameObject.SetActive(true);
        optionButtons[buttonIndex].GetComponentInChildren<Text>().text = action.label;
        SetSelectedOptionMap.Add(buttonIndex, action);
        buttonIndex++;
      }
    }
    if (optionsCollection.options.Count > 0) {
      optionButtons[0].GetComponent<Button>().Select();
    }
    SetSelectedOption = optionChooser;
    while (SetSelectedOption != null) {
      yield return null;
    }
    DisableButtons();
  }

  private void DisableButtons() {
    foreach (var button in optionButtons) {
      // Change selection or else the selected choice won't be highlighted on the subsequent screen.
      button.GetComponent<Button>().Select();
      button.gameObject.SetActive(false);
    }
  }

  public void SetOption(int selectedOption) {
    SelectedOptionAction action;
    if (SetSelectedOptionMap.TryGetValue(selectedOption, out action)) {
      action.Act(SetSelectedOption);
    } else {
      Debug.LogWarningFormat("Did not find selected option {0} in map!", selectedOption);
      SetSelectedOption(selectedOption);
    }
    ClearOptionMap();
    ClearLineText();
  }

  public override IEnumerator RunCommand(Yarn.Command command) {
    Debug.Log("Command: " + command.text);
    yield break;
  }

  public override IEnumerator DialogueStarted() {
    Debug.Log("Dialogue starting!");
    SetDialogueContainerActive(true);
    if (gameControlsContainer != null) {
      gameControlsContainer.gameObject.SetActive(false);
    }
    yield break;
  }

  public override IEnumerator DialogueComplete() {
    Debug.Log("Complete!");
    lineText.gameObject.SetActive(false);
    SetContinuePromptActive(false);
    SetDialogueContainerActive(false);
    if (gameControlsContainer != null) {
      gameControlsContainer.gameObject.SetActive(true);
    }
    yield break;
  }

  public void SetPuzzle(ref BitmaskPuzzle puzzle) {
    bitmasks.gameObject.SetActive(true);
    bitmasks.SetPuzzle(puzzle);
    puzzle_ = puzzle;
  }

  public void ClearPuzzle() {
    if (puzzle_ != null) {
      puzzle_.Reset();
    }
    bitmasks.SetPuzzle(null);
    bitmasks.gameObject.SetActive(false);
    puzzle_ = null;
  }

  private void ClearOptionMap() {
    SetSelectedOption = null; 
    SetSelectedOptionMap.Clear();
  }

  private void ClearLineText() {
    lineText.text = "";
  }

  public void ResetAll() {
    ClearOptionMap();
    ClearLineText();
    ClearPuzzle();
    DisableButtons();
    SetDialogueContainerActive(false);
    SetContinuePromptActive(false);
  }
}