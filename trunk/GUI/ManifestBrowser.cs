namespace FireFly.CourseEditor.GUI
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reflection;
    using System.Windows.Forms;
    using Course.Manifest;
    using WeifenLuo.WinFormsUI.Docking;
    using IContainer = Course.Manifest.IContainer;

    public partial class ManifestBrowser : EditorWindowBase
    {
        #region GUI event handler

        private void miAdd_DropDownOpening(object sender, EventArgs e)
        {
        }

        private void cmsManifestNode_Opening(object sender, CancelEventArgs e)
        {
            // See miAdd_DropDownOpening event handler.
            miAdd.Enabled = true;

            TreeNode curNode = tvManifest.SelectedNode;
            bool deleteEnabled = false;
            if (curNode.Parent != null)
                deleteEnabled = curNode.Parent.Tag is IContainer;
            miDelete.Enabled = deleteEnabled;
        }

        private void miAddOrganization_Click(object sender, EventArgs e)
        {
            IOrganizationContainer c = (IOrganizationContainer) tvManifest.SelectedNode.Tag;
            OrganizationType t = new OrganizationType();

            c.Organizations.Add(t);

            Forms.PropertyEditor.Show(t);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddItem_Click(object sender, EventArgs e)
        {
            IItemContainer c = (IItemContainer) tvManifest.SelectedNode.Tag;

            ItemType i = new ItemType();

            c.SubItems.Add(i);

            Forms.PropertyEditor.Show(i);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddResource_Click(object sender, EventArgs e)
        {
            IResourceContainer c = (IResourceContainer) tvManifest.SelectedNode.Tag;

            ResourceType r = new ResourceType();

            c.Resources.Add(r);

            Forms.PropertyEditor.Show(c);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddDependency_Click(object sender, EventArgs e)
        {
            ResourceType c = (ResourceType) tvManifest.SelectedNode.Tag;
            DependencyType d = new DependencyType();

            c.dependency.Add(d);

            Forms.PropertyEditor.Show(d);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddFile_Click(object sender, EventArgs e)
        {
            ResourceType c = (ResourceType) tvManifest.SelectedNode.Tag;

            FileType f = new FileType();

            c.file.Add(f);

            Forms.PropertyEditor.Show(f);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddCatalogEntry_Click(object sender, EventArgs e)
        {
            ResourceType c = (ResourceType) tvManifest.SelectedNode.Tag;

            CatalogentryType ce = new CatalogentryType();

            c.catalogentry.Add(ce);

            Forms.PropertyEditor.Show(ce);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddAuxiliaryResource_Click(object sender, EventArgs e)
        {
            SequencingType c = (SequencingType) tvManifest.SelectedNode.Tag;

            AuxiliaryResourceType a = new AuxiliaryResourceType();

            c.auxiliaryResources.Add(a);

            Forms.PropertyEditor.Show(a);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddObjective_Click(object sender, EventArgs e)
        {
            ObjectivesType c = (ObjectivesType) tvManifest.SelectedNode.Tag;

            ObjectivesTypeObjective o = new ObjectivesTypeObjective();

            c.objective.Add(o);

            Forms.PropertyEditor.Show(o);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddDescription_Click(object sender, EventArgs e)
        {
            ResourceType c = (ResourceType) tvManifest.SelectedNode.Tag;

            LangstringType l = new LangstringType();

            c.description.Parent = c;

            c.description.Add(l);

            Forms.PropertyEditor.Show(l);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddText_Click(object sender, EventArgs e)
        {
            ResourceType c = (ResourceType) tvManifest.SelectedNode.Tag;

            LangstringType t = new LangstringType();

            c.description.Parent = c;

            c.description.Add(t);

            Forms.PropertyEditor.Show(t);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddMapInfo_Click(object sender, EventArgs e)
        {
            ObjectiveType c = (ObjectiveType) tvManifest.SelectedNode.Tag;

            ObjectiveTypeMapInfo o = new ObjectiveTypeMapInfo();

            c.mapInfo.Add(o);

            Forms.PropertyEditor.Show(o);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddRuleCondition_Click(object sender, EventArgs e)
        {
            SequencingRuleTypeRuleConditions c = (SequencingRuleTypeRuleConditions) tvManifest.SelectedNode.Tag;

            SequencingRuleTypeRuleConditionsRuleCondition r = new SequencingRuleTypeRuleConditionsRuleCondition();

            c.ruleCondition.Parent = c;

            c.ruleCondition.Add(r);

            Forms.PropertyEditor.Show(r);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddPreConditionRule_Click(object sender, EventArgs e)
        {
            SequencingRulesType c = (SequencingRulesType) tvManifest.SelectedNode.Tag;

            PreConditionRuleType p = new PreConditionRuleType();

            c.preConditionRule.Parent = c;

            c.preConditionRule.Add(p);

            Forms.PropertyEditor.Show(p);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddPostConditionRule_Click(object sender, EventArgs e)
        {
            SequencingRulesType c = (SequencingRulesType) tvManifest.SelectedNode.Tag;

            PostConditionRuleType p = new PostConditionRuleType();

            c.postConditionRule.Parent = c;

            c.postConditionRule.Add(p);

            Forms.PropertyEditor.Show(p);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddExitConditionRule_Click(object sender, EventArgs e)
        {
            SequencingRulesType c = (SequencingRulesType) tvManifest.SelectedNode.Tag;

            ExitConditionRuleType ec = new ExitConditionRuleType();

            c.exitConditionRule.Parent = c;

            c.exitConditionRule.Add(ec);

            Forms.PropertyEditor.Show(ec);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddNavigationInterface_Click(object sender, EventArgs e)
        {
            PresentationType c = (PresentationType) tvManifest.SelectedNode.Tag;

            HideLMSUIType n = new HideLMSUIType();

            c.navigationInterface.Add(n);

            Forms.PropertyEditor.Show(n);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddRollupRule_Click(object sender, EventArgs e)
        {
            RollupRulesType c = (RollupRulesType) tvManifest.SelectedNode.Tag;

            RollupRuleType r = new RollupRuleType();

            c.rollupRule.Parent = c;

            c.rollupRule.Add(r);

            Forms.PropertyEditor.Show(r);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddRollupCondition_Click(object sender, EventArgs e)
        {
            RollupRuleTypeRollupConditions c = (RollupRuleTypeRollupConditions) tvManifest.SelectedNode.Tag;

            RollupRuleTypeRollupConditionsRollupCondition r = new RollupRuleTypeRollupConditionsRollupCondition();

            c.rollupCondition.Parent = c;

            c.rollupCondition.Add(r);

            Forms.PropertyEditor.Show(r);
            tvManifest.SelectedNode.Expand();
        }

        private void miProperties_Click(object sender, EventArgs e)
        {
            TreeNode sn = tvManifest.SelectedNode;

            if (sn != null)
                Forms.PropertyEditor.Show(sn.Tag);
        }

        private void tvManifest_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            tvManifest.SelectedNode = e.Node;
            Forms.PropertyEditor.CurrentObject = e.Node.Tag;
        }

        private void miDelete_Click(object sender, EventArgs e)
        {
            Debug.Assert(tvManifest.SelectedNode != null, "Any node selected");
            Debug.Assert(tvManifest.SelectedNode.Parent != null, "Selected node has not parent");

            IContainer c =
                tvManifest.SelectedNode.Parent.Tag as IContainer;

            Debug.Assert(c != null, "Parent of the selected object is not support Manifest.IContainer");

            c.RemoveChild(tvManifest.SelectedNode.Tag as IManifestNode);
            if (tvManifest.SelectedNode.Parent != null)
            {
                tvManifest.SelectedNode = tvManifest.SelectedNode.Parent;
            }
        }

        private void miExpandAll_Click(object sender, EventArgs e)
        {
            tvManifest.SelectedNode.ExpandAll();
        }

        private void miAddManifest_Click(object sender, EventArgs e)
        {
            ManifestType c = (ManifestType) tvManifest.SelectedNode.Tag;

            ManifestType m = new ManifestType(c.Identifier + "_submanifest");

            if (c.manifest.Count == 0)
            {
                c.manifest = new ManifestNodeList<ManifestType>(c.manifest);
            }

            c.manifest.Add(m);

            Forms.PropertyEditor.Show(m);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddPresentation_Click(object sender, EventArgs e)
        {
            ItemType c = (ItemType) tvManifest.SelectedNode.Tag;

            Debug.Assert(c.Presentation == null, "Presentation already exists");

            c.Presentation = new PresentationType();

            Forms.PropertyEditor.Show(c.Presentation);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddSequencing_Click(object sender, EventArgs e)
        {
            ItemType c = (ItemType) tvManifest.SelectedNode.Tag;

            Debug.Assert(c.Sequencing == null, "Sequencing already exists");

            c.Sequencing = new SequencingType();

            Forms.PropertyEditor.Show(c.Sequencing);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddDeliveryControls_Click(object sender, EventArgs e)
        {
            SequencingType c = (SequencingType) tvManifest.SelectedNode.Tag;

            Debug.Assert(c.deliveryControls == null, "Delivery Controls already exists");

            c.deliveryControls = new DeliveryControlsType();

            Forms.PropertyEditor.Show(c.deliveryControls);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddLimitConditions_Click(object sender, EventArgs e)
        {
            SequencingType c = (SequencingType) tvManifest.SelectedNode.Tag;

            Debug.Assert(c.limitConditions == null, "Limit Conditions already exists");

            c.limitConditions = new LimitConditionsType();

            Forms.PropertyEditor.Show(c.limitConditions);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddRandomizationControl_Click(object sender, EventArgs e)
        {
            SequencingType c = (SequencingType) tvManifest.SelectedNode.Tag;

            Debug.Assert(c.randomizationControls == null, "Randomization Controls already exists");

            c.randomizationControls = new RandomizationType();

            Forms.PropertyEditor.Show(c.randomizationControls);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddRollupRules_Click(object sender, EventArgs e)
        {
            SequencingType c = (SequencingType) tvManifest.SelectedNode.Tag;

            Debug.Assert(c.rollupRules == null, "Rollup Rules already exists");

            c.rollupRules = new RollupRulesType();

            Forms.PropertyEditor.Show(c.rollupRules);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddSequencingRules_Click(object sender, EventArgs e)
        {
            SequencingType c = (SequencingType) tvManifest.SelectedNode.Tag;

            Debug.Assert(c.sequencingRules == null, "Sequencing Rules already exists");

            c.sequencingRules = new SequencingRulesType();

            Forms.PropertyEditor.Show(c.sequencingRules);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddObjectives_Click(object sender, EventArgs e)
        {
            SequencingType c = (SequencingType) tvManifest.SelectedNode.Tag;

            Debug.Assert(c.objectives == null, "Objectives already exists");

            c.objectives = new ObjectivesType();

            Forms.PropertyEditor.Show(c.objectives);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddControlMode_Click(object sender, EventArgs e)
        {
            SequencingType c = (SequencingType) tvManifest.SelectedNode.Tag;

            Debug.Assert(c.controlMode == null, "Control Mode already exists");

            c.controlMode = new ControlModeType();

            Forms.PropertyEditor.Show(c.controlMode);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddRuleConditions_Click(object sender, EventArgs e)
        {
            SequencingRuleType c = (SequencingRuleType) tvManifest.SelectedNode.Tag;

            Debug.Assert(c.ruleConditions == null, "Rule Condition already exists");

            c.ruleConditions = new SequencingRuleTypeRuleConditions();

            Forms.PropertyEditor.Show(c.ruleConditions);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddRuleAction_Click(object sender, EventArgs e)
        {
            if (tvManifest.SelectedNode.Tag is PreConditionRuleType)
            {
                PreConditionRuleType c = (PreConditionRuleType) tvManifest.SelectedNode.Tag;

                Debug.Assert(c.ruleAction == null, "Rule Action already exists");

                c.ruleAction = new PreConditionRuleTypeRuleAction();

                Forms.PropertyEditor.Show(c.ruleAction);
                tvManifest.SelectedNode.Expand();
            }
            if (tvManifest.SelectedNode.Tag is PostConditionRuleType)
            {
                PostConditionRuleType c = (PostConditionRuleType) tvManifest.SelectedNode.Tag;

                Debug.Assert(c.ruleAction == null, "Rule Action already exists");

                c.ruleAction = new PostConditionRuleTypeRuleAction();

                Forms.PropertyEditor.Show(c.ruleAction);
                tvManifest.SelectedNode.Expand();
            }
            if (tvManifest.SelectedNode.Tag is ExitConditionRuleType)
            {
                ExitConditionRuleType c = (ExitConditionRuleType) tvManifest.SelectedNode.Tag;

                Debug.Assert(c.ruleAction == null, "Rule Action already exists");

                c.ruleAction = new ExitConditionRuleTypeRuleAction();

                Forms.PropertyEditor.Show(c.ruleAction);
                tvManifest.SelectedNode.Expand();
            }
        }

        private void miAddRollupAction_Click(object sender, EventArgs e)
        {
            RollupRuleType c = (RollupRuleType) tvManifest.SelectedNode.Tag;

            Debug.Assert(c.rollupAction == null, "Rollup Action already exists");

            c.rollupAction = new RollupRuleTypeRollupAction();

            Forms.PropertyEditor.Show(c.rollupAction);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddRollupConditions_Click(object sender, EventArgs e)
        {
            RollupRuleType c = (RollupRuleType) tvManifest.SelectedNode.Tag;

            Debug.Assert(c.rollupConditions == null, "Rollup Conditions already exists");

            c.rollupConditions = new RollupRuleTypeRollupConditions();

            Forms.PropertyEditor.Show(c.rollupConditions);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddPrimaryObjective_Click(object sender, EventArgs e)
        {
            ObjectivesType c = (ObjectivesType) tvManifest.SelectedNode.Tag;

            Debug.Assert(c.primaryObjective == null, "Primary objective already exists");

            c.primaryObjective = new ObjectivesTypePrimaryObjective();

            Forms.PropertyEditor.Show(c.primaryObjective);
            tvManifest.SelectedNode.Expand();
        }


        private void miAddRollupConsiderations_Click(object sender, EventArgs e)
        {
            SequencingType c = (SequencingType)tvManifest.SelectedNode.Tag;

            Debug.Assert(c.rollupConsiderations == null, "Rollup Considerations already exist");

            c.rollupConsiderations = new RollupConsiderationsType();

            Forms.PropertyEditor.Show(c.rollupConsiderations);
            tvManifest.SelectedNode.Expand();
        }

        private void miAddConstrainedChoiceConsiderations_Click(object sender, EventArgs e)
        {
            SequencingType c = (SequencingType)tvManifest.SelectedNode.Tag;

            Debug.Assert(c.constrainedChoiceConsiderations == null, "Constrained Choice Considerations already exist");

            c.constrainedChoiceConsiderations = new ConstrainedChoiceConsiderationsType();

            Forms.PropertyEditor.Show(c.constrainedChoiceConsiderations);
            tvManifest.SelectedNode.Expand();
        }

        //!!!!!!!!!!!!!!
        private void miAddExtensionObjectives_Click(object sender, EventArgs e)
        {
            //////// !!!!!!!!!!!! 
            ///Still have to be implemented
        }
 
        private void miAddCollectionSequencing_Click(object sender, EventArgs e)
        {
            SequencingCollection c = (SequencingCollection)tvManifest.SelectedNode.Tag;

            Debug.Assert(c.sequencingCollection != null, "Empty sequencing Collection.");

            c.sequencingCollection.Add(new SequencingType());

            Forms.PropertyEditor.Show(c.sequencingCollection);
            tvManifest.SelectedNode.Expand();
        }
        #endregion

        public ManifestBrowser(DockPanel parentDockPanel)
            : base(parentDockPanel)
        {
            InitializeComponent();
            Show(DockingPanel);
            tvManifest.AfterSelect += (sender, e) =>
            {
                bool en = false;
                Type pm = typeof (PossibilityManager);
                object[] ps = new object[] {tvManifest.SelectedNode.Tag};
                foreach (ToolStripMenuItem mi in miAdd.DropDownItems)
                {
                    MethodInfo m = pm.GetMethod
                        ("Can" + mi.Name.Substring(2), BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod);
                    Debug.Assert(m != null);

                    if (mi.Visible = (bool) m.Invoke(null, ps))
                    {
                        en = true;
                    }
                }
                miAdd.Enabled = en;
            };
        }

       
    }
}