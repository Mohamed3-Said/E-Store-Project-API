using Azure;
using DomainLayer.Exceptions;
using Shared.ErrorModels;

namespace E_Store.Web.CustomExceptionMiddelWares
{
    public class CustomExceptionHandlerMiddelWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddelWare> _logger;

        //1-ctor
        public CustomExceptionHandlerMiddelWare(RequestDelegate Next, ILogger<CustomExceptionHandlerMiddelWare> logger)
        {
            _next = Next;
            _logger = logger;
        }
        //2-Method: Invoke
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
                await HandleNotFoundEndPoint(httpContext);

            }
            catch (Exception ex)
            {
                //Log Exception in Server:
                _logger.LogError(ex, "Something Went Wrong !!");

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            //3-Return object in The Response Body:
            var ResponseObject = new ErrorToReturn
            {
                StatusCode = httpContext.Response.StatusCode,
                ErrorMessage = ex.Message
            };
            //Return Exception to Frontend:
            //1-set Status Code
            httpContext.Response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                BadRequestException badRequestException => GetBadRequestErrors(badRequestException, ResponseObject),
                _ => StatusCodes.Status500InternalServerError
            };
            //2-set Content Type For Response => application/json

           
            //4-Return object as JSON
            await httpContext.Response.WriteAsJsonAsync(ResponseObject);
        }

        private static int GetBadRequestErrors(BadRequestException badRequestException, ErrorToReturn response)
        {
            response.Errors = badRequestException.Errors;
            return StatusCodes.Status400BadRequest;
        }

        private static async Task HandleNotFoundEndPoint(HttpContext httpContext)
        {
            if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                var Response = new ErrorToReturn()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorMessage = $"End Point {httpContext.Request.Path} is Not Found "
                };
                await httpContext.Response.WriteAsJsonAsync(Response);
            }
        }
    }
}
