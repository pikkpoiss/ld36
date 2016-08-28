using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueStorage : VariableStorageBehaviour {
  public Computer computer;
  public Hackboy hackboy;

  private Dictionary<string, Yarn.Value> variables = new Dictionary<string, Yarn.Value>();

  private HashSet<BitmaskOperation> enabledOperations_ = new HashSet<BitmaskOperation>();

  [System.Serializable]
  public class DefaultVariable {
    public string name;
    public string value;
    public Yarn.Value.Type type;
  }
    
  public DefaultVariable[] defaultVariables;

  [Header("Optional debugging tools")]
  public UnityEngine.UI.Text debugTextView;

  void Awake() {
    ResetToDefaults();
  }
    
  public override void ResetToDefaults() {
    Clear();
    foreach (var variable in defaultVariables) {
      object value;
      switch (variable.type) {
        case Yarn.Value.Type.Number:
          float f = 0.0f;
          float.TryParse(variable.value, out f);
          value = f;
          break;
        case Yarn.Value.Type.String:
          value = variable.value;
          break;
        case Yarn.Value.Type.Bool:
          bool b = false;
          bool.TryParse(variable.value, out b);
          value = b;
          break;
        case Yarn.Value.Type.Variable:
          Debug.LogErrorFormat("Can't set variable {0} to {1}: You can't " +
            "set a default variable to be another variable, because it " +
            "may not have been initialised yet.", variable.name, variable.value);
          continue;
        case Yarn.Value.Type.Null:
          value = null;
          break;
        default:
          throw new System.ArgumentOutOfRangeException();
      }
      var v = new Yarn.Value(value);
      SetValue("$" + variable.name, v);
    }
    if (debugTextView != null) {
      debugTextView.enabled = false;
    }
    if (Debug.isDebugBuild || Application.isEditor) {
      SetValue("$debug", new Yarn.Value(true));
    }
  }
    
  public void SetValue(string variableName, string value) {
    SetValue(variableName, new Yarn.Value(value));
  }

  public override void SetValue(string variableName, Yarn.Value value) {
    variables[variableName] = new Yarn.Value(value);
    if (variableName.StartsWith("$computer_")) {
      switch (value.type) {
        case Yarn.Value.Type.Bool:
          computer.SetValue(variableName, value.AsBool);
          break;
      }
    }
  }

  public override Yarn.Value GetValue(string variableName) {
    if (variables.ContainsKey(variableName) == false) {
      return Yarn.Value.NULL;
    }
    return variables[variableName];
  }
    
  public override void Clear() {
    variables.Clear();
  }
    
  void Update() {
    if (debugTextView != null && debugTextView.enabled) {
      var stringBuilder = new System.Text.StringBuilder();
      foreach (KeyValuePair<string,Yarn.Value> item in variables) {
        stringBuilder.AppendLine(string.Format("{0} = {1}", 
          item.Key, 
          item.Value));
      }
      debugTextView.text = stringBuilder.ToString();
    }
  }

  public void EnableOperation(string name) {
    BitmaskOperation op = BitmaskOperation.GetByName(name);
    enabledOperations_.Add(op);
    hackboy.SetEnabledOperations(enabledOperations_);
  }

  public HashSet<BitmaskOperation> EnabledOperations() {
    return enabledOperations_;
  }
}