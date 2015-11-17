using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Collections.Concurrent;

namespace MitchellClaimWSLib
{
    /// <summary>
    /// ClaimService provides the implementation for the IClaimService to leverage the MitchellClaimType
    /// </summary>
    public class ClaimService : IClaimService
    {
        /// <summary>
        /// variables as a helper information according to the requirements
        /// File Paths for the claim store, xsd, xml_namespace
        /// </summary>
        static string strXMLClaimStore = "App_Data/ClaimStore.xml";
        static string strXSDPath = "App_Data/MitchellClaim.xsd";
        static string xmlNamespace = "http://www.mitchell.com/examples/claim";
        private bool isValidXML = true;

        /// <summary>
        /// CreateClaim to create a new claim by accepting xml input
        /// Returns: true/false to indicate if the claim is successfully added or not 
        /// </summary>
        public bool CreateClaim(string strMitchellClaimXML)
        {
            bool created = true;
            XDocument MitchellClaimXML;
            try
            {
                // Create XDocument file from the xml string
                MitchellClaimXML = XDocument.Parse(strMitchellClaimXML);
                // Save the file locally to be manipulated
                MitchellClaimXML.Save("App_Data/temp.xml");
            }
            catch(Exception ex)
            {
                return false;
            }
            // Validating the newly created xml file from the user's xml input
            if (IsValidXml("App_Data/temp.xml", xmlNamespace, strXSDPath))
            {
                try
                {
                    // Go through the xml file and do another round of validation against the xsd file
                    // then return a new claim of DataContract type MitchellClaimType
                    MitchellClaimType claimResult = ReadClaimWithoutRoot(MitchellClaimXML);
                    // Save the new MitchellClaimType object
                    return SaveNewClaim(claimResult); ;
                }
                catch
                {
                    created = false;
                }
            }
            return created;
        }

        /// <summary>
        /// GetClaimByClaimNumber to search for a claim in the backing store by claim number
        /// Returns: MitchellClaimType or null 
        /// </summary>
        public MitchellClaimType GetClaimByClaimNumber(string claimNumberField)
        {
            try
            {
                // Load the backing store
                XDocument claimXML = XDocument.Load(strXMLClaimStore);
                // read the claim from the backing store and search for the input claim number
                MitchellClaimType claimResult = ReadClaimWithRoot(claimXML).Where(claim => claim.ClaimNumber ==
                    claimNumberField).First();
                // Return the MitchellClaimType if exists
                return claimResult;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// GetClaimsBetweenDates to search for a claim in the backing store between two dates
        /// Returns: List of MitchellClaimType 
        /// </summary>
        public List<MitchellClaimType> GetClaimsBetweenDates(DateTime lossDate1, DateTime lossDate2)
        {
            try
            {
                // Load the backing store of claims
                XDocument claimXML = XDocument.Load(strXMLClaimStore);
                // Search for the claims falling between the provided two dates
                List<MitchellClaimType> claimResult = ReadClaimWithRoot(claimXML).Where(claim => claim.LossDate > lossDate1 &&
                                claim.LossDate < lossDate2).ToList<MitchellClaimType>();
                // Returns the list or null if no items exists
                return claimResult;
            }
            catch
            {
                return null;
            }
        }

        // <summary>
        /// ReadClaimWithRoot to read/parse the backing store of claims
        /// Returns: Complete List of MitchellClaimType 
        /// </summary>
        private List<MitchellClaimType> ReadClaimWithRoot(XDocument claimDocument)
        {
            List<MitchellClaimType> claimResult = new List<MitchellClaimType>();
            try
            {
                // Set the deafult namespace
                XNamespace cla = "http://www.mitchell.com/examples/claim";
                // Read the claims from the backing store file according to the MitchellClaimType data model
                claimResult = claimDocument.Element(cla+"Claims") //.Element("Claims")
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
                            }).ToList<MitchellClaimType>();
                return claimResult;
            }
            catch
            {
                claimResult = null;
                return null;
            }
        }
        
        // <summary>
        /// ReadClaimWithoutRoot to read/parse the new claim provided by the user
        /// Returns: MitchellClaimType 
        /// </summary>
        private MitchellClaimType ReadClaimWithoutRoot(XDocument claimDocument)
        {
            // find claim in xml file
            MitchellClaimType claimResult = new MitchellClaimType();
            try
            {
                // Set namespace
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
            catch
            {
                claimResult = null;
                return null;
            }
        }
        
        // <summary>
        /// SaveNewClaim to save the claim input in the backing store as xml
        /// Returns: true/false to indicate whether the claim added or not 
        /// </summary>
        private bool SaveNewClaim(MitchellClaimType newClaim)
        {
            // Set xml namespace
            XNamespace cla = "http://www.mitchell.com/examples/claim";
            bool added = true;
            try
            {
                // Load the backing store
                XDocument claimStore = XDocument.Load(strXMLClaimStore);
                // Create a new MitchellClaim xml element
                XElement newClaimXml = new XElement(cla + "MitchellClaim",
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
                // Add the new MitchellClaim element to the backing store
                claimStore.Element(cla + "Claims").Add(newClaimXml);
                // Save the backing store
                claimStore.Save(strXMLClaimStore);
            }
            catch
            {
                added = false;
            }
            return added;
        }
        
        // <summary>
        /// IsValidXml to indicate whether the input xml is valid according to the xsd
        /// Returns: true/false to indicate whether the claim xml is valid
        /// </summary>
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
            catch
            {
                isValidXML = false;
            }
            return isValidXML;
        }
        // Validation callback function to handle errors or warnings
        private void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning || args.Severity == XmlSeverityType.Error)
                isValidXML = false;
        }  
        
    }
}
