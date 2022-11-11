// <copyright company="nhantinsoft.vn">
// Author: Sinh
// Created Date: 24/11/2017 15:08
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.EMSEP
{
   public class EMSEP1000SearchEmployee: SearchCommonModel
    {
        public string SearchNameEmployee { get; set; }
        public string SearchPhoneEmployee { get; set; }
        public string SearchCodeEmployee { get; set; }
        public string CustomerId { get; set; }

    }
}
