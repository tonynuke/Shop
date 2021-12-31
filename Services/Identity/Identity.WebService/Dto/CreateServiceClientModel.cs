using System;
using System.Collections.Generic;

namespace Identity.WebService.Dto
{
    public class CreateServiceClientModel
    {
        public string Name { get; set; }

        public string Secret { get; set; }

        public IReadOnlyCollection<string> Scopes { get; set; } = Array.Empty<string>();
    }
}
