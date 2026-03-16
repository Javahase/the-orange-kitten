using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine.Assertions;

/// <summary>
/// Contains an error-value (Left) or a correct value (Right)
///
/// Used for validation purposes with messages.
/// </summary>
/// <typeparam name="TErr">The error type.</typeparam>
/// <typeparam name="TVal">The data type.</typeparam>
public abstract class WebEither<TErr, TVal>
{
    /// <summary>
    /// Whether this WebEither is a left (has an error value).
    /// </summary>
    /// <returns>Whether this WebEither is a left.</returns>
    public bool IsLeft() => this is Left<TErr, TVal>;

    /// <summary>
    /// Whether this WebEither is a right (has a correct value).
    /// </summary>
    /// <returns>Whether this WebEither is a right.</returns>
    public bool IsRight() => this is Right<TErr, TVal>;

    /// <summary>
    /// Whether this WebEither is a left (has an error value), but outputs the error value.
    /// </summary>
    /// <param name="error">Error value output.</param>
    /// <returns>Whether this WebEither is a left.</returns>
    public bool IsLeft(out TErr error)
    {
        error = GetLeft();
        return IsLeft();
    }

    /// <summary>
    /// Whether this WebEither is a right (has a correct value), but outputs the correct value.
    /// </summary>
    /// <param name="right">Correct value output.</param>
    /// <returns>Whether this WebEither is a right.</returns>
    public bool IsRight(out TVal right)
    {
        right = GetRight();
        return IsRight();
    }

    /// <summary>
    /// Gets the left value, or a fallback value if this is not a left.
    /// </summary>
    /// <param name="fallback">The fallback value.</param>
    /// <returns>A left value.</returns>
    public TErr GetLeft(TErr fallback = default) => IsLeft() ? ((Left<TErr, TVal>)this).Error : fallback;

    /// <summary>
    /// Gets the right value, or a fallback value if this is not a right.
    /// </summary>
    /// <param name="fallback">The fallback value.</param>
    /// <returns>A right value.</returns>
    public TVal GetRight(TVal fallback = default) => IsRight() ? ((Right<TErr, TVal>)this).Value : fallback;

    /// <summary>
    /// Creates a WebEither from a predicate, a condition that <see cref="value"/> must fulfill.
    /// </summary>
    /// <param name="value">The value to be put into the WebEither.</param>
    /// <param name="predicate">The predicate that <see cref="value"/> must fulfill.</param>
    /// <param name="error">The left value that gets put in if <see cref="predicate"/> is false.</param>
    /// <returns>A right, if <see cref="predicate"/> is true, or a left, if it isn't.</returns>
    public static WebEither<TErr, TVal> FromPredicate(TVal value, Predicate<TVal> predicate, TErr error) =>
        predicate(value) ? new Right<TErr, TVal>(value) : new Left<TErr, TVal>(error);

    /// <summary>
    /// Creates a WebEither by condition that <see cref="value"/> is not null.
    /// </summary>
    /// <param name="value">The value to be put into the WebEither.</param>
    /// <param name="error">The left value that gets put in if <see cref="value"/> is null.</param>
    /// <returns>A right, if <see cref="value"/> is not null, or a left, if it is null.</returns>
    public static WebEither<TErr, TVal> Either(TVal value, TErr error) =>
        FromPredicate(value, v => v != null, error);

    /// <summary>
    /// Maps an existing right value to a new right value.<br/>
    /// Keeps the same left value if it's a left.
    /// </summary>
    /// <param name="transform">The transform-function for turning the right value to a new right value.</param>
    /// <typeparam name="TNewVal">The type of the new right value.</typeparam>
    /// <returns>A WebEither with (potentially) new right type and new right value, or just the left value, with new right type.</returns>
    public WebEither<TErr, TNewVal> Map<TNewVal>(Func<TVal, TNewVal> transform) => this switch
    {
        Left<TErr, TVal> l => new Left<TErr, TNewVal>(l.Error),
        Right<TErr, TVal> r => new Right<TErr, TNewVal>(transform(r.Value)),
        _ => null
    };

    /// <summary>
    /// Maps an existing right value to a new right value and checks whether the result of <see cref="transform"/>
    /// is not null. If it is null, a Left will be created instead, with <see cref="error"/> as its value.
    /// </summary>
    /// <param name="transform">The transform function that turns the right value into a new right value.</param>
    /// <param name="error">The value within the left if <c>transform(value)</c> is null.</param>
    /// <typeparam name="TNewVal">The type of the new right value.</typeparam>
    /// <returns>A right, if <c>transform(value)</c> is not null. Otherwise, refer to <see cref="Map"/>.</returns>
    public WebEither<TErr, TNewVal> MapNonNull<TNewVal>(Func<TVal, TNewVal> transform, TErr error) =>
        Map(transform).RequireNonNull(error);

    /// <summary>
    /// Maps an existing left value to a new left value.<br/>
    /// If this is a Right, then nothing happens except a new error type for the WebEither.
    /// </summary>
    /// <param name="transform">The function that transforms the left value.</param>
    /// <typeparam name="TNewErr">The new error type.</typeparam>
    /// <returns>A left, if this was a left, a right otherwise.</returns>
    public WebEither<TNewErr, TVal> MapError<TNewErr>(Func<TErr, TNewErr> transform) => this switch
    {
        Left<TErr, TVal> l => new Left<TNewErr, TVal>(transform(l.Error)),
        Right<TErr, TVal> r => new Right<TNewErr, TVal>(r.Value),
        _ => null
    };

    /// <summary>
    /// Takes out the right value and binds it to a function that returns a new WebEither.<br/>
    /// Can be used to change the context from Right to Left.
    /// </summary>
    /// <param name="bind">The bind function.</param>
    /// <typeparam name="TNewVal">The new potential value type that is created.</typeparam>
    /// <returns>A new WebEither. Keeps the previous error-type, though, and won't bind if this is a Left.</returns>
    public WebEither<TErr, TNewVal> AndThen<TNewVal>(Func<TVal, WebEither<TErr, TNewVal>> bind) => this switch
    {
        Left<TErr, TVal> l => new Left<TErr, TNewVal>(l.Error),
        Right<TErr, TVal> r => bind(r.Value),
        _ => null
    };

    /// <summary>
    /// Validates the right value using a <see cref="predicate"/>.<br/>
    /// Returns an Left if the predicate is not true, with <see cref="error"/>, or itself, if it was a Left before.<br/>
    /// Returns itself if the predicate is true and this is a Right.
    /// </summary>
    /// <param name="predicate">The predicate function.</param>
    /// <param name="error">The error if the predicate is not true.</param>
    /// <returns>A Right, if the predicate is true, or a Left, if it isn't or if it was a Left before.</returns>
    public WebEither<TErr, TVal> Validate(Predicate<TVal> predicate, TErr error) => this switch
    {
        Left<TErr, TVal> _ => this,
        Right<TErr, TVal> r => predicate(r.Value) ? this : new Left<TErr, TVal>(error),
        _ => null
    };

    /// <summary>
    /// Validates whether the right value is not null.
    ///
    /// <seealso cref="Validate"/>
    /// </summary>
    /// <param name="error">The error if the right value is null.</param>
    /// <returns>A right, if the right value is not null, or a left, if it is null or if it was a left before.</returns>
    public WebEither<TErr, TVal> RequireNonNull(TErr error) => Validate((r) => r != null, error);

    /// <summary>
    /// Casts the right value of this WebEither to a new type.
    /// </summary>
    /// <param name="error">The Left value that comes out if this right value could not be cast to <see cref="TCast"/>.</param>
    /// <typeparam name="TCast">The new type that <see cref="TVal"/> should have.</typeparam>
    /// <returns>A Right, if the <see cref="TCast"/> succeeds, or a Left, if it doesn't or if this WebEither was a Left before.</returns>
    public WebEither<TErr, TCast> MapAs<TCast>(TErr error) =>
        Validate((r) => r is TCast, error)
            .Map(r => r is TCast arg ? arg : default);
}

/// <summary>
/// Holds an error value.
/// </summary>
/// <typeparam name="TErr">The error type.</typeparam>
/// <typeparam name="TVal">The value type.</typeparam>
public class Left<TErr, TVal> : WebEither<TErr, TVal>
{
    /// <summary>
    /// The error value.
    /// </summary>
    public TErr Error;

    /// <summary>
    /// Initializes an error value.
    /// </summary>
    /// <param name="error">The error value.</param>
    public Left(TErr error) => Error = error;
}

/// <summary>
/// Holds a right (correct) value.
/// </summary>
/// <typeparam name="TErr">The error type.</typeparam>
/// <typeparam name="TVal">The value type.</typeparam>
public class Right<TErr, TVal> : WebEither<TErr, TVal>
{
    /// <summary>
    /// The right (correct) value.
    /// </summary>
    public TVal Value;

    /// <summary>
    /// Initializes a right (correct) value.
    /// </summary>
    /// <param name="value">The right (correct) value.</param>
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
        => either.ContainsKey(key)
            .MapNonNull(obj => obj[key].ToObject<TObj>(), $"Not a {typeof(TObj).Name}: \"{key}\"");

    public static WebEither<string, string> GetAsString(this WebEither<string, JObject> either, string key)
        => either.GetObject<string>(key);
}