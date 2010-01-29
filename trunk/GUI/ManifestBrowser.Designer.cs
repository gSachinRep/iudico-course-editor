using FireFly.CourseEditor.GUI;

namespace FireFly.CourseEditor.GUI
{
    partial class ManifestBrowser
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManifestBrowser));
            this.ilNodes = new System.Windows.Forms.ImageList(this.components);
            this.cmsManifestNode = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddOrganization = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddResource = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddDependency = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddCatalogEntry = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddDescription = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddText = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddAuxiliaryResource = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddObjective = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddNavigationInterface = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddMapInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddExitConditionRule = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddPostConditionRule = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddPreConditionRule = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddRuleCondition = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddRollupRule = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddRollupCondition = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddManifest = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddPresentation = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddSequencing = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddDeliveryControls = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddLimitConditions = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddRandomizationControl = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddRollupRules = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddPrimaryObjective = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddSequencingRules = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddRollupAction = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddRollupConditions = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddObjectives = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddControlMode = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddRuleAction = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddRuleConditions = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddRollupConsiderations = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddConstrainedChoiceConsiderations = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddExtensionObjectives = new System.Windows.Forms.ToolStripMenuItem();
            this.miDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.miProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.miExpandAll = new System.Windows.Forms.ToolStripMenuItem();
            this.tvManifest = new FireFly.CourseEditor.GUI.FFTreeView();
            this.miAddCollectionSequencing = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsManifestNode.SuspendLayout();
            this.SuspendLayout();
            // 
            // ilNodes
            // 
            this.ilNodes.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilNodes.ImageStream")));
            this.ilNodes.TransparentColor = System.Drawing.Color.Fuchsia;
            this.ilNodes.Images.SetKeyName(0, "resourceType");
            this.ilNodes.Images.SetKeyName(1, "manifestType");
            this.ilNodes.Images.SetKeyName(2, "dependencyType");
            this.ilNodes.Images.SetKeyName(3, "fileType");
            this.ilNodes.Images.SetKeyName(4, "organizationType");
            this.ilNodes.Images.SetKeyName(5, "list");
            this.ilNodes.Images.SetKeyName(6, "itemType");
            this.ilNodes.Images.SetKeyName(7, "sequencingType");
            this.ilNodes.Images.SetKeyName(8, "objectiveType");
            this.ilNodes.Images.SetKeyName(9, "objectivesTypePrimaryObjective");
            this.ilNodes.Images.SetKeyName(10, "sequencingRulesType");
            this.ilNodes.Images.SetKeyName(11, "Chapter");
            this.ilNodes.Images.SetKeyName(12, "Question");
            this.ilNodes.Images.SetKeyName(13, "PageWithError");
            this.ilNodes.Images.SetKeyName(14, "Summary");
            this.ilNodes.Images.SetKeyName(15, "Theory");
            // 
            // cmsManifestNode
            // 
            this.cmsManifestNode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAdd,
            this.miDelete,
            this.miProperties,
            this.miExpandAll});
            this.cmsManifestNode.Name = "cmsManifest";
            this.cmsManifestNode.Size = new System.Drawing.Size(153, 114);
            this.cmsManifestNode.Opening += new System.ComponentModel.CancelEventHandler(this.cmsManifestNode_Opening);
            // 
            // miAdd
            // 
            this.miAdd.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAddOrganization,
            this.miAddItem,
            this.miAddResource,
            this.miAddDependency,
            this.miAddFile,
            this.miAddCatalogEntry,
            this.miAddDescription,
            this.miAddText,
            this.miAddAuxiliaryResource,
            this.miAddObjective,
            this.miAddNavigationInterface,
            this.miAddMapInfo,
            this.miAddExitConditionRule,
            this.miAddPostConditionRule,
            this.miAddPreConditionRule,
            this.miAddRuleCondition,
            this.miAddRollupRule,
            this.miAddRollupCondition,
            this.miAddManifest,
            this.miAddPresentation,
            this.miAddSequencing,
            this.miAddDeliveryControls,
            this.miAddLimitConditions,
            this.miAddRandomizationControl,
            this.miAddRollupRules,
            this.miAddPrimaryObjective,
            this.miAddSequencingRules,
            this.miAddRollupAction,
            this.miAddRollupConditions,
            this.miAddObjectives,
            this.miAddControlMode,
            this.miAddRuleAction,
            this.miAddRuleConditions,
            this.miAddRollupConsiderations,
            this.miAddConstrainedChoiceConsiderations,
            this.miAddExtensionObjectives,
            this.miAddCollectionSequencing});
            this.miAdd.Image = global::FireFly.CourseEditor.Properties.Resources.add_to_list16;
            this.miAdd.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miAdd.Name = "miAdd";
            this.miAdd.Size = new System.Drawing.Size(152, 22);
            this.miAdd.Text = "&Add";
            this.miAdd.DropDownOpening += new System.EventHandler(this.miAdd_DropDownOpening);
            // 
            // miAddOrganization
            // 
            this.miAddOrganization.Image = global::FireFly.CourseEditor.Properties.Resources.Chapter;
            this.miAddOrganization.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miAddOrganization.Name = "miAddOrganization";
            this.miAddOrganization.Size = new System.Drawing.Size(239, 22);
            this.miAddOrganization.Text = "&Organization";
            this.miAddOrganization.Click += new System.EventHandler(this.miAddOrganization_Click);
            // 
            // miAddItem
            // 
            this.miAddItem.Image = global::FireFly.CourseEditor.Properties.Resources.Theory;
            this.miAddItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miAddItem.Name = "miAddItem";
            this.miAddItem.Size = new System.Drawing.Size(239, 22);
            this.miAddItem.Text = "&Item";
            this.miAddItem.Click += new System.EventHandler(this.miAddItem_Click);
            // 
            // miAddResource
            // 
            this.miAddResource.Image = global::FireFly.CourseEditor.Properties.Resources.R_purple_16;
            this.miAddResource.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miAddResource.Name = "miAddResource";
            this.miAddResource.Size = new System.Drawing.Size(239, 22);
            this.miAddResource.Text = "&Resource";
            this.miAddResource.Click += new System.EventHandler(this.miAddResource_Click);
            // 
            // miAddDependency
            // 
            this.miAddDependency.Image = global::FireFly.CourseEditor.Properties.Resources.D_blue_16;
            this.miAddDependency.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miAddDependency.Name = "miAddDependency";
            this.miAddDependency.Size = new System.Drawing.Size(239, 22);
            this.miAddDependency.Text = "&Dependency";
            this.miAddDependency.Click += new System.EventHandler(this.miAddDependency_Click);
            // 
            // miAddFile
            // 
            this.miAddFile.Image = global::FireFly.CourseEditor.Properties.Resources.F_green_16;
            this.miAddFile.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miAddFile.Name = "miAddFile";
            this.miAddFile.Size = new System.Drawing.Size(239, 22);
            this.miAddFile.Text = "&File";
            this.miAddFile.Click += new System.EventHandler(this.miAddFile_Click);
            // 
            // miAddCatalogEntry
            // 
            this.miAddCatalogEntry.Enabled = false;
            this.miAddCatalogEntry.Name = "miAddCatalogEntry";
            this.miAddCatalogEntry.Size = new System.Drawing.Size(239, 22);
            this.miAddCatalogEntry.Text = "&CatalogEntry";
            this.miAddCatalogEntry.Click += new System.EventHandler(this.miAddCatalogEntry_Click);
            // 
            // miAddDescription
            // 
            this.miAddDescription.Name = "miAddDescription";
            this.miAddDescription.Size = new System.Drawing.Size(239, 22);
            this.miAddDescription.Text = "&Description";
            this.miAddDescription.Click += new System.EventHandler(this.miAddDescription_Click);
            // 
            // miAddText
            // 
            this.miAddText.Name = "miAddText";
            this.miAddText.Size = new System.Drawing.Size(239, 22);
            this.miAddText.Text = "&Text";
            this.miAddText.Click += new System.EventHandler(this.miAddText_Click);
            // 
            // miAddAuxiliaryResource
            // 
            this.miAddAuxiliaryResource.Name = "miAddAuxiliaryResource";
            this.miAddAuxiliaryResource.Size = new System.Drawing.Size(239, 22);
            this.miAddAuxiliaryResource.Text = "AuxiliaryResource";
            this.miAddAuxiliaryResource.Click += new System.EventHandler(this.miAddAuxiliaryResource_Click);
            // 
            // miAddObjective
            // 
            this.miAddObjective.Name = "miAddObjective";
            this.miAddObjective.Size = new System.Drawing.Size(239, 22);
            this.miAddObjective.Text = "Objective";
            this.miAddObjective.Click += new System.EventHandler(this.miAddObjective_Click);
            // 
            // miAddNavigationInterface
            // 
            this.miAddNavigationInterface.Name = "miAddNavigationInterface";
            this.miAddNavigationInterface.Size = new System.Drawing.Size(239, 22);
            this.miAddNavigationInterface.Text = "NavigationInterface";
            this.miAddNavigationInterface.Click += new System.EventHandler(this.miAddNavigationInterface_Click);
            // 
            // miAddMapInfo
            // 
            this.miAddMapInfo.Name = "miAddMapInfo";
            this.miAddMapInfo.Size = new System.Drawing.Size(239, 22);
            this.miAddMapInfo.Text = "mapInfo";
            this.miAddMapInfo.Click += new System.EventHandler(this.miAddMapInfo_Click);
            // 
            // miAddExitConditionRule
            // 
            this.miAddExitConditionRule.Name = "miAddExitConditionRule";
            this.miAddExitConditionRule.Size = new System.Drawing.Size(239, 22);
            this.miAddExitConditionRule.Text = "exitConditionRule";
            this.miAddExitConditionRule.Click += new System.EventHandler(this.miAddExitConditionRule_Click);
            // 
            // miAddPostConditionRule
            // 
            this.miAddPostConditionRule.Name = "miAddPostConditionRule";
            this.miAddPostConditionRule.Size = new System.Drawing.Size(239, 22);
            this.miAddPostConditionRule.Text = "postConditionRule";
            this.miAddPostConditionRule.Click += new System.EventHandler(this.miAddPostConditionRule_Click);
            // 
            // miAddPreConditionRule
            // 
            this.miAddPreConditionRule.Name = "miAddPreConditionRule";
            this.miAddPreConditionRule.Size = new System.Drawing.Size(239, 22);
            this.miAddPreConditionRule.Text = "preConditionRule";
            this.miAddPreConditionRule.Click += new System.EventHandler(this.miAddPreConditionRule_Click);
            // 
            // miAddRuleCondition
            // 
            this.miAddRuleCondition.Name = "miAddRuleCondition";
            this.miAddRuleCondition.Size = new System.Drawing.Size(239, 22);
            this.miAddRuleCondition.Text = "ruleCondition";
            this.miAddRuleCondition.Click += new System.EventHandler(this.miAddRuleCondition_Click);
            // 
            // miAddRollupRule
            // 
            this.miAddRollupRule.Name = "miAddRollupRule";
            this.miAddRollupRule.Size = new System.Drawing.Size(239, 22);
            this.miAddRollupRule.Text = "rollupRule";
            this.miAddRollupRule.Click += new System.EventHandler(this.miAddRollupRule_Click);
            // 
            // miAddRollupCondition
            // 
            this.miAddRollupCondition.Name = "miAddRollupCondition";
            this.miAddRollupCondition.Size = new System.Drawing.Size(239, 22);
            this.miAddRollupCondition.Text = "rollupCondition";
            this.miAddRollupCondition.Click += new System.EventHandler(this.miAddRollupCondition_Click);
            // 
            // miAddManifest
            // 
            this.miAddManifest.Name = "miAddManifest";
            this.miAddManifest.Size = new System.Drawing.Size(239, 22);
            this.miAddManifest.Text = "Manifest";
            this.miAddManifest.Click += new System.EventHandler(this.miAddManifest_Click);
            // 
            // miAddPresentation
            // 
            this.miAddPresentation.Name = "miAddPresentation";
            this.miAddPresentation.Size = new System.Drawing.Size(239, 22);
            this.miAddPresentation.Text = "Presentation";
            this.miAddPresentation.Click += new System.EventHandler(this.miAddPresentation_Click);
            // 
            // miAddSequencing
            // 
            this.miAddSequencing.Name = "miAddSequencing";
            this.miAddSequencing.Size = new System.Drawing.Size(239, 22);
            this.miAddSequencing.Text = "Sequencing";
            this.miAddSequencing.Click += new System.EventHandler(this.miAddSequencing_Click);
            // 
            // miAddDeliveryControls
            // 
            this.miAddDeliveryControls.Name = "miAddDeliveryControls";
            this.miAddDeliveryControls.Size = new System.Drawing.Size(239, 22);
            this.miAddDeliveryControls.Text = "deliveryControls";
            this.miAddDeliveryControls.Click += new System.EventHandler(this.miAddDeliveryControls_Click);
            // 
            // miAddLimitConditions
            // 
            this.miAddLimitConditions.Name = "miAddLimitConditions";
            this.miAddLimitConditions.Size = new System.Drawing.Size(239, 22);
            this.miAddLimitConditions.Text = "limitConditions";
            this.miAddLimitConditions.Click += new System.EventHandler(this.miAddLimitConditions_Click);
            // 
            // miAddRandomizationControl
            // 
            this.miAddRandomizationControl.Name = "miAddRandomizationControl";
            this.miAddRandomizationControl.Size = new System.Drawing.Size(239, 22);
            this.miAddRandomizationControl.Text = "randomizationControl";
            this.miAddRandomizationControl.Click += new System.EventHandler(this.miAddRandomizationControl_Click);
            // 
            // miAddRollupRules
            // 
            this.miAddRollupRules.Name = "miAddRollupRules";
            this.miAddRollupRules.Size = new System.Drawing.Size(239, 22);
            this.miAddRollupRules.Text = "rollupRules";
            this.miAddRollupRules.Click += new System.EventHandler(this.miAddRollupRules_Click);
            // 
            // miAddPrimaryObjective
            // 
            this.miAddPrimaryObjective.Name = "miAddPrimaryObjective";
            this.miAddPrimaryObjective.Size = new System.Drawing.Size(239, 22);
            this.miAddPrimaryObjective.Text = "primaryObjective";
            this.miAddPrimaryObjective.Click += new System.EventHandler(this.miAddPrimaryObjective_Click);
            // 
            // miAddSequencingRules
            // 
            this.miAddSequencingRules.Name = "miAddSequencingRules";
            this.miAddSequencingRules.Size = new System.Drawing.Size(239, 22);
            this.miAddSequencingRules.Text = "SequencingRules";
            this.miAddSequencingRules.Click += new System.EventHandler(this.miAddSequencingRules_Click);
            // 
            // miAddRollupAction
            // 
            this.miAddRollupAction.Name = "miAddRollupAction";
            this.miAddRollupAction.Size = new System.Drawing.Size(239, 22);
            this.miAddRollupAction.Text = "rollupAction";
            this.miAddRollupAction.Click += new System.EventHandler(this.miAddRollupAction_Click);
            // 
            // miAddRollupConditions
            // 
            this.miAddRollupConditions.Name = "miAddRollupConditions";
            this.miAddRollupConditions.Size = new System.Drawing.Size(239, 22);
            this.miAddRollupConditions.Text = "rollupConditions";
            this.miAddRollupConditions.Click += new System.EventHandler(this.miAddRollupConditions_Click);
            // 
            // miAddObjectives
            // 
            this.miAddObjectives.Name = "miAddObjectives";
            this.miAddObjectives.Size = new System.Drawing.Size(239, 22);
            this.miAddObjectives.Text = "objectives";
            this.miAddObjectives.Click += new System.EventHandler(this.miAddObjectives_Click);
            // 
            // miAddControlMode
            // 
            this.miAddControlMode.Name = "miAddControlMode";
            this.miAddControlMode.Size = new System.Drawing.Size(239, 22);
            this.miAddControlMode.Text = "ControlMode";
            this.miAddControlMode.Click += new System.EventHandler(this.miAddControlMode_Click);
            // 
            // miAddRuleAction
            // 
            this.miAddRuleAction.Name = "miAddRuleAction";
            this.miAddRuleAction.Size = new System.Drawing.Size(239, 22);
            this.miAddRuleAction.Text = "ruleAction";
            this.miAddRuleAction.Click += new System.EventHandler(this.miAddRuleAction_Click);
            // 
            // miAddRuleConditions
            // 
            this.miAddRuleConditions.Name = "miAddRuleConditions";
            this.miAddRuleConditions.Size = new System.Drawing.Size(239, 22);
            this.miAddRuleConditions.Text = "ruleConditions";
            this.miAddRuleConditions.Click += new System.EventHandler(this.miAddRuleConditions_Click);
            // 
            // miAddRollupConsiderations
            // 
            this.miAddRollupConsiderations.Name = "miAddRollupConsiderations";
            this.miAddRollupConsiderations.Size = new System.Drawing.Size(239, 22);
            this.miAddRollupConsiderations.Text = "rollupConsiderations";
            this.miAddRollupConsiderations.Click += new System.EventHandler(this.miAddRollupConsiderations_Click);
            // 
            // miAddConstrainedChoiceConsiderations
            // 
            this.miAddConstrainedChoiceConsiderations.Name = "miAddConstrainedChoiceConsiderations";
            this.miAddConstrainedChoiceConsiderations.Size = new System.Drawing.Size(239, 22);
            this.miAddConstrainedChoiceConsiderations.Text = "constrainChoiceConsiderations";
            this.miAddConstrainedChoiceConsiderations.Click += new System.EventHandler(this.miAddConstrainedChoiceConsiderations_Click);
            // 
            // miAddExtensionObjectives
            // 
            this.miAddExtensionObjectives.Name = "miAddExtensionObjectives";
            this.miAddExtensionObjectives.Size = new System.Drawing.Size(239, 22);
            this.miAddExtensionObjectives.Text = "objectives(extension)";
            this.miAddExtensionObjectives.Click += new System.EventHandler(this.miAddExtensionObjectives_Click);
            // 
            // miDelete
            // 
            this.miDelete.Image = global::FireFly.CourseEditor.Properties.Resources.delete_16;
            this.miDelete.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miDelete.Name = "miDelete";
            this.miDelete.Size = new System.Drawing.Size(152, 22);
            this.miDelete.Text = "&Delete";
            this.miDelete.Click += new System.EventHandler(this.miDelete_Click);
            // 
            // miProperties
            // 
            this.miProperties.Image = global::FireFly.CourseEditor.Properties.Resources.properties_16;
            this.miProperties.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miProperties.Name = "miProperties";
            this.miProperties.Size = new System.Drawing.Size(152, 22);
            this.miProperties.Text = "&Properties";
            this.miProperties.Click += new System.EventHandler(this.miProperties_Click);
            // 
            // miExpandAll
            // 
            this.miExpandAll.Name = "miExpandAll";
            this.miExpandAll.Size = new System.Drawing.Size(152, 22);
            this.miExpandAll.Text = "&Expand All";
            this.miExpandAll.Click += new System.EventHandler(this.miExpandAll_Click);
            // 
            // tvManifest
            // 
            this.tvManifest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvManifest.ImageIndex = 0;
            this.tvManifest.ImageList = this.ilNodes;
            this.tvManifest.Location = new System.Drawing.Point(0, 0);
            this.tvManifest.Name = "tvManifest";
            this.tvManifest.NodeContextMenuStrip = this.cmsManifestNode;
            this.tvManifest.SelectedImageIndex = 0;
            this.tvManifest.Size = new System.Drawing.Size(227, 374);
            this.tvManifest.TabIndex = 1;
            this.tvManifest.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvManifest_NodeMouseClick);
            // 
            // miAddCollectionSequencing
            // 
            this.miAddCollectionSequencing.Name = "miAddCollectionSequencing";
            this.miAddCollectionSequencing.Size = new System.Drawing.Size(239, 22);
            this.miAddCollectionSequencing.Text = "Sequencing";
            this.miAddCollectionSequencing.Click += new System.EventHandler(this.miAddCollectionSequencing_Click);
            // 
            // ManifestBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(227, 374);
            this.Controls.Add(this.tvManifest);
            this.MinimumSize = new System.Drawing.Size(185, 250);
            this.Name = "ManifestBrowser";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.TabText = "Manifest Browser";
            this.Text = "Manifest Browser";
            this.cmsManifestNode.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem miProperties;
        private System.Windows.Forms.ToolStripMenuItem miAdd;
        private System.Windows.Forms.ToolStripMenuItem miAddOrganization;
        private System.Windows.Forms.ToolStripMenuItem miDelete;
        private System.Windows.Forms.ToolStripMenuItem miAddItem;
        private System.Windows.Forms.ToolStripMenuItem miAddResource;
        private System.Windows.Forms.ToolStripMenuItem miAddDependency;
        private System.Windows.Forms.ToolStripMenuItem miAddFile;
        public System.Windows.Forms.ImageList ilNodes;
        public System.Windows.Forms.ContextMenuStrip cmsManifestNode;
        private FFTreeView tvManifest;
        private System.Windows.Forms.ToolStripMenuItem miExpandAll;
        private System.Windows.Forms.ToolStripMenuItem miAddCatalogEntry;
        private System.Windows.Forms.ToolStripMenuItem miAddDescription;
        private System.Windows.Forms.ToolStripMenuItem miAddText;
        private System.Windows.Forms.ToolStripMenuItem miAddAuxiliaryResource;
        private System.Windows.Forms.ToolStripMenuItem miAddObjective;
        private System.Windows.Forms.ToolStripMenuItem miAddNavigationInterface;
        private System.Windows.Forms.ToolStripMenuItem miAddMapInfo;
        private System.Windows.Forms.ToolStripMenuItem miAddExitConditionRule;
        private System.Windows.Forms.ToolStripMenuItem miAddPostConditionRule;
        private System.Windows.Forms.ToolStripMenuItem miAddPreConditionRule;
        private System.Windows.Forms.ToolStripMenuItem miAddRuleCondition;
        private System.Windows.Forms.ToolStripMenuItem miAddRollupRule;
        private System.Windows.Forms.ToolStripMenuItem miAddRollupCondition;
        private System.Windows.Forms.ToolStripMenuItem miAddManifest;
        private System.Windows.Forms.ToolStripMenuItem miAddPresentation;
        private System.Windows.Forms.ToolStripMenuItem miAddSequencing;
        private System.Windows.Forms.ToolStripMenuItem miAddDeliveryControls;
        private System.Windows.Forms.ToolStripMenuItem miAddLimitConditions;
        private System.Windows.Forms.ToolStripMenuItem miAddRandomizationControl;
        private System.Windows.Forms.ToolStripMenuItem miAddRollupRules;
        private System.Windows.Forms.ToolStripMenuItem miAddSequencingRules;
        private System.Windows.Forms.ToolStripMenuItem miAddObjectives;
        private System.Windows.Forms.ToolStripMenuItem miAddControlMode;
        private System.Windows.Forms.ToolStripMenuItem miAddRuleAction;
        private System.Windows.Forms.ToolStripMenuItem miAddRuleConditions;
        private System.Windows.Forms.ToolStripMenuItem miAddRollupConditions;
        private System.Windows.Forms.ToolStripMenuItem miAddRollupAction;
        private System.Windows.Forms.ToolStripMenuItem miAddPrimaryObjective;
        private System.Windows.Forms.ToolStripMenuItem miAddRollupConsiderations;
        private System.Windows.Forms.ToolStripMenuItem miAddConstrainedChoiceConsiderations;
        private System.Windows.Forms.ToolStripMenuItem miAddExtensionObjectives;
        private System.Windows.Forms.ToolStripMenuItem miAddCollectionSequencing;
    }
}
