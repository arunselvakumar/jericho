namespace Jericho.Models.v1.Entities.Extensions
{
    using Jericho.Extensions;
    using System;

    public static class PostEntityExtensions
    {
        public static void ApplyPresets(this PostEntity postEntity)
        {
            postEntity.Url = $"{postEntity.Title.Trim().Replace(' ', '_').ToLower()}_{DateTime.UtcNow.ToTimeStamp()}";
            postEntity.UpVotes = 0;
            postEntity.DownVotes = 0;
            postEntity.CreatedOn = DateTime.Now;
        }
    }
}
