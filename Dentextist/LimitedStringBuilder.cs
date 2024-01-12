namespace Dentextist;

public class LimitedStringBuilder
{
    private char[] _buffer;
    private int _length;
    private int _capacity;

    public int Length => _length;
    public int Capacity => _capacity;

    public string NewLine { get; set; } = NewLines.CRLF;
    public double GrowFactor { get; set; } = 2;

    public LimitedStringBuilder()
        : this(16) { }

    public LimitedStringBuilder(int initialCapacity)
    {
        if (initialCapacity <= 0)
            throw new ArgumentException("Initial capacity must be greater than zero.", nameof(initialCapacity));

        _buffer = new char[initialCapacity];
        _capacity = initialCapacity;
        _length = 0;
    }

    public LimitedStringBuilder Append(string text)
    {
        EnsureCapacity(text.Length);
        text.CopyTo(0, _buffer, _length, text.Length);
        _length += text.Length;
        return this;
    }

    public LimitedStringBuilder Append(char c)
    {
        EnsureCapacity(1);
        _buffer[_length] = c;
        _length++;
        return this;
    }

    public LimitedStringBuilder Append(char c, int count)
    {
        if (count <= 0)
            return this;

        EnsureCapacity(count);
        for (int i = 0; i < count; i++)
        {
            _buffer[_length + i] = c;
        }
        _length += count;
        return this;
    }

    public LimitedStringBuilder Append(Span<char> span)
    {
        return Append((SpanString)span);
    }

    public LimitedStringBuilder Append(SpanString span)
    {
        EnsureCapacity(span.Length);
        span.CopyTo(new Span<char>(_buffer, _length, span.Length));
        _length += span.Length;
        return this;
    }

    public LimitedStringBuilder AppendLine(string text)
    {
        return Append(text).AppendLine();
    }
    public LimitedStringBuilder AppendLine(char text)
    {
        return Append(text).AppendLine();
    }
    public LimitedStringBuilder AppendLine(Span<char> text)
    {
        return Append(text).AppendLine();
    }
    public LimitedStringBuilder AppendLine(SpanString text)
    {
        return Append(text).AppendLine();
    }

    public LimitedStringBuilder AppendLine()
    {
        return Append(NewLine);
    }

    public override string ToString()
    {
        return new string(_buffer, 0, _length);
    }

    private void EnsureCapacity(int additionalLength)
    {
        if (_length + additionalLength <= _capacity)
            return;

        int grownCapacity = (int)(_capacity * GrowFactor);
        int newCapacity = Math.Max(_length + additionalLength, grownCapacity);
        ResizeBuffer(newCapacity);
    }

    private void ResizeBuffer(int newCapacity)
    {
        char[] newBuffer = new char[newCapacity];
        _buffer.CopyTo(newBuffer, 0);
        _buffer = newBuffer;
        _capacity = newCapacity;
    }
}
