using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SupportBot.Helper
{
    public class Approvals
    {
        public async Task GetPending(IDialogContext context)
        {
            ServiceReference4.ServiceNowSoapClient soapClient = new ServiceReference4.ServiceNowSoapClient();
            soapClient.ClientCredentials.UserName.UserName = "admin";
            soapClient.ClientCredentials.UserName.Password = "Info@123";

            ServiceReference4.getRecords getRecords = new ServiceReference4.getRecords();
            ServiceReference4.getRecordsResponseGetRecordsResult[] response = { new ServiceReference4.getRecordsResponseGetRecordsResult() };

            getRecords.approver = "admin";

            response = soapClient.getRecords(getRecords);
            await context.PostAsync("You have " + response.Count() + " pending approval requests :");
            foreach (var approval in response)
            {
                await context.PostAsync("Approval for Admin. " + Environment.NewLine + "Link : " + "https://dev30426.service-now.com/nav_to.do?uri=%2Fsysapproval_approver.do%3Fsys_id%3D" + approval.sys_id);
            }
        }
    }
}