
namespace DataLayerLib
{
    public class User
    {
        public int UserID {  get; set; }
        public string? UserName { get; set; }
        public List<DataObjectV2> DataObjects { get; set; } = new List<DataObjectV2>();
    }

}
