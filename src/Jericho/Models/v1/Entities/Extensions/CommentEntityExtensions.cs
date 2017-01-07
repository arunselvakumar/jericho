namespace Jericho.Models.v1.Entities.Extensions
{
    using Jericho.Extensions;
    using MongoDB.Bson;
    using System;

    public static class CommentEntityExtensions
    {
        //ToDo : Create a CreateCommentDto class and move the below code to Automapper
        public static void ApplyPresets(this CommentEntity commentEntity)
        {
            commentEntity.Id = ObjectId.Empty;
            commentEntity.UpVotes = 0;
            commentEntity.DownVotes = 0;
            commentEntity.CreatedOn = DateTime.Now;
        }
    }
}
