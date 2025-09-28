using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Attributes
{
    public class CacheAttribute(int DurationInSecond=90) : ActionFilterAttribute
    {

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Implement caching logic here:
            //1-Create Cache Key:
            string ChacheKey = CreateChacheKey(context.HttpContext.Request);
            //2-Search For Value with CacheKey:
            ICachService cachService = context.HttpContext.RequestServices.GetRequiredService<ICachService>();
            var chachevalue = await cachService.GetAsync(ChacheKey);
            //3- Return value is not null :
            if (chachevalue is not null)
            {
                context.Result = new ContentResult()
                {
                    Content = chachevalue,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }
            //4- Return Value is null:
            //Invoked .Next :
            var ExecutedContent = await next.Invoke();
            //5-Set Value With Cache Key :
            if(ExecutedContent.Result is OkObjectResult result)
            {
               await cachService.SetAsync(ChacheKey, result.Value, TimeSpan.FromSeconds(DurationInSecond));
            }




        }


        private string CreateChacheKey(HttpRequest request)
        {
            StringBuilder key = new StringBuilder();
            key.Append(request.Path + '?');
            foreach(var Item in request.Query.OrderBy(Q=>Q.Key))
            {
                key.Append($"{Item.Key}={Item.Value}&");
            }
            return key.ToString();
        }
    }
}
