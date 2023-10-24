using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WeatherApi.DotNet.Application.Middlewares;

public class GlobalErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalErrorHandlingMiddleware(RequestDelegate next)
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

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode statusCode;
        string stackTrace = String.Empty; //infos da exception
        string message; 
        var exceptionType = exception.GetType(); //buscar tipo da exception

        if(exceptionType == typeof(DBConcurrencyException))
        {
            message = exception.Message;
            statusCode = HttpStatusCode.BadRequest;
            stackTrace = exception.StackTrace;
        } 
        else
        {
            message = exception.Message;
            statusCode = HttpStatusCode.InternalServerError;
            stackTrace = exception.StackTrace;
        }

        var result = JsonSerializer.Serialize(new { statusCode, message, stackTrace });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        return context.Response.WriteAsync(result);
    }
}
