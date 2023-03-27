namespace WalksAPI.Models.DTO
{
    public class AddRegion
    {
        //client need to give values to property as expected
        //shouldn't null,shouldn't empty ,not contain white spaces
        public string Code { get; set; }
        public string Name { get; set; }
        public double Area { get; set; }
        public double Long { get; set; }
        public double Lat { get; set; }
        public long Population { get; set; }
    }
}
