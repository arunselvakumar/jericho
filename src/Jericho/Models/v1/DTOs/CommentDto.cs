namespace Jericho.Models.v1.DTOs
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;
    using System;

    public class CommentDto
    {
        #region Public Properties

        public string Id { get; set; }
            
        [Required]
        [DataType(DataType.Text)]
        public string PostId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string ParentId { get; set; }

        [DataType(DataType.Text)]
        public string Comment { get; set; }

        //ToDo: Is this needs [Required] ?
        [Required]
        [DataType(DataType.Text)]
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