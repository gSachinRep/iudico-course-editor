using System;
using FireFly.CourseEditor.Common;

namespace FireFly.CourseEditor.Course.Manifest
{
    /// <summary>
    /// This class represents GUI support. Can be used to determine can or not some action be made for specified node.
    /// Methods of this class contains no xml-documentation becase theirs names are selfdocumented, and write so many documentation can take a lot of time.
    /// </summary>
    public static class PossibilityManager
    {
        public static bool CanAddOrganization(object currentNode)
        {
            return currentNode is OrganizationsType;
        }

        public static bool CanAddItem(object currentNode)
        {
            return currentNode is IItemContainer;
        }

        public static bool CanAddResource(object currentNode)
        {
            return currentNode is ResourcesType;
        }

        public static bool CanAddDependency(object currentNode)
        {
            return currentNode is ResourceType;
        }

        public static bool CanAddFile(object currentNode)
        {
            return currentNode is ResourceType;
        }

        public static bool CanAddCatalogEntry(object currentNode)
        {
            return currentNode is ResourceType;
        }

        public static bool CanAddDescription(object currentNode)
        {
            return currentNode is ResourceType;
        }

        public static bool CanAddText(object currentNode)
        {
            return currentNode is ResourceType;
        }

        public static bool CanAddAuxiliaryResource(object currentNode)
        {
            return currentNode is SequencingType;
        }

        public static bool CanAddObjective(object currentNode)
        {
            return currentNode is ObjectivesType; 
        }

        public static bool CanAddMapInfo(object currentNode)
        {
            return currentNode is ObjectiveType;
        }

        public static bool CanAddNavigationInterface(object currentNode)
        {
            return currentNode is PresentationType;
        }

        public static bool CanAddExitConditionRule(object currentNode)
        {
            return currentNode is SequencingRulesType;
        }

        public static bool CanAddPostConditionRule(object currentNode)
        {
            return currentNode is SequencingRulesType;
        }

        public static bool CanAddPreConditionRule(object currentNode)
        {
            return currentNode is SequencingRulesType;
        }

        public static bool CanAddRuleCondition(object currentNode)
        {
            return currentNode is SequencingRuleTypeRuleConditions;
        }

        public static bool CanAddRollupRule(object currentNode)
        {
            return currentNode is RollupRulesType;
        }

        public static bool CanAddRollupCondition(object currentNode)
        {
            return currentNode is RollupRuleTypeRollupConditions;
        }

        public static bool CanAddManifest(object currentNode)
        {
            if (currentNode == null)
            {
                throw new ArgumentNullException("currentNode");
            }
            return currentNode is ManifestType;
        }

        public static bool CanAddPresentation(object currentNode)
        {
            return currentNode is ItemType && ((ItemType) currentNode).Presentation == null;
        }

        public static bool CanAddSequencing(object currentNode)
        {
            return currentNode is ItemType && ((ItemType) currentNode).Sequencing == null;
        }

        public static bool CanAddDeliveryControls(object currentNode)
        {
            return currentNode is SequencingType && ((SequencingType) currentNode).deliveryControls == null;
        }

        public static bool CanAddLimitConditions(object currentNode)
        {
            return currentNode is SequencingType && ((SequencingType) currentNode).limitConditions == null;
        }

        public static bool CanAddRandomizationControl(object currentNode)
        {
            return currentNode is SequencingType && ((SequencingType) currentNode).randomizationControls == null;
        }

        public static bool CanAddRollupRules(object currentNode)
        {
            return currentNode is SequencingType && ((SequencingType) currentNode).rollupRules == null;
        }

        public static bool CanAddSequencingRules(object currentNode)
        {
            return currentNode is SequencingType && ((SequencingType) currentNode).sequencingRules == null;
        }

        public static bool CanAddObjectives(object currentNode)
        {
            return currentNode is SequencingType && ((SequencingType) currentNode).objectives == null;
        }

        public static bool CanAddControlMode(object currentNode)
        {
            return currentNode is SequencingType && ((SequencingType) currentNode).controlMode == null;
        }

        public static bool CanAddRuleAction(object currentNode)
        {
            if (currentNode is PreConditionRuleType && ((PreConditionRuleType) currentNode).ruleAction == null)
            {
                return true;
            }
            if (currentNode is PostConditionRuleType && ((PostConditionRuleType) currentNode).ruleAction == null)
            {
                return true;
            }
            if (currentNode is ExitConditionRuleType && ((ExitConditionRuleType) currentNode).ruleAction == null)
            {
                return true;
            }
            return false;
        }

        public static bool CanAddRuleConditions(object currentNode)
        {
            return currentNode is SequencingRuleType && ((SequencingRuleType) currentNode).ruleConditions == null;
        }

        public static bool CanAddRollupConditions(object currentNode)
        {
            return currentNode is RollupRuleType && ((RollupRuleType) currentNode).rollupConditions == null;
        }

        public static bool CanAddRollupAction(object currentNode)
        {
            return currentNode is RollupRuleType && ((RollupRuleType) currentNode).rollupAction == null;
        }

        public static bool CanAddPrimaryObjective(object currentNode)
        {
            return currentNode is ObjectivesType && ((ObjectivesType) currentNode).primaryObjective == null;
        }

        public static bool CanAddSummaryPage(object currentNode)
        {
            var page = currentNode as OrganizationType;
            return page != null && !page.Items.Exists(i => i.PageType == PageType.Summary) && page.Items.Exists(i => i.PageType == PageType.Question);
        }

        public static bool CanAddPage(object currentNode)
        {
            var it = currentNode as IItemContainer;
            var i = it as ItemType;
            if (i != null && i.PageType != PageType.Chapter && i.PageType != PageType.ControlChapter)
            {
                return false;
            }
            if (it != null)
            {
                return !it.SubItems.Exists(x => (x.PageType == PageType.Chapter || x.PageType == PageType.ControlChapter));
            }
            return false;
        }

        public static bool CanAddChapter(object currentNode)
        {
            var it = currentNode as IItemContainer;
            var i = it as ItemType;
            if (i != null && i.PageType != PageType.Chapter && i.PageType != PageType.ControlChapter)
            {
                return false;
            }
            if (it != null)
            {
                return !it.SubItems.Exists(x => (x.PageType != PageType.Chapter && x.PageType != PageType.ControlChapter));
            }
            return false;
        }

        public static bool CanRemove(object currentNode)
        {
            if (currentNode is ItemType)
            {
                return true;
            }
            return currentNode is OrganizationType && Course.Manifest.organizations.Organizations.Count > 1;
        }

        public static bool CanAddRollupConsiderations(object currentNode)
        {
            return currentNode is SequencingType && ((SequencingType)currentNode).rollupConsiderations == null;
        }

        public static bool CanAddConstrainedChoiceConsiderations(object currentNode)
        {
            return currentNode is SequencingType && ((SequencingType)currentNode).constrainedChoiceConsiderations == null;
        }

        //!!!!!!!!!!!!!! Should be implemented
        public static bool CanAddExtensionObjectives(object currentNode)
        {
            //Zaglushka
            return false;
        }

        public static bool CanAddCollectionSequencing(object currentNode)
        {            
            return (currentNode is SequencingCollection) && ((SequencingCollection)currentNode).sequencingCollection != null;
        }
    }
}
