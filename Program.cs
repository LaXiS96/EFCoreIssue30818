using EFCoreIssue30818;
using DbFunctionsExtensions = EFCoreIssue30818.DbFunctionsExtensions;

using (var setupContext = new Context())
{
    setupContext.Database.EnsureDeleted();
    setupContext.Database.EnsureCreated();
    setupContext.Add(new Entity { Dictionary = { ["key"] = 1234 } });
    setupContext.Add(new Entity { Dictionary = { ["key"] = "string" } });
    setupContext.Add(new Entity { JsonString = @"{""key"":6543}" });
    setupContext.SaveChanges();
}

using var context = new Context();
var dbSet = context.Set<Entity>(); // .AsNoTracking() makes no difference

// Convert over user-defined function: WORKING
var query1 = dbSet
    .Where(x => Convert.ToInt32(DbFunctionsExtensions.JsonValue(x.JsonString, @"$.""key""")) == 6543);
var result1 = query1.ToArray();

// Convert over dictionary whose access is translated to JsonValue: NOT WORKING
var query2 = dbSet
    .Where(x => Convert.ToInt32(x.Dictionary["key"]) == 1234);
var result2 = query2.ToArray();

// Check the intercepted and translated expression by breaking on JsonDictionaryQueryInterceptor line 11 (return newExpr;)

return;
