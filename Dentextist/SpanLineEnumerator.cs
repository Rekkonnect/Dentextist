namespace Dentextist;

public ref struct SpanLineEnumerator
{
    private readonly SpanString _string;
    private int _start;

    public SpanString Current { get; private set; }

    public SpanLineEnumerator(SpanString @string)
    {
        _string = @string;
    }

    public void Reset()
    {
        Current = default;
        _start = 0;
    }

    public bool MoveNext()
    {
        int length = _string.Length;

        if (_start >= length)
            return false;

        int index = _start;
        for (; index < length; index++)
        {
            char c = _string[index];
            if (c is '\r' or '\n')
            {
                Current = _string[_start..index];

                if (c is '\r' && index + 1 < length && _string[index + 1] is '\n')
                {
                    index++;
                }
                _start = index + 1;

                return true;
            }
        }

        // Consume the remaining string that does not contain any further newlines
        if (_start < length)
        {
            Current = _string[_start..length];
            _start = length;
            return true;
        }

        return false;
    }

    public readonly SpanLineEnumerator GetEnumerator() => this;
}
