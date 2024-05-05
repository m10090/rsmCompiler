using System.Text.RegularExpressions;

namespace Lexer;

public enum TokenType {
  NUMBER,
  STRING,
  IDENTIFIER,
  KEYWORD,
  OPERATOR,
  SEMICOLON,
  EOF
}

public record Lexeme {
  public TokenType tokenType;
  public string value = null!;
  public int line;
}

public class Scanner : IEnumerator {
  int i = 0;
  List<Lexeme> list = new List<Lexeme>();

  private bool isNumber(string token) {
    Regex regex = new Regex(@"^\d+$");
    return regex.IsMatch(token);
  }

  private bool isString(string token) { return false; }

  public bool MoveNext() {
    i++;
    return i < list.Count;
  }

  public void Reset() { i = 0; }

  public Scanner(string source) {
    Current.Add(
        new Lexeme { tokenType = TokenType.OPERATOR, value = "<", line = 0 });
  }

  object IEnumerator.Current {
    get { return Current; }
  }
  public Lexeme Current {
    get { return Current; }
  }
}
