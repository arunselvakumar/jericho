using Jericho.Models.v1.BOs;
using Jericho.Models.v1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jericho.Aggregators.Interfaces
{
    public interface ICommentAggregator
    {
        Task AggregateCommentsForPost(PostBo postBo);
    }
}
