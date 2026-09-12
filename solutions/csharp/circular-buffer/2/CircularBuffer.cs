public class CircularBuffer<T>
{
    private readonly int _capacity;
    private readonly T[] _buffer;
    private int _count;
    private int _readCursor;
    private int _writeCursor;

    public CircularBuffer(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(capacity);
        _capacity = capacity;
        _buffer = new T[_capacity];
    }

    public T Read()
    {
        if (_count == 0)
        {
            throw new InvalidOperationException("Buffer is empty.");
        }

        var item = _buffer[_readCursor];

        _buffer[_readCursor] = default!;
        _count--;

        _readCursor = (_readCursor + 1) % _capacity;
        return item;
    }

    public void Write(T item)
    {
        if (_count == _capacity)
        {
            throw new InvalidOperationException("Buffer is full.");
        }

        _buffer[_writeCursor] = item;
        _count++;

        _writeCursor = (_writeCursor + 1) % _capacity;
    }

    public void Overwrite(T item)
    {
        if (_count < _capacity)
        {
            Write(item);
            return;
        }

        _buffer[_readCursor] = item;
        _readCursor = (_readCursor + 1) % _capacity;
        _writeCursor = _readCursor;
    }

    public void Clear()
    {
        Array.Clear(_buffer);
        _count = 0;
        _readCursor = 0;
        _writeCursor = 0;
    }
}
