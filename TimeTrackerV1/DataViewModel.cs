using DataLayerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrackerV1
{
    public class DataViewModel
    {
        public string? Date {  get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Timespan { get; set; }


        public static List<DataViewModel> ConvertDataList(List<DataObjectV2> sepp)
        {
            var list = new List<DataViewModel>();
            foreach (DataObjectV2 v in sepp) {
              DataViewModel dummy = new DataViewModel { Date = v.Date, Title = v.Title, Description = v.Description, Timespan = v.Timespan };
                list.Add(dummy);
            }
            return list;
        }
    }
}
