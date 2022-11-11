using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Model.Commons
{
    public class CheckUploadEntity
    {
        private string linkFolder;
        public string LinkFolder
        {
            get { return linkFolder; }
            set { linkFolder = value; }
        }

        private List<LinkFileErrorEntity> linkFileErrorEntity;

        public List<LinkFileErrorEntity> LinkFileErrorEntity
        {
            get { return linkFileErrorEntity; }
            set { linkFileErrorEntity = value; }
        }

        private string manuFacturer;

        public string ManuFacturer
        {
            get { return manuFacturer; }
            set { manuFacturer = value; }
        }

        private bool statusError;

        public bool StatusError
        {
            get { return statusError; }
            set { statusError = value; }
        }
    }
}
