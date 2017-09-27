
namespace RTLS.Domains
{
    public class Record
    {
        public LocationData[] records { get; set; }
    }

    public class ListOfArea
    {
        public Record device_notification { get; set; }
    }
}
