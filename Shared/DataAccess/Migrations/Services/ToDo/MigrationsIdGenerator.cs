using System;
using System.Globalization;

namespace DataAccess.Migrations.Services.ToDo
{
    public class MigrationsIdGenerator
    {
        private const string Format = "yyyyMMddHHmmss";
        private readonly object _lock = new ();
        private DateTime _lastTimestamp = DateTime.MinValue;

        public virtual string GenerateId(string name)
        {
            var now = DateTime.UtcNow;
            var timestamp = new DateTime(
                now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

            lock (_lock)
            {
                if (timestamp <= _lastTimestamp)
                {
                    timestamp = _lastTimestamp.AddSeconds(1);
                }

                _lastTimestamp = timestamp;
            }

            return timestamp.ToString(Format, CultureInfo.InvariantCulture) + "_" + name;
        }
    }
}