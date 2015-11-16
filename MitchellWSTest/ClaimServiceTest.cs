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
        // MitchellClaimWSLib object
        static readonly IClaimService claimService = new ClaimService();
        
        /// <summary>
        /// Testing to add a corrupted xml
        /// Expected result is false
        /// </summary>
        [TestMethod]
        public void testCreateClaimCorruptedXMLfile()
        {
            Assert.IsFalse(claimService.CreateClaim(XDocument.Load("App_Data/CreateClaim_Corrupted.xml").ToString()));
        }
        
        /// <summary>
        /// Testing to add a valid xml
        /// Expected result is true
        /// </summary>
        [TestMethod]
        public void testCreateClaimCorrectXMLfile()
        {
            XDocument test = XDocument.Load("App_Data/CreateClaim.xml");
            Assert.IsTrue(claimService.CreateClaim(XDocument.Load("App_Data/CreateClaim.xml").ToString()));
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
            claimService.CreateClaim(XDocument.Load("App_Data/CreateClaim.xml").ToString());
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
            claimService.CreateClaim(XDocument.Load("App_Data/CreateClaim.xml").ToString());
            Assert.IsNotNull(claimService.GetClaimsBetweenDates(DateTime.Parse("2013-07-09T17:19:13.631-07:00")
                , DateTime.Parse("2015-07-09T17:19:13.631-07:00"))[0]);
        }
    }
}
