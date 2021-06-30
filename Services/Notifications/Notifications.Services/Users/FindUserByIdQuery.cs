using System;
using CSharpFunctionalExtensions;
using MediatR;
using Notifications.Domain;

namespace Notifications.Services.Users
{
    public class FindUserByIdQuery : IRequest<Maybe<User>>
    {
        public FindUserByIdQuery(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
    }
}