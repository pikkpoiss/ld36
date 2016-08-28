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
    private int option_;
    private BitmaskOperation operation_ = null;
    private BitmaskPuzzle puzzle_ = null;

    public SelectedOptionAction(int option) {
      option_ = option;
    }

    public SelectedOptionAction(BitmaskOperation operation, int option, ref BitmaskPuzzle puzzle) {
      operation_ = operation;
      option_ = option;
      puzzle_ = puzzle;
    }

    public void Act(Yarn.OptionChooser chooser) {
      Debug.LogFormat("Act on {0} {1}", option_, operation_);
      if (operation_ != null) {
        operation_.Act(ref puzzle_);
      }
      chooser(option_);
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
    //SetContinuePromptActive(true);
    //yield return new WaitForSeconds(2.0f);
    //while (Input.anyKeyDown == false) {
    //  yield return null;
    //}
    //lineText.gameObject.SetActive(false);
    //SetContinuePromptActive(false);
  }

  public override IEnumerator RunOptions(Yarn.Options optionsCollection, Yarn.OptionChooser optionChooser) {
    if (optionsCollection.options.Count > optionButtons.Count) {
      Debug.LogWarning("There are more options to present than there are" +
      "buttons to present them in. This will cause problems.");
    }
    HashSet<BitmaskOperation> operations = new HashSet<BitmaskOperation>();
    int buttonIndex = 0;
    int optionIndex = 0;
    int commandsIndex = 0;
    foreach (var optionString in optionsCollection.options) {
      switch (optionString.Trim()) {
        case "SUCCESS":
          break;
        case "FAILURE":
          break;
        case "COMMANDS":
          operations.UnionWith(storage.EnabledOperations());
          commandsIndex = buttonIndex;
          break;
        case "ABORT":
          operations.Add(BitmaskOperation.abort);
          break;
        default:
          optionButtons[buttonIndex].gameObject.SetActive(true);
          optionButtons[buttonIndex].GetComponentInChildren<Text>().text = optionString;
          SetSelectedOptionMap.Add(buttonIndex, new SelectedOptionAction(optionIndex));
          buttonIndex++;
          break;
      }
      optionIndex++;
    }
    if (operations != null) {
      if (buttonIndex + operations.Count > optionButtons.Count) {
        Debug.LogWarning("Attempting to add options but count is greater than number of buttons!");
      }
      foreach (BitmaskOperation op in operations) {
        optionButtons[buttonIndex].gameObject.SetActive(true);
        optionButtons[buttonIndex].GetComponentInChildren<Text>().text = op.Label();
        SetSelectedOptionMap.Add(buttonIndex, new SelectedOptionAction(op, commandsIndex, ref puzzle_));
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
    SetSelectedOption = null; 
    SetSelectedOptionMap.Clear();
    lineText.text = "";
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
}