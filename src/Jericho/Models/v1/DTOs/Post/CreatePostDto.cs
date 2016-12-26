namespace Jericho.Models.v1.DTOs.Post
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CreatePostDto
    {
        //#region Public Properties

        //public string Type { get; set; }

        //public string Title { get; set; }

        //public string Content { get; set; }

        //public string CategoryId { get; set; }

        //public IEnumerable<string> Tags { get; set; }

        //#endregion

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

        public long UpVotes { get; set; }

        public long DownVotes { get; set; }

        public string Url { get; set; }

        public System.DateTime CreatedOn { get; set; }

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
