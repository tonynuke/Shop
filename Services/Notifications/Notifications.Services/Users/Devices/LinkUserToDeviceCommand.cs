using System;
using MediatR;

namespace Notifications.Services.Users.Devices
{
    public class LinkUserToDeviceCommand : IRequest<Unit>
    {
        public LinkUserToDeviceCommand(Guid userId, string token)
        {
            UserId = userId;
            Token = token;
        }

        public Guid UserId { get; }

        public string Token { get; }
    }
}