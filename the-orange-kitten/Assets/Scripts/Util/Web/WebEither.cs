using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;

public struct WebEither<L, R>
{
    private L _error;
    private R _value;

    public static WebEither<L, R> Left(L error) => new() { _error = error };

    public static WebEither<L, R> Right(R value) => new() { _value = value };

    public static WebEither<L, R> Either(L error, R value) => value == null ? Left(error) : Right(value);

    public bool IsLeft() => _value == null;

    public bool IsRight() => !IsLeft();

    public bool IsRight(out R right)
    {
        if (!IsRight())
        {
            right = default;
            return false;
        }

        right = _value;
        return true;
    }

    public L GetLeft() => _error;

    public R GetRight() => _value;

    public WebEither<L, TV> Map<TV>(Func<R, TV> f) =>
        IsLeft() ? WebEither<L, TV>.Left(_error) : WebEither<L, TV>.Right(f(_value));

    public WebEither<L, TV> MapNonNull<TV>(Func<R, TV> f, L error) => Map(f).RequireNonNull(error);

    public WebEither<TL, R> MapError<TL>(Func<L, TL> f) =>
        IsLeft() ? WebEither<TL, R>.Left(f(_error)) : WebEither<TL, R>.Right(_value);

    public WebEither<L, TR> AndThen<TR>(Func<R, WebEither<L, TR>> bind) =>
        IsLeft() ? WebEither<L, TR>.Left(_error) : bind(_value);

    public WebEither<L, R> Validate(Func<R, bool> validate, L error) =>
        (IsLeft() || validate(_value)) ? this : Left(IsLeft() ? _error : error);

    public WebEither<L, R> RequireNonNull(L error) => _value == null ? Left(error) : this;

    public WebEither<L, TR> MapAs<TR>(L error) =>
        Validate(r => r is TR, error).Map(r => r is TR arg ? arg : default); // Default case should never happen.
}

public static class WebUtil
{
    public static WebEither<string, TR> ContainsKey<TR>(this WebEither<string, TR> either, string key)
        where TR : JObject
        => either.Validate((o) => o.ContainsKey(key), $"Malformed JSON: \"{key}\"-Field not found.");

    public static WebEither<string, TP> GetJToken<TR, TP>(this WebEither<string, TR> either, string key)
        where TR : JObject
        where TP : JToken
        => either.ContainsKey(key).Map(obj => obj[key]).MapAs<TP>($"Not a {typeof(TP).Name}: \"{key}\"");

    public static WebEither<string, JObject> GetAsJObject<TR>(this WebEither<string, TR> either, string key)
        where TR : JObject
        => either.GetJToken<TR, JObject>(key);

    public static WebEither<string, JArray> GetAsJArray<TR>(this WebEither<string, TR> either, string key)
        where TR : JObject
        => either.GetJToken<TR, JArray>(key);

    public static WebEither<string, TO> GetObject<TR, TO>(this WebEither<string, TR> either, string key)
        where TR : JObject
        => either.ContainsKey(key).MapNonNull(obj => obj[key].ToObject<TO>(), $"Not a {typeof(TO).Name}: \"{key}\"");

    public static WebEither<string, string> GetAsString<TR>(this WebEither<string, TR> either, string key)
        where TR : JObject
        => either.GetObject<TR, string>(key);
}