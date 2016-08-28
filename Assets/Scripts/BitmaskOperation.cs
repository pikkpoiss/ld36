public class ShiftLeft : BitmaskOperation {
  override public string Label() {
    return "Shift Left";
  }

  override public void Act(ref BitmaskPuzzle puzzle) {
    puzzle.currentValue = puzzle.currentValue << 1;
  }
}

public class ShiftRight : BitmaskOperation {
  override public string Label() {
    return "Shift Right";
  }

  override public void Act(ref BitmaskPuzzle puzzle) {
    puzzle.currentValue = puzzle.currentValue >> 1;
  }
}

public class Negate : BitmaskOperation {
  override public string Label() {
    return "Negate";
  }

  override public void Act(ref BitmaskPuzzle puzzle) {
    puzzle.currentValue = ~puzzle.currentValue;
  }
}

public class Abort : BitmaskOperation {
  override public string Label() {
    return "Abort";
  }
}

public class BitmaskOperation  {
  public static ShiftLeft shiftLeft = new ShiftLeft();
  public static ShiftRight shiftRight = new ShiftRight();
  public static Negate negate = new Negate();
  public static Abort abort = new Abort();

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

  virtual public void Act(ref BitmaskPuzzle puzzle) {
  }
}
