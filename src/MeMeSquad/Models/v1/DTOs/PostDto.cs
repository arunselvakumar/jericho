namespace Jericho.Models.v1.DTOs
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    public class PostDto
    {
        #region Public Properties

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Post Type")]
        public string Type { get; set; }

        public Dictionary<char, string> Content { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Category Id")]
        public string CategoryId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Posted By")]
        public string PostedBy { get; set; }

        public IEnumerable<string> Tags { get; set; }

        [Display(Name = "Up Votes")]
        public long UpVotes { get; set; }

        [Display(Name = "Down Votes")]
        public long DownVotes { get; set; }

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