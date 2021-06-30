using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TestUtils
{
    /// <summary>
    /// App settings helper methods.
    /// </summary>
    public static class AppSettingsHelper
    {
        /// <summary>
        /// Application settings file name.
        /// </summary>
        private const string AppSettingsFileName = "appsettings.json";

        /// <summary>
        /// Gets application settings file path.
        /// </summary>
        public static string AppSettingsPath => Path.Combine(Directory.GetCurrentDirectory(), AppSettingsFileName);

        /// <summary>
        /// Gets overriden host settings.
        /// </summary>
        /// <param name="networkDependencyPort">Mock server port for network dependencies.</param>
        /// <returns>Stream with overriden settings.</returns>
        /// <remarks>Override ports to <paramref name="networkDependencyPort"/>.</remarks>
        public static Stream GetOverridenAppSettings(int networkDependencyPort)
        {
            var appSettings = new StringBuilder(File.ReadAllText(AppSettingsPath));

            var regex = new Regex(@"http:\/\/localhost:\d{4}");
            var oldEndpoints = regex.Matches(appSettings.ToString())
                .Select(match => match.Value);

            string newEndpoint = $@"http://localhost:{networkDependencyPort}";
            foreach (var oldEndpoint in oldEndpoints)
            {
                // HACK: exclude local elastic search port.
                if (oldEndpoint.Contains("9200"))
                {
                    continue;
                }

                appSettings.Replace(oldEndpoint, newEndpoint);
            }

            var bytes = Encoding.UTF8.GetBytes(appSettings.ToString());
            return new MemoryStream(bytes);
        }
    }
}