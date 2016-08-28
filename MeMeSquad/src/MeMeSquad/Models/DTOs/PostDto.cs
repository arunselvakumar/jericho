using System;
using System.Collections.Generic;
using MeMeSquad.Models.Entities.Enums;

namespace MeMeSquad.Models.DTOs
{
    public class PostDto
    {
        public PostTypeEnum Type { get; set; }

        public Dictionary<char, string> Content { get; set; }

        public string CategoryId { get; set; }

        public string PostedBy { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public long UpVotes { get; set; }

        public long DownVotes { get; set; }
    }
}