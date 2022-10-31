// <copyright company="nhantinsoft.vn">
// Author: NTS-THANHPQ
// Created Date: 09/08/2016 15:08
// </copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTS.Model.EMSEP
{
    public class EMSEP2000FunctionModel
    {
        public string FunctionId {get;set;}
        public string FunctionName {get;set;}
        public string FunctionCode {get;set;}
        public string GroupFunctionId {get;set;}
        public string CreateBy {get;set;}
        public string Search {get;set;}
        public string GroupSelect {get;set;}
        public int pageNumber {get;set;}
        public int pageSize {get;set;}
        public string Index{get;set;}
        public DateTime? CreateDate {get;set;}
        public string UpdateBy {get;set;}
        public DateTime? UpdateDate {get;set;}
        public bool Checked{get;set;}
    }
}