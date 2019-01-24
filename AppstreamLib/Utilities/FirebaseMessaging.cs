using AppstreamLib.Interfaces;
using RestSharp;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;

namespace AppstreamLib.Utilities
{
    public sealed class FirebaseMessaging : IMessage
    {
        private readonly string fcmUri = "https://fcm.googleapis.com";
        private readonly string fcmSend = "fcm/send";
        private readonly string fcmLogo = "";
        private readonly string appUri = "";

        private RestClient client;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static FirebaseMessaging()
        {
            Instance = new FirebaseMessaging();
        }

        private FirebaseMessaging()
        {
            client = new RestClient(fcmUri);
        }

        public static FirebaseMessaging Instance
        {
            get;
            private set;
        }

        public bool Send(string to, string title, string body, object data = null)
        {
            string authKey = ConfigurationManager.AppSettings["fcm.authkey"];
            var request = new RestRequest(fcmSend, Method.POST);
            request.AddHeader("Authorization", authKey);
            request.AddHeader("Accept", "application/json");

            // Set property/method during run time hence 'dynamic'
            dynamic message = new ExpandoObject();
            message.to = to;
            message.data = data;

            if (title != string.Empty && body != string.Empty)
            {
                message.notification = new
                {
                    title,
                    body
                };
            }

            request.AddJsonBody(message);

            //IRestResponse response = client.Execute(request);
            //var content = response.Content; // raw content as string
            //var statusCode = response.StatusCode.ToString();

            IRestResponse<fcmResponse> response = client.Execute<fcmResponse>(request);
            var failure = response.Data.failure;

            return !(failure > 0);
        }
    }

    internal class fcmResponse
    {
        public decimal multicast_id { get; set; }
        public int success { get; set; }
        public int failure { get; set; }
        public int canonical_ids { get; set; }
        public List<message> results { get; set; }
    }

    internal class message
    {
        public string message_id { get; set; }
        public string error { get; set; }
    }
}
