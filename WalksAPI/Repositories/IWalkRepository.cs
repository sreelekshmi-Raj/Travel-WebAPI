using WalksAPI.Models.Domain;

namespace WalksAPI.Repositories
{
    public interface IWalkRepository
    {
        Task<IEnumerable<Walk>> GetAllAsync();
        Task<Walk> GetWalk(Guid id);
        Task<Walk> AddWalkAsync(Walk walk);
        Task<Walk> UpdateAsync(Guid id, Walk walk);
        Task<Walk> DeleteAsync(Guid id);
    }
}
