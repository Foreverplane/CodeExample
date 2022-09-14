using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class AuthorData : IEntityData
{
    [Key(0)]
    public int authorId;

    public AuthorData(int authorId)
    {
        this.authorId = authorId;
    }

    public AuthorData()
    {
    }
}