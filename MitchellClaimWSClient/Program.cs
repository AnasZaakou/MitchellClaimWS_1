using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MitchellClaimWSClient
{
    class Program
    {
        /// <summary>
        /// Testing the referene of the web service
        /// </summary>
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Welcome to Mitchell Claim WS's Client: ");
                // Creating the reference
                MitchellClaimServiceLibReference.ClaimServiceClient client = new MitchellClaimServiceLibReference.ClaimServiceClient();
                Console.WriteLine("Adding new claim with claimNumber = 1");
                // Creating a new claim
                XDocument newClaim = XDocument.Load("App_Data/CreateClaim.xml");
                bool result = client.CreateClaim(newClaim.ToString());
                Console.WriteLine("Claim added successfully");
            }
            catch
            {

            }
        }
    }
}
