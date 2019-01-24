using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppstreamLib.RestCall;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var request = new
            {
                SRNo = "349"
            };

            try
            {

                var response = AppstreamLib.RestCall.RequestHandler.MakePostRequest<List<VendorSingle>>("MDEwMDAwMDBGOEQ0QjVGNzUwNkRDNDIzQThEMUVCNzQzM0M5NUFGODRGQ0UyMjJGOTc4NzkyRjk3MjE5NDgxNzM3RDhEMjcxNDZFRjZENkE1MThENDdDQTgyNzgxQ0I3NzMzOTNBMjY2QUEyNTE3MjAyNUE3MEI5OjM6QURNSU4%3d", "System/GetQualifyVendorList", request, "Authorization").Result;

                var test = response;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    public class VendorSingle
    {
        public string VendorID { get; set; }
        public string CmpyName { get; set; }
    }
}
