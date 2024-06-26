using DataLayerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrackerV1
{
    public class Printobject
    {
        public string? Name { get; set; }
        public List<string> data { get; set; } = new List<string>();
        public string? TimeSum { get; set; }
    }
}
