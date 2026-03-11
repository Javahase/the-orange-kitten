using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;

public abstract class WebEither<TErr, TVal>
{
    public bool IsLeft() => this is Left<TErr, TVal>;
    public bool IsRight() => this is Right<TErr, TVal>;

    public bool IsLeft(out TErr error)
    {
        error = GetLeft();
        return IsLeft();
    }

    public bool IsRight(out TVal right)
    {
        right = GetRight();
        return IsRight();
    }


    public TErr GetLeft(TErr fallback = default) => IsLeft() ? ((Left<TErr, TVal>)this).Error : fallback;
    public TVal GetRight(TVal fallback = default) => IsRight() ? ((Right<TErr, TVal>)this).Value : fallback;

    public static WebEither<TErr, TVal> Either(TErr error, TVal value) =>
        value == null ? new Left<TErr, TVal>(error) : new Right<TErr, TVal>(value);

    public WebEither<TErr, TNewVal> Map<TNewVal>(Func<TVal, TNewVal> f) => this switch
    {
        Left<TErr, TVal> l => new Left<TErr, TNewVal>(l.Error),
        Right<TErr, TVal> r => new Right<TErr, TNewVal>(f(r.Value)),
        _ => null
    };

    public WebEither<TErr, TNewVal> MapNonNull<TNewVal>(Func<TVal, TNewVal> f, TErr error) =>
        Map(f).RequireNonNull(error);

    public WebEither<TNewErr, TVal> MapError<TNewErr>(Func<TErr, TNewErr> f) => this switch
    {
        Left<TErr, TVal> l => new Left<TNewErr, TVal>(f(l.Error)),
        Right<TErr, TVal> r => new Right<TNewErr, TVal>(r.Value),
        _ => null
    };

    public WebEither<TErr, TNewVal> AndThen<TNewVal>(Func<TVal, WebEither<TErr, TNewVal>> bind) => this switch
    {
        Left<TErr, TVal> l => new Left<TErr, TNewVal>(l.Error),
        Right<TErr, TVal> r => bind(r.Value),
        _ => null
    };

    public WebEither<TErr, TVal> Validate(Func<TVal, bool> f, TErr error) => this switch
    {
        Left<TErr, TVal> _ => this,
        Right<TErr, TVal> r => f(r.Value) ? this : new Left<TErr, TVal>(error),
        _ => null
    };

    public WebEither<TErr, TVal> RequireNonNull(TErr error) => Validate((r) => r != null, error);

    public WebEither<TErr, TCast> MapAs<TCast>(TErr error) =>
        Validate((r) => r is TCast, error)
            .Map(r => r is TCast arg ? arg : default);
}

public class Left<TErr, TVal> : WebEither<TErr, TVal>
{
    public TErr Error;

    public Left(TErr error) => Error = error;
}

public class Right<TErr, TVal> : WebEither<TErr, TVal>
{
    public TVal Value;
    public Right(TVal value) => Value = value;
}

public static class WebUtil
{
    public static WebEither<string, TVal> ContainsKey<TVal>(this WebEither<string, TVal> either, string key)
        where TVal : JObject
        => either.Validate((o) => o.ContainsKey(key), $"Malformed JSON: \"{key}\"-Field not found.");

    public static WebEither<string, TJToken> GetJToken<TJToken>(this WebEither<string, JObject> either, string key)
        where TJToken : JToken
        => either.ContainsKey(key).Map(obj => obj[key]).MapAs<TJToken>($"Not a {typeof(TJToken).Name}: \"{key}\"");

    public static WebEither<string, JObject> GetAsJObject(this WebEither<string, JObject> either, string key)
        => either.GetJToken<JObject>(key);

    public static WebEither<string, JArray> GetAsJArray(this WebEither<string, JObject> either, string key)
        => either.GetJToken<JArray>(key);

    public static WebEither<string, TObj> GetObject<TObj>(this WebEither<string, JObject> either, string key)
        => either.ContainsKey(key).MapNonNull(obj => obj[key].ToObject<TObj>(), $"Not a {typeof(TObj).Name}: \"{key}\"");

    public static WebEither<string, string> GetAsString(this WebEither<string, JObject> either, string key)
        => either.GetObject<string>(key);
}