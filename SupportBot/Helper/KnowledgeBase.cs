using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SupportBot.Helper
{
    public class KnowledgeBase
    {
        public async Task GetNews(IDialogContext context)
        {
            ServiceReference3.ServiceNowSoapClient soapClient = new ServiceReference3.ServiceNowSoapClient();
            soapClient.ClientCredentials.UserName.UserName = "admin";
            soapClient.ClientCredentials.UserName.Password = "Info@123";

            ServiceReference3.getRecords getRecords = new ServiceReference3.getRecords();
            ServiceReference3.getRecordsResponseGetRecordsResult[] response = { };

            getRecords.topic = "News";
            try
            {
                response = soapClient.getRecords(getRecords);
            }
            catch (Exception ex)
            {

            }

            foreach(var resp in response)
            {
                await context.PostAsync(resp.short_description);
            }
        }

        public string GetArticle(string id)
        {
            ServiceReference3.ServiceNowSoapClient soapClient = new ServiceReference3.ServiceNowSoapClient();
            soapClient.ClientCredentials.UserName.UserName = "admin";
            soapClient.ClientCredentials.UserName.Password = "Info@123";

            ServiceReference3.getRecords getRecords = new ServiceReference3.getRecords();
            ServiceReference3.getRecordsResponseGetRecordsResult[] response = { };

            getRecords.number = id;
            try
            {
                response = soapClient.getRecords(getRecords);
            }
            catch (Exception ex)
            {

            }
            return response[0].wiki;
        }
    }
}