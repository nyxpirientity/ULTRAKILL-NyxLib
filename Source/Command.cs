using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nyxpiri.ULTRAKILL.NyxLib;

public class StringQueue
{
    public StringQueue(string str)
    {
        Str = str;
    }

    public char Peek()
    {
        return Str[Index];
    }

    public char Pop()
    {
        char c = Str[Index];
        Index += 1;
        return c;
    }

    public string PopUntil(IReadOnlyCollection<char> chars, out char? stopper)
    {
        var str = new StringBuilder();

        stopper = null;

        while (!Finished)
        {
            char c = Peek();

            if (chars.Contains(c))
            {
                stopper = c;
                break;
            }

            str.Append(Pop());
        }

        return str.ToString();
    }

    public void Reset(string str)
    {
        Str = str;
    }

    public bool Finished { get => Str.Length <= Index; }


    private int Index = 0;
    private string Str = null;

    public string GetRemaining()
    {
        return Str[Index..];
    }
}

public struct CommandScope
{
    public string Name { get; private set; } = null;

    public IReadOnlyCollection<char> Flags { get => _Flags; }
    public IReadOnlyDictionary<string, CommandScope> Scopes { get => _Scopes; }
    public IReadOnlyList<string> Parameters { get => _Parameters; }

    public bool HasFlag(char flag)
    {
        return Flags.Contains(flag);
    }

    public bool HasScope(string scopeName)
    {
        return Scopes.ContainsKey(scopeName);
    }

    public void HaveScopeOrThrow(string scopeName, Exception e)
    {
        if (!HasScope(scopeName))
        {
            throw e;
        }
    }

    public bool HasParam(string paramStr)
    {
        return Parameters.Contains(paramStr);
    }

    public bool HasNumParams(int num)
    {
        return Parameters.Count >= num;
    }

    public void HaveNumParamsOrThrow(int num, Exception e)
    {
        if (!HasNumParams(num))
        {
            throw e;
        }
    }

    public bool HasChain(string chainStr)
    {
        var strQueue = new StringQueue(chainStr);

        bool nextIsScope = false;

        CommandScope current = this;

        while (!strQueue.Finished)
        {
            string str = strQueue.PopUntil(['/', ':'], out char? stopper);

            if (str.Length > 0)
            {
                if (nextIsScope)
                {
                    if (!current.Scopes.TryGetValue(str, out var next))
                    {
                        return false;
                    }

                    current = next;
                }
                else
                {
                    if (int.TryParse(str, out var val))
                    {
                        if (!current.HasNumParams(val))
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        throw new FormatException($"invalid int formatting on '{str}', in chainStr {chainStr}");
                    }
                }
            }

            if (stopper.HasValue)
            {
                nextIsScope = stopper.Value == '/';
            }
            else
            {
                nextIsScope = false;
            }

            strQueue.Pop();
        }

        return true;
    }

    public void HaveChainOrThrow(string chainStr, Exception e)
    {
        if (!HasChain(chainStr))
        {
            throw e;
        }
    }

    public CommandScope(string name)
    {
        Name = name;

        _Parameters = [];
        _Flags = [];
        _Scopes = [];
    }

    public CommandScope(StringQueue strQueue, string name)
    {
        Name = name;

        _Parameters = [];
        _Flags = [];
        _Scopes = [];

        ParseScope(strQueue);
    }

    public void ParseScope(StringQueue strQueue)
    {
        bool buildingFlags = false;
        bool isBracketScope = false;
        StringBuilder currentToken = new();

        var self = this;
        var addFlags = () =>
        {
            string flagsString = currentToken.ToString();

            foreach (var c in flagsString)
            {
                self._Flags.Add(c);
            }
        };

        var addNewToken = () =>
        {
            if (currentToken.Length == 0)
            {
                return;
            }

            if (currentToken[^1] == ':')
            {
                currentToken.Remove(currentToken.Length - 1, 1);
                if (!self._Scopes.ContainsKey(currentToken.ToString()))
                {
                    self._Scopes[currentToken.ToString()] = new(strQueue, currentToken.ToString());
                }
                else
                {
                    self._Scopes[currentToken.ToString()].ParseScope(strQueue);
                }
                currentToken.Clear();
                return;
            }

            if (!buildingFlags)
            {
                self._Parameters.Add(currentToken.ToString());
            }
            else
            {
                buildingFlags = false;
                addFlags();
            }

            currentToken.Clear();
        };

        while (!strQueue.Finished)
        {
            char c = strQueue.Peek();
            // todo: escape character and quotes
            switch (c)
            {
                case ' ':
                case '\t':
                case '\n':
                    strQueue.Pop();
                    addNewToken();

                    if (!isBracketScope && (_Parameters.Count > 0 || Scopes.Count > 0 || _Flags.Count > 0))
                    {
                        // we're done!!
                        return;
                    }

                    break;
                case '{':
                    if (currentToken.Length > 0)
                    {
                        // syntax error!
                        throw new FormatException($"Unexpected '{{' after first character of token in CommandScope string");
                    }

                    isBracketScope = true;
                    strQueue.Pop();
                    break;
                case '}':
                    if (isBracketScope)
                    {
                        // we're done!!
                        strQueue.Pop();
                        addNewToken();
                        return;
                    }
                    else
                    {
                        strQueue.Pop();
                        addNewToken();

                        if (_Parameters.Count > 0 || Scopes.Count > 0 || _Flags.Count > 0)
                        {
                            // we're done!!
                            return;
                        }
                        else
                        {
                            throw new FormatException($"Unexpected '}}' in non-bracket-scope CommandScope by name '{Name}'");
                        }
                    }
                case '~':
                    strQueue.Pop();

                    if (currentToken.Length == 0)
                    {
                        buildingFlags = true;
                        break;
                    }

                    currentToken.Append(c);
                    break;
                default:
                    strQueue.Pop();
                    currentToken.Append(c);
                    break;
            }
        }

        addNewToken();
    }

    public string ToDebugString(int depth = 0)
    {
        StringBuilder debugStringBuilder = new(512);
        StringBuilder newLineStringBuilder = new(depth + 1);


        newLineStringBuilder.Append('\n');
        for (int i = 0; i < depth; i++)
        {
            newLineStringBuilder.Append('\t');
        }

        string newLine = newLineStringBuilder.ToString();

        debugStringBuilder.Append($"Flags: -");

        foreach (var flag in Flags)
        {
            debugStringBuilder.Append(flag);
        }

        debugStringBuilder.Append(newLine);
        debugStringBuilder.Append($"Parameters:");

        foreach (var param in Parameters)
        {
            debugStringBuilder.Append(newLine);
            debugStringBuilder.Append($"\t{param}");
        }

        debugStringBuilder.Append(newLine);
        debugStringBuilder.Append($"Scopes:");

        foreach (var scope in Scopes.Values)
        {
            debugStringBuilder.Append(newLine);
            debugStringBuilder.Append($"\t{scope.Name}: {{");
            debugStringBuilder.Append($"{newLine}\t\t");
            debugStringBuilder.Append($"{scope.ToDebugString(depth + 2)}");
            debugStringBuilder.Append($"{newLine}\t}}");
        }

        return debugStringBuilder.ToString();
    }

    private HashSet<char> _Flags = null;
    private Dictionary<string, CommandScope> _Scopes = null;
    private List<string> _Parameters = null;
}

public class Command
{
    public IReadOnlyCollection<char> Flags { get => MainScope.Flags; }

    public IReadOnlyList<string> Parameters { get => MainScope.Parameters; }
    public bool HasParams { get => Parameters.Count > 0; }
    public bool HasNoParams { get => !HasParams; }

    public IReadOnlyDictionary<string, CommandScope> Scopes { get => MainScope.Scopes; }

    public bool HasFlag(char flag) => MainScope.HasFlag(flag);

    public bool HasScope(string scopeName) => MainScope.HasScope(scopeName);
    public void HaveScopeOrThrow(string scopeName, Exception e) => MainScope.HaveScopeOrThrow(scopeName, e);

    public bool HasParam(string paramStr) => MainScope.HasParam(paramStr);
    public bool HasNumParams(int num) => MainScope.HasNumParams(num);
    public void HaveNumParamsOrThrow(int count, Exception e) => MainScope.HaveNumParamsOrThrow(count, e);

    public bool HasChain(string chainStr) => MainScope.HasChain(chainStr);
    public void HaveChainOrThrow(string chainStr, Exception e) => MainScope.HaveChainOrThrow(chainStr, e);

    public CommandScope MainScope { get; private set; }

    public Command()
    {
        MainScope = new();
    }

    public Command(string command)
    {
        ParseCommand(command);
    }

    public void ParseCommand(string command)
    {
        StringQueue cmdStrQueue = new($"{{{command}}}");

        MainScope = new(cmdStrQueue, null);

        if (!cmdStrQueue.Finished)
        {
            string remainingStr = cmdStrQueue.GetRemaining();
            throw new FormatException($"Unexpected continuation of command from where it seemingly should've ended, remaining string is '{remainingStr}'");
        }
    }

    public string ToDebugString()
    {
        return MainScope.ToDebugString();
    }
}