using NTS.Model.ExportTemplate;
using NTS.Model.GeneralTemplate;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.CheckDesignPlan;
using QLTK.Business.GeneralCheckList;
using QLTK.Business.GeneralConfirmElectronicRecord;
using QLTK.Business.GeneralDesignRecord;
using QLTK.Business.GeneralDrawControlAlgorithm;
using QLTK.Business.GeneralElectronicRecord;
using QLTK.Business.GeneralEquipmentByFunction;
using QLTK.Business.GeneralFormElectric;
using QLTK.Business.GeneralListFunction;
using QLTK.Business.GeneralMechanicalRecords;
using QLTK.Business.GeneralProgramableData;
using QLTK.Business.GeneralSetUpSpecification;
using QLTK.Business.GeneralTempalteMechanical;
using QLTK.Business.GeneralTemplate;
using QLTK.Business.GeneralTemplateElectronic;
using QLTK.Business.GeneralTestProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using MaterialModel = NTS.Model.GeneralTemplate.MaterialModel;


namespace QLTK.Api.Controllers.GeneralTemplate
{
    [RoutePrefix("api/General")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F090110;F090203;F090309;F090406;F090502")]
    public class GeneralTemplateController : BaseController
    {
        private readonly GeneralElectronic _generalElectronic = new GeneralElectronic();
        private readonly GeneralPrinciples _generalPrinciples = new GeneralPrinciples();
        private readonly GeneralPrinciplesCalculate _generalPrinciplesCalculate = new GeneralPrinciplesCalculate();
        private readonly GeneralCheckElectronic _generalCheckElectronic = new GeneralCheckElectronic();
        private readonly GeneralElectronicCircuitAssembly _generalElectronicCircuitAssembly = new GeneralElectronicCircuitAssembly();
        private readonly GeneralMaterial _generalMaterial = new GeneralMaterial();
        private readonly GeneralDesignOptions _generalDesignOptions = new GeneralDesignOptions();
        private readonly GeneralFunctionDesignOptions _generalFunctionDesignOptions = new GeneralFunctionDesignOptions();
        private readonly GeneralFunctionDesignMaterial _generalFunctionDesignMaterial = new GeneralFunctionDesignMaterial();
        private readonly GeneralElectronicRecord _generalElectronicRecord = new GeneralElectronicRecord();
        private readonly GeneralMechanicalRecord _generalMechanicalRecord = new GeneralMechanicalRecord();
        private readonly GeneralDesignRecord _generalDesignRecord = new GeneralDesignRecord();
        private readonly GeneralCheckList _generalCheckList = new GeneralCheckList();
        private readonly GeneralConfirmElectronicRecord _generalConfirmElectronicRecord = new GeneralConfirmElectronicRecord();
        private readonly GeneralProgramableData _generalProgramableData = new GeneralProgramableData();
        private readonly GeneralDrawControlAlgorithm _generalDrawControlAlgorithm = new GeneralDrawControlAlgorithm();
        private readonly GeneralListFunction _generalListFunction = new GeneralListFunction();
        private readonly GeneralTestProcess _generalTestProcess = new GeneralTestProcess();
        private readonly GeneralEquipmentByFunction _generalEquipmentByFunction = new GeneralEquipmentByFunction();
        private readonly GeneralSetUpSpecification _generalSetUpSpecification = new GeneralSetUpSpecification();
        private readonly CheckDesignPlanBusiness checkDesignPlanBusiness = new CheckDesignPlanBusiness();
        private readonly GeneralTempalteMechanical _generalTempalteMechanical = new GeneralTempalteMechanical();
        private readonly GeneralPreliminaryEstimate _preliminaryEstimate = new GeneralPreliminaryEstimate();
        private readonly GeneralProfileElectronicDesign _profileElectronicDesign = new GeneralProfileElectronicDesign();
        private readonly TableCheckPrinciplesElectricBussiness _tabCheckElectric = new TableCheckPrinciplesElectricBussiness();
        private readonly DataProgramElectricBussiness _dataProgramElectric = new DataProgramElectricBussiness();
        private readonly DesignArticleBussiness _designArticleElectric = new DesignArticleBussiness();

        private readonly GetMaterialBussiness _getMaterial = new GetMaterialBussiness();
        [Route("GeneralElectronic")]
        [NTSAuthorize(AllowFeature = "F090109")]
        public HttpResponseMessage GeneralTemplateElectronic(GeneralElectronicModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            string path = _generalElectronic.GeneralTemplateElectronic(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("GeneralPrinciples")]
        [NTSAuthorize(AllowFeature = "F090101")]
        public HttpResponseMessage GeneralTemplateElectronicPrinciples(GeneralElectronicModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            string path = _generalPrinciples.GeneralTemplateElectronicPrinciples(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("GeneralCheckElectronic")]
        [NTSAuthorize(AllowFeature = "F090105")]
        public HttpResponseMessage GeneralTempalteCheckElectronic(GeneralElectronicModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            string path = _generalCheckElectronic.GeneralTempalteCheckElectronic(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("GeneralPrinciplesCalculate")]
        [NTSAuthorize(AllowFeature = "F090102")]
        public HttpResponseMessage GeneralTemplatePrinciplesCalculate(GeneralElectronicModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            string path = _generalPrinciplesCalculate.GeneralTemplatePrinciplesCalculate(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("GeneralElectronicCircuitAssembly")]
        [NTSAuthorize(AllowFeature = "F090106")]
        public HttpResponseMessage GeneralTemplateElectronicCircuitAssembly(GeneralElectronicModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            string path = _generalElectronicCircuitAssembly.GeneralTemplateElectronicCircuitAssembly(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("GeneralMaterial")]
        [NTSAuthorize(AllowFeature = "F090108")]
        public HttpResponseMessage GeneralTemplateMaterial(GeneralElectronicModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            string path = _generalMaterial.GeneralTemplateMaterial(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("GeneralDesignOptions")]
        [NTSAuthorize(AllowFeature = "F090107")]
        public HttpResponseMessage GeneralTempalteDesignOptions(GeneralElectronicModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            string path = _generalDesignOptions.GeneralTempalteDesignOptions(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("GeneralFunctionDesignOptions")]
        [NTSAuthorize(AllowFeature = "F090103")]
        public HttpResponseMessage GeneralTemplateFunctionDesignOptions(GeneralElectronicModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            string path = _generalFunctionDesignOptions.GeneralTemplateFunctionDesignOptions(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("GeneralFunctionDesignMaterial")]
        [NTSAuthorize(AllowFeature = "F090104")]
        public HttpResponseMessage GeneralTemplateFunctionDesignMaterial(GeneralElectronicModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            string path = _generalFunctionDesignMaterial.GeneralTemplateFunctionDesignMaterial(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("GeneralElectronicRecord")]
        [NTSAuthorize(AllowFeature = "F090201")]
        public HttpResponseMessage GeneralElectronicRecord(ElectronicRecordsModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            string path = _generalElectronicRecord.GeneralTemplateMaterial(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("GeneralMechanicalRecord")]
        public HttpResponseMessage GeneralMechanicalRecord(MechanicalRecordsModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            string path = _generalMechanicalRecord.GeneralTemplateMechanicalRecord(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("GeneralDesignRecord")]
        public HttpResponseMessage GeneralDesignRecord(DesignRecordModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            string path = _generalDesignRecord.GeneralTempalteDesignRecord(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("GeneralCheckList")]
        public HttpResponseMessage GeneralCheckList(GeneralCheckListModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            string path = _generalCheckList.GeneralTempalteCheckList(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }


        [Route("GeneralConfirmElectronicRecord")]
        public HttpResponseMessage GeneralConfirmElectronicRecord(ConfirmElectronicRecordModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            string path = _generalConfirmElectronicRecord.GeneralTemplateConfirmElectronicRecord(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("GeneralProgramableData")]
        [NTSAuthorize(AllowFeature = "F090301")]
        public HttpResponseMessage GeneralProgramableData(ProgrammableDataModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            string path = _generalProgramableData.GeneralTemplateProgramableData(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("GeneralDrawControlAlgorithmModel")]
        [NTSAuthorize(AllowFeature = "F090302")]
        public HttpResponseMessage GeneralDrawControlAlgorithmModel(DrawControlAlgorithmModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            string path = _generalDrawControlAlgorithm.GeneralTempalteDrawControlAlgorithm(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("GeneralListFunction")]
        [NTSAuthorize(AllowFeature = "F090303")]
        public HttpResponseMessage GeneralListFunction(ListFunctionModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            string path = _generalListFunction.GeneralTemplatelListFunction(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("GeneralTestProcess")]
        [NTSAuthorize(AllowFeature = "F090304")]
        public HttpResponseMessage GeneralTestProcess(TestProcessModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            string path = _generalTestProcess.GeneralTempalteTestProcess(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("GeneralEquipmentByFunction")]
        [NTSAuthorize(AllowFeature = "F090305")]
        public HttpResponseMessage GeneralEquipmentByFunction(EquipmentByFunctionModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            string path = _generalEquipmentByFunction.GeneralTempalteEquipmentByFunction(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("GeneralSetUpSpecification")]
        [NTSAuthorize(AllowFeature = "F090404")]
        public HttpResponseMessage GeneralSetUpSpecification(GeneralMechanicalModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            string path = _generalSetUpSpecification.GeneralTemplateSetUpSpecification(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("CheckDesignPlan")]
        [NTSAuthorize(AllowFeature = "F090402")]
        public HttpResponseMessage CheckDesignPlan(GeneralMechanicalModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            string path = checkDesignPlanBusiness.CheckDesignPlan(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("GeneralDegignMechanical")]
        [NTSAuthorize(AllowFeature = "F090401")]
        public HttpResponseMessage GeneralDegignMechanical(GeneralMechanicalModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            string path = _generalTempalteMechanical.GeneralDegignMechanical(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("GeneralPreliminaryEstimate")]
        [NTSAuthorize(AllowFeature = "F090403")]
        public HttpResponseMessage GeneralPreliminaryEstimate(GeneralMechanicalModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            string path = _preliminaryEstimate.GeneralTemplatePreliminaryEstimate(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("GeneralProfileElectronicDesign")]
        [NTSAuthorize(AllowFeature = "F090501")]
        public HttpResponseMessage GeneralProfileElectronicDesign(ProfileElectronicDesignModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            string path = _profileElectronicDesign.GeneralTemplateProfileElectronicDesign(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("GeneralCheckPrinciplesElectric")]
        [NTSAuthorize(AllowFeature = "F090307")]
        public HttpResponseMessage GeneralCheckPrinciplesElectric(TableCheckPrinciplesElectricModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            string path = _tabCheckElectric.GeneralCheckPrinciplesElectric(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("GeneralDataProgramElectric")]
        public HttpResponseMessage GeneralDataProgramElectric(DataProgramElectricModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            string path = _dataProgramElectric.GeneralDataProgramElectric(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("DesignArticleElectric")]
        [NTSAuthorize(AllowFeature = "F090308")]
        public HttpResponseMessage DesignArticleElectric(DesignArticleModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            string path = _designArticleElectric.DesignArticleElectric(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("GetMaterials")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090403")]
        public HttpResponseMessage GetMaterials(MaterialSearchModel model)
        {
            var result = _getMaterial.SearchMaterial(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

    }
}