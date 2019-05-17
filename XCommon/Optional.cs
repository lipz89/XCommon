using System;

namespace XCommon
{
    public interface IOptional
    {
        bool HasValue { get; }
        object Value { get; }
    }

    public interface IOptional<T> : IOptional
    {
        new T Value { get; }
        T GetValueOrDefault(T defaultValue = default);
    }

    public struct Optional<T> : IOptional<T>
    {
        private readonly T value;

        public Optional(T value)
        {
            this.HasValue = value != null;
            this.value = value;
        }

        public bool HasValue { get; }

        public T Value
        {
            get
            {
                if (!HasValue)
                {
                    throw new InvalidOperationException("Optional<T> 没有值");
                }
                return value;
            }
        }

        public static Optional<T> Empty { get; } = default;

        object IOptional.Value => Value;

        public T GetValueOrDefault(T defaultValue = default)
        {
            return HasValue ? value : defaultValue;
        }

        public override bool Equals(object other)
        {
            if (!(other is Optional<T> option))
            {
                return false;
            }

            if (!HasValue)
            {
                return !option.HasValue;
            }

            if (!option.HasValue)
            {
                return false;
            }

            return Equals(value, option.Value);
        }

        public override int GetHashCode()
        {
            if (!HasValue || value == null)
            {
                return 0;
            }
            return value.GetHashCode();
        }

        public override string ToString()
        {
            if (!HasValue)
            {
                return "Empty";
            }
            if (value == null)
            {
                return "null";
            }
            return value.ToString();
        }

        public static implicit operator Optional<T>(T value)
        {
            return new Optional<T>(value);
        }

        public static explicit operator T(Optional<T> value)
        {
            return value.Value;
        }
    }
    public static class ObjectExtensions
    {
        internal static bool IsNotNull(this object obj)
        {
            return obj != null;
        }

        internal static bool IsNull(this object obj)
        {
            return obj == null;
        }

        public static Optional<T> ToOptional<T>(this T value)
        {
            return value;
        }

        public static Optional<TResult> ToOptional<TResult>(this object obj)
        {
            if (obj is TResult result)
            {
                return new Optional<TResult>(result);
            }
            return default;
        }
        public static Optional<T> ToOptional<T>(this T? value) where T : struct
        {
            return value.HasValue ? new Optional<T>(value.Value) : default;
        }
        public static T? ToNullable<T>(this Optional<T> value) where T : struct
        {
            return value.HasValue ? value.Value : (T?)null;
        }
    }
    public static class OptionalExtensions
    {
        public static Optional<T> ThrowEmpty<T, TException>(this Optional<T> value)
            where TException : Exception, new()
        {
            if (value.HasValue)
            {
                return value;
            }
            throw new TException();
        }

        public static Optional<T> ThrowEmpty<T, TException>(this Optional<T> value, Func<TException> func)
            where TException : Exception
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (value.HasValue)
            {
                return value;
            }
            throw func();
        }

        public static Optional<T> Do<T>(this Optional<T> value, Action<T> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (value.HasValue)
            {
                action(value.Value);
            }
            return value;
        }
        public static Optional<T> DoIfMatch<T>(this Optional<T> value, Func<T, bool> predicate, Action<T> action)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (value.HasValue && predicate(value.Value))
            {
                action(value.Value);
            }
            return value;
        }

        public static Optional<T> DoIfType<T, TTarget>(this Optional<T> value, Action<TTarget> action)
            where TTarget : T
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (value.HasValue && value.Value is TTarget target)
            {
                action(target);
            }
            return value;
        }

        public static Optional<T> DoElse<T>(this Optional<T> value, Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (!value.HasValue)
            {
                action();
            }
            return value;
        }

        public static Optional<T> DoAnyway<T>(this Optional<T> value, Action<T> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            action(value.Value);
            return value;
        }

        public static Optional<TResult> Map<T, TResult>(this Optional<T> value, Func<T, Optional<TResult>> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (!value.HasValue)
            {
                return default;
            }
            return func(value.Value);
        }

        public static Optional<TResult> Map<T, TResult>(this Optional<T> value, Func<T, TResult> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (!value.HasValue)
            {
                return default;
            }
            return func(value.Value);
        }

        public static Optional<TResult> Map<T, TResult>(this Optional<T> value, Func<T, bool> predicate, Func<T, TResult> func)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (!value.HasValue || !predicate(value.Value))
            {
                return default;
            }
            return func(value.Value);
        }

        public static Optional<T> WhenEmpty<T>(this Optional<T> value, Func<T> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (!value.HasValue)
            {
                return func();
            }
            return value;
        }

        public static Optional<T> Where<T>(this Optional<T> value, Func<T, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (!value.HasValue || predicate(value.Value))
            {
                return value;
            }
            return default;
        }
    }
}
