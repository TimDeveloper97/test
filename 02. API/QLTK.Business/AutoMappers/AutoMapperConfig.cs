using AutoMapper;
using NTS.Model.Bussiness;
using NTS.Model.CustomerHistory;
using NTS.Model.CustomerTypeHistory;
using NTS.Model.DegreeHistory;
using NTS.Model.ErrorLogHistory;
using NTS.Model.ExpertHistory;
using NTS.Model.FunctionHistory;
using NTS.Model.HistoryVersion;
using NTS.Model.IndustryHistory;
using NTS.Model.MaterialGroup;
using NTS.Model.MaterialGroupTPA;
using NTS.Model.Materials;
using NTS.Model.ModuleHistory;
using NTS.Model.PracticeHistory;
using NTS.Model.PracticeSkillHistory;
using NTS.Model.ProductStandardHistory;
using NTS.Model.ProjectHistory;
using NTS.Model.ProjectSolution;
using NTS.Model.RawMaterialHistory;
using NTS.Model.ReportProblemExist;
using NTS.Model.Repositories;
using NTS.Model.Sale.SaleProduct;
using NTS.Model.SaleGroups;
using NTS.Model.SimilarMaterialHistory;
using NTS.Model.SpecializeHistory;
using NTS.Model.SupplierHistory;
using NTS.Model.TaskModuleGroupHistory;
using NTS.Model.TestCriteriaHistory;
using NTS.Model.UnitHistory;
using NTS.Model.UserHistory;
using NTS.Model.WorkPlaceHistory;

namespace QLTK.Business.AutoMappers
{
    public static class AutoMapperConfig
    {
        public static IMapper Mapper { get; private set; }
        public static void Init()
        {
            var config = new MapperConfiguration(cf =>
            {
                //cf.CreateMap<MaterialGroupTPA, MarterialGroupTPAHistoryModel>();
                //cf.CreateMap<Material, MaterialHistoryModel>();
                //cf.CreateMap<MaterialGroup, MaterialGroupHistoryModel>();

                //cf.CreateMap<Unit, UnitHistoryModel>(); // đơn vị
                //cf.CreateMap<RawMaterial, RawMaterialHistoryModel>(); // vật liệu
                //cf.CreateMap<Supplier, SupplierHistoryModel>(); // nhà cc
                //cf.CreateMap<SimilarMaterial, SimilarMaterialHistoryModel>(); // vật tư tương tự
                //cf.CreateMap<Manufacture, ManufactureHistoryModel>(); // Hãng sản xuất

                //cf.CreateMap<Module, ModuleHistoryModel>(); // Module
                //cf.CreateMap<TestCriteria, TestCriteriasHistoryModel>(); // tiêu chí kiếm tra
                //cf.CreateMap<ProductStandard, ProductStandardLogHistoryModel>(); // tiêu chuẩn sản phẩm
                //cf.CreateMap<NTS.Model.Repositories.Function, FunctionHistoryModel>(); // tính năng
                //cf.CreateMap<Industry, IndustryHistoryModel>(); //nghành hàng
                //cf.CreateMap<NTS.Model.Repositories.NSMaterialGroup, NSMaterialGroupHistoryModel>(); //nhóm vật tư tiêu chuẩn

                //cf.CreateMap<Practice, PracticeHistoryModel>(); // BTH
                //cf.CreateMap<Skill, PracticeSkillHistoryModel>(); // Kỹ năng BTH
                //cf.CreateMap<NTS.Model.Repositories.Degree, DegreeHistoryModel>(); // Trình độ
                //cf.CreateMap<NTS.Model.Repositories.Specialize, SpecializeHistoryModel>(); // Chuyên môn
                //cf.CreateMap<Workplace, WorkPlaceHistoryModel>(); // đơn vị công tác
                //cf.CreateMap<NTS.Model.Repositories.Expert, ExpertHistoryModel>(); //Chuyên gia
                //cf.CreateMap<Subject, SubjectHistoryModel>();
                //cf.CreateMap<Education, EducationHistoryModel>();
                //cf.CreateMap<NTS.Model.Repositories.EducationProgram, EducationHistoryModel>();
                //cf.CreateMap<NTS.Model.Repositories.ClassRoom, ClassRoomHistoryModel>();
                //cf.CreateMap<Job, JobHistoryModel>();
                //cf.CreateMap<Plan, PlanHistoryModel>();
                //cf.CreateMap<Holiday, HolidayHistoryModel>();
                //cf.CreateMap<Solution, SolutionHistoryModel>();
                //cf.CreateMap<EmployeeTraning, EmployeeTrainingHistoryModel>();
                //cf.CreateMap<WorkDiary, WorkDiaryHistoryModel>();
                //cf.CreateMap<NTS.Model.Repositories.GroupUser, GroupUserHistoryModel>();
                //cf.CreateMap<FolderDefinition, FolderDefinitionHistoryModel>();
                //cf.CreateMap<ConfigScanFile, ScanFileHistoryModel>();
                //cf.CreateMap<DataDistributionFile, DataDistributionFileHistoryModel>();
                //cf.CreateMap<DataDistribution, DataDistributionHistoryModel>();

                //cf.CreateMap<Project, ProjectHistoryModel>(); // dự án
                //cf.CreateMap<CustomerType, CustomerTypeHistoryModel>(); // loại khách hàng
                //cf.CreateMap<Customer, CustomerHistoryModel>(); //  khách hàng
                //cf.CreateMap<Error, ErrorLogHistoryModel>(); //  Lỗi / vấn đề tồn đọng
                //cf.CreateMap<Task, TaskModuleGroupHistoryModel>(); // công việc cho theo nhóm module

                //cf.CreateMap<Department, DepartmentHistoryModel>(); //  Phòng ban
                //cf.CreateMap<JobPosition, JobPositionHistoryModel>(); // chức vụ
                //cf.CreateMap<SBU, SBUHistoryModel>(); // SBU
                //cf.CreateMap<Employee, EmployeeHistoryModel>(); // nhân viên
                //cf.CreateMap<EmployeeGroup, EmployeeGroupHistoryModel>(); // nhóm nhân viên
                //cf.CreateMap<Course, CourseHistoryModel>(); // nhóm nhân viên
                //cf.CreateMap<WorkSkill, WorkSkilHistoryModel>(); // kỹ năng của nhân viên
                //cf.CreateMap<WorkType, WorkTypeHistoryModel>(); // vị trí công việc
                //cf.CreateMap<ProductStandardTPAType, ProductStandardTPATypeHistoryModel>();

                //cf.CreateMap<SaleProduct, NTS.Model.Sale.SaleProduct.SaleProductModel>();
                //cf.CreateMap<Application, ApplicationModel>();
                //cf.CreateMap<NTS.Model.Repositories.SaleGroup, SaleGroupModel>();

                //cf.CreateMap<NTS.Model.Repositories.SaleProductType, SaleProductTypeHistoryModel>();
                //cf.CreateMap<NTS.Model.Repositories.SaleProductExport, SaleProductExportHistory>();
                //cf.CreateMap<NTS.Model.Repositories.RecruitmentChannel, RecruitmentChannelHistoryModel>();
                //cf.CreateMap<NTS.Model.Repositories.Candidate, CandidateHistoryModel>();
                //cf.CreateMap<NTS.Model.Repositories.CandidateApply, CandidateApplyHistoryModel>();
                //cf.CreateMap<NTS.Model.Repositories.SalaryLevel, SalaryLevelHistoryModel>();
                //cf.CreateMap<NTS.Model.Repositories.SalaryGroup, SalaryGroupHistoryModel>();
                //cf.CreateMap<NTS.Model.Repositories.SalaryType, SalaryTypeHistoryModel>();
                //cf.CreateMap<ProjectSolution, SolutionHistoryModel>();
            });
            
            Mapper = config.CreateMapper();
        }
    }
}
