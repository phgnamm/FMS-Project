using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http.Extensions;

namespace ChillDe.FMS.API.Middlewares;

public class KebabQueryStringMiddleware
{
    private readonly RequestDelegate _next;

    public KebabQueryStringMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        if (httpContext.Request.Query.Any(q => q.Key.Contains("-")))
        {
            var queryitems = httpContext.Request.Query
                .SelectMany(x => x.Value, (col, value) => new KeyValuePair<string, string>(col.Key, value)).ToList();

            List<KeyValuePair<string, string>> queryparameters = new List<KeyValuePair<string, string>>();
            foreach (var item in queryitems)
            {
                var key = KebabToCamelCase(item.Key);
                KeyValuePair<string, string> newqueryparameter = new KeyValuePair<string, string>(key, item.Value);
                queryparameters.Add(newqueryparameter);
            }

            var qb1 = new QueryBuilder(queryparameters);
            httpContext.Request.QueryString = qb1.ToQueryString();
        }

        await _next(httpContext);
    }

    string KebabToCamelCase(string input) => Regex.Replace(input, "-.", m => m.Value.ToUpper().Substring(1));
}