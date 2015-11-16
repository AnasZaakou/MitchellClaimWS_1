using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

public enum StatusCode {
    
    /// <remarks/>
    OPEN,
    
    /// <remarks/>
    CLOSED,
}

public partial class LossInfoType {
    
    private CauseOfLossCode causeOfLossField;
    
    private bool causeOfLossFieldSpecified;
    
    private System.DateTime reportedDateField;
    
    private bool reportedDateFieldSpecified;
    
    private string lossDescriptionField;
    
    /// <remarks/>
    public CauseOfLossCode CauseOfLoss {
        get {
            return this.causeOfLossField;
        }
        set {
            this.causeOfLossField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool CauseOfLossSpecified {
        get {
            return this.causeOfLossFieldSpecified;
        }
        set {
            this.causeOfLossFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    public System.DateTime ReportedDate {
        get {
            return this.reportedDateField;
        }
        set {
            this.reportedDateField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool ReportedDateSpecified {
        get {
            return this.reportedDateFieldSpecified;
        }
        set {
            this.reportedDateFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    public string LossDescription {
        get {
            return this.lossDescriptionField;
        }
        set {
            this.lossDescriptionField = value;
        }
    }
}

public enum CauseOfLossCode {
    
    /// <remarks/>
    Collision,
    
    /// <remarks/>
    Explosion,
    
    /// <remarks/>
    Fire,
    
    /// <remarks/>
    Hail,
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("Mechanical Breakdown")]
    MechanicalBreakdown,
    
    /// <remarks/>
    Other,
}

public partial class VehicleInfoType {
    
    private int modelYearField;
    
    private string makeDescriptionField;
    
    private string modelDescriptionField;
    
    private string engineDescriptionField;
    
    private string exteriorColorField;
    
    private string vinField;
    
    private string licPlateField;
    
    private string licPlateStateField;
    
    private System.DateTime licPlateExpDateField;
    
    private bool licPlateExpDateFieldSpecified;
    
    private string damageDescriptionField;
    
    private int mileageField;
    
    private bool mileageFieldSpecified;
    
    /// <remarks/>
    public int ModelYear {
        get {
            return this.modelYearField;
        }
        set {
            this.modelYearField = value;
        }
    }
    
    /// <remarks/>
    public string MakeDescription {
        get {
            return this.makeDescriptionField;
        }
        set {
            this.makeDescriptionField = value;
        }
    }
    
    /// <remarks/>
    public string ModelDescription {
        get {
            return this.modelDescriptionField;
        }
        set {
            this.modelDescriptionField = value;
        }
    }
    
    /// <remarks/>
    public string EngineDescription {
        get {
            return this.engineDescriptionField;
        }
        set {
            this.engineDescriptionField = value;
        }
    }
    
    /// <remarks/>
    public string ExteriorColor {
        get {
            return this.exteriorColorField;
        }
        set {
            this.exteriorColorField = value;
        }
    }
    
    /// <remarks/>
    public string Vin {
        get {
            return this.vinField;
        }
        set {
            this.vinField = value;
        }
    }
    
    /// <remarks/>
    public string LicPlate {
        get {
            return this.licPlateField;
        }
        set {
            this.licPlateField = value;
        }
    }
    
    /// <remarks/>
    public string LicPlateState {
        get {
            return this.licPlateStateField;
        }
        set {
            this.licPlateStateField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType="date")]
    public System.DateTime LicPlateExpDate {
        get {
            return this.licPlateExpDateField;
        }
        set {
            this.licPlateExpDateField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool LicPlateExpDateSpecified {
        get {
            return this.licPlateExpDateFieldSpecified;
        }
        set {
            this.licPlateExpDateFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    public string DamageDescription {
        get {
            return this.damageDescriptionField;
        }
        set {
            this.damageDescriptionField = value;
        }
    }
    
    /// <remarks/>
    public int Mileage {
        get {
            return this.mileageField;
        }
        set {
            this.mileageField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool MileageSpecified {
        get {
            return this.mileageFieldSpecified;
        }
        set {
            this.mileageFieldSpecified = value;
        }
    }
}
