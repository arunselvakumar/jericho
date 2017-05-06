namespace Jericho.Models.v1.BOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    public class PostBo
    {
        #region Public Properties

        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Type { get; set; }

        public string Status { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Content { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string CategoryId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string PostedBy { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public IEnumerable<CommentBo> Comments { get; set; }

        public long UpVotes { get; set; }

        public long DownVotes { get; set; }

        public string Url { get; set; }

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
