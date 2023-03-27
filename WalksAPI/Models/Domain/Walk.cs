namespace WalksAPI.Models.Domain
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Length { get; set; }
        public Guid RegionId { get; set; }
        public Guid WalkDifficultyId { get; set; }

        //Navigation property one walk related to one region and one walkdifficulty
        public Region Region { get; set; }
        public WalkDifficulty WalkDifficulty { get; set; }
    }
}
