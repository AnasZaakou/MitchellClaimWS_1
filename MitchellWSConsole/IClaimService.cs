using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace MitchellWSConsole
{
    public interface IClaimService
    {
        bool CreateClaim(string MitchellClaimXML);
        MitchellClaimType GetClaimByClaimNumber(string claimNumberField);
        List<MitchellClaimType> GetClaimsBetweenDates(DateTime lossDate1, DateTime lossDate2);
    }
}
