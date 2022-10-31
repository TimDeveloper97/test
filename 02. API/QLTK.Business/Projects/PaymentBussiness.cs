using NTS.Common;
using NTS.Model.Payment;
using NTS.Model.Plans;
using NTS.Model.Repositories;
using RabbitMQ.Client.Framing.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NTS.Model.Payment.PaymentModel;

namespace QLTK.Business.Projects
{
    public class PaymentBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        public TotalWithPaymentModel GetAllPayment(string projectId)
        {
            TotalWithPaymentModel model = new TotalWithPaymentModel();
            var listPayment = db.Payments.Where(x => x.ProjectId.Equals(projectId)).ToList();
            foreach (var item in listPayment)
            {
                UpdatePlanDate(new PlanPaymentModel { PaymentId = item.Id });
            }
            
            var payments = db.Payments.Where(x => x.ProjectId.Equals(projectId)).Select(t => new PaymentModel
            {
                Id = t.Id,
                ProjectId = t.ProjectId,
                Name = t.Name,
                PlanPaymentDate = t.PlanPaymentDate,
                PlanAmount = t.PlanAmount,
                TotalAmount = t.TotalAmount,
                PaymentCondition = t.PaymentCondition,
                ActualPaymentDate1 = t.ActualPaymentDate1,
                ActualPaymentDate2 = t.ActualPaymentDate2,
                ActualPaymentDate3 = t.ActualPaymentDate3,
                ActualPaymentDate4 = t.ActualPaymentDate4,
                ActualPaymentDate5 = t.ActualPaymentDate5,
                ActualAmount1 = t.ActualAmount1,
                ActualAmount2 = t.ActualAmount2,
                ActualAmount3 = t.ActualAmount3,
                ActualAmount4 = t.ActualAmount4,
                ActualAmount5 = t.ActualAmount5,
                PaymentMilestone = t.PaymentMilestone,
                MoneyCollectionTime = t.MoneyCollectionTime,


            }).OrderBy(r => r.PlanPaymentDate).ToList();


            var dataPlan = (from a in db.Plans.AsNoTracking()
                            join b in db.PaymentPlans.AsNoTracking() on a.Id equals b.PlanId
                            join c in db.Payments.AsNoTracking() on b.PaymentrId equals c.Id
                            select new { PlanId = b.PlanId, PaymentId = c.Id, Status = a.Status, Date = a.PlanDueDate, Weight = a.Weight, DoneRatio = a.DoneRatio }).ToList();

            
            foreach (var item in payments)
            {
                int weight = 0;
                int weightDone = 0;
                // Tổng số công việc
                var TotalPlan = dataPlan.Where(a => a.PaymentId.Equals(item.Id)).Select(a => a.PlanId).Distinct().Count();

                // Công việc chưa xong 
                var UnfinishedPlan = dataPlan.Where(a => a.PaymentId.Equals(item.Id) && a.Status == (int)Constants.ScheduleStatus.Open || a.Status == (int)Constants.ScheduleStatus.Ongoing).Select(a => a.PlanId).Distinct().Count();

                // Số công việc trễ
                var LatePlan = dataPlan.Where(a => a.PaymentId.Equals(item.Id) && a.Status == (int)Constants.ScheduleStatus.Open || a.Status == (int)Constants.ScheduleStatus.Ongoing && a.Date.Value.AddDays(1) < DateTime.Now).Select(a => a.PlanId).Distinct().Count();

                var PlanInPayment = dataPlan.Where(a => a.PaymentId.Equals(item.Id)).Select(a => new { Weight = a.Weight, DoneRatio = a.DoneRatio}).ToList();
                if (PlanInPayment.Count > 0)
                {
                    foreach (var data in PlanInPayment)
                    {
                        weight = weight + data.Weight;
                        weightDone = weightDone + (data.DoneRatio * data.Weight);
                    }
                    // % Hoàn thành trung bình
                    var AverageDoneRatio = weightDone / weight;
                    item.AverageDoneRatio = AverageDoneRatio;
                }
                else
                {
                    item.AverageDoneRatio = 0;
                }
                
                item.TotalPlan = TotalPlan;
                item.UnfinishedPlan = UnfinishedPlan;
                item.LatePlan = LatePlan;
                

                item.CollectionDate = item.PaymentMilestone.HasValue ? item.PaymentMilestone.Value.AddDays(item.MoneyCollectionTime) : item.PaymentMilestone;
            }

            model.PaymentModels = payments;

            model.TotalPlanAmount = payments.Sum(r => r.PlanAmount);
            model.ActualPlanAmount = payments.Sum(r => r.TotalAmount);
            return model;
        }

        public decimal GetSum(string projectId)
        {
            var paymentsActualAmount = db.Payments.Where(x => x.ProjectId.Equals(projectId));

            return paymentsActualAmount.Sum(r => r.TotalAmount);
        }

        public PaymentModel GetAllPaymentById(string paymentId)
        {
            var payment = db.Payments.FirstOrDefault(x => x.Id.Equals(paymentId));
            try
            {
                if (payment == null)
                {
                    return null;
                }
                else
                {
                    var paymentModel = new PaymentModel
                    {
                        Id = payment.Id,
                        ProjectId = payment.ProjectId,
                        Name = payment.Name,
                        PlanPaymentDate = payment.PlanPaymentDate,
                        PlanAmount = payment.PlanAmount,
                        PaymentCondition = payment.PaymentCondition,
                        ActualPaymentDate1 = payment.ActualPaymentDate1,
                        ActualPaymentDate2 = payment.ActualPaymentDate2,
                        ActualPaymentDate3 = payment.ActualPaymentDate3,
                        ActualPaymentDate4 = payment.ActualPaymentDate4,
                        ActualPaymentDate5 = payment.ActualPaymentDate5,
                        ActualAmount1 = payment.ActualAmount1,
                        ActualAmount2 = payment.ActualAmount2,
                        ActualAmount3 = payment.ActualAmount3,
                        ActualAmount4 = payment.ActualAmount4,
                        ActualAmount5 = payment.ActualAmount5,
                        PaymentMilestone = payment.PaymentMilestone,
                        MoneyCollectionTime = payment.MoneyCollectionTime,
                    };
                    return paymentModel;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void UpdatePayment(PaymentModel paymentModel)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    Payment payment = db.Payments.FirstOrDefault(x => x.Id.Equals(paymentModel.Id));
                    if (paymentModel.Id != string.Empty)
                    {

                        if (payment != null)
                        {
                            payment.Name = paymentModel.Name;
                            payment.PlanPaymentDate = paymentModel.PlanPaymentDate;
                            payment.PlanAmount = paymentModel.PlanAmount;
                            payment.PaymentCondition = paymentModel.PaymentCondition;
                            payment.ActualPaymentDate1 = paymentModel.ActualPaymentDate1;
                            payment.ActualPaymentDate2 = paymentModel.ActualPaymentDate2;
                            payment.ActualPaymentDate3 = paymentModel.ActualPaymentDate3;
                            payment.ActualPaymentDate4 = paymentModel.ActualPaymentDate4;
                            payment.ActualPaymentDate5 = paymentModel.ActualPaymentDate5;

                            payment.ActualAmount1 = paymentModel.ActualAmount1;
                            payment.ActualAmount2 = paymentModel.ActualAmount2;
                            payment.ActualAmount3 = paymentModel.ActualAmount3;
                            payment.ActualAmount4 = paymentModel.ActualAmount4;
                            payment.ActualAmount5 = paymentModel.ActualAmount5;
                            payment.TotalAmount = paymentModel.ActualAmount1 + paymentModel.ActualAmount2 + paymentModel.ActualAmount3 + paymentModel.ActualAmount4 + paymentModel.ActualAmount5;

                            payment.PaymentMilestone = paymentModel.PaymentMilestone;
                            payment.MoneyCollectionTime = paymentModel.MoneyCollectionTime;
                        }
                    }
                    else
                    {
                        payment = new Payment
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProjectId = paymentModel.ProjectId,
                            Name = paymentModel.Name,
                            PlanPaymentDate = paymentModel.PlanPaymentDate,
                            PlanAmount = paymentModel.PlanAmount,
                            PaymentCondition = paymentModel.PaymentCondition,
                            ActualPaymentDate1 = paymentModel.ActualPaymentDate1,
                            ActualPaymentDate2 = paymentModel.ActualPaymentDate2,
                            ActualPaymentDate3 = paymentModel.ActualPaymentDate3,
                            ActualPaymentDate4 = paymentModel.ActualPaymentDate4,
                            ActualPaymentDate5 = paymentModel.ActualPaymentDate5,
                            ActualAmount1 = paymentModel.ActualAmount1,
                            ActualAmount2 = paymentModel.ActualAmount2,
                            ActualAmount3 = paymentModel.ActualAmount3,
                            ActualAmount4 = paymentModel.ActualAmount4,
                            ActualAmount5 = paymentModel.ActualAmount5,
                            TotalAmount = paymentModel.ActualAmount1 + paymentModel.ActualAmount2 + paymentModel.ActualAmount3 + paymentModel.ActualAmount4 + paymentModel.ActualAmount5,
                            PaymentMilestone = paymentModel.PaymentMilestone,
                            MoneyCollectionTime = paymentModel.MoneyCollectionTime,
                        };

                        db.Payments.Add(payment);
                    }

                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(paymentModel, ex);
                }
            }
        }

        public void DeletePayment(string id)
        {
            var payment = db.Payments.FirstOrDefault(x => x.Id.Equals(id));
            try
            {
                if (payment != null)
                {
                    var listPlanPayment = db.PaymentPlans.Where(x => x.Id.Equals(payment.Id)).ToList();
                    if (listPlanPayment.Count > 0)
                    {
                        db.PaymentPlans.RemoveRange(listPlanPayment);
                    }
                    db.Payments.Remove(payment);
                }
            }
            catch (Exception)
            {
                throw;
            }
            db.SaveChanges();


        }

        public PlanPaymentModel GetPaymentByPlanId(string planId)
        {
            var PlanPayment = db.PaymentPlans.FirstOrDefault(x => x.PlanId.Equals(planId));
            if (PlanPayment != null)
            {
                var planPaymentModel = new PlanPaymentModel
                {
                    Id = PlanPayment.Id,
                    PlanId = PlanPayment.PlanId,
                    PaymentId = PlanPayment.PaymentrId,
                };
                return planPaymentModel;
            }
            return null;
        }

        public void UpdatePlanPayment(PlanPaymentModel model)
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                PaymentPlan paymentPlan = new PaymentPlan
                {
                    Id = Guid.NewGuid().ToString(),
                    PlanId = model.PlanId,
                    PaymentrId = model.PaymentId,
                };
                db.PaymentPlans.Add(paymentPlan);
            }
            else
            {
                var planPayment = db.PaymentPlans.FirstOrDefault(a => a.Id.Equals(model.Id));
                if (planPayment != null)
                {
                    planPayment.PaymentrId = model.PaymentId;
                    planPayment.PlanId = model.PlanId;
                }
            }
            db.SaveChanges();
        }

        public void UpdatePlanDate(PlanPaymentModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var listPlan = (from a in db.Plans.AsNoTracking()
                                    join b in db.PaymentPlans.AsNoTracking() on a.Id equals b.PlanId
                                    join c in db.Payments.AsNoTracking() on b.PaymentrId equals c.Id
                                    where c.Id == model.PaymentId
                                    select new PlanPaymentDate
                                    {
                                        Id = b.Id,
                                        PlanId = a.Id,
                                        PaymentId = c.Id,
                                        PlanDate = a.PlanDueDate,
                                    }
                            ).ToList();
                    var maxDate = listPlan.Select(a => a.PlanDate).Max();
                    var payment = db.Payments.FirstOrDefault(a => a.Id.Equals(model.PaymentId));
                    payment.PlanPaymentDate = maxDate;
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
        }

        public List<PlanPayment> GetPlanByPaymentId(string paymentId)
        {
            var Plans = (from a in db.Plans.AsNoTracking()
                         join b in db.PaymentPlans.AsNoTracking() on a.Id equals b.PlanId
                         join c in db.Payments.AsNoTracking() on b.PaymentrId equals c.Id
                         where c.Id == paymentId
                         select new PlanPayment
                         {
                             Id = a.Id,
                             Name = a.Name,
                             ContractStartDate = a.ContractStartDate,
                             ContractEndDate = a.ContractDueDate,
                             PlanStartDate = a.PlanStartDate,
                             PlanEndtDate = a.PlanDueDate,
                             Status = a.Status,
                         }).ToList();
            return Plans;
        }

        public void DeletePlanById(string planId)
        {
            var paymentPlan = db.PaymentPlans.FirstOrDefault(x => x.PlanId.Equals(planId));
            try
            {
                if (paymentPlan != null)
                {
                    db.PaymentPlans.Remove(paymentPlan);
                }
            }
            catch (Exception)
            {
                throw;
            }
            db.SaveChanges();
        }
    }
}
