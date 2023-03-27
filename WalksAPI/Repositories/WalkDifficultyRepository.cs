using Microsoft.EntityFrameworkCore;
using WalksAPI.Data;
using WalksAPI.Models.Domain;

namespace WalksAPI.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly WalksDbContext walksDbContext;
        public WalkDifficultyRepository(WalksDbContext walksDbContext)
        {
            this.walksDbContext = walksDbContext;
        }
        public async Task<WalkDifficulty> CreateAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id=Guid.NewGuid();
            await walksDbContext.WalkDifficulties.AddAsync(walkDifficulty);
            await walksDbContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var walkDifficulty=walksDbContext.WalkDifficulties.FirstOrDefault(x => x.Id==id);
            if (walkDifficulty == null)
                return null;
            walksDbContext.WalkDifficulties.Remove(walkDifficulty);
            await walksDbContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await walksDbContext.WalkDifficulties.ToListAsync();

        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            var walkDifficulty = await walksDbContext.WalkDifficulties.FirstOrDefaultAsync(x => x.Id == id);
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty newDifficulty) 
        {
            //or use FindAsync(id)
            var walkDifficulty=walksDbContext.WalkDifficulties.FirstOrDefault(x => x.Id == id);
            if (walkDifficulty == null)
                return null;
            walkDifficulty.Code=newDifficulty.Code;
            await walksDbContext.SaveChangesAsync();
            return walkDifficulty;


        }
    }
}
