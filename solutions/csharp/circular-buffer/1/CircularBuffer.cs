public class CircularBuffer<T>
{
    private readonly int Capacity;
    private readonly T[] Buffer;
    private int Count;
    private int ReadCursor;
    private int WriteCursor;

    public CircularBuffer(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(capacity);

        Capacity = capacity;
        Buffer = new T[Capacity];
        Count = 0;
        ReadCursor = GetInitialCursor(Capacity);
        WriteCursor = ReadCursor;
    }

    public T Read()
    {
        if (Count == 0)
        {
            throw new InvalidOperationException("Buffer is empty.");
        }

        var item = Buffer[ReadCursor];
        Buffer[ReadCursor] = default!;
        Count--;

        ReadCursor = (ReadCursor + 1) % Capacity;
        if (Count == 0)
        {
            WriteCursor = ReadCursor;
        }

        return item;
    }

    public void Write(T item)
    {
        if (Count == Capacity)
        {
            throw new InvalidOperationException("Buffer is full.");
        }

        Buffer[WriteCursor] = item;
        Count++;

        WriteCursor = (WriteCursor + 1) % Capacity;
    }

    public void Overwrite(T item)
    {
        if (Count < Capacity)
        {
            Write(item);
            return;
        }

        Buffer[ReadCursor] = item;
        ReadCursor = (ReadCursor + 1) % Capacity;
        WriteCursor = ReadCursor;
    }

    public void Clear()
    {
        Array.Clear(Buffer);
        Count = 0;
        ReadCursor = GetInitialCursor(Capacity);
        WriteCursor = ReadCursor;
    }

    private static int GetInitialCursor(int capacity)
    {
        return capacity / 2;
    }
}
