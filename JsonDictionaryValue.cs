using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFCoreIssue30818
{
    internal class JsonDictionaryValueConverter : ValueConverter<JsonDictionary, string>
    {
        public JsonDictionaryValueConverter()
            : base(
                dict => dict.ToJson(),
                str => JsonDictionary.FromJson(str))
        {
        }
    }

    internal class JsonDictionaryValueComparer : ValueComparer<JsonDictionary>
    {
        public JsonDictionaryValueComparer()
            : base(
                (d1, d2) => EqualityComparer<JsonDictionary>.Default.Equals(d1, d2),
                d => d.GetHashCode(),
                d => new JsonDictionary(d))
        {
        }
    }
}
