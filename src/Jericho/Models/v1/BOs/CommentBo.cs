using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jericho.Models.v1.BOs
{
    public class CommentBo
    {
        #region Public Properties

        public string Id { get; set; }
        
        public string PostId { get; set; }
        
        public string ParentId { get; set; }
        
        public string Text { get; set; }
        
        public string Url { get; set; }
        
        public string PostedBy { get; set; }

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
