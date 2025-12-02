using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Abstraction.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Presentation.CacheAttributes
{
    public class CacheAttribute(int DurationInSec = 90):ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Create Cache Key
            string CacheKey = CreateCacheKey(context.HttpContext.Request);
            //Search For Value with Cache Key
            ICacheServices cacheServices = context.HttpContext.RequestServices.GetRequiredService<ICacheServices>();
            var CachedValue = await cacheServices.GetAsync(CacheKey);
            //Return Value If not Null
            if (CachedValue is not null)
            {
                context.Result=new ContentResult()
                {
                    Content=CachedValue,
                    ContentType="application/json",
                    StatusCode=StatusCodes.Status200OK
                };
                return;
            }
            //Return Value if Null
            //Invoke Next
            var ExecutedContext = await next.Invoke();
            //Set Value With Cache Key
            if (ExecutedContext.Result is ObjectResult result)
            {
                await cacheServices.SetAsync(CacheKey, result.Value!, TimeSpan.FromSeconds(DurationInSec));
            }
        }
        private string CreateCacheKey(HttpRequest request)
        {
            StringBuilder key = new StringBuilder();
            key.Append(request.Path + '?');
            foreach (var Item in request.Query.OrderBy(Q => Q.Key))
            {
                key.Append($"{Item.Key}={Item.Value}&");
            }
            return key.ToString();
        }
    }
}
