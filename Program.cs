using EFCoreIssue30818;

using (var context = new Context())
{
    if (context.Database.EnsureCreated())
    {
        context.Add(new Entity { Dictionary = { ["key"] = 1234 } });
        context.Add(new Entity { Dictionary = { ["key"] = "string" } });
        context.SaveChanges();
    }
}

using (var context = new Context())
{
    var thisWorks = context.Set<Entity>()
        .Where(x => x.Dictionary["key"] == (object)1234)
        .ToArray();
}

using (var context = new Context())
{
    var thisDoesntWork = context.Set<Entity>()
        .Where(x => Convert.ToInt32(x.Dictionary["key"]) == 1234)
        .ToArray();
}

return;
