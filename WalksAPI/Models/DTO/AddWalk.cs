﻿namespace WalksAPI.Models.DTO
{
    public class AddWalk
    {
        public string Name { get; set; }
        public double Length { get; set; }
        public Guid RegionId { get; set; }
        public Guid WalkDifficultyId { get; set; }


    }
}
