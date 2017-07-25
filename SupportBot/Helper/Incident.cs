using SupportBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupportBot.Helper
{
    public class Incident
    {
        public string Create(string shortDescription)
        {
            ServiceReference1.ServiceNowSoapClient soapClient = new ServiceReference1.ServiceNowSoapClient();
            soapClient.ClientCredentials.UserName.UserName = "admin";
            soapClient.ClientCredentials.UserName.Password = "Info@123";

            ServiceReference1.insert insert = new ServiceReference1.insert();
            ServiceReference1.insertResponse response = new ServiceReference1.insertResponse();

            insert.category = "Inquiry / Help";
            insert.short_description = shortDescription;

            response = soapClient.insert(insert);
            return "Done! I created incident " + response.number + " for you. Link : https://dev30426.service-now.com/nav_to.do?uri=%2Fincident.do%3Fsys_id%3D" + response.sys_id;
        }
    }
}