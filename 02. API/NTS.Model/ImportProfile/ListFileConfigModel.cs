using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ImportProfile
{
    public class ListFileConfigModel
    {
        public string Id { get; set; }
        public int Step { get; set; }
        public string Name { get; set; }
        public bool IsRequired { get; set; }
        public string Note { get; set; }
        public bool Checked { get; set; }
    }
}
