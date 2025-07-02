using Newtonsoft.Json.Linq;

namespace Ntreev.Library.Psd.Readers.ResourceReader;

internal class EmptyResourceReader(PsdBinaryReader reader, long length) : ValueReader<JToken>(reader, length, null)
    {
    protected override JObject ReadValue() => [];
    }

