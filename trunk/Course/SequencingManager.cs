using System;
using System.Collections.Generic;
using System.Text;
using FireFly.CourseEditor.Course.Manifest;

namespace FireFly.CourseEditor.Course.Manifest
{
    /// <summary>
    /// Implements simple sequencing strategy managing during designing Course.
    /// </summary>
    public static class SequencingManager
    {
        #region Create Sequencing

        /// <summary>
        /// Main entry point for creating Sequencing object.
        /// </summary>
        /// <param name="pageType">PageType value to customize default sequencing srategy for.</param>
        /// <returns>SequencingType value with default sequencing strategy elements for current page type.</returns>
        public static SequencingType CreateNewSequencing(PageType pageType)
        {
            SequencingType result = new SequencingType();

            switch (pageType)
            { 
                case PageType.Chapter:
                    CustomizeChapter(ref result);
                    break;
                case PageType.ControlChapter:
                    CustomizeControlChapter(ref result);
                    break;
                case PageType.Question:
                    CustomizeQuestionPage(ref result);
                    break;
                case PageType.Theory:
                    CustomizeTheoryPage(ref result);
                    break;
                default:
                    
                    break;
            }

            return result;
        }

        /// <summary>
        /// Creates simple Sequencing for organization.
        /// </summary>
        /// <returns>SequencingType value with default sequencing strategy elements for organization.</returns>
        public static SequencingType CreateOrganizationDefaultSequencing()
        {
            SequencingType result = new SequencingType();

            result.controlMode = new ControlModeType();
            result.controlMode.choice = true;
            result.controlMode.flow = true;

            return result;
        }
        
        /// <summary>
        /// Customizes sequencing for simple chapter.
        /// </summary>
        /// <param name="sequencing">SequencingType value represents object to customize.</param>
        public static void CustomizeChapter(ref SequencingType sequencing)
        {
            if (sequencing.controlMode == null)
            {
                sequencing.controlMode = new ControlModeType();
            }
            sequencing.controlMode.flow = true;
            sequencing.controlMode.choice = true;
        }

        /// <summary>
        /// Customizes sequencing for control chapter.
        /// </summary>
        /// <param name="sequencing">SequencingType value represents object to customize.</param>
        public static void CustomizeControlChapter(ref SequencingType sequencing)
        {
            if (sequencing.controlMode == null)
            { 
                sequencing.controlMode = new ControlModeType();
            }            
            sequencing.controlMode.flow = true;
            sequencing.controlMode.forwardOnly = true;
            sequencing.controlMode.choice = false;
            sequencing.controlMode.choiceExit = false;

            if (sequencing.limitConditions == null)
            {
                sequencing.limitConditions = new LimitConditionsType();
            }
            sequencing.limitConditions.attemptLimit = "1";
        }

        /// <summary>
        /// Customizes sequencing for question page.
        /// </summary>
        /// <param name="sequencing">SequencingType value represents object to customize.</param>
        public static void CustomizeQuestionPage(ref SequencingType sequencing)
        {
            if (sequencing.deliveryControls == null)
            { 
                sequencing.deliveryControls = new DeliveryControlsType();
            }            
            sequencing.deliveryControls.completionSetByContent = true;
            sequencing.deliveryControls.tracked = true;
            
            if (sequencing.limitConditions == null)
            {
                sequencing.limitConditions = new LimitConditionsType();
                sequencing.limitConditions.attemptLimit = "1";
            }
        }

        /// <summary>
        /// Customizes sequencing for theory page.
        /// </summary>
        /// <param name="sequencing">SequencingType value represents object to customize.</param>
        public static void CustomizeTheoryPage(ref SequencingType sequencing)
        {
            if (sequencing.deliveryControls == null)
            {
                sequencing.deliveryControls = new DeliveryControlsType();
            }
            sequencing.deliveryControls.tracked = false;
        }

        /// <summary>
        /// Creates PreConditionRule with simple parameters.
        /// </summary>
        /// <param name="condition">SequencingRuleConditionType enumerable value represents Rule Condition.</param>
        /// <param name="action">PreConditionRuleActionType enumerable value represents PreCondition Rule Action.</param>
        /// <returns>PreConditionRuleType value with appropriate parameters.</returns>
        public static PreConditionRuleType CreateSimplePreConditionRule(SequencingRuleConditionType condition, PreConditionRuleActionType action)
        {  
            PreConditionRuleType preConditionRule = new PreConditionRuleType();
            preConditionRule.ruleConditions = new SequencingRuleTypeRuleConditions();
            preConditionRule.ruleConditions.ruleCondition = new ManifestNodeList<SequencingRuleTypeRuleConditionsRuleCondition>(preConditionRule.ruleConditions);
            preConditionRule.ruleConditions.ruleCondition.Add(new SequencingRuleTypeRuleConditionsRuleCondition());
            preConditionRule.ruleConditions.ruleCondition[0].condition = condition;
            preConditionRule.ruleAction = new PreConditionRuleTypeRuleAction();
            preConditionRule.ruleAction.action = action;
            return preConditionRule;
        }

        #endregion
    }
}
