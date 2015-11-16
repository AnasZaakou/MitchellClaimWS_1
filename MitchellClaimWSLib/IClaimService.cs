using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml.Linq;

namespace MitchellClaimWSLib
{
    [ServiceContract]
    public interface IClaimService
    {
        // <summary>
        /// CreateClaim to create a new claim by accepting xml input
        /// Returns: true/false to indicate if the claim is successfully added or not 
        /// </summary>
        [OperationContract]
        bool CreateClaim(string MitchellClaimXML);
        
        /// <summary>
        /// GetClaimByClaimNumber to search for a claim in the backing store by claim number
        /// Returns: MitchellClaimType or null 
        /// </summary>
        /// 
        [OperationContract]
        MitchellClaimType GetClaimByClaimNumber(string claimNumberField);
        
        /// <summary>
        /// GetClaimsBetweenDates to search for a claim in the backing store between two dates
        /// Returns: List of MitchellClaimType 
        /// </summary>
        [OperationContract]
        List<MitchellClaimType> GetClaimsBetweenDates(DateTime lossDate1, DateTime lossDate2);
    }

    [DataContract]
    public partial class MitchellClaimType
    {
        [DataMember]
        private string claimNumberField;
        [DataMember]
        private string claimantFirstNameField;
        [DataMember]
        private string claimantLastNameField;
        [DataMember]
        private StatusCode statusField;
        [DataMember]
        private bool statusFieldSpecified;
        [DataMember]
        private System.DateTime lossDateField;
        [DataMember]
        private bool lossDateFieldSpecified;
        [DataMember]
        private LossInfoType lossInfoField;
        [DataMember]
        private long assignedAdjusterIDField;
        [DataMember]
        private bool assignedAdjusterIDFieldSpecified;
        [DataMember]
        private List<VehicleInfoType> vehiclesField;

        /// <remarks/>
        public string ClaimNumber
        {
            get
            {
                return this.claimNumberField;
            }
            set
            {
                this.claimNumberField = value;
            }
        }

        /// <remarks/>
        public string ClaimantFirstName
        {
            get
            {
                return this.claimantFirstNameField;
            }
            set
            {
                this.claimantFirstNameField = value;
            }
        }

        /// <remarks/>
        public string ClaimantLastName
        {
            get
            {
                return this.claimantLastNameField;
            }
            set
            {
                this.claimantLastNameField = value;
            }
        }

        /// <remarks/>
        public StatusCode Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StatusSpecified
        {
            get
            {
                return this.statusFieldSpecified;
            }
            set
            {
                this.statusFieldSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime LossDate
        {
            get
            {
                return this.lossDateField;
            }
            set
            {
                this.lossDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LossDateSpecified
        {
            get
            {
                return this.lossDateFieldSpecified;
            }
            set
            {
                this.lossDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public LossInfoType LossInfo
        {
            get
            {
                return this.lossInfoField;
            }
            set
            {
                this.lossInfoField = value;
            }
        }

        /// <remarks/>
        public long AssignedAdjusterID
        {
            get
            {
                return this.assignedAdjusterIDField;
            }
            set
            {
                this.assignedAdjusterIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AssignedAdjusterIDSpecified
        {
            get
            {
                return this.assignedAdjusterIDFieldSpecified;
            }
            set
            {
                this.assignedAdjusterIDFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("VehicleDetails", IsNullable = false)]
        public List<VehicleInfoType> Vehicles
        {
            get
            {
                return this.vehiclesField;
            }
            set
            {
                this.vehiclesField = value;
            }
        }
    }
}
