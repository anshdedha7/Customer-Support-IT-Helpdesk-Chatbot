using SupportBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupportBot.Helper
{
    public class User
    {
        public string Create()
        {
            ServiceReference2.ServiceNowSoapClient soapClient = new ServiceReference2.ServiceNowSoapClient();
            soapClient.ClientCredentials.UserName.UserName = "admin";
            soapClient.ClientCredentials.UserName.Password = "Info@123";

            ServiceReference2.insert insert = new ServiceReference2.insert();
            ServiceReference2.insertResponse response = new ServiceReference2.insertResponse();

            insert.first_name = UserData.FirstName;
            insert.last_name = UserData.LastName;
            insert.user_name = UserData.FirstName + "." + UserData.LastName;
            insert.email = UserData.FirstName + "." + UserData.LastName + "@example.com";
            UserData.FirstName = null;
            UserData.LastName = null;
            try
            {
                response = soapClient.insert(insert);
            }
            catch(Exception ex)
            {

            }
            return "Done! Link to new user : " + Environment.NewLine + "https://dev30426.service-now.com/nav_to.do?uri=%2Fsys_user.do%3Fsys_id%3D" + response.sys_id;
        }
    }
}