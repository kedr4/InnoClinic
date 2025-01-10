﻿using Business.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace Presentation.Middleware
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            if (exception is AppException appException)
            {
                context.Response.StatusCode = appException.StatusCode;

                var errorDetails = new
                {
                    message = appException.Message,
                    statusCode = context.Response.StatusCode,
                    errors = exception is ValidationAppException validationAppException
                        ? validationAppException.ValidationFailures.Select(e => new { e.PropertyName, e.ErrorMessage }).ToList()
                        : null
                };

                var result = JsonConvert.SerializeObject(errorDetails);
                return context.Response.WriteAsync(result);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var errorDetails = new
                {
                    message = exception.Message,
                    statusCode = context.Response.StatusCode
                };

                var result = JsonConvert.SerializeObject(errorDetails);
                return context.Response.WriteAsync(result);
            }
        }
    }
}
