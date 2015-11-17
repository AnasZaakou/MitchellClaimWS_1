using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Schema;
using System.Xml.Linq;
using MitchellClaimWSLib;

namespace MitchellWSTest
{
    /// <summary>
    /// Testing the MitchellClaimWSLib web service 
    /// </summary>
    [TestClass]
    public class ClaimServiceTest
    {
        string strCreateClaimXml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
                            <cla:MitchellClaim xmlns:cla=""http://www.mitchell.com/examples/claim"">
                              <cla:ClaimNumber>1</cla:ClaimNumber>
                              <cla:ClaimantFirstName>George</cla:ClaimantFirstName>
                              <cla:ClaimantLastName>Washington</cla:ClaimantLastName>
                              <cla:Status>OPEN</cla:Status>
                              <cla:LossDate>2014-07-09T17:19:13.631-07:00</cla:LossDate>
                              <cla:LossInfo>
                                <cla:CauseOfLoss>Collision</cla:CauseOfLoss>
                                <cla:ReportedDate>2014-07-10T17:19:13.676-07:00</cla:ReportedDate>
                                <cla:LossDescription>Crashed into an apple tree.</cla:LossDescription>
                              </cla:LossInfo>
                              <cla:AssignedAdjusterID>12345</cla:AssignedAdjusterID>
                              <cla:Vehicles>
                                <cla:VehicleDetails>
                                  <cla:ModelYear>2015</cla:ModelYear>
                                  <cla:MakeDescription>Ford</cla:MakeDescription>
                                  <cla:ModelDescription>Mustang</cla:ModelDescription>
                                  <cla:EngineDescription>EcoBoost</cla:EngineDescription>
                                  <cla:ExteriorColor>Deep Impact Blue</cla:ExteriorColor>
                                  <cla:Vin>1M8GDM9AXKP042788</cla:Vin>
                                  <cla:LicPlate>NO1PRES</cla:LicPlate>
                                  <cla:LicPlateState>VA</cla:LicPlateState>
                                  <cla:LicPlateExpDate>2015-03-10-07:00</cla:LicPlateExpDate>
                                  <cla:DamageDescription>Front end smashed in. Apple dents in roof.</cla:DamageDescription>
                                  <cla:Mileage>1776</cla:Mileage>
                                </cla:VehicleDetails>
                              </cla:Vehicles>
                            </cla:MitchellClaim>";

        string strCreateClaim_CorruptedXml = "<notvalid></notvalid>";

        // MitchellClaimWSLib object
        static readonly IClaimService claimService = new ClaimService();
        
        /// <summary>
        /// Testing to add a corrupted xml
        /// Expected result is false
        /// </summary>
        [TestMethod]
        public void testCreateClaimCorruptedXMLfile()
        {
            Assert.IsFalse(claimService.CreateClaim(strCreateClaim_CorruptedXml));
        }
        
        /// <summary>
        /// Testing to add a valid xml
        /// Expected result is true
        /// </summary>
        [TestMethod]
        public void testCreateClaimCorrectXMLfile()
        {
            Assert.IsTrue(claimService.CreateClaim(strCreateClaimXml));
        }
        
        /// <summary>
        /// Testing to get claim by claim number with input as empty string
        /// Expected result is null
        /// </summary>
        [TestMethod]
        public void testGetClaimByClaimNumberEmptyString()
        {
            Assert.IsNull(claimService.GetClaimByClaimNumber(string.Empty));
        }

        /// <summary>
        /// Testing to get a claim by claim number with input as null string
        /// Expected result is false
        /// </summary>
        [TestMethod]
        public void testGetClaimByClaimNumberNullString()
        {
            Assert.IsNull(claimService.GetClaimByClaimNumber(null));
        }
        
        /// <summary>
        /// Testing to get an existing claim by claim number  
        /// Expected result claim numbers are equal
        /// </summary>
        [TestMethod]
        public void testGetClaimByClaimNumberClaimExists()
        {
            claimService.CreateClaim(strCreateClaimXml);
            Assert.AreEqual("1", claimService.GetClaimByClaimNumber("1").ClaimNumber);
        }

        /// <summary>
        /// Testing to get claims that are not in the backing store
        /// Expected result is zero as the the number of elements of the returned list
        /// </summary>
        [TestMethod]
        public void testGetGetClaimsBetweenFutureDates()
        {
            Assert.AreEqual(0, claimService.GetClaimsBetweenDates(DateTime.Now.AddYears(100), DateTime.Now.AddYears(100)).Count);
        }

        /// <summary>
        /// Testing to get claims that are not in the backing store
        /// Expected result is zero as the the number of elements of the returned list
        /// </summary>
        [TestMethod]
        public void testGetGetClaimsBetweenNowFutureDates()
        {
            Assert.AreEqual(0, claimService.GetClaimsBetweenDates(DateTime.Now, DateTime.Now.AddYears(100)).Count);
        }

        /// <summary>
        /// Testing to get existing claims in the backing store that fall between the input two dates
        /// Expected result is not null to indicate a list that has at least one element
        /// </summary>
        [TestMethod]
        public void testGetGetClaimsBetweenDatesClaimExists()
        {
            claimService.CreateClaim(strCreateClaimXml);
            Assert.IsNotNull(claimService.GetClaimsBetweenDates(DateTime.Parse("2013-07-09T17:19:13.631-07:00")
                , DateTime.Parse("2015-07-09T17:19:13.631-07:00"))[0]);
        }
    }
}
