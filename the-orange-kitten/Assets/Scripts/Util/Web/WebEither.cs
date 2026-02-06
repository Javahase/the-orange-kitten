using System.Collections;
using Unity.VisualScripting;

public struct WebEither<L, R>
{
    private L _error;
    private R _value;

    public static WebEither<L, R> Left(L error) => new() { _error = error };

    public static WebEither<L, R> Right(R value) => new() { _value = value };

    public static WebEither<L, R> Either(L error, R value) => value == null ? Left(error) : Right(value);

    public bool IsLeft() => _error != null && _value == null;

    public bool IsRight() => _error == null && _value != null;

    public L GetLeft() => _error;

    public R GetRight() => _value;
}