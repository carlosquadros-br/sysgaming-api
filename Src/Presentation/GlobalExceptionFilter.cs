using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SysgamingApi.Src.Presentation;

public class GlobalExceptionFilter : IExceptionFilter
{

    public void OnException(ExceptionContext context)
    {

        var error = (context.Exception) switch
        {
            Application.Utils.ValidationException validationException => StatusCodes.Status406NotAcceptable,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };
        context.HttpContext.Response.StatusCode = (int)error;
        context.ExceptionHandled = true;
        context.Result = new ObjectResult(new
        {
            error = context.Exception.Message
        })
        {
            StatusCode = (int)error
        };

    }
}
