namespace Jericho.Providers.Interfaces
{
    interface IServiceResult<T>
    {
        bool Succeeded { get; set; }    

        T Value { get; set; }

        object Errors { get; set; }
    }
}
