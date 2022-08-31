using System.Collections.Generic;

namespace ikeamind_backend.Core.Models.ReturnModels
{
    public record GetBestscoresRM
    {
        public Dictionary<string, int> Bestscores { get; set; }
    }
}
