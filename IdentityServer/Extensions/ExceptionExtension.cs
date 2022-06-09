using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace Identity.Extensions
{
    public static class ExceptionExtension
    {
        //public static BadRequestObjectResult BadRequest(this Exception exception, ILogger logger)
        //{
        //    logger.LogError(exception, exception.StackTrace);
        //    return new BadRequestObjectResult(new ApiErrorResponse(ApiError.Exception, exception.Message)) { StatusCode = (int)HttpStatusCode.BadRequest };
        //}
    }
}
