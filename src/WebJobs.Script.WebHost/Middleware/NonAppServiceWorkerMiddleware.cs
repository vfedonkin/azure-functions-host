// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Script.Extensions;

namespace Microsoft.Azure.WebJobs.Script.WebHost.Middleware
{
    public class NonAppServiceWorkerMiddleware
    {
        private const string DisguisedHostHeader = "DISGUISED-HOST";
        private const string HostHeader = "HOST";
        private const string ForwardedProtocolHeader = "X-Forwarded-Proto";
        private readonly RequestDelegate _next;

        public NonAppServiceWorkerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Headers.ContainsKey(DisguisedHostHeader))
            {
                httpContext.Request.Headers[HostHeader] = httpContext.Request.Headers[DisguisedHostHeader];
            }

            if (httpContext.Request.Headers.ContainsKey(ForwardedProtocolHeader))
            {
                httpContext.Request.Scheme = httpContext.Request.Headers[ForwardedProtocolHeader];
            }

            await _next(httpContext);
        }
    }
}