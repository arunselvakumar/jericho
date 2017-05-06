namespace Jericho.Aggregators.Interfaces
{
    using System.Threading.Tasks;

    using Jericho.Models.v1.BOs;

    public interface ICommentAggregator
    {
        Task AggregateCommentsForPost(PostBo postBo);
    }
}
