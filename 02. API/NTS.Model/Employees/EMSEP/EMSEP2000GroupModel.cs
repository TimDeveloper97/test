// <copyright company="nhantinsoft.vn">
// Author: Sinh
// Created Date: 30-11-2017
// </copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTS.Model.EMSEP
{
    public class EMSEP2000GroupModel:Common.CommonModel
    {

        public string GroupId {get;set;}
        public string Search {get;set;}
        public string Name {get;set;}
        public string Status {get;set;}
        public string Default {get;set;}
        public string Description {get;set;}  
        public string GroupType { get; set; }
        public List<EMSEP1100UserPermissionModel> ListFunction;

        public int TotalRows { get; set; }

        public EMSEP2000GroupModel()
        {
            ListFunction = new List<EMSEP1100UserPermissionModel>();
        } 
    }
}