using Identity.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace Identity.Extensions
{
    public static class ResponseBaseExtensions
    {
        public static OkObjectResult OkResult(this IResponseBase response)
        {
            return new OkObjectResult(response) { StatusCode = (int)HttpStatusCode.OK };
        }

        public static OkObjectResult OkResult(this IEnumerable<IResponseBase> adminResponse)
        {
            return new OkObjectResult(adminResponse) { StatusCode = (int)HttpStatusCode.OK };
        }
    }
}
