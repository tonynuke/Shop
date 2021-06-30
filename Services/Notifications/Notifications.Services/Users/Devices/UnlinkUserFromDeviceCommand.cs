using System;
using MediatR;

namespace Notifications.Services.Users.Devices
{
    public class UnlinkUserFromDeviceCommand : IRequest<Unit>
    {
        public UnlinkUserFromDeviceCommand(Guid userId, string token)
        {
            UserId = userId;
            Token = token;
        }

        public Guid UserId { get; }

        public string Token { get; }
    }
}