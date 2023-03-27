using Microsoft.EntityFrameworkCore;
using WalksAPI.Data;
using WalksAPI.Models.Domain;

namespace WalksAPI.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly WalksDbContext walksDbContext;
        public WalkRepository(WalksDbContext walksDbContext)
        {
            this.walksDbContext = walksDbContext;
        }

        public async Task<Walk> AddWalkAsync(Walk walk)
        {
            walk.Id=Guid.NewGuid();
            await walksDbContext.Walks.AddAsync(walk);
            await walksDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var walkDb = await walksDbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (walkDb==null)
                return null;
            walksDbContext.Walks.Remove(walkDb);
            await walksDbContext.SaveChangesAsync();
            return walkDb;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await walksDbContext.Walks.Include(x=>x.Region)
                .Include(x=>x.WalkDifficulty)
                .ToListAsync();
        }

        public async Task<Walk> GetWalk(Guid id)
        {
            //green line indicate that this can return null value
            return await walksDbContext.Walks.Include(x=>x.Region)
                .Include(x=>x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id==id);
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            //FirstOrDefaultAsync or use FindAsync
            var walkDb =await walksDbContext.Walks.FirstOrDefaultAsync(x=>x.Id==id);
            if (walkDb == null)
                return null;
            walkDb.Name=walk.Name;
            walkDb.Length=walk.Length;
            walkDb.RegionId=walk.RegionId;
            walkDb.WalkDifficultyId = walk.WalkDifficultyId;

            await walksDbContext.SaveChangesAsync();

             return walkDb;
            //return await walksDbContext.Walks.Include(x => x.Region)
            //     .Include(x => x.WalkDifficulty)
            //     .FirstOrDefaultAsync(x => x.Id == id);

        }
    }
}
