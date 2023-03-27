using Microsoft.EntityFrameworkCore;
using WalksAPI.Data;
using WalksAPI.Models.Domain;

namespace WalksAPI.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly WalksDbContext walksDbContext;
        public RegionRepository(WalksDbContext walksDbContext)
        {
            this.walksDbContext = walksDbContext;
        }

        public async Task<Region> AddAsync(Region region)
        {
            region.Id=Guid.NewGuid();
            await walksDbContext.AddAsync(region);
            await walksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            var region=await walksDbContext.Regions.FirstOrDefaultAsync(x => x.Id==id);
            if (region == null)
                return null;
            
            walksDbContext.Regions.Remove(region);
            await walksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await walksDbContext.Regions.ToListAsync();
        }

        public async Task<Region> GetAsync(Guid id)
        {
           return await walksDbContext.Regions.FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
            var dbRegion=await walksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if(dbRegion==null)
                return null;

           //don't update id because it is primary key
            dbRegion.Code= region.Code;
            dbRegion.Name= region.Name;
            dbRegion.Area= region.Area;
            dbRegion.Lat= region.Lat;
            dbRegion.Long= region.Long;
            dbRegion.Population= region.Population;

            await walksDbContext.SaveChangesAsync();

            return dbRegion;
           
        }
    }
}
