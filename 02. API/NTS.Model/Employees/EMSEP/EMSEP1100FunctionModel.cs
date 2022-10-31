// <copyright company="nhantinsoft.vn">
// Author: Sinh
// Created Date: 24/11/2017 15:08
// </copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTS.Model.EMSEP
{
    public class EMSEP1100FunctionModel:SearchCommonModel
    {

        public string FunctionId { get; set; }
        public string GroupFunctionId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


         //ahahahah
        public string FunctionName { get; set; }
        public string FunctionCode { get; set; } 

        public string Search { get; set; }
        public string GroupSelect { get; set; }

        public string Index { get; set; }
        public bool Checked { get; set; }
        public bool Select { get; set; }
    }
}