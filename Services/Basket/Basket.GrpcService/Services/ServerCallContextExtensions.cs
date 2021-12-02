using System;
using Common.Hosting.Auth;
using Grpc.Core;

namespace Basket.GrpcService.Services
{
    public static class ServerCallContextExtensions
    {
        public static Guid GetUserId(this ServerCallContext context)
        {
            var httpContext = context.GetHttpContext();
            return httpContext.User.GetUserId();
        }
    }
}