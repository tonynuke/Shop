using System;
using Common.Auth;
using Grpc.Core;

namespace Basket.GrpcService.Services
{
    public static class ServerCallContextExtensions
    {
        public static Guid GetUserId(this ServerCallContext context)
        {
            return context.GetHttpContext().User.GetUserId();
        }
    }
}