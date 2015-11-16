using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Schema;

namespace MitchellWSConsole
{
    public class ClaimService : IClaimService
    {
        static string strXMLClaimStore = "App_Data/ClaimStore.xml";
        static string strXSDPath = "App_Data/MitchellClaim.xsd";
        static string xmlNamespace = "http://www.mitchell.com/examples/claim";
        private bool isValidXML = true;

        public bool CreateClaim(string strMitchellClaimXML)
        {
            bool created = true;
            XDocument MitchellClaimXML;
            try
            {
                MitchellClaimXML = XDocument.Parse(strMitchellClaimXML);
                MitchellClaimXML.Save("App_Data/temp.xml");
            }
            catch
            {
                return false;
            }
            if (IsValidXml("App_Data/temp.xml", xmlNamespace, strXSDPath))
            {
                try
                {
                    MitchellClaimType claimResult = ReadClaimWithoutRoot(MitchellClaimXML);
                    return SaveNewClaim(claimResult); ;
                }
                catch (Exception ex)
                {
                    created = false;
                }
            }
            return created;
        }
        public MitchellClaimType GetClaimByClaimNumber(string claimNumberField)
        {
            //bool isvalid = IsValidXml(strXMLClaimStore, xmlNamespace, strXSDPath);
            try
            {
                XDocument claimXML = XDocument.Load(strXMLClaimStore);
                MitchellClaimType claimResult = ReadClaimWithRoot(claimXML).Where(claim => claim.ClaimNumber ==
                    claimNumberField).First();

                return claimResult;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<MitchellClaimType> GetClaimsBetweenDates(DateTime lossDate1, DateTime lossDate2)
        {
            try
            {
                XDocument claimXML = XDocument.Load(strXMLClaimStore);
                List<MitchellClaimType> claimResult = ReadClaimWithRoot(claimXML).Where(claim => claim.LossDate > lossDate1 &&
                                claim.LossDate < lossDate2).ToList<MitchellClaimType>();
                return claimResult;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private List<MitchellClaimType> ReadClaimWithRoot(XDocument claimDocument)
        {
            // find claim in xml file
            List<MitchellClaimType> claimResult = new List<MitchellClaimType>();
            try
            {
                XNamespace cla = "http://www.mitchell.com/examples/claim";
                claimResult = claimDocument.Element("Claims") //.Element("Claims")
                        .Descendants(cla + "MitchellClaim")
                            .Select(claim => new MitchellClaimType
                            {
                                ClaimNumber = claim.Element(cla+"ClaimNumber").Value,
                                ClaimantFirstName = claim.Element(cla + "ClaimantFirstName").Value,
                                ClaimantLastName = claim.Element(cla + "ClaimantLastName").Value,
                                Status = (StatusCode)Enum.Parse(typeof(StatusCode), claim.Element(cla + "Status").Value),
                                LossDate = Convert.ToDateTime(claim.Element(cla + "LossDate").Value),
                                LossInfo = (LossInfoType)claim.Descendants(cla + "LossInfo").Select(lossInfo => new LossInfoType
                                {
                                    CauseOfLoss = (CauseOfLossCode)Enum.Parse(typeof(CauseOfLossCode), lossInfo.Element(cla + "CauseOfLoss").Value),
                                    ReportedDate = Convert.ToDateTime(lossInfo.Element(cla + "ReportedDate").Value),
                                    LossDescription = lossInfo.Element(cla + "LossDescription").Value
                                }).First(),
                                AssignedAdjusterID = Convert.ToInt64(claim.Element(cla + "AssignedAdjusterID").Value)
                                ,
                                Vehicles = claim.Element(cla + "Vehicles").Descendants(cla + "VehicleDetails")
                                  .Select(vehicle => new VehicleInfoType
                                  {
                                      ModelYear = Convert.ToInt16(vehicle.Element(cla + "ModelYear").Value),
                                      MakeDescription = vehicle.Element(cla + "MakeDescription").Value,
                                      ModelDescription = vehicle.Element(cla + "ModelDescription").Value,
                                      EngineDescription = vehicle.Element(cla + "EngineDescription").Value,
                                      ExteriorColor = vehicle.Element(cla + "ExteriorColor").Value,
                                      Vin = vehicle.Element(cla + "Vin").Value,
                                      LicPlate = vehicle.Element(cla + "LicPlate").Value,
                                      LicPlateState = vehicle.Element(cla + "LicPlateState").Value,
                                      LicPlateExpDate = Convert.ToDateTime(vehicle.Element(cla + "LicPlateExpDate").Value),
                                      DamageDescription = vehicle.Element(cla + "DamageDescription").Value,
                                      Mileage = Convert.ToInt16(vehicle.Element(cla + "Mileage").Value)
                                  }).ToList<VehicleInfoType>()
                            }).ToList<MitchellClaimType>();
                return claimResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private MitchellClaimType ReadClaimWithoutRoot(XDocument claimDocument)
        {
            // find claim in xml file
            MitchellClaimType claimResult = new MitchellClaimType();
            try
            {
                XNamespace cla = "http://www.mitchell.com/examples/claim";
                claimResult = claimDocument//.Element("Claims")
                        .Descendants(cla + "MitchellClaim")
                            .Select(claim => new MitchellClaimType
                            {
                                ClaimNumber = claim.Element(cla + "ClaimNumber").Value,
                                ClaimantFirstName = claim.Element(cla + "ClaimantFirstName").Value,
                                ClaimantLastName = claim.Element(cla + "ClaimantLastName").Value,
                                Status = (StatusCode)Enum.Parse(typeof(StatusCode), claim.Element(cla + "Status").Value),
                                LossDate = Convert.ToDateTime(claim.Element(cla + "LossDate").Value),
                                LossInfo = (LossInfoType)claim.Descendants(cla + "LossInfo").Select(lossInfo => new LossInfoType
                                {
                                    CauseOfLoss = (CauseOfLossCode)Enum.Parse(typeof(CauseOfLossCode), lossInfo.Element(cla + "CauseOfLoss").Value),
                                    ReportedDate = Convert.ToDateTime(lossInfo.Element(cla + "ReportedDate").Value),
                                    LossDescription = lossInfo.Element(cla + "LossDescription").Value
                                }).First(),
                                AssignedAdjusterID = Convert.ToInt64(claim.Element(cla + "AssignedAdjusterID").Value)
                                ,
                                Vehicles = claim.Element(cla + "Vehicles").Descendants(cla + "VehicleDetails")
                                  .Select(vehicle => new VehicleInfoType
                                  {
                                      ModelYear = Convert.ToInt16(vehicle.Element(cla + "ModelYear").Value),
                                      MakeDescription = vehicle.Element(cla + "MakeDescription").Value,
                                      ModelDescription = vehicle.Element(cla + "ModelDescription").Value,
                                      EngineDescription = vehicle.Element(cla + "EngineDescription").Value,
                                      ExteriorColor = vehicle.Element(cla + "ExteriorColor").Value,
                                      Vin = vehicle.Element(cla + "Vin").Value,
                                      LicPlate = vehicle.Element(cla + "LicPlate").Value,
                                      LicPlateState = vehicle.Element(cla + "LicPlateState").Value,
                                      LicPlateExpDate = Convert.ToDateTime(vehicle.Element(cla + "LicPlateExpDate").Value),
                                      DamageDescription = vehicle.Element(cla + "DamageDescription").Value,
                                      Mileage = Convert.ToInt16(vehicle.Element(cla + "Mileage").Value)
                                  }).ToList<VehicleInfoType>()
                            }).First();
                return claimResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool SaveNewClaim(MitchellClaimType newClaim)
        {
            XNamespace cla = "http://www.mitchell.com/examples/claim";
            bool added = true;
            try
            {
                XDocument claimStore = XDocument.Load(strXMLClaimStore);

                XElement newClaimXml = new XElement(cla+"MitchellClaim",
                                               new XElement(cla + "ClaimNumber", newClaim.ClaimNumber),
                                               new XElement(cla + "ClaimantFirstName", newClaim.ClaimantFirstName),
                                               new XElement(cla + "ClaimantLastName", newClaim.ClaimantLastName),
                                               new XElement(cla + "Status", newClaim.Status),
                                               new XElement(cla + "LossDate", newClaim.LossDate),
                                               new XElement(cla + "LossInfo",
                                                   new XElement(cla + "CauseOfLoss", newClaim.LossInfo.CauseOfLoss),
                                                   new XElement(cla + "ReportedDate", newClaim.LossInfo.ReportedDate),
                                                   new XElement(cla + "LossDescription", newClaim.LossInfo.LossDescription)),
                                               new XElement(cla + "AssignedAdjusterID", newClaim.AssignedAdjusterID),
                                               new XElement(cla + "Vehicles",
                                                   new XElement(cla + "VehicleDetails",
                                                       new XElement(cla + "ModelYear", newClaim.Vehicles[0].ModelYear),
                                                       new XElement(cla + "MakeDescription", newClaim.Vehicles[0].MakeDescription),
                                                       new XElement(cla + "ModelDescription", newClaim.Vehicles[0].ModelDescription),
                                                       new XElement(cla + "EngineDescription", newClaim.Vehicles[0].EngineDescription),
                                                       new XElement(cla + "ExteriorColor", newClaim.Vehicles[0].ExteriorColor),
                                                       new XElement(cla + "Vin", newClaim.Vehicles[0].Vin),
                                                       new XElement(cla + "LicPlate", newClaim.Vehicles[0].LicPlate),
                                                       new XElement(cla + "LicPlateState", newClaim.Vehicles[0].LicPlateState),
                                                       new XElement(cla + "LicPlateExpDate", newClaim.Vehicles[0].LicPlateExpDate),
                                                       new XElement(cla + "DamageDescription", newClaim.Vehicles[0].DamageDescription),
                                                       new XElement(cla + "Mileage", newClaim.Vehicles[0].Mileage)))
                                           );

                claimStore.Element(cla + "Claims").Add(newClaimXml);
                claimStore.Save(strXMLClaimStore);
            }
            catch (Exception ex)
            {
                added = false;
            }
            return added;
        }
        private bool IsValidXml(string xmlFilePath, string namespaceName, string xsdFilePath)
        {
            isValidXML = true;
            // Parse the file.
            try
            {
                // Set the validation settings.
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.DtdProcessing = DtdProcessing.Parse;
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationFlags = XmlSchemaValidationFlags.ProcessInlineSchema |
                XmlSchemaValidationFlags.AllowXmlAttributes |
                    //XmlSchemaValidationFlags.ReportValidationWarnings |
                XmlSchemaValidationFlags.ProcessIdentityConstraints |
                XmlSchemaValidationFlags.ProcessSchemaLocation;
                settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
                settings.Schemas.Add(namespaceName, XmlReader.Create(xsdFilePath, settings));

                // Create the XmlReader object.
                XmlReader reader = XmlReader.Create(xmlFilePath, settings);

                while (reader.Read()) ;
                reader.Close();
            }
            catch(Exception ex)
            {
                isValidXML = false;
            }
            return isValidXML;     
        }
        private void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning || args.Severity == XmlSeverityType.Error)
                isValidXML = false;
        }  
    }
}
