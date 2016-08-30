public class ShiftLeft : BitmaskOperation {
  override public string Label() {
    return "Shift Left";
  }

  override public int Act(int input) {
    return input << 1;
  }

  override public int LEDIndex() {
    return 0;
  }
}

public class Invert : BitmaskOperation {
  override public string Label() {
    return "Invert";
  }

  override public int Act(int input) {
    return ~input;
  }

  override public int LEDIndex() {
    return 1;
  }
}

public class Add32 : BitmaskOperation {
  override public string Label() {
    return "Add32";
  }

  override public int Act(int input) {
    return input + 32;
  }

  override public int LEDIndex() {
    return 2;
  }
}

public class Xor5 : BitmaskOperation {
  override public string Label() {
    return "Xor5";
  }

  override public int Act(int input) {
    return input ^ 5;
  }

  override public int LEDIndex() {
    return 3;
  }
}

public class BitmaskOperation  {
  public static ShiftLeft shiftLeft = new ShiftLeft();
  public static Add32 add32 = new Add32();
  public static Xor5 xor5 = new Xor5();
  public static Invert invert = new Invert();

  public static BitmaskOperation GetByName(string name) {
    switch (name.ToLower()) {
      case "shiftleft":
        return shiftLeft;
      case "add32":
        return add32;
      case "invert":
        return invert;
      case "xor5":
        return xor5;
    }
    return null;
  }

  virtual public string Label() {
    return "Bitmask Operation";
  }

  virtual public int Act(int input) {
    return input;
  }

  virtual public int LEDIndex() {
    return -1;
  }

  virtual public void Apply(ref BitmaskPuzzle puzzle) {
    puzzle.currentValue = Act(puzzle.currentValue);
  }
}
