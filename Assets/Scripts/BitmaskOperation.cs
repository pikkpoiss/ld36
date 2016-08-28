public class ShiftLeft : BitmaskOperation {
  override public string Label() {
    return "Shift Left";
  }

  override public int Act(int input) {
    return input << 1;
  }
}

public class ShiftRight : BitmaskOperation {
  override public string Label() {
    return "Shift Right";
  }

  override public int Act(int input) {
    return input >> 1;
  }
}

public class Negate : BitmaskOperation {
  override public string Label() {
    return "Negate";
  }

  override public int Act(int input) {
    return ~input;
  }
}

public class BitmaskOperation  {
  public static ShiftLeft shiftLeft = new ShiftLeft();
  public static ShiftRight shiftRight = new ShiftRight();
  public static Negate negate = new Negate();

  public static BitmaskOperation GetByName(string name) {
    switch (name.ToLower()) {
      case "shiftleft":
        return shiftLeft;
      case "shiftright":
        return shiftRight;
      case "negate":
        return negate;
    }
    return null;
  }

  virtual public string Label() {
    return "Bitmask Operation";
  }

  virtual public int Act(int input) {
    return input;
  }

  virtual public void Apply(ref BitmaskPuzzle puzzle) {
    puzzle.currentValue = Act(puzzle.currentValue);
  }
}
