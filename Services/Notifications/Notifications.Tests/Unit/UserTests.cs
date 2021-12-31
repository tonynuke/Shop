using System;
using AutoFixture;
using FluentAssertions;
using Notifications.Domain;
using Xunit;
using Xunit.Abstractions;

namespace Notifications.Tests.Unit
{
    public class UserTests
    {
        private readonly Fixture _fixture = new ();

        private readonly ITestOutputHelper _testOutputHelper;

        public UserTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Link_user_to_device()
        {
            var user = new User(Guid.NewGuid());
            user.Devices.Should().BeEmpty();

            var device = _fixture.Create<Device>();
            user.LinkToDevice(device);

            user.Devices.Should().Contain(device);
        }

        [Fact]
        public void Unlink_user_from_device()
        {
            var user = new User(Guid.NewGuid());
            var device = _fixture.Create<Device>();
            user.LinkToDevice(device);
            user.UnlinkFromDevice(device);

            user.Devices.Should().BeEmpty();
        }

        [Fact]
        public void Unlink_user_from_device_when_device_is_not_linked()
        {
            var user = new User(Guid.NewGuid());
            var device = _fixture.Create<Device>();
            user.UnlinkFromDevice(device);

            user.Devices.Should().BeEmpty();
        }
    }
}
