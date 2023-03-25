namespace Chat.Services.Abstractions
{
    public interface ITokenService<TIn, TOut>
    {
        Task<TOut> CreateAsync(TIn tokenSubject);
    }
}
