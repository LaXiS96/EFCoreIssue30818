namespace EFCoreIssue30818
{
    internal class Entity
    {
        public int Id { get; set; }
        public JsonDictionary Dictionary { get; set; } = new();
    }
}
