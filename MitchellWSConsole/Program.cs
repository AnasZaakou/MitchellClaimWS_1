using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MitchellWSConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ClaimService service = new ClaimService();
            //Console.WriteLine(service.ReadClaim("22c9c23bac142856018ce14a26b6c299").ClaimantFirstName);
            XDocument newClaim = XDocument.Load("App_Data/CreateClaim.xml");
            bool result = service.CreateClaim(newClaim.ToString());
            //MitchellClaimType result = service.GetClaimByClaimNumber("22c9c23bac142856018ce14a26b6c299");
            //service.GetClaimList(Convert.ToDateTime("2013-07-09T17:19:13.631-07:00"), Convert.ToDateTime("2015-07-09T17:19:13.631-07:00"));
        }
    }
}
