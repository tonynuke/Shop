using System;
using System.Collections.Generic;

namespace Identity.WebService.Dto
{
    public class CreateClientModel
    {
        public string Name { get; set; }

        public IReadOnlyCollection<string> Scopes { get; set; } = Array.Empty<string>();
    }
}