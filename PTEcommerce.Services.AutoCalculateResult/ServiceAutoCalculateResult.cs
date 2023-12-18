using Framework.EF;
using marketplace;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using PTEcommerce.Business;
using System.ServiceProcess;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using Framework.Helper.Extensions;
using RestSharp;

namespace PTEcommerce.Services.AutoCalculateResult
{
    public partial class ServiceAutoCalculateResult : ServiceBase
    {
        private readonly Timer TimeReload;
        private readonly IAccountCustomer accountCustomer;
        private readonly IHistoryTransfer historyTransfer;
        private readonly IOrderPakages orderPakages;
        private readonly IPakages pakages;
        public ServiceAutoCalculateResult()
        {
            InitializeComponent();
            accountCustomer = SingletonIpl.GetInstance<IplAccountCustomer>();
            historyTransfer = SingletonIpl.GetInstance<IplHistoryTransfer>();
            orderPakages = SingletonIpl.GetInstance<IplOrderPakages>();
            pakages = SingletonIpl.GetInstance<IplPakages>();
            long timeLoop = long.Parse(ConfigurationManager.AppSettings["timeLoop"]);
            //Set khoảng thời gian chạy lại
            TimeReload = new Timer(timeLoop);
            TimeReload.Elapsed += new ElapsedEventHandler(WorkProcess);
        }
        public void WorkProcess(object o, ElapsedEventArgs e)
        {
            try
            {
                TimeReload.Stop();
                var list = orderPakages.GetListUnProcess();
                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        LogService("Proccess order pakage: #" + item.Id);
                        item.Status = 2;
                        item.FinalPrice = item.Prices + (item.Prices * item.RateNow / 100);
                        var result = orderPakages.Update(item);
                        if (result)
                        {
                            var pakageData = pakages.GetById(item.PakageId);
                            var dataAccount = accountCustomer.GetById(item.IdAccount);
                            if (dataAccount != null)
                            {
                                var dataBefore = dataAccount.AmountAvaiable;
                                dataAccount.AmountAvaiable += item.FinalPrice;
                                accountCustomer.Update(dataAccount);
                                historyTransfer.Insert(new HistoryTransfer
                                {
                                    IdAccount = dataAccount.Id,
                                    CreatedDate = DateTime.Now,
                                    AmountBefore = dataBefore,
                                    AmountModified = item.FinalPrice,
                                    AmountAfter = dataBefore + item.FinalPrice,
                                    Note = "Hoàn trả lợi nhuận gói đầu tư " + pakageData.Name,
                                    Type = 5
                                });
                                UpdateReferalByIdAccount(dataAccount.FullName, dataAccount.Id, item.Prices * item.RateNow / 100);
                            }
                        }
                        LogService("End process order pakage: #" + item.Id);
                    }
                }
                TimeReload.Start();
            }
            catch (Exception ex)
            {
                LogService("Error: " + JsonConvert.SerializeObject(ex));
            }
        }
        private void UpdateReferalByIdAccount(string accountName, int idAccount, decimal prices)
        {
            var dataAccountList = orderPakages.GetListAccountReferal(idAccount);
            if (dataAccountList != null && dataAccountList.Count > 0)
            {
                foreach (var item in dataAccountList)
                {
                    if (item.Id != idAccount)
                    {
                        var accountData = accountCustomer.GetById(item.Id);
                        if (accountData != null)
                        {
                            var rate = ConvertRef(item.Level);
                            var dataBefore = accountData.AmountAvaiable;
                            accountData.AmountAvaiable += prices * rate;
                            accountData.AmountRef += prices * rate;
                            LogService("User " + accountData.Email + " receive ref " + prices * rate);
                            historyTransfer.Insert(new HistoryTransfer
                            {
                                IdAccount = accountData.Id,
                                CreatedDate = DateTime.Now,
                                AmountBefore = dataBefore,
                                AmountModified = prices * rate,
                                AmountAfter = accountData.AmountAvaiable,
                                Note = "Nhận hoa hồng " + rate + "% khi đối tác " + accountName + "(Mã #" + idAccount + ") nhận lợi nhuận đầu tư",
                                Type = 6
                            });
                            accountCustomer.Update(accountData);
                        }
                    }
                }
            }
        }
        private decimal ConvertRef(int level)
        {
            decimal result = 0;
            switch (level)
            {
                case 1:
                    result = (decimal)0.12;
                    break;
                case 2:
                    result = (decimal)0.11;
                    break;
                case 3:
                    result = (decimal)0.1;
                    break;
                case 4:
                    result = (decimal)0.09;
                    break;
                case 5:
                    result = (decimal)0.08;
                    break;
                case 6:
                    result = (decimal)0.07;
                    break;
                case 7:
                    result = (decimal)0.06;
                    break;
                case 8:
                    result = (decimal)0.05;
                    break;
                case 9:
                    result = (decimal)0.04;
                    break;
                default:
                    result = (decimal)0.03;
                    break;
            }
            return result;
        }
        protected override void OnStart(string[] args)
        {
            LogService("=========== Service Recharge Zing started on: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " ===========");
            TimeReload.Enabled = true;
        }

        protected override void OnStop()
        {
            LogService("=========== Service Recharge Zing stopped on: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " ===========");
            TimeReload.Enabled = false;
        }
        public void LogService(string content)
        {
            string url = ConfigurationManager.AppSettings["LogServiceUrl"];
            string fileName = @"\RechargeGarena_" + DateTime.Now.ToString("dd_MM_yyyy") + "_Log.txt";
            FileStream fs = new FileStream(url + fileName, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0, SeekOrigin.End);
            sw.WriteLine(content);
            sw.Flush();
            sw.Close();
        }
    }
}
