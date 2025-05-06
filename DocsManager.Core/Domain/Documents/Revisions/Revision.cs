using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DocsManager.Core.Domain.Documents.Revisions
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Revision
    {
        Zero,
        A,
        B,
        C,
        D,
        E,
        F,
        G
    }

    public static class RevisionExtensions
    {
        public static string ToString(this Revision revision)
        {
            return revision switch
            {
                Revision.Zero => "0",
                Revision.A => "A",
                Revision.B => "B",
                Revision.C => "C",
                Revision.D => "D",
                Revision.E => "E",
                Revision.F => "F",
                Revision.G => "G",
                _ => throw new ArgumentException($"Invalid revision: {revision}")
            };
        }
    }
}
