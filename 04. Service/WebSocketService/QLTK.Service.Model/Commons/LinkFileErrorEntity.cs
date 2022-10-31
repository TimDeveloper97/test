using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Model.Commons
{
    public class LinkFileErrorEntity
    {
        private string linkFile;

        public string LinkFile
        {
            get { return linkFile; }
            set { linkFile = value; }
        }

        private bool statusFile;

        public bool StatusFile
        {
            get { return statusFile; }
            set { statusFile = value; }
        }

        private string fileName;

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
    }
}
