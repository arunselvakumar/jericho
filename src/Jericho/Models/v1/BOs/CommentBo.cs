namespace Jericho.Models.v1.BOs
{
    using System;

    using Newtonsoft.Json;

    public class CommentBo
    {
        #region Public Properties

        public string Id { get; set; }
        
        public string PostId { get; set; }
        
        public string ParentId { get; set; }
        
        public string Comment { get; set; }
       
        public string CommentedBy { get; set; }

        public long UpVotes { get; set; }

        public long DownVotes { get; set; }        

        public DateTime CreatedOn { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns JSON string that represents the current object.
        /// </summary>
        /// <returns>JSON formatted string</returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        #endregion
    }
}
