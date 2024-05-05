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
  int index = 0;
  const string[] keywords = { "if",   "else",  "while",  "for",
                              "int",  "float", "string", "bool",
                              "true", "false", "return" };

  List<Lexeme> list = new List<Lexeme>();

  private bool isNumber(string token) {
    Regex regex = new Regex("^\d+(\.\d*)?$");
    return regex.IsMatch(token);
  }

  private bool isString(string token) {
    if (token[0] == '"' && token.EndsWith('"') &&
        token[token.Length - 2] != '\\') {
      for (int i = 1; i <= token.Length - 2; i++) {
        if (token[i] == '"' && token[i - 1] != '\\')
          return false;
      }
      return true;
    }
    return false;
  }
  private bool isIdentifier(string token) {
    if (keywords.Contains(token))
      return false;
    Regex regex = new Regex("^[a-zA-Z_][a-zA-Z0-9_]*$");
    return regex.IsMatch(token);
  }

  private bool isKeyword(string token) { return keywords.Contains(token); }
  private TokenType getTokenType(string token) {
    if (isNumber(token))
      return TokenType.NUMBER;
    if (isString(token))
      return TokenType.STRING;
    if (isIdentifier(token))
      return TokenType.IDENTIFIER;
    if (isKeyword(token))
      return TokenType.KEYWORD;
    return TokenType.OPERATOR;
  }
  public bool MoveNext() {
    index++;
    return index < list.Count;
  }
  public bool isOperator(string token) {
    // TODO: add more operators (reda)
    Regex regex = new Regex("^[+\\-*/%<>=!&|]$");
    return regex.IsMatch(token);
  }
  public void Reset() { index = 0; }

  public Scanner(string source) {
    string temp = "";
    int lineNumber = 0;
    for (int i = 0; i < source.Length; i++) {
      char x = source[i];
      if (source[i] == '\n')
        lineNumber++;
      if (x == ' ' || x == '\n' || x == '\t') {
        if (temp != "") {
          list.Add(new Lexeme { tokenType = TokenType.IDENTIFIER, value = temp,
                                line = lineNumber });
          temp = "";
        }
      } else if (x == ';') {
        if (temp != "") {
          list.Add(new Lexeme { tokenType = getTokenType(temp), value = temp,
                                line = lineNumber });
          temp = "";
        }
        list.Add(new Lexeme { tokenType = TokenType.SEMICOLON, value = ";",
                              line = lineNumber });
      } else if (isOperator(x.ToString())) {
        if (temp != "") {
          list.Add(new Lexeme { tokenType = TokenType.IDENTIFIER, value = temp,
                                line = lineNumber });
          temp = "";
        }
        list.Add(new Lexeme { tokenType = TokenType.OPERATOR,
                              value = x.ToString(), line = lineNumber });
        if (i + 1 < source.Length && isOperator(x.ToString() + source[i + 1])) {
          list.Add(new Lexeme { tokenType = TokenType.OPERATOR,
                                value = x.ToString() + source[i + 1],
                                line = lineNumber });
          i++;
        }
      } else if (x == '"') {
        while (i < source.Length && !isString(temp)) {
          temp += source[i];
          i++;
        }
        if (isString(temp)) {
          list.Add(new Lexeme { tokenType = TokenType.STRING, value = temp,
                                line = lineNumber });
          temp = "";
        }
        // TODO: handle error " athis \"\"
        else {
          Console.Write("");
        }
      } else {
        temp += x;
      }
      list.Add(
          new Lexeme { tokenType = TokenType.OPERATOR, value = "<", line = 0 });
    }

    object IEnumerator.Current {
      get { return Current; }
    }
    public Lexeme Current {
      get { return Current; }
    }
  }
