using System;
using System.Collections.Generic;

// Generated with Claude Sonnet 4.6

public sealed class Trie
{
    private const int Alphabet = 128;

    private readonly List<int[]> _children = new();
    private readonly List<int>   _terminalLength = new();

    public Trie()
    {
        _children.Add(new int[Alphabet]);
        _terminalLength.Add(-1);
    }
    public void Insert(string prefix)
    {
        if (string.IsNullOrEmpty(prefix))
        {
            throw new ArgumentException("Prefix must be a non-empty string.", nameof(prefix));
        }

        int node = 0;
        for (int i = 0; i < prefix.Length; i++)
        {
            char c = prefix[i];
            if (c >= Alphabet)
            {
                throw new ArgumentException($"Non-ASCII character '{c}' at index {i}.", nameof(prefix));
            }

            ref int child = ref _children[node][c];
            if (child == 0)
            {
                child = _children.Count;
                _children.Add(new int[Alphabet]);
                _terminalLength.Add(-1);
            }
            node = child;
        }

        _terminalLength[node] = prefix.Length;
    }
    
    public bool FindPrefix(ReadOnlySpan<char> input, out string prefix)
    {
        int node      = 0;
        int lastMatch = -1;

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];
            if (c >= Alphabet) break;

            int next = _children[node][c];
            if (next == 0) break;

            node = next;

            if (_terminalLength[node] >= 0)
            {
                lastMatch = _terminalLength[node];
            }
        }

        if (lastMatch >= 0)
        {
            prefix = input[..lastMatch].ToString();
            return true;
        }
        else
        {
            prefix = null;
            return false;
        }
    }

    public bool HasPrefix(ReadOnlySpan<char> input)
    {
        int node = 0;

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];
            if (c >= Alphabet) return false;

            int next = _children[node][c];
            if (next == 0) return false;

            node = next;

            if (_terminalLength[node] >= 0)
            {
                return true;
            }
        }

        return false;
    }
}