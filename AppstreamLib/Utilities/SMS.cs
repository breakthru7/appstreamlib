using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using RestSharp;

namespace AppstreamLib.Utilities
{
    public class SMS
    {
        public static void SendSMS( string phonenum, string message)
        {
            var client = new RestClient(ConfigurationManager.AppSettings["sms.client"]);

            var request = new RestRequest(ConfigurationManager.AppSettings["sms.endpoint"], Method.POST);
            request.AddQueryParameter("user", ConfigurationManager.AppSettings["sms.user"]);
            request.AddQueryParameter("pass", ConfigurationManager.AppSettings["sms.password"]);
            request.AddQueryParameter("type", "0"); //ascii
            request.AddQueryParameter("to", phonenum);  //receiver phone num 
            request.AddQueryParameter("from", ConfigurationManager.AppSettings["sms.sender"]);   //sender name
            request.AddQueryParameter("text", message);
            request.AddQueryParameter("servid", ConfigurationManager.AppSettings["sms.servid"]);   //service id

            var response = client.Execute(request);
            var content = response.Content; // raw content as string
            var statuscode = response.StatusCode;

            if (statuscode != System.Net.HttpStatusCode.OK || response.Content == "401")
            {
                throw new Exception(response.ErrorMessage);
            }

        }
    }
}
