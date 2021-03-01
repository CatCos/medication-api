namespace MedicationApi.Application.Middlewares
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using MedicationApi.Application.Exceptions;
    using Microsoft.AspNetCore.Http;

    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var message = "Something went wrong. Please try again.";

            switch (exception)
            {
                case NotFoundException _:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    message = exception.Message;
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }


            return context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = message
            }.ToString());
        }
    }
}
