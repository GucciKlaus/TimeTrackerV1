using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerLib
{
    public class DataObjectV2
    {
        public int DataObjectV2ID { get; set; }
        public string? Date { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Timespan {  get; set; }

        public int UserUserID {  get; set; }
        public User? DataObjectV2User { get; set; }
    }
}
