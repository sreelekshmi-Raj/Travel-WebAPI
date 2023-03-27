using AutoMapper;

namespace WalksAPI.Profiles
{
    public class RegionProfile:Profile
    {
        public RegionProfile()
        {
            CreateMap<Models.Domain.Region, Models.DTO.Region>().ReverseMap();
            //if both variable name are diff then use below map src-domain dest-dto
            //.ForMember(dest=>dest.Id,options=>options.MapFrom(src=>src.RegionId));
        }
    }
}
