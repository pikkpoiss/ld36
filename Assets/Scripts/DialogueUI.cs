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

  private Yarn.OptionChooser SetSelectedOption;

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
    SetDialogueContainerActive(false);
    SetContinuePromptActive(false);
    foreach (var button in optionButtons) {
      button.gameObject.SetActive (false);
    }
    lineText.gameObject.SetActive(false);
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
    var renderedText = CheckVars(line.text);
    if (textSpeed > 0.0f) {
      var stringBuilder = new StringBuilder();
      foreach (char c in renderedText) {
        stringBuilder.Append(c);
        lineText.text = stringBuilder.ToString();
        yield return new WaitForSeconds(textSpeed);
      }
    } else {
      lineText.text = renderedText;
    }
    SetContinuePromptActive(true);
    yield return new WaitForSeconds(2.0f);
    while (Input.anyKeyDown == false) {
      yield return null;
    }
    lineText.gameObject.SetActive(false);
    SetContinuePromptActive(false);
  }

  public override IEnumerator RunOptions(Yarn.Options optionsCollection, Yarn.OptionChooser optionChooser) {
    if (optionsCollection.options.Count > optionButtons.Count) {
      Debug.LogWarning("There are more options to present than there are" +
      "buttons to present them in. This will cause problems.");
    }
    int i = 0;
    Debug.Log("Options Count " + optionsCollection.options.Count);
    foreach (var optionString in optionsCollection.options) {
      optionButtons[i].gameObject.SetActive(true);
      optionButtons[i].GetComponentInChildren<Text>().text = optionString;
      i++;
    }
    SetSelectedOption = optionChooser;
    while (SetSelectedOption != null) {
      yield return null;
    }
    foreach (var button in optionButtons) {
      button.gameObject.SetActive(false);
    }
  }

  public void SetOption(int selectedOption) {
    SetSelectedOption(selectedOption);
    SetSelectedOption = null; 
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
    SetDialogueContainerActive(false);
    if (gameControlsContainer != null) {
      gameControlsContainer.gameObject.SetActive(true);
    }
    yield break;
  }
}