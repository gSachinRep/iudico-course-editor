using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace FireFly.CourseEditor.Course.Manifest
{
    using Common;

    public struct ManifestNamespaces
    {
        public const string Imscp = "http://www.imsglobal.org/xsd/imscp_v1p1";
        public const string Imsss = "http://www.imsglobal.org/xsd/imsss";
        public const string Imsmd = "http://www.imsglobal.org/xsd/imsmd_v1p2";
        public const string Adlcp = "http://www.adlnet.org/xsd/adlcp_v1p3";
        public const string Adlseq = "http://www.adlnet.org/xsd/adlseq_v1p3";
        public const string Adlnav = "http://www.adlnet.org/xsd/adlnav_v1p3";

        public static readonly XmlSerializerNamespaces SerializerNamespaces = new XmlSerializerNamespaces(new[] {                    
                new XmlQualifiedName("imsss", Imsss), 
                new XmlQualifiedName("imsmd", Imsmd), 
                new XmlQualifiedName("adlcp", Adlcp),
                new XmlQualifiedName("adlseq", Adlseq), 
                new XmlQualifiedName("adlnav", Adlnav)});
    }    

    #region Sequencing

    [Description("Sequencing information is associated with items in a hierarchical structure by associating a single <sequencing> element with the hierarchical item. Encapsulates all of the necessary sequencing information for a given activity.")]
    [Category("Main")]
    [XmlType("sequencing", Namespace = ManifestNamespaces.Imsss)]
    public class SequencingType : AbstractManifestNode, IContainer
    {
        private ControlModeType controlModeField;

        private SequencingRulesType sequencingRulesField;

        private LimitConditionsType limitConditionsField;

        private ManifestNodeList<AuxiliaryResourceType> auxiliaryResourcesField;

        private RollupRulesType rollupRulesField;

        private ObjectivesType objectivesField;

        private RandomizationType randomizationControlsField;

        private DeliveryControlsType deliveryControlsField;

        private XmlElement[] anyField;

        private string idField;

        private string iDRefField;

        private ConstrainedChoiceConsiderationsType constrainedChoiceConsiderationsField;

        private RollupConsiderationsType rollupConsiderationsField;

        [Description("Container for the sequencing control mode information including descriptions of the types of sequencing behaviors specified for an activity . This element captures information dealing with the types of sequencing requests that are permitted.")]
        [Category("Main")]
        public ControlModeType controlMode
        {
            get
            {
                return controlModeField;
            }
            set
            {
                controlModeField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [Description("Container for a sequencing rule description. Each rule describes the sequencing behavior for an activity.")]
        [Category("Main")]
        public SequencingRulesType sequencingRules
        {
            get
            {
                return sequencingRulesField;
            }
            set
            {
                sequencingRulesField = value;
                Course.NotifyManifestChanged(this, new IManifestNode[1] { value }, ManifestChangeTypes.ChildrenAdded);
            }
        }

        [Description("The limit condition deals with attempts on the activity and maximum time allowed in the attempt.")]
        [Category("Main")]
        public LimitConditionsType limitConditions
        {
            get
            {
                return limitConditionsField;
            }
            set
            {
                limitConditionsField = value;
                Course.NotifyManifestChanged(this, new IManifestNode[1] { value }, ManifestChangeTypes.ChildrenAdded);
            }
        }

        [Description("At this time, ADL recommends to use the auxiliaryResources element with caution.")]
        [XmlArrayItem("auxiliaryResource")]
        public ManifestNodeList<AuxiliaryResourceType> auxiliaryResources
        {
            get
            {
                return auxiliaryResourcesField;
            }
            set
            {
                auxiliaryResourcesField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [Description("Container for the set of rollup rules defined for the activity.")]
        [Category("Main")]
        public RollupRulesType rollupRules
        {
            get
            {
                return rollupRulesField;
            }
            set
            {
                rollupRulesField = value;
                Course.NotifyManifestChanged(this, new IManifestNode[1] { value }, ManifestChangeTypes.ChildrenAdded);
            }
        }

        [Description("Container for the set of objectives that are to be associated with an activity.")]
        [Category("Main")]
        public ObjectivesType objectives
        {
            get
            {
                return objectivesField;
            }
            set
            {
                objectivesField = value;
                Course.NotifyManifestChanged(this, new IManifestNode[1] { value }, ManifestChangeTypes.ChildrenAdded);
            }
        }

        [Description("Element is the container for the descriptions of how children of an activity should be ordered during the sequence process.")]
        [Category("Main")]
        public RandomizationType randomizationControls
        {
            get
            {
                return randomizationControlsField;
            }
            set
            {
                randomizationControlsField = value;
                Course.NotifyManifestChanged(this, new IManifestNode[1] { value }, ManifestChangeTypes.ChildrenAdded);
            }
        }

        [Description("Ñontainer for the descriptions of how children of an activity should be ordered during the sequence process.")]
        [Category("Main")]
        public DeliveryControlsType deliveryControls
        {
            get
            {
                return deliveryControlsField;
            }
            set
            {
                deliveryControlsField = value;
                Course.NotifyManifestChanged(this, new IManifestNode[1] { value }, ManifestChangeTypes.ChildrenAdded);
            }
        }

        [XmlAnyElementAttribute]
        public XmlElement[] Any
        {
            get
            {
                return anyField;
            }
            set
            {
                anyField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [Description("The unique identifier assigned to this set of sequencing information.")]
        [XmlAttribute(DataType = "ID")]
        public string ID
        {
            get
            {
                return idField;
            }
            set
            {
                idField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [Description("A reference to a unique identifier (i.e., ID attribute of a <sequencing> element) assigned to a set of sequencing information.")]
        [XmlAttribute(DataType = "IDREF")]
        public string IDRef
        {
            get
            {
                return iDRefField;
            }
            set
            {
                iDRefField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [Description("Container for the descriptions of how choice navigation requests should be constrained during the sequencing process.")]
        [Category("Main")]
        public ConstrainedChoiceConsiderationsType constrainedChoiceConsiderations
        {
            get
            {
                return this.constrainedChoiceConsiderationsField;
            }
            set
            {
                this.constrainedChoiceConsiderationsField = value;
                Course.NotifyManifestChanged(this, new IManifestNode[1] { value }, ManifestChangeTypes.ChildrenAdded);
            }
        }

        [Description("Container for the descriptions of when an activity should be included in the rollup process.")]
        [Category("Main")]
        public RollupConsiderationsType rollupConsiderations
        {
            get
            {
                return this.rollupConsiderationsField;
            }
            set
            {
                this.rollupConsiderationsField = value;
                Course.NotifyManifestChanged(this, new IManifestNode[1] { value }, ManifestChangeTypes.ChildrenAdded);
            }
        }

        #region IContainer Members

        public void RemoveChild(IManifestNode child)
        {
            switch (child.GetType().Name)
            {
                case "AuxiliaryResourceType":
                    {
                        var item = child as AuxiliaryResourceType;
                        if (auxiliaryResources.Contains(item))
                        {
                            auxiliaryResources.Remove(item);
                            return;
                        }
                        break;
                    }
                case "ControlModeType":
                    {
                        if (controlMode != null)
                        {
                            controlMode = null;
                            return;
                        }
                        break;
                    }
                case "SequencingRulesType":
                    {
                        if (sequencingRules != null)
                        {
                            sequencingRules = null;
                            return;
                        }
                        break;
                    }
                case "LimitConditionsType":
                    {
                        if (limitConditions != null)
                        {
                            limitConditions = null;
                            return;
                        }
                        break;
                    }
                case "RollupRulesType":
                    {
                        if (rollupRules != null)
                        {
                            rollupRules = null;
                            return;
                        }
                        break;
                    }
                case "ObjectivesType":
                    {
                        if (objectives != null)
                        {
                            objectives = null;
                            return;
                        }
                        break;
                    }
                case "RandomizationType":
                    {
                        if (randomizationControls != null)
                        {
                            randomizationControls = null;
                            return;
                        }
                        break;
                    }
                case "DeliveryControlsType":
                    {
                        if (deliveryControls != null)
                        {
                            deliveryControls = null;
                            return;
                        }
                        break;
                    }
            }
            throw new FireFlyException("Item '{0}' not found", child);
        }

        #endregion
    }

    [XmlType(Namespace = ManifestNamespaces.Imsss)]
    [Description("Describe actions the LMS will take prior to an attempt on an activity beginning and after the attempt ends.")]
    [Category("Main")]
    public class DeliveryControlsType : AbstractManifestNode, IManifestNode
    {
        private bool trackedField;

        private bool completionSetByContentField;

        private bool objectiveSetByContentField;

        public DeliveryControlsType()
        {
            trackedField = true;
            completionSetByContentField = false;
            objectiveSetByContentField = false;
        }

        [XmlAttribute]
        [Description("This attribute indicates that the objective progress information and activity/attempt progress information for the attempt should be recorded (true or false) and the data will contribute to the rollup for its parent activity.")]
        [DefaultValue(true)]
        public bool tracked
        {
            get
            {
                return trackedField;
            }
            set
            {
                trackedField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlAttribute]
        [Description("This attribute indicates that the attempt completion status for the activity will be set by the SCO (true or false).")]
        [DefaultValue(false)]
        public bool completionSetByContent
        {
            get
            {
                return completionSetByContentField;
            }
            set
            {
                completionSetByContentField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlAttribute]
        [Description("This attribute indicates that the objective satisfied status for the activity’s associated objective that contributes to rollup will be set by the SCO.")]
        [DefaultValue(false)]
        public bool objectiveSetByContent
        {
            get
            {
                return objectiveSetByContentField;
            }
            set
            {
                objectiveSetByContentField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType(Namespace = ManifestNamespaces.Imsss)]
    [Description("Element is the container for the descriptions of how children of an activity should be ordered during the sequence process.")]
    [Category("Main")]
    public class RandomizationType : AbstractManifestNode, IManifestNode
    {
        private RandomTimingType randomizationTimingField;

        private string selectCountField;

        private bool reorderChildrenField;

        private RandomTimingType selectionTimingField;

        public RandomizationType()
        {
            randomizationTimingField = RandomTimingType.never;
            reorderChildrenField = false;
            selectionTimingField = RandomTimingType.never;
        }

        [XmlAttribute]
        [Description("This attribute indicates when the ordering of the children of the activity should occur.")]
        [DefaultValue(RandomTimingType.never)]
        public RandomTimingType randomizationTiming
        {
            get
            {
                return randomizationTimingField;
            }
            set
            {
                randomizationTimingField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlAttribute(DataType = "nonNegativeInteger")]
        [Description("This attribute indicates the number of child activities that must be selected from the set of child activities associated with the activity.")]
        public string selectCount
        {
            get
            {
                return selectCountField;
            }
            set
            {
                selectCountField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlAttribute]
        [Description("This attribute indicates that the order of the child activities is randomized.")]
        [DefaultValue(false)]
        public bool reorderChildren
        {
            get
            {
                return reorderChildrenField;
            }
            set
            {
                reorderChildrenField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlAttribute]
        [Description("This attribute indicates when the selection should occur.")]
        [DefaultValue(RandomTimingType.never)]
        public RandomTimingType selectionTiming
        {
            get
            {
                return selectionTimingField;
            }
            set
            {
                selectionTimingField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType(Namespace = ManifestNamespaces.Imsss)]
    [Description("This attribute indicates when the ordering of the children of the activity should occur.")]
    public enum RandomTimingType
    {
        never,
        once,
        onEachNewAttempt,
    }

    [XmlType(Namespace = ManifestNamespaces.Imsss)]
    [Description("Container for the objective map description. This defines the mapping of an activity’s local objective information to and from a shared global objective.")]
    [Category("Main")]
    public class ObjectiveTypeMapInfo : AbstractManifestNode, IManifestNode
    {
        public ObjectiveTypeMapInfo()
        {
            readSatisfiedStatus = true;
            readNormalizedMeasure = true;
            writeSatisfiedStatus = false;
            writeNormalizedMeasure = false;
        }

        [XmlAttribute(DataType = "anyURI")]
        [Description("The identifier of the global shared objective targeted for the mapping.")]
        public string targetObjectiveID { get; set; }

        [XmlAttribute]
        [Description("This attribute indicates that the satisfaction status for the identified local objective should be retrieved (true or false) from the identified shared global objective when the progress for the global objective is defined.")]
        [DefaultValue(true)]
        public bool readSatisfiedStatus { get; set; }

        [XmlAttribute]
        [Description("This attribute indicates that the normalized measure for the identified local objective should be retrieved (true or false) from the identified shared global objective when the measure for the global objective is defined.")]
        [DefaultValue(true)]
        public bool readNormalizedMeasure { get; set; }

        [XmlAttribute]
        [Description("This attribute indicates that the satisfaction status for the identified local objective should be transferred (true or false) to the identified shared global objective upon termination ( Termination(“”) ) of the attempt on the activity.")]
        [DefaultValue(false)]
        public bool writeSatisfiedStatus { get; set; }

        [XmlAttribute]
        [Description("This attribute indicates that the normalized measure for the identified local objective should be transferred (true or false) to the identified shared global objective upon termination ( Termination(“”) ) of the attempt on the activity.")]
        [DefaultValue(false)]
        public bool writeNormalizedMeasure { get; set; }

        public override string ToString()
        {
            if (this.targetObjectiveID == null || this.targetObjectiveID == "")
            {
                return "mapInfo";
            }
            return this.targetObjectiveID;
        }
    }

    [XmlInclude(typeof(ObjectivesTypeObjective))]
    [XmlInclude(typeof(ObjectivesTypePrimaryObjective))]
    [XmlType(Namespace = ManifestNamespaces.Imsss)]
    [Description("Identifies objectives that do not contribute to rollup associated with the activity. This element can only exist if a <primaryObjective> has been defined.")]
    public class ObjectiveType : AbstractManifestNode, IManifestNode
    {
        private decimal minNormalizedMeasureField;

        private List<ObjectiveTypeMapInfo> mapInfoField = new List<ObjectiveTypeMapInfo>();

        public ObjectiveType()
        {
            minNormalizedMeasureField = 1.00000m;
            satisfiedByMeasure = false;
        }

        [XmlElement(Namespace=ManifestNamespaces.Imsss)]
        [Description("Identifies minimum satisfaction measure for the objective. The value is normalized between –1 and 1 (inclusive).")]
        [DefaultValue(typeof(decimal), "1.00000")]
        public decimal minNormalizedMeasure
        {
            get
            {
                return minNormalizedMeasureField;
            }
            set
            {
                minNormalizedMeasureField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlElement("mapInfo")]
        [Description("Container for the objective map description.")]
        [Category("Main")]
        public List<ObjectiveTypeMapInfo> mapInfo
        {
            get
            {
                if (mapInfoField == null)
                    mapInfoField = new List<ObjectiveTypeMapInfo>();

                return mapInfoField;
            }
            set
            {
                mapInfoField = value; /* Course._NotifyManifestChanged(this, ManifestChangeType.Changed); */
            }
        }

        [XmlAttribute]
        [Description("This attribute indicates that the <minNormalizedMeasure> shall be used (if value is set to true) in place of any other method to determine if the objective associated with the activity is satisfied.")]
        [DefaultValue(false)]
        public bool satisfiedByMeasure { get; set; }
    }

    [XmlType(AnonymousType = true)]
    [Description("Container for the objective map description. This defines the mapping of an activity’s local objective information to and from a shared global objective.")]
    public class ObjectiveMappingType : ObjectiveTypeMapInfo
    {
        public ObjectiveMappingType(string TargetObjectiveID, bool ReadNormalizedMeasure, bool WriteNormalizedMeasure)
            : this(TargetObjectiveID)
        {
            writeSatisfiedStatus = true;
            readNormalizedMeasure = ReadNormalizedMeasure;
            writeNormalizedMeasure = WriteNormalizedMeasure;
        }

        public ObjectiveMappingType(string TargetObjectiveID)
        {
            targetObjectiveID = TargetObjectiveID;
        }

        public ObjectiveMappingType()
        {
        }

        public static ObjectiveMappingType CreateForQuestion(string TargetID)
        {
            var result = new ObjectiveMappingType(TargetID);
            result.readNormalizedMeasure = result.readSatisfiedStatus =
                result.writeNormalizedMeasure = result.writeSatisfiedStatus = true;
            return result;
        }

        public static ObjectiveMappingType CreateForSummary(string TargetID)
        {
            var result = new ObjectiveMappingType(TargetID);
            result.readNormalizedMeasure = result.readSatisfiedStatus = true;
            result.writeNormalizedMeasure = result.writeSatisfiedStatus = false;
            return result;
        }
    }

    [XmlType(Namespace = ManifestNamespaces.Imsss)]
    [Description("Container for the set of objectives that are to be associated with an activity.")]
    [Category("Main")]
    public class ObjectivesType : AbstractManifestNode, IContainer
    {
        private ObjectivesTypePrimaryObjective primaryObjectiveField;

        private ManifestNodeList<ObjectivesTypeObjective> objectiveField;

        [Description("Identifies the objective that contributes to the rollup associated with the activity.")]
        public ObjectivesTypePrimaryObjective primaryObjective
        {
            get
            {
                return primaryObjectiveField;
            }
            set
            {
                primaryObjectiveField = value;
                Course.NotifyManifestChanged(this, new IManifestNode[1] { value }, ManifestChangeTypes.ChildrenAdded);
            }
        }

        [XmlElement("objective")]
        [Description("Identifies objectives that do not contribute to rollup associated with the activity.")]
        public ManifestNodeList<ObjectivesTypeObjective> objective
        {
            get
            {
                if (objectiveField == null)
                    objectiveField = new ManifestNodeList<ObjectivesTypeObjective>(this);

                return objectiveField;
            }
            set
            {
                objectiveField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        #region IContainer Members

        public void RemoveChild(IManifestNode child)
        {
            switch (child.GetType().Name)
            {
                case "ObjectivesTypeObjective":
                    {
                        var item = child as ObjectivesTypeObjective;
                        if (objective.Contains(item))
                        {
                            objective.Remove(item);
                            return;
                        }
                        break;
                    }
                case "ObjectivesTypePrimaryObjective":
                    {
                        if (primaryObjective != null)
                        {
                            primaryObjective = null;
                            return;
                        }
                        break;
                    }
            }
            throw new FireFlyException("Manifest item '{0}' not found", child);
        }

        #endregion
    }

    [XmlType(AnonymousType = true, Namespace = ManifestNamespaces.Imsss)]
    [Description("Identifies the objective that contributes to the rollup associated with the activity.")]
    public class ObjectivesTypePrimaryObjective : ObjectiveType
    {
        private string objectiveIDField;

        [XmlAttribute(DataType = "anyURI")]
        [Description("The identifier of the objective associated with the activity.")]
        public string objectiveID
        {
            get
            {
                return objectiveIDField;
            }
            set
            {
                objectiveIDField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public override string ToString()
        {
            if (this.objectiveID == null || this.objectiveID == "")
            {
                return "primaryObjective";
            }
            return this.objectiveID;
        }

        public ObjectivesTypePrimaryObjective(string ID)
        {
            objectiveID = ID;
        }

        public ObjectivesTypePrimaryObjective()
        {
        }

        public ObjectivesTypePrimaryObjective(string ID, bool SatisfiedByMeasure)
            : this(ID)
        {
            satisfiedByMeasure = SatisfiedByMeasure;
        }
    }

    [XmlType(AnonymousType = true, Namespace = ManifestNamespaces.Imsss)]
    [Description("Identifies objectives that do not contribute to rollup associated with the activity. This element can only exist if a <primaryObjective> has been defined.")]
    public class ObjectivesTypeObjective : ObjectiveType
    {
        public ObjectivesTypeObjective(string ID)
        {
            objectiveID = ID;
        }

        public ObjectivesTypeObjective()
        {
        }

        private string objectiveIDField;

        [XmlAttribute(DataType = "anyURI")]
        [Description("The identifier of the objective associated with the activity.")]
        public string objectiveID
        {
            get
            {
                return objectiveIDField;
            }
            set
            {
                objectiveIDField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public override string ToString()
        {
            if (this.objectiveID == null || this.objectiveID == "")
            {
                return "objective";
            }
            return this.objectiveID;
        }

        public static ObjectivesTypeObjective CreateGlobalObj(string ID)
        {
            var result = new ObjectivesTypeObjective(ID);
            result.mapInfo.Add(ObjectiveMappingType.CreateForSummary(ID));
            return result;
        }
    }

    [XmlType(Namespace = ManifestNamespaces.Imsss)]
    [Description("Container for each rollup rule that is to be applied to an activity. The general format for a rule can be expressed informally as 'If child-activity set, condition set Then action'. Multiple conditions are permitted.")]
    [Category("Main")]
    public class RollupRuleType : AbstractManifestNode, IContainer
    {
        private RollupRuleTypeRollupConditions rollupConditionsField;

        private RollupRuleTypeRollupAction rollupActionField;

        private ChildActivityType childActivitySetField;

        private string minimumCountField;

        private decimal minimumPercentField;

        public RollupRuleType()
        {
            childActivitySetField = ChildActivityType.all;
            minimumCountField = "0";
            minimumPercentField = 0m;
        }
        
        [Description("Container for the set of conditions that are applied within a single rollup rule.")]
        [Category("Main")]
        public RollupRuleTypeRollupConditions rollupConditions
        {
            get
            {
                return rollupConditionsField;
            }
            set
            {
                rollupConditionsField = value;
                Course.NotifyManifestChanged(this, new IManifestNode[1] { value }, ManifestChangeTypes.ChildrenAdded);
            }
        }

        [Description("Identifies a condition to be applied in the rollup rule.")]
        public RollupRuleTypeRollupAction rollupAction
        {
            get
            {
                return rollupActionField;
            }
            set
            {
                rollupActionField = value;
                Course.NotifyManifestChanged(this, new IManifestNode[1] { value }, ManifestChangeTypes.ChildrenAdded);
            }
        }

        [XmlAttribute]
        [Description("This attribute indicates whose data values are used to evaluate the rollup condition.")]
        [DefaultValue(ChildActivityType.all)]
        public ChildActivityType childActivitySet
        {
            get
            {
                return childActivitySetField;
            }
            set
            {
                childActivitySetField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlAttribute(DataType = "nonNegativeInteger")]
        [Description("The minimumCount attribute shall be used when the childActivitySet attribute is set to \"atLeastCount\".")]
        [DefaultValue("0")]
        public string minimumCount
        {
            get
            {
                return minimumCountField;
            }
            set
            {
                minimumCountField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlAttribute]
        [Description("The minimumPercent attribute shall be used when the childActivitySet attribute is set to atLeastPercent.")]
        [DefaultValue(typeof(decimal), "0")]
        public decimal minimumPercent
        {
            get
            {
                return minimumPercentField;
            }
            set
            {
                minimumPercentField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        #region IContainer Members

        public void RemoveChild(IManifestNode child)
        {
            switch (child.GetType().Name)
            {
                case "RollupRuleTypeRollupConditions":
                    {
                        if (rollupConditions != null)
                        {
                            rollupConditions = null;
                            return;
                        }
                        break;
                    }
                case "RollupRuleTypeRollupAction":
                    {
                        if (rollupAction != null)
                        {
                            rollupAction = null;
                            return;
                        }
                        break;
                    }
            }
            throw new FireFlyException("Manifest item '{0}' not found", child);
        }

        #endregion
    }

    [XmlType(AnonymousType = true)]
    [Description("Container for the set of conditions that are applied within a single rollup rule.")]
    [Category("Main")]
    public class RollupRuleTypeRollupConditions : AbstractManifestNode, IContainer
    {
        private ManifestNodeList<RollupRuleTypeRollupConditionsRollupCondition> rollupConditionField;

        private ConditionCombinationType conditionCombinationField;

        public RollupRuleTypeRollupConditions()
        {
            conditionCombinationField = ConditionCombinationType.any;
        }

        [XmlElement("rollupCondition")]
        [Description("Identifies a condition to be applied in the rollup rule.")]
        [Category("Main")]
        public ManifestNodeList<RollupRuleTypeRollupConditionsRollupCondition> rollupCondition
        {
            get
            {
                if (rollupConditionField == null)
                    rollupConditionField = new ManifestNodeList<RollupRuleTypeRollupConditionsRollupCondition>(this);

                return rollupConditionField;
            }
            set
            {
                rollupConditionField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlAttribute]
        [DefaultValue(ConditionCombinationType.any)]
        [Description("This attribute indicates how rule conditions are combined in evaluating the rule.")]
        [Category("Main")]
        public ConditionCombinationType conditionCombination
        {
            get
            {
                return conditionCombinationField;
            }
            set
            {
                conditionCombinationField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        #region IContainer Members

        public void RemoveChild(IManifestNode child)
        {
            switch (child.GetType().Name)
            {
                case "RollupRuleTypeRollupConditionsRollupCondition":
                    {
                        var item = child as RollupRuleTypeRollupConditionsRollupCondition;
                        if (rollupCondition.Contains(item))
                        {
                            rollupCondition.Remove(item);
                            return;
                        }
                        break;
                    }
            }
            throw new FireFlyException("Item '{0}' not found", child);
        }

        #endregion
    }

    [XmlType(AnonymousType = true)]
    [Description("Identifies a condition to be applied in the rollup rule.")]
    [Category("Main")]
    public class RollupRuleTypeRollupConditionsRollupCondition : AbstractManifestNode, IManifestNode
    {
        private ConditionOperatorType operatorField;

        private RollupRuleConditionType conditionField;

        public RollupRuleTypeRollupConditionsRollupCondition()
        {
            operatorField = ConditionOperatorType.noOp;
        }

        [XmlAttribute]
        [Description("The unary logical operator to be applied to the individual condition.")]
        [DefaultValue(ConditionOperatorType.noOp)]
        public ConditionOperatorType @operator
        {
            get
            {
                return operatorField;
            }
            set
            {
                operatorField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlAttribute]
        [Description("Indicates the condition element for the rule.")]
        public RollupRuleConditionType condition
        {
            get
            {
                return conditionField;
            }
            set
            {
                conditionField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType(Namespace = ManifestNamespaces.Imsss)]
    [Description("The unary logical operator to be applied to the individual condition.")]
    public enum ConditionOperatorType
    {
        not,
        noOp,
    }

    [XmlType(Namespace = ManifestNamespaces.Imsss)]
    [Description("Indicates the condition element for the rule.")]
    public enum RollupRuleConditionType
    {
        satisfied,
        objectiveStatusKnown,
        objectiveMeasureKnown,
        completed,
        activityProgressKnown,
        attempted,
        attemptLimitExceeded,
        timeLimitExceeded,
        outsideAvailableTimeRange,
    }

    [XmlType(Namespace = ManifestNamespaces.Imsss)]
    [Description("This attribute indicates how rule conditions are combined in evaluating the rule")]
    [Category("Main")]
    public enum ConditionCombinationType
    {
        all,
        any,
    }

    [XmlType(AnonymousType = true)]
    public class RollupRuleTypeRollupAction : AbstractManifestNode, IManifestNode
    {
        private RollupActionType actionField;

        [XmlAttribute]
        [Description("Element identifies a condition to be applied in the rollup rule")]
        [Category("Main")]
        public RollupActionType action
        {
            get
            {
                return actionField;
            }
            set
            {
                actionField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType(Namespace = ManifestNamespaces.Imsss)]
    [Description("Element identifies a condition to be applied in the rollup rule")]
    [Category("Main")]
    public enum RollupActionType
    {
        satisfied,
        notSatisfied,
        completed,
        incomplete,
    }

    [XmlType(Namespace = ManifestNamespaces.Imsss)]
    public enum ChildActivityType
    {
        all,
        any,
        none,
        atLeastCount,
        atLeastPercent,
    }

    [XmlType(Namespace = ManifestNamespaces.Imsss)]
    [Description("Container for the set of rollup rules defined for the activity.")]
    [Category("Main")]
    public class RollupRulesType : AbstractManifestNode, IContainer
    {
        private ManifestNodeList<RollupRuleType> rollupRuleField;

        private bool rollupObjectiveSatisfiedField;

        private bool rollupProgressCompletionField;

        private decimal objectiveMeasureWeightField;

        public RollupRulesType()
        {
            rollupObjectiveSatisfiedField = true;
            rollupProgressCompletionField = true;
            objectiveMeasureWeightField = 1.0000m;
        }

        [XmlElement("rollupRule")]
        [Description("Container for the set of rollup rules defined for the activity.")]
        [Category("Main")]
        public ManifestNodeList<RollupRuleType> rollupRule
        {
            get
            {
                if (rollupRuleField == null)
                    rollupRuleField = new ManifestNodeList<RollupRuleType>(this);

                return rollupRuleField;
            }
            set
            {
                rollupRuleField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [Description("This attribute indicates that the objective’s satisfied status associated with the activity is included in the rollup for its parent activity.")]
        [XmlAttribute]
        [DefaultValue(true)]
        public bool rollupObjectiveSatisfied
        {
            get
            {
                return rollupObjectiveSatisfiedField;
            }
            set
            {
                rollupObjectiveSatisfiedField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [Description("This attribute indicates that the attempt’s completion status associated with the activity is included in the rollup for its parent activity.")]
        [XmlAttribute]
        [DefaultValue(true)]
        public bool rollupProgressCompletion
        {
            get
            {
                return rollupProgressCompletionField;
            }
            set
            {
                rollupProgressCompletionField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [Description("This attribute indicates the weighting factor applied to the objectives normalized measure used during rollup for the parent activity.")]
        [XmlAttribute]
        [DefaultValue(typeof(decimal), "1.0000")]
        public decimal objectiveMeasureWeight
        {
            get
            {
                return objectiveMeasureWeightField;
            }
            set
            {
                objectiveMeasureWeightField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        #region IContainer Members

        public void RemoveChild(IManifestNode Child)
        {
            switch (Child.GetType().Name)
            {
                case "RollupRuleType":
                    {
                        var item = Child as RollupRuleType;
                        if (rollupRule.Contains(item))
                        {
                            rollupRule.Remove(item);
                            return;
                        }
                        break;
                    }
            }
        }

        #endregion
    }

    [XmlType(Namespace = ManifestNamespaces.Imsss)]
    [Description("At this time, ADL recommends to use the auxiliaryResources element with caution.")]
    public class AuxiliaryResourceType : AbstractManifestNode, IManifestNode
    {
        private string auxiliaryResourceIDField;

        private string purposeField;

        [XmlAttribute(DataType = "anyURI")]
        public string auxiliaryResourceID
        {
            get
            {
                return auxiliaryResourceIDField;
            }
            set
            {
                auxiliaryResourceIDField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlAttribute]
        public string purpose
        {
            get
            {
                return purposeField;
            }
            set
            {
                purposeField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public override string ToString()
        {
            return auxiliaryResourceID;
        }
    }

    [XmlType(Namespace = ManifestNamespaces.Imsss)]
    [Description("The limit condition deals with attempts on the activity and maximum time allowed in the attempt.")]
    [Category("Main")]
    public class LimitConditionsType : AbstractManifestNode, IManifestNode
    {
        private string attemptLimitField;

        private string attemptAbsoluteDurationLimitField;

        [Description("This value indicates the maximum number of attempts for the activity.")]
        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string attemptLimit
        {
            get
            {
                return attemptLimitField;
            }
            set
            {
                attemptLimitField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [Description("This value indicates the maximum time duration that the learner is permitted to spend on any single learner attempt on the activity. The limit applies to only the time the learner is actually interacting with the activity and does not apply when the activity is suspended.")]
        [XmlAttribute(DataType = "duration")]
        public string attemptAbsoluteDurationLimit
        {
            get
            {
                return attemptAbsoluteDurationLimitField;
            }
            set
            {
                attemptAbsoluteDurationLimitField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlInclude(typeof(PreConditionRuleType))]
    [XmlInclude(typeof(PostConditionRuleType))]
    [XmlInclude(typeof(ExitConditionRuleType))]
    [XmlType(Namespace = ManifestNamespaces.Imsss)]
    [Description("Container for a sequencing rule description. Each rule describes the sequencing behavior for an activity.")]
    [Category("Main")]
    public abstract class SequencingRuleType : AbstractManifestNode, IManifestNode
    {
        private SequencingRuleTypeRuleConditions ruleConditionsField;
        
        [Description("Container for the set of conditions that are to be applied either the pre-condition, post-condition and exit condition rules.")]
        [Category("Main")]
        public SequencingRuleTypeRuleConditions ruleConditions
        {
            get
            {
                return ruleConditionsField;
            }
            set
            {
                ruleConditionsField = value;
                Course.NotifyManifestChanged(this, new IManifestNode[1] { value }, ManifestChangeTypes.ChildrenAdded);
            }
        }
    }

    [XmlType(AnonymousType = true)]
    [Description("Container for the set of conditions that are to be applied either the pre-condition, post-condition and exit condition rules.")]
    [Category("Main")]
    public class SequencingRuleTypeRuleConditions : AbstractManifestNode, IContainer
    {
        private ManifestNodeList<SequencingRuleTypeRuleConditionsRuleCondition> ruleConditionField;

        private ConditionCombinationType conditionCombinationField;

        public SequencingRuleTypeRuleConditions()
        {
            conditionCombinationField = ConditionCombinationType.all;
        }

        [XmlElement("ruleCondition")]
        [Description("Element represents the condition that is evaluated.")]
        [Category("Main")]
        public ManifestNodeList<SequencingRuleTypeRuleConditionsRuleCondition> ruleCondition
        {
            get
            {
                if (ruleConditionField == null)
                    ruleConditionField = new ManifestNodeList<SequencingRuleTypeRuleConditionsRuleCondition>(this);

                return ruleConditionField;
            }
            set
            {
                ruleConditionField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlAttribute]
        [DefaultValue(ConditionCombinationType.all)]
        [Description("This attribute indicates how rule conditions are combined in evaluating the rule.")]
        [Category("Main")]
        public ConditionCombinationType conditionCombination
        {
            get
            {
                return conditionCombinationField;
            }
            set
            {
                conditionCombinationField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        #region IContainer Members

        public void RemoveChild(IManifestNode child)
        {
            switch (child.GetType().Name)
            {
                case "SequencingRuleTypeRuleConditionsRuleCondition":
                    {
                        var item = child as SequencingRuleTypeRuleConditionsRuleCondition;
                        if (ruleCondition.Contains(item))
                        {
                            ruleCondition.Remove(item);
                            return;
                        }
                        break;
                    }
            }
            throw new FireFlyException("Item '{0}' not found", child);
        }

        #endregion
    }

    [XmlType(AnonymousType = true)]
    [Description("Element represents the condition that is evaluated.")]
    [Category("Main")]
    public class SequencingRuleTypeRuleConditionsRuleCondition : AbstractManifestNode, IManifestNode
    {
        private string referencedObjectiveField;

        private decimal measureThresholdField;

        private ConditionOperatorType operatorField;

        private SequencingRuleConditionType conditionField;

        public SequencingRuleTypeRuleConditionsRuleCondition()
        {
            operatorField = ConditionOperatorType.noOp;
        }

        [Description("This attribute represents the identifier of a local objective associated with the activity. The status of this objective will be used during the evaluation of the condition.")]
        [XmlAttribute(DataType = "anyURI")]
        public string referencedObjective
        {
            get
            {
                return referencedObjectiveField;
            }
            set
            {
                referencedObjectiveField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [Description("The value used as a threshold during measurebased condition evaluations.")]
        [XmlAttribute]
        public decimal measureThreshold
        {
            get
            {
                return measureThresholdField;
            }
            set
            {
                measureThresholdField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [Description("The unary logical operator to be applied to the condition.")]
        [XmlAttribute]
        [DefaultValue(ConditionOperatorType.noOp)]
        public ConditionOperatorType @operator
        {
            get
            {
                return operatorField;
            }
            set
            {
                operatorField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [Description("This attribute represents the actual condition for the rule.")]
        [XmlAttribute]
        public SequencingRuleConditionType condition
        {
            get
            {
                return conditionField;
            }
            set
            {
                conditionField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [Description("This attribute represents the actual condition for the rule.")]
    [XmlType(Namespace = ManifestNamespaces.Imsss)]
    public enum SequencingRuleConditionType
    {
        satisfied,
        objectiveStatusKnown,
        objectiveMeasureKnown,
        objectiveMeasureGreaterThan,
        objectiveMeasureLessThan,
        completed,
        activityProgressKnown,
        attempted,
        attemptLimitExceeded,
        timeLimitExceeded,
        always,
    }

    [XmlType(Namespace = ManifestNamespaces.Imsss)]
    [Description("Container for the description of actions that control sequencing decisions and delivery of a specific activity. Rules that include such actions are used to determine if the activity will be delivered.")]
    [Category("Main")]
    public class PreConditionRuleType : SequencingRuleType, IContainer
    {
        private PreConditionRuleTypeRuleAction ruleActionField;
        
        [Description("Element is the desired sequencing behavior if the rule evaluates to true.")]
        [Category("Main")]
        public PreConditionRuleTypeRuleAction ruleAction
        {
            get
            {
                return ruleActionField;
            }
            set
            {
                ruleActionField = value;
                Course.NotifyManifestChanged(this, new IManifestNode[1] { value }, ManifestChangeTypes.ChildrenAdded);
            }
        }

        #region IContainer Members

        public void RemoveChild(IManifestNode child)
        {
            switch (child.GetType().Name)
            {
                case "SequencingRuleTypeRuleConditions":
                    {
                        if (ruleConditions != null)
                        {
                            ruleConditions = null;
                            return;
                        }
                        break;
                    }
                case "PreConditionRuleTypeRuleAction":
                    {
                        if (ruleAction != null)
                        {
                            ruleAction = null;
                            return;
                        }
                        break;
                    }
            }
            throw new FireFlyException("Item '{0}' not found", child);
        }

        #endregion
    }

    [XmlType(AnonymousType = true)]
    [Description("Element is the desired sequencing behavior if the rule evaluates to true.")]
    [Category("Main")]
    public class PreConditionRuleTypeRuleAction : AbstractManifestNode, IManifestNode
    {
        private PreConditionRuleActionType actionField;

        [XmlAttribute]
        [Description("Element is the desired sequencing behavior if the rule evaluates to true.")]
        [Category("Main")]
        public PreConditionRuleActionType action
        {
            get
            {
                return actionField;
            }
            set
            {
                actionField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType(Namespace = ManifestNamespaces.Imsss)]
    [Description("Element is the desired sequencing behavior if the rule evaluates to true.")]
    [Category("Main")]
    public enum PreConditionRuleActionType
    {
        skip,
        disabled,
        hiddenFromChoice,
        stopForwardTraversal,
    }

    [XmlType(Namespace = ManifestNamespaces.Imsss)]
    [Description("Container for the description of actions that control sequencing decisions and delivery of a specific activity. Rules that include such actions are applied when the activity attempt terminates.")]
    [Category("Main")]
    public class PostConditionRuleType : SequencingRuleType
    {
        private PostConditionRuleTypeRuleAction ruleActionField;
        
        [Description("Element is the desired sequencing behavior if the rule evaluates to true.")]
        [Category("Main")]
        public PostConditionRuleTypeRuleAction ruleAction
        {
            get
            {
                return ruleActionField;
            }
            set
            {
                ruleActionField = value;
                Course.NotifyManifestChanged(this, new IManifestNode[1] { value }, ManifestChangeTypes.ChildrenAdded);
            }
        }

        #region IContainer Members

        public void RemoveChild(IManifestNode child)
        {
            switch (child.GetType().Name)
            {
                case "SequencingRuleTypeRuleConditions":
                    {
                        if (ruleConditions != null)
                        {
                            ruleConditions = null;
                            return;
                        }
                        break;
                    }
                case "PostConditionRuleTypeRuleAction":
                    {
                        if (ruleAction != null)
                        {
                            ruleAction = null;
                            return;
                        }
                        break;
                    }
            }
            throw new FireFlyException("Item '{0}' not found", child);
        }

        #endregion
    }

    [XmlType(AnonymousType = true)]
    [Description("Element is the desired sequencing behavior if the rule evaluates to true.")]
    [Category("Main")]
    public class PostConditionRuleTypeRuleAction : AbstractManifestNode, IManifestNode
    {
        private PostConditionRuleActionType actionField;

        [XmlAttribute]
        [Description("Element is the desired sequencing behavior if the rule evaluates to true.")]
        [Category("Main")]
        public PostConditionRuleActionType action
        {
            get
            {
                return actionField;
            }
            set
            {
                actionField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public override string ToString()
        {
            return action.ToString();
        }
    }

    [XmlType(Namespace = ManifestNamespaces.Imsss)]
    [Description("Element is the desired sequencing behavior if the rule evaluates to true.")]
    [Category("Main")]
    public enum PostConditionRuleActionType
    {
        exitParent,
        exitAll,
        retry,
        retryAll,
        @continue,
        previous,
    }

    [XmlType(Namespace = ManifestNamespaces.Imsss)]
    [Description("Container for the description of actions that control sequencing decisions and delivery of a specific activity. Rules that include such actions are applied after an activity attempt on a descendent activity terminates.")]
    [Category("Main")]
    public class ExitConditionRuleType : SequencingRuleType, IContainer
    {
        private ExitConditionRuleTypeRuleAction ruleActionField;
        
        [Description("Element is the desired sequencing behavior if the rule evaluates to true.")]
        [Category("Main")]
        public ExitConditionRuleTypeRuleAction ruleAction
        {
            get
            {
                return ruleActionField;
            }
            set
            {
                ruleActionField = value;
                Course.NotifyManifestChanged(this, new IManifestNode[1] { value }, ManifestChangeTypes.ChildrenAdded);
            }
        }

        #region IContainer Members

        public void RemoveChild(IManifestNode child)
        {
            switch (child.GetType().Name)
            {
                case "SequencingRuleTypeRuleConditions":
                    {
                        if (ruleConditions != null)
                        {
                            ruleConditions = null;
                            return;
                        }
                        break;
                    }
                case "ExitConditionRuleTypeRuleAction":
                    {
                        if (ruleAction != null)
                        {
                            ruleAction = null;
                            return;
                        }
                        break;
                    }
            }
            throw new FireFlyException("Item '{0}' not found", child);
        }

        #endregion
    }

    [XmlType(AnonymousType = true)]
    [Description("Element is the desired sequencing behavior if the rule evaluates to true.")]
    [Category("Main")]
    public class ExitConditionRuleTypeRuleAction : AbstractManifestNode, IManifestNode
    {
        private ExitConditionRuleActionType actionField;

        [XmlAttribute]
        [Description("Element is the desired sequencing behavior if the rule evaluates to true.")]
        [Category("Main")]
        public ExitConditionRuleActionType action
        {
            get
            {
                return actionField;
            }
            set
            {
                actionField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType(Namespace = ManifestNamespaces.Imsss)]
    [Description("Element is the desired sequencing behavior if the rule evaluates to true.")]
    [Category("Main")]
    public enum ExitConditionRuleActionType
    {
        exit,
    }

    [XmlType(Namespace = ManifestNamespaces.Imsss)]
    [Description("Container for a sequencing rule description. Each rule describes the sequencing behavior for an activity.")]
    [Category("Main")]
    public class SequencingRulesType : AbstractManifestNode, IContainer
    {
        private ManifestNodeList<PreConditionRuleType> preConditionRuleField;

        private ManifestNodeList<ExitConditionRuleType> exitConditionRuleField;

        private ManifestNodeList<PostConditionRuleType> postConditionRuleField;

        [XmlElement("preConditionRule")]
        [Description("Container for the description of actions that control sequencing decisions and delivery of a specific activity. Rules that include such actions are used to determine if the activity will be delivered.")]
        [Category("Main")]
        public ManifestNodeList<PreConditionRuleType> preConditionRule
        {
            get
            {
                if (preConditionRuleField == null)
                    preConditionRuleField = new ManifestNodeList<PreConditionRuleType>(this);

                return preConditionRuleField;
            }
            set
            {
                preConditionRuleField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlElement("exitConditionRule")]
        [Description("Container for the description of actions that control sequencing decisions and delivery of a specific activity. Rules that include such actions are applied after an activity attempt on a descendent activity terminates.")]
        [Category("Main")]
        public ManifestNodeList<ExitConditionRuleType> exitConditionRule
        {
            get
            {
                if (exitConditionRuleField == null)
                    exitConditionRuleField = new ManifestNodeList<ExitConditionRuleType>(this);

                return exitConditionRuleField;
            }
            set
            {
                exitConditionRuleField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlElement("postConditionRule")]
        [Description("Container for the description of actions that control sequencing decisions and delivery of a specific activity. Rules that include such actions are applied when the activity attempt terminates.")]
        [Category("Main")]
        public ManifestNodeList<PostConditionRuleType> postConditionRule
        {
            get
            {
                if (postConditionRuleField == null)
                    postConditionRuleField = new ManifestNodeList<PostConditionRuleType>(this);

                return postConditionRuleField;
            }
            set
            {
                postConditionRuleField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        #region IContainer Members

        public void RemoveChild(IManifestNode child)
        {
            switch (child.GetType().Name)
            {
                case "PreConditionRuleType":
                    {
                        var item = child as PreConditionRuleType;
                        if (preConditionRule.Contains(item))
                        {
                            preConditionRule.Remove(item);
                            return;
                        }
                        break;
                    }
                case "PostConditionRuleType":
                    {
                        var item = child as PostConditionRuleType;
                        if (postConditionRule.Contains(item))
                        {
                            postConditionRule.Remove(item);
                            return;
                        }
                        break;
                    }
                case "ExitConditionRuleType":
                    {
                        var item = child as ExitConditionRuleType;
                        if (exitConditionRule.Contains(item))
                        {
                            exitConditionRule.Remove(item);
                            return;
                        }
                        break;
                    }
            }
            throw new FireFlyException("Item '{0}' not found", child);
        }

        #endregion
    }

    [XmlType(Namespace = ManifestNamespaces.Imsss)]
    [Description("Container for the sequencing control mode information including descriptions of the types of sequencing behaviors specified for an activity . This element captures information dealing with the types of sequencing requests that are permitted.")]
    [Category("Main")]
    public class ControlModeType : AbstractManifestNode
    {
        public ControlModeType()
        {
            choice = true;
            choiceExit = true;
            flow = false;
            forwardOnly = false;
            useCurrentAttemptObjectiveInfo = true;
            useCurrentAttemptProgressInfo = true;
        }

        [Description("Indicates that a choice sequencing request is permitted (or not permitted if value = false) to target the children of the activity.")]
        [XmlAttribute]
        [DefaultValue(true)]
        public bool choice { get; set; }

        [Description("Indicates that an active child of this activity is permitted to terminate (or not permitted if value = false) if a choice sequencing request is processed.")]
        [XmlAttribute]
        [DefaultValue(true)]
        public bool choiceExit { get; set; }

        [Description("Indicates the flow sequencing requests is permitted (or not permitted if value = false) to the children of this activity.")]
        [XmlAttribute]
        [DefaultValue(false)]
        public bool flow { get; set; }

        [Description("Indicates that backward targets (in terms of activity tree traversal) are not permitted (or are permitted if value = false) for the children of this activity.")]
        [XmlAttribute]
        [DefaultValue(false)]
        public bool forwardOnly { get; set; }

        [Description("Indicates that the objective progress information for the children of the activity will only be used (or not used if value = false) in rule evaluations and rollup if that information was recorded during the current attempt on the activity.")]
        [XmlAttribute]
        [DefaultValue(true)]
        public bool useCurrentAttemptObjectiveInfo { get; set; }

        [Description("Indicates that the attempt progress information for the children of the activity will only be used (or not used if value = false) in rule evaluations and rollup if that information was recorded during the current attempt on the activity.")]
        [XmlAttribute]
        [DefaultValue(true)]
        public bool useCurrentAttemptProgressInfo { get; set; }
    }

    [XmlType("constrainedChoiceConsiderations", Namespace = ManifestNamespaces.Adlseq)]
    [Description("Element is the container for the descriptions of how choice navigation requests should be constrained during the sequencing process. The constrained choice only applies to the activity for which it is defined.")]
    public class ConstrainedChoiceConsiderationsType : AbstractManifestNode, IManifestNode
    {
        private bool preventActivationField;

        private bool constrainChoiceField;

        public ConstrainedChoiceConsiderationsType()
        {
            preventActivationField = false;
            constrainChoiceField = false;
        }

        [XmlAttribute]
        [Description("This attribute indicates that attempts on children activities should not begin unless the current activity is the parent.")]
        [DefaultValue(false)]
        public bool preventActivation
        {
            get
            {
                return preventActivationField;
            }
            set
            {
                preventActivationField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }


        [XmlAttribute]
        [Description("This attribute indicates that only activities which are logically 'next' from the constrained activities can be targets of a choice navigation request.")]
        [DefaultValue(false)]
        public bool constrainChoice
        {
            get
            {
                return constrainChoiceField;
            }
            set
            {
                constrainChoiceField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public override string ToString()
        {
            return "constrainedChoiceConsiderations";
        }
    }

    [XmlType("rollupConsiderations", Namespace = ManifestNamespaces.Adlseq)]
    [Description("Element is the container for the descriptions of when an activity should be included in the rollup process.")]
    public class RollupConsiderationsType : AbstractManifestNode, IManifestNode
    {
        private RollupConsiderationType requiredForSatisfiedField;

        private RollupConsiderationType requiredForNotSatisfiedField;

        private RollupConsiderationType requiredForCompletedField;

        private RollupConsiderationType requiredForIncompleteField;

        private bool measureSatisfactionIfActiveField;

        public RollupConsiderationsType()
        {
            requiredForSatisfiedField = RollupConsiderationType.always;
            requiredForNotSatisfiedField = RollupConsiderationType.always;
            requiredForCompletedField = RollupConsiderationType.always;
            requiredForIncompleteField = RollupConsiderationType.always;
            measureSatisfactionIfActiveField = true;
        }

        [XmlAttribute]
        [Description("This attribute indicates the condition under which the activity is included in its parent’s evaluation of a satisfied rollup rule.")]
        [DefaultValue(RollupConsiderationType.always)]
        public RollupConsiderationType requiredForSatisfied
        {
            get
            {
                return requiredForSatisfiedField;
            }
            set
            {
                requiredForSatisfiedField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlAttribute]
        [Description("This attribute indicates the condition under which the activity is included in its parent’s evaluation of a not satisfied rollup rule.")]
        [DefaultValue(RollupConsiderationType.always)]
        public RollupConsiderationType requiredForNotSatisfied
        {
            get
            {
                return requiredForNotSatisfiedField;
            }
            set
            {
                requiredForNotSatisfiedField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlAttribute]
        [Description("This attribute indicates the condition under which the activity is included in its parent’s evaluation of a completed rollup rule.")]
        [DefaultValue(RollupConsiderationType.always)]
        public RollupConsiderationType requiredForCompleted
        {
            get
            {
                return requiredForCompletedField;
            }
            set
            {
                requiredForCompletedField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }


        [XmlAttribute]
        [Description("This attributeindicates the condition under which the activity is included in its parent’s evaluation of a incomplete rollup rule.")]
        [DefaultValue(RollupConsiderationType.always)]
        public RollupConsiderationType requiredForIncomplete
        {
            get
            {
                return requiredForIncompleteField;
            }
            set
            {
                requiredForIncompleteField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlAttribute]
        [Description("This attribute indicates if the measure should be used to determine satisfaction during rollup when the activity is active.")]
        [DefaultValue(true)]
        public bool measureSatisfactionIfActive
        {
            get
            {
                return measureSatisfactionIfActiveField;
            }
            set
            {
                measureSatisfactionIfActiveField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public override string ToString()
        {
            return "rollupConsiderations";
        }
    }

    [XmlType(Namespace = ManifestNamespaces.Adlseq)]
    [Description("When an activity should be included in the rollup process.")]
    public enum RollupConsiderationType
    {
        always,
        ifAttempted,
        ifNotSkipped,
        ifNotSuspended,
    }

    [XmlType(Namespace = ManifestNamespaces.Imsss)]
    [Description("Container for the set of sequencing information. The reuse happens when the IDRef attribute of the <sequencing> element references an ID attribute of a <sequencing> element that it is a child element of the <sequencingCollection>.")]
    [Category("Main")]
    public class SequencingCollection : AbstractManifestNode, IContainer
    {
        private ManifestNodeList<SequencingType> sequencingCollectionField;
                
        [XmlElement("sequencing", Namespace=ManifestNamespaces.Imsss)]
        [Description("Container for the set of sequencing information.")]
        [Category("Main")]
        public ManifestNodeList<SequencingType> sequencingCollection
        {
            get
            {
                return this.sequencingCollectionField;
            }
            set
            {
                if (value == null)
                {
                    this.sequencingCollectionField = new ManifestNodeList<SequencingType>();
                }
                else
                {
                    this.sequencingCollectionField = value;
                }
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public SequencingCollection()
        {
            this.sequencingCollection = new ManifestNodeList<SequencingType>();
        }

        public override string ToString()
        {
            return "sequencingCollection";
        }

        #region IContainer Members

        public void RemoveChild(IManifestNode child)
        {
            switch (child.GetType().Name)
            {
                case "SequencingType":
                    {
                        SequencingType item = child as SequencingType;
                        if (this.sequencingCollection.Contains(item))
                        {
                            this.sequencingCollection.Remove(item);
                            return;
                        }
                        break;
                    }                
            }
            throw new FireFlyException("Item '{0}' not found", child);
        }

        #endregion
    }

    #endregion //Sequencing
    
    /// <summary>
    /// Defines type of item-page.
    /// </summary>
    public enum PageType
    {
        Unknown,
        Theory,
        Question,
        Summary,
        Chapter,
        ControlChapter
    }

    #region Other Manifest Elements

    [XmlType("dependency", Namespace = ManifestNamespaces.Imscp)]
    [Description("Element used to group sets of files.")]
    [Category("Main")]
    public class DependencyType : AbstractManifestNode, IManifestNode
    {
        private string identifierrefField;

        [XmlAttribute]
        public string identifierref
        {
            get
            {
                return identifierrefField;
            }
            set
            {
                identifierrefField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public override string ToString()
        {
            return identifierref;
        }
    }

    [XmlType("file", Namespace = ManifestNamespaces.Imscp)]
    [Description("Describes an Asset in a context insensitive manner.")]
    [Category("Main")]
    public class FileType : AbstractManifestNode, IManifestNode
    {
        private MetadataType metadataField;

        private string hrefField;

        public FileType()
        {
        }

        public FileType(string href)
        {
            this.href = href;
        }
        
        [Description("Contains relevant information that describes the content package as a whole.")]
        [Category("Main")]
        public MetadataType metadata
        {
            get
            {
                return metadataField;
            }
            set
            {
                metadataField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlAttribute(DataType = "anyURI")]
        public string href
        {
            get
            {
                return hrefField;
            }
            set
            {
                hrefField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public override string ToString()
        {
            return href;
        }
    }
    
    [XmlRoot("manifest", Namespace = ManifestNamespaces.Imscp)]
    [Description("Element represents a reusable unit of instruction that encapsulates meta-data, organizations and resource references.")]
    [Category("Main")]
    public class ManifestType : AbstractManifestNode, IManifestNode, ITitled
    {
        [NotNull]
        public static XmlSerializer Serializer;

        /// <summary>
        /// Create empty manifest.
        /// </summary>
        /// <param name="identifier">Identifier of new manifest.</param>
        public ManifestType(string identifier)
        {
            var org = new OrganizationType { SubItems = new ManifestNodeList<ItemType>(this), Title = "Chapter 1" };
            organizations = new OrganizationsType();
            organizations.Organizations.Add(org);
            resources = new ResourcesType();
            Identifier = identifier;
        }

        /// <summary>
        /// Create new manifest.
        /// </summary>
        public ManifestType()
        {
            this.sequencingCollection = new SequencingCollection();
        }

        private MetadataType metadataField;

        private OrganizationsType organizationsField;

        private ResourcesType resourcesField;

        private SequencingCollection sequencingCollectionField;

        private ManifestNodeList<ManifestType> manifestField;

        private string _identifier;

        private int versionField;

        [Description("Contains relevant information that describes the content package as a whole.")]
        [Category("Main")]
        public MetadataType metadata
        {
            get
            {
                return metadataField;
            }
            set
            {
                metadataField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
        
        [Description("Contains the content structure or organization of the learning resources making up a stand-alone unit or units of instruction.")]
        [Category("Main")]
        public OrganizationsType organizations
        {
            get
            {
                return organizationsField;
            }
            set
            {
                organizationsField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
        
        [Description("Element is a collection of references to resources.")]
        [Category("Main")]
        public ResourcesType resources
        {
            get
            {
                return resourcesField;
            }
            set
            {
                resourcesField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlElement("sequencingCollection", Namespace = ManifestNamespaces.Imsss)]
        [Description("Container for the set of sequencing information.")]
        [Category("Main")]
        public SequencingCollection sequencingCollection
        {
            get
            {
                return this.sequencingCollectionField;
            }
            set
            {
                this.sequencingCollectionField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlElement("manifest")]
        [Description("Element represents a reusable unit of instruction that encapsulates meta-data, organizations and resource references.")]
        [Category("Main")]
        public ManifestNodeList<ManifestType> manifest
        {
            get
            {
                if (manifestField == null)
                    manifestField = new ManifestNodeList<ManifestType>(this);

                return manifestField;
            }
            set
            {
                manifestField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlAttribute("identifier")]
        [Description("This attribute identifies the manifest. The identifier is unique within the manifest element.")]
        public string Identifier
        {
            get
            {
                return _identifier;
            }
            set
            {
                if (value != Identifier)
                {
                    _identifier = value;
                    if (TitleChanged != null)
                    {
                        TitleChanged();
                    }
                    Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
                }
            }
        }

        [XmlAttribute]
        [Description("The version attribute identifies the version of the manifest.")]
        [DefaultValue(0)]
        public int version
        {
            get
            {
                return versionField;
            }
            set
            {
                versionField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public override string ToString()
        {
            return Identifier;
        }

        public event Action TitleChanged;

        [XmlIgnore]
        public string Title
        {
            get
            {
                return Identifier;
            }
            set
            {
                Identifier = value;
            }
        }
    }

    [TypeConverter(typeof(MetadataConverter))]
    [XmlType("metadata", Namespace = ManifestNamespaces.Imscp)]
    [Description("Contains relevant information that describes the content package as a whole.")]
    [Category("Main")]
    public class MetadataType : AbstractManifestNode
    {
        public MetadataType(string Schema, string Version)
        {
            schema = Schema;
            schemaversion = Version;
        }

        /// <summary>
        ///  Parameterless constructor for serializer
        /// </summary>
        public MetadataType()
        {
        }

        [Description("The <schema> element describes the schema that defines and controls the manifest element.")]
        public string schema { get; set; }

        [Description("The <schemaversion> element describes the version of the above schema (<schema>).")]
        public string schemaversion { get; set; }
    }

    [XmlType("resource", Namespace = ManifestNamespaces.Imscp)]
    [Description("Element is a reference to a resource. There are two primary types of resources defined within SCORM: Asset and SCO.")]
    [Category("Main")]
    public class ResourceType : AbstractManifestNode, IManifestNode, IDisposable
    {
        public ResourceType()
        {
        }

        public ResourceType(string identifier, string type, PageType pageType, string href)
        {
            this.identifier = identifier;
            this.type = type;
            scormType = (pageType == PageType.Question) || (pageType == PageType.Summary)
                            ? ScormType.sco
                            : ScormType.asset;
            this.href = href;
            file.Add(new FileType(href));
        }

        private MetadataType metadataField;

        private ManifestNodeList<FileType> fileField;

        private ManifestNodeList<DependencyType> dependencyField;

        private ManifestNodeList<LangstringType> descriptionField;

        private ManifestNodeList<CatalogentryType> catalogentryField;

        private ScormType scormTypeField;

        private List<string> textField;

        private string identifierField;

        private string typeField;

        private string baseField;

        private string hrefField;

        [Description("Contains relevant information that describes the content package as a whole.")]
        [Category("Main")]
        public MetadataType metadata
        {
            get
            {
                return metadataField;
            }
            set
            {
                metadataField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
        
        [Description("scormType shall be set to sco or asset")]
        [Category("Main")]
        [XmlAttribute("scormType", Namespace = ManifestNamespaces.Adlcp)]
        public ScormType scormType
        {
            get
            {
                return scormTypeField;
            }
            set
            {
                scormTypeField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
        
        [Description("Describes an Asset in a context insensitive manner")]
        [Category("Main")]
        [XmlElement("file")]
        public ManifestNodeList<FileType> file
        {
            get
            {
                if (fileField == null)
                    fileField = new ManifestNodeList<FileType>(this);

                return fileField;
            }
            set
            {
                fileField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlElement("dependency")]
        [Description("Element used to group sets of files")]
        [Category("Main")]
        public ManifestNodeList<DependencyType> dependency
        {
            get
            {
                if (dependencyField == null)
                    dependencyField = new ManifestNodeList<DependencyType>(this);

                return dependencyField;
            }
            set
            {
                dependencyField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [Editor("string", typeof(string))]
        [XmlArrayItem("langstring", IsNullable = false)]
        [Description("Data type that represents one or more characterstrings, in which the language for which the characterstring is represented in is identified.")]
        [Category("Main")]
        public ManifestNodeList<LangstringType> description
        {
            get
            {
                return descriptionField;
            }
            set
            {
                descriptionField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlElement("catalogentry")]
        public ManifestNodeList<CatalogentryType> catalogentry
        {
            get
            {
                if (catalogentryField == null)
                    catalogentryField = new ManifestNodeList<CatalogentryType>(this);

                return catalogentryField;
            }
            set
            {
                catalogentryField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlTextAttribute]
        public List<string> Text
        {
            get
            {
                return textField;
            }
            set
            {
                textField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlAttribute("identifier", DataType = "ID")]
        public string identifier
        {
            get
            {
                return identifierField;
            }
            set
            {
                if (identifier != value && identifier.IsNotNull())
                {
                    string f = MapPath(href);
                    if (File.Exists(f))
                    {
                        string ex = Path.GetExtension(href);
                        string newfile = IdGenerator.GenerateUniqueFileName(value, ex, Path.GetDirectoryName(f)) + ex;
                        File.Move(f, MapPath(href = newfile));
                    }
                }

                identifierField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlAttribute]
        public string type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlAttribute(Form = XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string @base
        {
            get
            {
                return baseField;
            }
            set
            {
                baseField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlAttribute(DataType = "anyURI")]
        public string href
        {
            get
            {
                return hrefField;
            }
            set
            {
                hrefField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public override string ToString()
        {
            return identifier;
        }

        /// <summary>
        /// Maps specified path using base of resource and base of course location to absolute path.
        /// </summary>
        /// <param name="Path">Path to map</param>
        /// <returns>Mapped absolute path</returns>
        public string MapPath(string Path)
        {
            return Course.MapPath(@base.IsNull() ? Path : System.IO.Path.Combine(@base, Path));
        }

        #region IDisposable Members

        new public void Dispose()
        {
            foreach (FileType f in file)
            {
                File.Delete(MapPath(f.href));
            }
            //File.Delete(MapPath(href));
            base.Dispose();
        }

        #endregion
    }

    [XmlType("resources", Namespace = ManifestNamespaces.Imscp)]
    [Description("Element is a collection of references to resources.")]
    [Category("Main")]
    public class ResourcesType : AbstractManifestNode, IResourceContainer
    {
        private ManifestNodeList<ResourceType> resourceField;
        private string baseField;

        public ResourcesType()
        {
            resourceField = new ManifestNodeList<ResourceType>(this);
        }

        [XmlAttribute(Form = XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string @base
        {
            get
            {
                return baseField;
            }
            set
            {
                baseField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        #region IContainer Members

        public void RemoveChild(IManifestNode child)
        {
            switch (child.GetType().Name)
            {
                case "ResourceType":
                    {
                        var res = child as ResourceType;
                        if (Resources.Contains(res))
                        {
                            Resources.Remove(res);
                            return;
                        }
                        break;
                    }
            }
            throw new FireFlyException("Item '{0}' not found", child);
        }

        #endregion

        #region IResourceContainer Members

        public ResourceType this[string ResourceId]
        {
            get
            {
                return Resources.Find(obj => obj.identifier == ResourceId);
            }
        }

        [XmlElement("resource")]
        public ManifestNodeList<ResourceType> Resources
        {
            get
            {
                if (resourceField == null)
                    resourceField = new ManifestNodeList<ResourceType>(this);

                return resourceField;
            }
            set
            {
                resourceField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        #endregion
    }

    [XmlType("aggregationlevel", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element shall describe the functional granularity of the learning object.")]
    [Category("Main")]
    public class AggregationlevelType : AbstractManifestNode, IManifestNode
    {
        private SourceType sourceField;

        private ValueType valueField;
        
        [Description("Element describes or names the classification system. This data element may use any recognized official taxonomy or any user-defined taxonomy.")]
        [Category("Main")]
        public SourceType source
        {
            get
            {
                return sourceField;
            }
            set
            {
                sourceField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public ValueType value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("annotation", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Provides comments on the educational use of the SCORM Content Model Component and information on when and by whom the comments were created.")]
    [Category("Main")]
    public class AnnotationType : AbstractManifestNode, IManifestNode
    {
        private PersonType personField;

        private DateType dateField;

        private ManifestNodeList<LangstringType> descriptionField;

        private List<string> textField;

        public PersonType person
        {
            get
            {
                return personField;
            }
            set
            {
                personField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
        [Description("Element identifies the date of the contribution made by the entity")]
        [Category("Main")]
        public DateType date
        {
            get
            {
                return dateField;
            }
            set
            {
                dateField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
        
        [Description("Data type that represents one or more characterstrings, in which the language for which the characterstring is represented in is identified.")]
        [Category("Main")]
        [XmlArrayItem("langstring", IsNullable = false)]
        public ManifestNodeList<LangstringType> description
        {
            get
            {
                return descriptionField;
            }
            set
            {
                descriptionField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlTextAttribute]
        public List<string> Text
        {
            get
            {
                return textField;
            }
            set
            {
                textField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("catalogentry", Namespace = ManifestNamespaces.Imsmd)]
    public class CatalogentryType : AbstractManifestNode, IManifestNode
    {
        private string catalogField;

        private ManifestNodeList<LangstringType> entryField;

        private List<string> textField;

        public string catalog
        {
            get
            {
                return catalogField;
            }
            set
            {
                catalogField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlArrayItem("langstring", IsNullable = false)]
        [Description("Data type that represents one or more characterstrings, in which the language for which the characterstring is represented in is identified.")]
        [Category("Main")]
        public ManifestNodeList<LangstringType> entry
        {
            get
            {
                return entryField;
            }
            set
            {
                entryField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlTextAttribute]
        public List<string> Text
        {
            get
            {
                return textField;
            }
            set
            {
                textField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public override string ToString()
        {
            return catalog;
        }
    }

    [XmlType("centity", Namespace = ManifestNamespaces.Imsmd)]
    public class CentityType : AbstractManifestNode, IManifestNode
    {
        private string vcardField;


        public string vcard
        {
            get
            {
                return vcardField;
            }
            set
            {
                vcardField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("classification", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Describes where the SCORM Content Model Component falls within a particular classification system. Multiple Classification categories may be used to define multiple classifications")]
    [Category("Main")]
    public class ClassificationType : AbstractManifestNode, IManifestNode
    {
        private PurposeType purposeField;

        private ManifestNodeList<TaxonpathType> taxonpathField;

        private ManifestNodeList<LangstringType> descriptionField;

        private List<List<LangstringType>> keywordField;

        private List<string> textField;
        [Description("Element defines the purpose for classifying the SCORM Content Model Component")]
        [Category("Main")]
        public PurposeType purpose
        {
            get
            {
                return purposeField;
            }
            set
            {
                purposeField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
        [Description("Element describes a taxonomic path in a specific classification system. Each succeeding level is a refinement in the definition of the proceeding level.")]
        [Category("Main")]
        [XmlElement("taxonpath")]
        public ManifestNodeList<TaxonpathType> taxonpath
        {
            get
            {
                return taxonpathField;
            }
            set
            {
                taxonpathField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlArrayItem("langstring", IsNullable = false)]
        [Description("Data type that represents one or more characterstrings, in which the language for which the characterstring is represented in is identified.")]
        [Category("Main")]
        public ManifestNodeList<LangstringType> description
        {
            get
            {
                return descriptionField;
            }
            set
            {
                descriptionField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlArrayItem("langstring", typeof(LangstringType), IsNullable = false)]
        public List<List<LangstringType>> keyword
        {
            get
            {
                return keywordField;
            }
            set
            {
                keywordField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlTextAttribute]
        public List<string> Text
        {
            get
            {
                return textField;
            }
            set
            {
                textField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("context", Namespace = ManifestNamespaces.Imsmd)]
    public class ContextType : AbstractManifestNode, IManifestNode
    {
        private SourceType sourceField;

        private ValueType valueField;
        
        [Description("Element describes or names the classification system. This data element may use any recognized official taxonomy or any user-defined taxonomy.")]
        [Category("Main")]
        public SourceType source
        {
            get
            {
                return sourceField;
            }
            set
            {
                sourceField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public ValueType value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("contribute", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element shall be used to describe those entities (i.e., people, organizations) that have contributed to the state of the SCORM Content Model Component during its lifecycle (e.g., creation, edits, reviews, publications, etc)")]
    [Category("Main")]
    public class ContributeType : AbstractManifestNode, IManifestNode
    {
        private RoleType roleField;

        private ManifestNodeList<CentityType> centityField;

        private DateType dateField;

        private List<string> textField;
        [Description("Element defines the kind or type of contribution made by the contributor (identified by the Entity element.")]
        [Category("Main")]
        public RoleType role
        {
            get
            {
                return roleField;
            }
            set
            {
                roleField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlElement("centity")]
        public ManifestNodeList<CentityType> centity
        {
            get
            {
                return centityField;
            }
            set
            {
                centityField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
        [Description("Element identifies the date of the contribution made by the entity.")]
        [Category("Main")]
        public DateType date
        {
            get
            {
                return dateField;
            }
            set
            {
                dateField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlTextAttribute]
        public List<string> Text
        {
            get
            {
                return textField;
            }
            set
            {
                textField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("copyrightandotherrestrictions", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element describes whether copyright or other restrictions apply to the use of the SCORM Content Model Component.")]
    [Category("Main")]
    public class CopyrightandotherrestrictionsType : AbstractManifestNode, IManifestNode
    {
        private SourceType sourceField;

        private ValueType valueField;
        [Description("Element describes or names the classification system. This data element may use any recognized official taxonomy or any user-defined taxonomy.")]
        [Category("Main")]
        public SourceType source
        {
            get
            {
                return sourceField;
            }
            set
            {
                sourceField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public ValueType value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("cost", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element represents whether or not the SCORM Content Model Component requires some sort of payment.")]
    [Category("Main")]
    public class CostType : AbstractManifestNode, IManifestNode
    {
        private SourceType sourceField;

        private ValueType valueField;
        [Description("Element describes or names the classification system. This data element may use any recognized official taxonomy or any user-defined taxonomy.")]
        [Category("Main")]
        public SourceType source
        {
            get
            {
                return sourceField;
            }
            set
            {
                sourceField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public ValueType value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }


    [XmlType("coverage", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element shall be used to describe the time, culture, geography or region to which the SCORM Content Model Component applies.")]
    [Category("Main")]
    public class CoverageType : AbstractManifestNode, IManifestNode
    {
        private ManifestNodeList<LangstringType> langstringField;

        [XmlElement("langstring")]
        public ManifestNodeList<LangstringType> langstring
        {
            get
            {
                return langstringField;
            }
            set
            {
                langstringField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("date", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element identifies the date of the contribution made by the entity.")]
    [Category("Main")]
    public class DateType : AbstractManifestNode, IManifestNode
    {
        private string datetimeField;

        private ManifestNodeList<LangstringType> descriptionField;

        public string datetime
        {
            get
            {
                return datetimeField;
            }
            set
            {
                datetimeField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlArrayItem("langstring", IsNullable = false)]
        [Description("Data type that represents one or more characterstrings, in which the language for which the characterstring is represented in is identified.")]
        [Category("Main")]
        public ManifestNodeList<LangstringType> description
        {
            get
            {
                return descriptionField;
            }
            set
            {
                descriptionField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("description", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element shall be used to comment on how the SCORM Content Model Component is to be used.")]
    [Category("Main")]
    public class DescriptionType : AbstractManifestNode, IManifestNode
    {
        private ManifestNodeList<LangstringType> langstringField;

        [XmlElement("langstring")]
        public ManifestNodeList<LangstringType> langstring
        {
            get
            {
                return langstringField;
            }
            set
            {
                langstringField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("difficulty", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element represents how hard it is to work with or through the SCORM Content Model Component for the typical intended target audience.")]
    [Category("Main")]
    public class DifficultyType : AbstractManifestNode, IManifestNode
    {
        private SourceType sourceField;

        private ValueType valueField;

        public SourceType source
        {
            get
            {
                return sourceField;
            }
            set
            {
                sourceField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public ValueType value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("educational", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Describes the key educational or pedagogic characteristics of the SCORM Content Model Component. This category allows for the description of the educational characteristics and is typically used by teachers, managers, authors and learners.")]
    [Category("Main")]
    public class EducationalType : AbstractManifestNode, IManifestNode
    {
        private InteractivitytypeType interactivitytypeField;

        private ManifestNodeList<LearningresourcetypeType> learningresourcetypeField;

        private InteractivitylevelType interactivitylevelField;

        private SemanticdensityType semanticdensityField;

        private ManifestNodeList<IntendedenduserroleType> intendedenduserroleField;

        private ManifestNodeList<ContextType> contextField;

        private List<List<LangstringType>> typicalagerangeField;

        private DifficultyType difficultyField;

        private TypicallearningtimeType typicallearningtimeField;

        private ManifestNodeList<LangstringType> descriptionField;

        private List<string> languageField;

        private List<string> textField;
        [Description("Element represents the dominant mode of learning supported by the SCORM Content Model Component")]
        [Category("Main")]
        public InteractivitytypeType interactivitytype
        {
            get
            {
                return interactivitytypeField;
            }
            set
            {
                interactivitytypeField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
        [Description(
    "Element represents the specific kind of the SCORM Content Model Component. This element is repeatable in order to fully describe the types of resources used in the component"
    )]
        [Category("Main")]
        [XmlElement("learningresourcetype")]
        public ManifestNodeList<LearningresourcetypeType> learningresourcetype
        {
            get
            {
                return learningresourcetypeField;
            }
            set
            {
                learningresourcetypeField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [Description(
    "Represents the degree of interactivity characterizing the SCORM Content Model Component. Interactivity in this context refers to the degree to which the learner can influence the aspect or behavior of the component"
    )]
        [Category("Main")]
        public InteractivitylevelType interactivitylevel
        {
            get
            {
                return interactivitylevelField;
            }
            set
            {
                interactivitylevelField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
        [Description(
    "Represents the degree of conciseness of the SCORM Content Model Component. The semantic density of a SCORM component may be estimated in terms of its size, span or, in the case of self-timed resources such as audio or video, duration"
    )]
        [Category("Main")]
        public SemanticdensityType semanticdensity
        {
            get
            {
                return semanticdensityField;
            }
            set
            {
                semanticdensityField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
        [Description("Element represents the principal user(s) for which the SCORM Content Model Component was designed. If multiple elements are used, the most dominant role should be first")]
        [Category("Main")]
        [XmlElement("intendedenduserrole")]
        public ManifestNodeList<IntendedenduserroleType> intendedenduserrole
        {
            get
            {
                return intendedenduserroleField;
            }
            set
            {
                intendedenduserroleField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlElement("context")]
        public ManifestNodeList<ContextType> context
        {
            get
            {
                return contextField;
            }
            set
            {
                contextField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlArrayItem("langstring", typeof(LangstringType), IsNullable = false)]
        public List<List<LangstringType>> typicalagerange
        {
            get
            {
                return typicalagerangeField;
            }
            set
            {
                typicalagerangeField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
        [Description(
    "Element represents how hard it is to work with or through the SCORM Content Model Component for the typical intended target audience"
    )]
        [Category("Main")]
        public DifficultyType difficulty
        {
            get
            {
                return difficultyField;
            }
            set
            {
                difficultyField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
        [Description(
    "Element represents the approximate of typical time it takes to work with or through the SCORM Content Model Component for the typical intended target audience"
    )]
        [Category("Main")]
        public TypicallearningtimeType typicallearningtime
        {
            get
            {
                return typicallearningtimeField;
            }
            set
            {
                typicallearningtimeField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlArrayItem("langstring", IsNullable = false)]
        [Description(
    "Data type that represents one or more characterstrings, in which the language for which the characterstring is represented in is identified"
    )]
        [Category("Main")]
        public ManifestNodeList<LangstringType> description
        {
            get
            {
                return descriptionField;
            }
            set
            {
                descriptionField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlElement("language")]
        public List<string> language
        {
            get
            {
                return languageField;
            }
            set
            {
                languageField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlTextAttribute]
        public List<string> Text
        {
            get
            {
                return textField;
            }
            set
            {
                textField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("entry", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element represents the value of the identifier within the identification or cataloging scheme (see <catalog> element) that designates or identifies the target SCORM Content Model Component.")]
    [Category("Main")]
    public class EntryType : AbstractManifestNode, IManifestNode
    {
        private ManifestNodeList<LangstringType> langstringField;

        [XmlElement("langstring")]
        public ManifestNodeList<LangstringType> langstring
        {
            get
            {
                return langstringField;
            }
            set
            {
                langstringField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("general", Namespace = ManifestNamespaces.Imsmd)]
    [Description("General information that describes the resource as a whole. The resource in this case is the particular SCORM Content Model Component (Asset, SCO, Activity or Content Organization) being described. This general information is sometimes viewed as key information in that it is important for describing the particular component.")]
    [Category("Main")]
    public class GeneralType : AbstractManifestNode, IManifestNode
    {
        private string identifierField;

        private ManifestNodeList<CatalogentryType> catalogentryField;

        private List<string> languageField;

        private List<List<LangstringType>> descriptionField;

        private List<List<LangstringType>> keywordField;

        private List<List<LangstringType>> coverageField;

        private StructureType structureField;

        private AggregationlevelType aggregationlevelField;

        private List<string> textField;

        public string identifier
        {
            get
            {
                return identifierField;
            }
            set
            {
                identifierField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public string title
        {
            get
            {
                return identifierField;
            }
            set
            {
                identifierField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlElement("catalogentry")]
        public ManifestNodeList<CatalogentryType> catalogentry
        {
            get
            {
                return catalogentryField;
            }
            set
            {
                catalogentryField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlElement("language")]
        public List<string> language
        {
            get
            {
                return languageField;
            }
            set
            {
                languageField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlArrayItem("langstring", typeof(LangstringType), IsNullable = false)]
        [Description("Data type that represents one or more characterstrings, in which the language for which the characterstring is represented in is identified.")]
        [Category("Main")]
        public List<List<LangstringType>> description
        {
            get
            {
                return descriptionField;
            }
            set
            {
                descriptionField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlArrayItem("langstring", typeof(LangstringType), IsNullable = false)]
        public List<List<LangstringType>> keyword
        {
            get
            {
                return keywordField;
            }
            set
            {
                keywordField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlArrayItem("langstring", typeof(LangstringType), IsNullable = false)]
        public List<List<LangstringType>> coverage
        {
            get
            {
                return coverageField;
            }
            set
            {
                coverageField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
        
        [Description("Element shall describe the underlying organizational structure of the SCORM Content Model Component.")]
        [Category("Main")]
        public StructureType structure
        {
            get
            {
                return structureField;
            }
            set
            {
                structureField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
        
        [Description("Element shall describe the functional granularity of the learning object.")]
        [Category("Main")]
        public AggregationlevelType aggregationlevel
        {
            get
            {
                return aggregationlevelField;
            }
            set
            {
                aggregationlevelField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlTextAttribute]
        public List<string> Text
        {
            get
            {
                return textField;
            }
            set
            {
                textField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("intendedenduserrole", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element represents the principal user(s) for which the SCORM Content Model Component was designed. If multiple elements are used, the most dominant role should be first.")]
    [Category("Main")]
    public class IntendedenduserroleType : AbstractManifestNode, IManifestNode
    {
        private SourceType sourceField;

        private ValueType valueField;

        public SourceType source
        {
            get
            {
                return sourceField;
            }
            set
            {
                sourceField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public ValueType value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("interactivitylevel", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Represents the degree of interactivity characterizing the SCORM Content Model Component. Interactivity in this context refers to the degree to which the learner can influence the aspect or behavior of the component.")]
    [Category("Main")]
    public class InteractivitylevelType : AbstractManifestNode, IManifestNode
    {
        private SourceType sourceField;

        private ValueType valueField;

        [Description("Element describes or names the classification system. This data element may use any recognized official taxonomy or any user-defined taxonomy.")]
        [Category("Main")]
        public SourceType source
        {
            get
            {
                return sourceField;
            }
            set
            {
                sourceField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public ValueType value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("interactivitytype", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element represents the dominant mode of learning supported by the SCORM Content Model Component.")]
    [Category("Main")]
    public class InteractivitytypeType : AbstractManifestNode, IManifestNode
    {
        private SourceType sourceField;

        private ValueType valueField;
        [Description("Element describes or names the classification system. This data element may use any recognized official taxonomy or any user-defined taxonomy.")]
        [Category("Main")]
        public SourceType source
        {
            get
            {
                return sourceField;
            }
            set
            {
                sourceField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public ValueType value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("keyword", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element contains keywords and phrases descriptive of the SCORM Content Model Component relative to the stated Purpose (<purpose>) of this specific classification, such as discipline, idea, skill level, educational objective, etc.")]
    [Category("Main")]
    public class KeywordType : AbstractManifestNode, IManifestNode
    {
        private ManifestNodeList<LangstringType> langstringField;

        [XmlElement("langstring")]
        public ManifestNodeList<LangstringType> langstring
        {
            get
            {
                return langstringField;
            }
            set
            {
                langstringField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("kind", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element describes the nature of the relationship between the SCORM Content Model Component and the target component identified by the Resource.")]
    [Category("Main")]
    public class KindType : AbstractManifestNode, IManifestNode
    {
        private SourceType sourceField;

        private ValueType valueField;
        
        [Description("Element describes or names the classification system. This data element may use any recognized official taxonomy or any user-defined taxonomy.")]
        [Category("Main")]
        public SourceType source
        {
            get
            {
                return sourceField;
            }
            set
            {
                sourceField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public ValueType value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("langstring", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Data type that represents one or more characterstrings, in which the language for which the characterstring is represented in is identified.")]
    [Category("Main")]
    public class LangstringType : AbstractManifestNode, IManifestNode
    {
        private string langField;

        private string valueField;

        [XmlAttribute(Form = XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string lang
        {
            get
            {
                return langField;
            }
            set
            {
                langField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlTextAttribute]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("learningresourcetype", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element represents the specific kind of the SCORM Content Model Component. This element is repeatable in order to fully describe the types of resources used in the component.")]
    [Category("Main")]
    public class LearningresourcetypeType : AbstractManifestNode, IManifestNode
    {
        private SourceType sourceField;

        private ValueType valueField;

        public SourceType source
        {
            get
            {
                return sourceField;
            }
            set
            {
                sourceField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public ValueType value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("lifecycle", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Related to the history and current state of the SCORM Content Model Component and those who have affected the component during its evolution.")]
    [Category("Main")]
    public class LifecycleType : AbstractManifestNode, IManifestNode
    {
        private ManifestNodeList<LangstringType> versionField;

        private StatusType statusField;

        private ManifestNodeList<ContributeType> contributeField;

        private List<string> textField;

        [XmlArrayItem("langstring", IsNullable = false)]
        public ManifestNodeList<LangstringType> version
        {
            get
            {
                return versionField;
            }
            set
            {
                versionField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
        
        [Description("Element shall describe the completion status or condition of the SCORM Content Model Component. A component may have several statuses during its lifetime (draft, final, etc…)")]
        [Category("Main")]
        public StatusType status
        {
            get
            {
                return statusField;
            }
            set
            {
                statusField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlElement("contribute")]
        public ManifestNodeList<ContributeType> contribute
        {
            get
            {
                return contributeField;
            }
            set
            {
                contributeField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlTextAttribute]
        public List<string> Text
        {
            get
            {
                return textField;
            }
            set
            {
                textField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("location", Namespace = ManifestNamespaces.Adlcp)]
    [Description("Element is a string that specifies the location of the SCORM Content Model Component described by the meta-data.")]
    [Category("Main")]
    public class LocationType : AbstractManifestNode, IManifestNode
    {
        private LocationTypeType typeField;

        private string valueField;

        public LocationType()
        {
            typeField = LocationTypeType.URI;
        }

        [XmlAttribute]
        [DefaultValue(LocationTypeType.URI)]
        public LocationTypeType type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlTextAttribute]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType(AnonymousType = true)]
    public enum LocationTypeType
    {
        URI,
        TEXT,
    }

    [XmlType("lom", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element contains important elements that SCORM requires to describe all of the SCORM Content Model Components.")]
    [Category("Main")]
    public class LomType : AbstractManifestNode, IManifestNode
    {
        private GeneralType generalField;

        private LifecycleType lifecycleField;

        private MetametadataType metametadataField;

        private TechnicalType technicalField;

        private EducationalType educationalField;

        private RightsType rightsField;

        private ManifestNodeList<RelationType> relationField;

        private ManifestNodeList<AnnotationType> annotationField;

        private ManifestNodeList<ClassificationType> classificationField;
        
        [Description("General information that describes the resource as a whole. The resource in this case is the particular SCORM Content Model Component (Asset, SCO, Activity or Content Organization) being described. This general information is sometimes viewed as key information in that it is important for describing the particular component.")]
        [Category("Main")]
        public GeneralType general
        {
            get
            {
                return generalField;
            }
            set
            {
                generalField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
        
        [Description("Related to the history and current state of the SCORM Content Model Component and those who have affected the component during its evolution.")]
        [Category("Main")]
        public LifecycleType lifecycle
        {
            get
            {
                return lifecycleField;
            }
            set
            {
                lifecycleField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
        
        [Description("Provides elements that describe the meta-data record itself and not the SCORM Content Model Component the record is describing. This category describes how the meta-data instance itself can be identified, who created the meta-data instance, how, when and with what references.")]
        [Category("Main")]
        public MetametadataType metametadata
        {
            get
            {
                return metametadataField;
            }
            set
            {
                metametadataField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
        
        [Description("Describes all of the technical characteristics and requirements of the SCORM Content Model Component.")]
        [Category("Main")]
        public TechnicalType technical
        {
            get
            {
                return technicalField;
            }
            set
            {
                technicalField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
        
        [Description("Describes the key educational or pedagogic characteristics of the SCORM Content Model Component. This category allows for the description of the educational characteristics and is typically used by teachers, managers, authors and learners.")]
        [Category("Main")]
        public EducationalType educational
        {
            get
            {
                return educationalField;
            }
            set
            {
                educationalField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
        
        [Description("Describes the intellectual property rights and conditions of use for the SCORM Content Model Component.")]
        [Category("Main")]
        public RightsType rights
        {
            get
            {
                return rightsField;
            }
            set
            {
                rightsField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlElement("relation")]
        [Description("Defines the relationship between the SCORM Content Model Component and other components, if any.")]
        [Category("Main")]
        public ManifestNodeList<RelationType> relation
        {
            get
            {
                return relationField;
            }
            set
            {
                relationField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
        
        [Description("Provides comments on the educational use of the SCORM Content Model Component and information on when and by whom the comments were created.")]
        [Category("Main")]
        [XmlElement("annotation")]
        public ManifestNodeList<AnnotationType> annotation
        {
            get
            {
                return annotationField;
            }
            set
            {
                annotationField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlElement("classification")]
        [Description("Describes where the SCORM Content Model Component falls within a particular classification system. Multiple Classification categories may be used to define multiple classifications.")]
        [Category("Main")]
        public ManifestNodeList<ClassificationType> classification
        {
            get
            {
                return classificationField;
            }
            set
            {
                classificationField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("metametadata", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Provides elements that describe the meta-data record itself and not the SCORM Content Model Component the record is describing. This category describes how the meta-data instance itself can be identified, who created the meta-data instance, how, when and with what references.")]
    [Category("Main")]
    public class MetametadataType : AbstractManifestNode, IManifestNode
    {
        private string identifierField;

        private ManifestNodeList<CatalogentryType> catalogentryField;

        private ManifestNodeList<ContributeType> contributeField;

        private List<string> metadataschemeField;

        private string languageField;

        private List<string> textField;

        public string identifier
        {
            get
            {
                return identifierField;
            }
            set
            {
                identifierField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlElement("catalogentry")]
        public ManifestNodeList<CatalogentryType> catalogentry
        {
            get
            {
                return catalogentryField;
            }
            set
            {
                catalogentryField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlElement("contribute")]
        [Description("Element shall be used to describe those entities (i.e., people, organizations) that have contributed to the state of the SCORM Content Model Component during its lifecycle (e.g., creation, edits, reviews, publications, etc)")]
        [Category("Main")]
        public ManifestNodeList<ContributeType> contribute
        {
            get
            {
                return contributeField;
            }
            set
            {
                contributeField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlElement("metadatascheme")]
        public List<string> metadatascheme
        {
            get
            {
                return metadataschemeField;
            }
            set
            {
                metadataschemeField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public string language
        {
            get
            {
                return languageField;
            }
            set
            {
                languageField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlTextAttribute]
        public List<string> Text
        {
            get
            {
                return textField;
            }
            set
            {
                textField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("name", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element represents the required technology to use the SCORM Content Model Component. The value used for the Name element depends on the value identified by the Value element.")]
    [Category("Main")]
    public class NameType : AbstractManifestNode, IManifestNode
    {
        private SourceType sourceField;

        private ValueType valueField;
        [Description("Element describes or names the classification system. This data element may use any recognized official taxonomy or any user-defined taxonomy.")]
        [Category("Main")]
        public SourceType source
        {
            get
            {
                return sourceField;
            }
            set
            {
                sourceField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public ValueType value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("purpose", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element defines the purpose for classifying the SCORM Content Model Component.")]
    [Category("Main")]
    public class PurposeType : AbstractManifestNode, IManifestNode
    {
        private SourceType sourceField;

        private ValueType valueField;
        [Description("Element describes or names the classification system. This data element may use any recognized official taxonomy or any user-defined taxonomy.")]
        [Category("Main")]
        public SourceType source
        {
            get
            {
                return sourceField;
            }
            set
            {
                sourceField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public ValueType value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("relation", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Defines the relationship between the SCORM Content Model Component and other components, if any.")]
    [Category("Main")]
    public class RelationType : AbstractManifestNode, IManifestNode
    {
        private KindType kindField;

        private ResourceType resourceField;

        private List<string> textField;
        
        [Description("Element describes the nature of the relationship between the SCORM Content Model Component and the target component identified by the Resource.")]
        [Category("Main")]
        public KindType kind
        {
            get
            {
                return kindField;
            }
            set
            {
                kindField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
        
        [Description("Element is a reference to a resource. There are two primary types of resources defined within SCORM: Asset and SCO.")]
        [Category("Main")]
        public ResourceType resource
        {
            get
            {
                return resourceField;
            }
            set
            {
                resourceField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlTextAttribute]
        public List<string> Text
        {
            get
            {
                return textField;
            }
            set
            {
                textField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("requirement", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element expresses the technical capabilities necessary for using the SCORM Content Model Component.")]
    [Category("Main")]
    public class RequirementType : AbstractManifestNode, IManifestNode
    {
        private TypeType typeField;

        private NameType nameField;

        private string minimumversionField;

        private string maximumversionField;

        private List<string> textField;
        
        [Description("Element represents the technology required to use the SCORM Content Model Component (e.g., hardware, software, network, etc.)")]
        [Category("Main")]
        public TypeType type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
        
        [Description("Element represents the required technology to use the SCORM Content Model Component. The value used for the Name element depends on the value identified by the Value element")]
        [Category("Main")]
        public NameType name
        {
            get
            {
                return nameField;
            }
            set
            {
                nameField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public string minimumversion
        {
            get
            {
                return minimumversionField;
            }
            set
            {
                minimumversionField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public string maximumversion
        {
            get
            {
                return maximumversionField;
            }
            set
            {
                maximumversionField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlTextAttribute]
        public List<string> Text
        {
            get
            {
                return textField;
            }
            set
            {
                textField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("rights", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Describes the intellectual property rights and conditions of use for the SCORM Content Model Component.")]
    [Category("Main")]
    public class RightsType : AbstractManifestNode, IManifestNode
    {
        private CostType costField;

        private CopyrightandotherrestrictionsType copyrightandotherrestrictionsField;

        private ManifestNodeList<LangstringType> descriptionField;

        private List<string> textField;

        public CostType cost
        {
            get
            {
                return costField;
            }
            set
            {
                costField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
        
        [Description("Element describes whether copyright or other restrictions apply to the use of the SCORM Content Model Component.")]
        [Category("Main")]
        public CopyrightandotherrestrictionsType copyrightandotherrestrictions
        {
            get
            {
                return copyrightandotherrestrictionsField;
            }
            set
            {
                copyrightandotherrestrictionsField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlArrayItem("langstring", IsNullable = false)]
        public ManifestNodeList<LangstringType> description
        {
            get
            {
                return descriptionField;
            }
            set
            {
                descriptionField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
        
        [XmlTextAttribute]
        public List<string> Text
        {
            get
            {
                return textField;
            }
            set
            {
                textField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("role", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element defines the kind or type of contribution made by the contributor (identified by the Entity element).")]
    [Category("Main")]
    public class RoleType : AbstractManifestNode, IManifestNode
    {
        private SourceType sourceField;

        private ValueType valueField;
        [Description("Element describes or names the classification system. This data element may use any recognized official taxonomy or any user-defined taxonomy.")]
        [Category("Main")]
        public SourceType source
        {
            get
            {
                return sourceField;
            }
            set
            {
                sourceField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public ValueType value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("semanticdensity", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Represents the degree of conciseness of the SCORM Content Model Component. The semantic density of a SCORM component may be estimated in terms of its size, span or, in the case of self-timed resources such as audio or video, duration.")]
    [Category("Main")]
    public class SemanticdensityType : AbstractManifestNode, IManifestNode
    {
        private SourceType sourceField;

        private ValueType valueField;
        [Description("Element describes or names the classification system. This data element may use any recognized official taxonomy or any user-defined taxonomy.")]
        [Category("Main")]
        public SourceType source
        {
            get
            {
                return sourceField;
            }
            set
            {
                sourceField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public ValueType value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("source", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element describes or names the classification system. This data element may use any recognized official taxonomy or any user-defined taxonomy.")]
    [Category("Main")]
    public class SourceType : AbstractManifestNode, IManifestNode
    {
        private LangstringType langstringField;

        public LangstringType langstring
        {
            get
            {
                return langstringField;
            }
            set
            {
                langstringField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("status", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element shall describe the completion status or condition of the SCORM Content Model Component. A component may have several statuses during its lifetime (draft, final, etc…)")]
    [Category("Main")]
    public class StatusType : AbstractManifestNode, IManifestNode
    {
        private SourceType sourceField;

        private ValueType valueField;
        
        [Description("Element describes or names the classification system. This data element may use any recognized official taxonomy or any user-defined taxonomy.")]
        [Category("Main")]
        public SourceType source
        {
            get
            {
                return sourceField;
            }
            set
            {
                sourceField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public ValueType value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("structure", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element shall describe the underlying organizational structure of the SCORM Content Model Component")]
    [Category("Main")]
    public class StructureType : AbstractManifestNode, IManifestNode
    {
        private SourceType sourceField;

        private ValueType valueField;
        
        [Description("Element describes or names the classification system. This data element may use any recognized official taxonomy or any user-defined taxonomy.")]
        [Category("Main")]
        public SourceType source
        {
            get
            {
                return sourceField;
            }
            set
            {
                sourceField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public ValueType value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("taxon", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element describes a particular term within a taxonomy. A taxon is a node that has a defined label or term. A taxon may also have an alphanumeric designation or identifier for standardized reference.")]
    [Category("Main")]
    public class TaxonType : AbstractManifestNode, IManifestNode
    {
        private string idField;

        private ManifestNodeList<LangstringType> entryField;

        private TaxonType taxonField;

        public string id
        {
            get
            {
                return idField;
            }
            set
            {
                idField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlArrayItem("langstring", IsNullable = false)]
        public ManifestNodeList<LangstringType> entry
        {
            get
            {
                return entryField;
            }
            set
            {
                entryField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
        
        [Description("Element describes a particular term within a taxonomy. A taxon is a node that has a defined label or term. A taxon may also have an alphanumeric designation or identifier for standardized reference.")]
        [Category("Main")]
        public TaxonType taxon
        {
            get
            {
                return taxonField;
            }
            set
            {
                taxonField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("taxonpath", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element describes a taxonomic path in a specific classification system. Each succeeding level is a refinement in the definition of the proceeding level.")]
    [Category("Main")]
    public class TaxonpathType : AbstractManifestNode, IManifestNode
    {
        private SourceType sourceField;

        private TaxonType taxonField;

        public SourceType source
        {
            get
            {
                return sourceField;
            }
            set
            {
                sourceField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
        
        [Description("Element describes a particular term within a taxonomy. A taxon is a node that has a defined label or term. A taxon may also have an alphanumeric designation or identifier for standardized reference.")]
        [Category("Main")]
        public TaxonType taxon
        {
            get
            {
                return taxonField;
            }
            set
            {
                taxonField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("technical", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Describes all of the technical characteristics and requirements of the SCORM Content Model Component.")]
    [Category("Main")]
    public class TechnicalType : AbstractManifestNode, IManifestNode
    {
        private List<string> formatField;

        private int sizeField;

        private bool sizeFieldSpecified;

        private ManifestNodeList<LocationType> locationField;

        private ManifestNodeList<RequirementType> requirementField;

        private ManifestNodeList<LangstringType> installationremarksField;

        private ManifestNodeList<LangstringType> otherplatformrequirementsField;

        private DurationType durationField;

        private List<string> textField;

        [XmlElement("format")]
        public List<string> format
        {
            get
            {
                return formatField;
            }
            set
            {
                formatField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public int size
        {
            get
            {
                return sizeField;
            }
            set
            {
                sizeField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlIgnoreAttribute]
        public bool sizeSpecified
        {
            get
            {
                return sizeFieldSpecified;
            }
            set
            {
                sizeFieldSpecified = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlElement("location")]
        [Description("Element is a string that specifies the location of the SCORM Content Model Component described by the meta-data.")]
        [Category("Main")]
        public ManifestNodeList<LocationType> location
        {
            get
            {
                return locationField;
            }
            set
            {
                locationField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlElement("requirement")]
        [Description("Element expresses the technical capabilities necessary for using the SCORM Content Model Component.")]
        [Category("Main")]
        public ManifestNodeList<RequirementType> requirement
        {
            get
            {
                return requirementField;
            }
            set
            {
                requirementField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlArrayItem("langstring", IsNullable = false)]
        public ManifestNodeList<LangstringType> installationremarks
        {
            get
            {
                return installationremarksField;
            }
            set
            {
                installationremarksField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlArrayItem("langstring", IsNullable = false)]
        public ManifestNodeList<LangstringType> otherplatformrequirements
        {
            get
            {
                return otherplatformrequirementsField;
            }
            set
            {
                otherplatformrequirementsField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
        
        [Description("Element represents the time a continuous SCORM Content Model Component takes when played at intended speed. This element is useful for sounds, movies, simulations and the like.")]
        [Category("Main")]
        public DurationType duration
        {
            get
            {
                return durationField;
            }
            set
            {
                durationField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlTextAttribute]
        public List<string> Text
        {
            get
            {
                return textField;
            }
            set
            {
                textField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("type", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element represents the technology required to use the SCORM Content Model Component (e.g., hardware, software, network, etc.)")]
    [Category("Main")]
    public class TypeType : AbstractManifestNode, IManifestNode
    {
        private SourceType sourceField;

        private ValueType valueField;
        
        [Description("Element describes or names the classification system. This data element may use any recognized official taxonomy or any user-defined taxonomy.")]
        [Category("Main")]
        public SourceType source
        {
            get
            {
                return sourceField;
            }
            set
            {
                sourceField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public ValueType value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("typicalagerange", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element represents the age of the typical end user. This element shall refer to the developmental age, if that would be different from the chronological age.")]
    [Category("Main")]
    public class TypicalagerangeType : AbstractManifestNode, IManifestNode
    {
        private ManifestNodeList<LangstringType> langstringField;

        [XmlElement("langstring")]
        public ManifestNodeList<LangstringType> langstring
        {
            get
            {
                return langstringField;
            }
            set
            {
                langstringField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("typicallearningtime", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element represents the approximate of typical time it takes to work with or through the SCORM Content Model Component for the typical intended target audience.")]
    [Category("Main")]
    public class TypicallearningtimeType : AbstractManifestNode, IManifestNode
    {
        private string datetimeField;

        private ManifestNodeList<LangstringType> descriptionField;

        public string datetime
        {
            get
            {
                return datetimeField;
            }
            set
            {
                datetimeField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlArrayItem("langstring", IsNullable = false)]
        public ManifestNodeList<LangstringType> description
        {
            get
            {
                return descriptionField;
            }
            set
            {
                descriptionField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("value", Namespace = ManifestNamespaces.Imsmd)]
    public class ValueType : AbstractManifestNode, IManifestNode
    {
        private LangstringType langstringField;

        public LangstringType langstring
        {
            get
            {
                return langstringField;
            }
            set
            {
                langstringField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("person", Namespace = ManifestNamespaces.Imsmd)]
    public class PersonType : AbstractManifestNode, IManifestNode
    {
        private string vcardField;

        public string vcard
        {
            get
            {
                return vcardField;
            }
            set
            {
                vcardField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("version", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element shall describe the edition of the SCORM Content Model Component. A component may have several versions or editions during its lifetime. The <version> element allows for the description of the version of the component.")]
    [Category("Main")]
    public class VersionType : AbstractManifestNode, IManifestNode
    {
        private ManifestNodeList<LangstringType> langstringField;

        [XmlElement("langstring")]
        public ManifestNodeList<LangstringType> langstring
        {
            get
            {
                return langstringField;
            }
            set
            {
                langstringField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("installationremarks", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element is used to represent any specific instructions on how to install the SCORM Content Model Component.")]
    [Category("Main")]
    public class InstallationremarksType : AbstractManifestNode, IManifestNode
    {
        private ManifestNodeList<LangstringType> langstringField;

        [XmlElement("langstring")]
        public ManifestNodeList<LangstringType> langstring
        {
            get
            {
                return langstringField;
            }
            set
            {
                langstringField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("otherplatformrequirements", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element is used to represent information about other software and hardware requirements of the SCORM Content Model Component. This element should be used to describe requirements that cannot be represented or expressed with the other Technical elements.")]
    [Category("Main")]
    public class OtherplatformrequirementsType : AbstractManifestNode, IManifestNode
    {
        private ManifestNodeList<LangstringType> langstringField;

        [XmlElement("langstring")]
        public ManifestNodeList<LangstringType> langstring
        {
            get
            {
                return langstringField;
            }
            set
            {
                langstringField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("duration", Namespace = ManifestNamespaces.Imsmd)]
    [Description("Element represents the time a continuous SCORM Content Model Component takes when played at intended speed. This element is useful for sounds, movies, simulations and the like.")]
    [Category("Main")]
    public class DurationType : AbstractManifestNode, IManifestNode
    {
        private string datetimeField;

        private ManifestNodeList<LangstringType> descriptionField;

        public string datetime
        {
            get
            {
                return datetimeField;
            }
            set
            {
                datetimeField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlArrayItem("langstring", IsNullable = false)]
        public ManifestNodeList<LangstringType> description
        {
            get
            {
                return descriptionField;
            }
            set
            {
                descriptionField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    [XmlType("scormType", Namespace = ManifestNamespaces.Adlcp)]
    [Description("scormType shall be set to sco or asset.")]
    [Category("Main")]
    public enum ScormType
    {
        [XmlEnumAttribute("sco")]
        sco,

        [XmlEnumAttribute("asset")]
        asset
    }

    [XmlType("timeLimitAction", Namespace = ManifestNamespaces.Adlcp)]
    [Description("Element defines the action that should be taken when the maximum time allowed in the current attempt of the activity is exceeded. All time tracking and time limit actions are controlled by the SCO.")]
    [Category("Main")]
    public enum TimeLimitActionType
    {
        [XmlEnumAttribute("exit,message")]
        exitmessage,

        [XmlEnumAttribute("exit,no message")]
        exitnomessage,

        [XmlEnumAttribute("continue,message")]
        continuemessage,

        [XmlEnumAttribute("continue,no message")]
        continuenomessage,
    }

    [XmlType("presentation", Namespace = ManifestNamespaces.Adlnav)]
    [Description("Element is a container element that encapsulates presentation information for a given learning activity.")]
    [Category("Main")]
    public class PresentationType : AbstractManifestNode, IManifestNode
    {
        private List<HideLMSUIType> navigationInterfaceField;
        
        [Description("Element indicates that the LMS should not provide user interface devices that enable the learner to trigger specific events.")]
        [Category("Main")]
        [XmlArrayItem("hideLMSUI", IsNullable = false)]
        public List<HideLMSUIType> navigationInterface
        {
            get
            {
                if (navigationInterfaceField == null)
                    navigationInterfaceField = new List<HideLMSUIType>();

                return navigationInterfaceField;
            }
            set
            {
                navigationInterfaceField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }
    
    [XmlType("hideLMSUI", Namespace = ManifestNamespaces.Adlnav)]
    [Description("Element indicates that the LMS should not provide user interface devices that enable the learner to trigger specific events.")]
    [Category("Main")]
    public enum HideLMSUIType
    {
        abandon,
        @continue,
        exit,
        previous,
    }

    [XmlType("navigationInterface", Namespace = ManifestNamespaces.Adlnav)]
    [Description("Element is a container element that encapsulates navigation interface presentation requirements for a given learning activity.")]
    [Category("Main")]
    public class NavigationInterfaceType : AbstractManifestNode, IManifestNode
    {
        private List<HideLMSUIType> hideLMSUIField;
        
        [Description("Element indicates that the LMS should not provide user interface devices that enable the learner to trigger specific events.")]
        [Category("Main")]
        [XmlElement("hideLMSUI")]
        public List<HideLMSUIType> hideLMSUI
        {
            get
            {
                return hideLMSUIField;
            }
            set
            {
                hideLMSUIField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }
    }

    #endregion

    internal class MetadataConverter : ExpandableObjectConverter
    {
        private const string None = "(none)";
        private const string CannotConvertToTypeMessage = "Cannot convert '{0}' to type '{1}'";

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(MetadataType))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            if (destinationType.Name == "String" && value is MetadataType)
            {
                var mt = value as MetadataType;

                if (mt.schema != null && mt.schema.Trim() == string.Empty)
                {
                    mt.schema = null;
                }
                if (mt.schemaversion != null && mt.schemaversion.Trim() == string.Empty)
                {
                    mt.schemaversion = null;
                }

                if (mt.schema == null && mt.schemaversion == null)
                {
                    return None;
                }
                return string.Format("{0}, {1}", mt.schema, mt.schemaversion);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                var sValue = value as string;
                try
                {
                    if (sValue == None || sValue.Trim(' ', ',') == string.Empty)
                    {
                        return null;
                    }
                    var metadataElems = sValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    string schema = null;
                    string schemaVersion = null;

                    if (metadataElems.Length > 0)
                    {
                        metadataElems[0] = metadataElems[0].Trim();
                        if (metadataElems[0] != string.Empty)
                        {
                            schema = metadataElems[0];
                        }
                    }

                    if (metadataElems.Length > 1)
                    {
                        metadataElems[1] = metadataElems[1].Trim();
                        if (metadataElems[1] != string.Empty)
                        {
                            schemaVersion = metadataElems[1];
                        }
                    }

                    if (schema != null || schemaVersion != null)
                    {
                        return new MetadataType(schema, schemaVersion);
                    }
                    return null;
                }
                catch (Exception e)
                {
                    throw new FireFlyException(CannotConvertToTypeMessage, e, sValue, "MetadataType");
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}