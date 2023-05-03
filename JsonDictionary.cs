using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace EFCoreIssue30818
{
    internal class JsonDictionary : Dictionary<string, object?>, IEquatable<JsonDictionary>
    {
        private static readonly IEqualityComparer<KeyValuePair<string, object?>> _itemComparer = new KeyValuePairComparer();

        public JsonDictionary()
            : base(StringComparer.OrdinalIgnoreCase) { }

        public JsonDictionary(IDictionary<string, object?> dictionary)
            : base(dictionary, StringComparer.OrdinalIgnoreCase) { }

        public JsonDictionary(IEnumerable<KeyValuePair<string, object?>> collection)
            : base(collection, StringComparer.OrdinalIgnoreCase) { }

        public static JsonDictionary? FromJson(string json)
            => JsonConvert.DeserializeObject<JsonDictionary>(json);

        public string ToJson()
            => JsonConvert.SerializeObject(this);

        public bool Equals(JsonDictionary? other)
        {
            if (other is null)
                return false;

            return Enumerable.SequenceEqual(
                this.OrderBy(kvp => kvp.Key),
                other.OrderBy(kvp => kvp.Key),
                _itemComparer);
        }

        public override bool Equals(object? obj)
            => Equals(obj as JsonDictionary);

        public override int GetHashCode()
        {
            return this
                .OrderBy(kvp => kvp.Key) // Enumeration order matters
                .Aggregate(0, (hc, kvp) => HashCode.Combine(hc, _itemComparer.GetHashCode(kvp)));
        }

        internal class KeyValuePairComparer : EqualityComparer<KeyValuePair<string, object?>>
        {
            public override bool Equals(KeyValuePair<string, object?> x, KeyValuePair<string, object?> y)
                => string.Equals(x.Key, y.Key, StringComparison.OrdinalIgnoreCase) &&
                    EqualityComparer<object?>.Default.Equals(x.Value, y.Value);

            public override int GetHashCode([DisallowNull] KeyValuePair<string, object?> obj)
                => HashCode.Combine(obj.Key.ToLowerInvariant(), obj.Value);
        }
    }
}
