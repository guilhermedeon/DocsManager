namespace DocsManager.Core.Domain.Documents.Revisions
{
    public static class RevisionFactory
    {
        public static Revision Create(string revision)
        {
            return revision switch
            {
                "0" => Revision.Zero,
                "A" => Revision.A,
                "B" => Revision.B,
                "C" => Revision.C,
                "D" => Revision.D,
                "E" => Revision.E,
                "F" => Revision.F,
                "G" => Revision.G,
                _ => throw new ArgumentException($"Invalid revision: {revision}")
            };
        }
    }
}
