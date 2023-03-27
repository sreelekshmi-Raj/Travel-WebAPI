using AutoMapper;

namespace WalksAPI.Profiles
{
    public class WalkProfile : Profile
    {
        public WalkProfile()
        {
            CreateMap<Models.Domain.Walk, Models.DTO.Walk>().ReverseMap();
            //map for walkdifficulty
            //walkdifficulty related to walk so can map here
            CreateMap<Models.Domain.WalkDifficulty, Models.DTO.WalkDifficulty>().ReverseMap();
        }
        
    }
}
