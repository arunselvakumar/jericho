namespace Jericho.Models.v1.Entities.Extensions
{
    using Jericho.Extensions;
    using MongoDB.Bson;
    using System;

    public static class PostEntityExtensions
    {
        //ToDo: Consider moving this to AutoMapper.
        //Previously we placed here, bcoz at that time we had only 1 dto for post.
        //NotInUse Now
        public static void ApplyPresets(this PostEntity postEntity)
        {
            postEntity.Id = ObjectId.Empty;
            postEntity.Url = $"{postEntity.Title.Trim().Replace(' ', '_').ToLower()}_{DateTime.UtcNow.ToTimeStamp()}";
            postEntity.UpVotes = 0;
            postEntity.DownVotes = 0;
            postEntity.CreatedOn = DateTime.Now;
        }
    }
}
