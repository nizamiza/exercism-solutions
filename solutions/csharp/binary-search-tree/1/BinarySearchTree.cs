using System.Collections;
using System.Linq;

public class BinarySearchTree<T> : IEnumerable<T>
{
    public BinarySearchTree(T value) => Value = value;
    public BinarySearchTree(IEnumerable<T> values)
    {
        ArgumentNullException.ThrowIfNull(values);
        using var enumerator = values.GetEnumerator();

        if (!enumerator.MoveNext())
        {
            throw new ArgumentException("Cannot construct a tree without the root.", nameof(values));
        }

        Value = enumerator.Current;
        while (enumerator.MoveNext())
        {
            Add(enumerator.Current);
        }
    }

    public T Value { get; }
    public BinarySearchTree<T>? Left { get; private set; }
    public BinarySearchTree<T>? Right { get; private set; }

    public BinarySearchTree<T> Add(T value)
    {
        if (Comparer<T>.Default.Compare(value, Value) > 0)
        {
            Right = Right?.Add(value) ?? new BinarySearchTree<T>(value);
        }
        else
        {
            Left = Left?.Add(value) ?? new BinarySearchTree<T>(value);
        }
        return this;
    }

    public IEnumerator<T> GetEnumerator()
    {
        if (Left is not null)
        {
            foreach (var value in Left)
            {
                yield return value;
            }
        }

        yield return Value;

        if (Right is not null)
        {
            foreach (var value in Right)
            {
                yield return value;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class BinarySearchTree : BinarySearchTree<int>
{
    public BinarySearchTree(int value) : base(value) { }
    public BinarySearchTree(int[] values) : base(values) { }
}
