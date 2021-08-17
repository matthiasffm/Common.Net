using System.Collections;

namespace Common;

// TODO: notnull für T und R?

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Option<T> : IEquatable<Option<T>>, IStructuralEquatable
     where T : notnull
{
    private Option() { }

    public static Option<T> Some(T value) => new Options.Some(value);

    public static Option<T> None { get; } = new Options.None();


    public abstract R Match<R>(Func<T, R> someFunc, Func<R> noneFunc);

    public abstract void Act(Action<T> someAction, Action noneAction);

    public Option<R> Map<R>(Func<T, R> mapper) where R : notnull => Match(t => Option<R>.Some(mapper(t)), () => Option<R>.None);

    public R Fold<R>(Func<R, T, R> folder, R seed) where R : notnull => Match(t => folder(seed, t), () => seed);

    public static Option<T> Return(T val) => Some(val);

    public Option<R> Bind<R>(Func<T, Option<R>> binder) where R : notnull => Match(t => binder(t).Match(r => Option<R>.Some(r), () => Option<R>.None), () => Option<R>.None);


    #region operator overloads

    public static bool operator == (Option<T> left, Option<T> right) => left.Equals(right);

    public static bool operator != (Option<T> left, Option<T> right) => !(left == right);

    #endregion

    #region IEquatable

    bool IEquatable<Option<T>>.Equals(Option<T>? other) => Equals(other as object);

    #endregion

    #region IStructuralEquatable

    public abstract bool Equals(object? other, IEqualityComparer comparer);

    public abstract int GetHashCode(IEqualityComparer comparer);

    #endregion

    public override bool Equals(object? obj) => base.Equals(obj);

    public override int GetHashCode() => base.GetHashCode();


    private sealed class Options
    {
        public sealed class None : Option<T>
        {
            public override R Match<R>(Func<T, R> someFunc, Func<R> noneFunc) => noneFunc();

            public override void Act(Action<T> someAction, Action noneAction) => noneAction();

            #region value semantics

            public override bool Equals(object? other) => other is None;

            public override bool Equals(object? other, IEqualityComparer comparer) => Equals(other);

            public override int GetHashCode() => "None".GetHashCode();

            public override int GetHashCode(IEqualityComparer comparer) => GetHashCode();

            public override string ToString() => $"none";

            #endregion
        }

        public sealed class Some : Option<T>
        {
            public T Value { get; }

            public Some(T value)
            {
                this.Value = value;
            }

            public override R Match<R>(Func<T, R> someFunc, Func<R> noneFunc) => someFunc(Value);

            public override void Act(Action<T> someAction, Action noneAction) => someAction(Value);

            #region value semantics

            public override bool Equals(object? other) => other is Some s && Value.Equals(s.Value);

            public override bool Equals(object? other, IEqualityComparer comparer) => other is Some s && comparer.Equals(Value, s.Value);

            public override int GetHashCode() => Value.GetHashCode();

            public override int GetHashCode(IEqualityComparer comparer) => comparer.GetHashCode(Value);

            public override string ToString() => $"some ({Value})";

            #endregion
        }
    }
}
