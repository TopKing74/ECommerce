using ECommerce.Domain.Exceptions;
using ECommerce.Shared.ErrorModels;

namespace ECommerce.Web.CustomMiddlewares
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<CustomExceptionMiddleware> logger;

        public CustomExceptionMiddleware(RequestDelegate Next,ILogger<CustomExceptionMiddleware>logger)
        {
            next = Next;
            this.logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
                if(context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    #region Response Body
                    var Response = new ErrorToReturn()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = $"End Point {context.Request.Path} is Not Found"
                    };
                    //Return Object As Json
                    await context.Response.WriteAsJsonAsync(Response);
                    #endregion
                }
            }

            catch (Exception ex) 
            {
                logger.LogError(ex, ex.Message);
                var Response = new ErrorToReturn()
                {
                    Message = ex.Message
                };
                context.Response.StatusCode = ex switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    UnAuthorizedException => StatusCodes.Status401Unauthorized,
                    BadRequestException badRequestException => GetBadRequestErrors(badRequestException, Response),
                    _ => StatusCodes.Status500InternalServerError
                };

                Response.StatusCode = context.Response.StatusCode;
                
                context.Response.ContentType = "application/json";
                
                await context.Response.WriteAsJsonAsync(Response);
            }
        }
        private int GetBadRequestErrors(BadRequestException exception, ErrorToReturn response)
        {
            response.Errors = exception.Errors;
            return StatusCodes.Status400BadRequest;
        }
    }
}
