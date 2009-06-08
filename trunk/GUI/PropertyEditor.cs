using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Collections.Generic;
using System.ComponentModel;

namespace FireFly.CourseEditor.GUI
{
    using Common;
    using Course.Manifest;

    public partial class PropertyEditor : EditorWindowBase
    {
        public PropertyEditor(DockPanel parentDockPanel)
            : base(parentDockPanel)
        {
            InitializeComponent();
            Show();
        }

        [Browsable(false)]
        [CanBeNull]
        public object CurrentObject
        {
            get { return propertyGrid.SelectedObject; }
            set { SetContext(value, null, null); }
        }

        public void Show([CanBeNull]object obj)
        {
            SetContext(obj, null, null);
            Show();
            BringToFront();
        }

        public FFTypeDescriptor SetContext([CanBeNull]object obj, [CanBeNull]IEnumerable<ITitled> scope, [CanBeNull]Action<object, FFTypeDescriptor> onSelectedObjectChanged)
        {
            _SelectedObjectChanged = onSelectedObjectChanged;
            ComboBox.ObjectCollection items = cbScope.Items;
            items.Clear();
            if (cbScope.Enabled = scope != null && obj != null)
            {
                foreach (var t in scope)
                {
                    items.AddTitledItem(t);
                }
            }
            else
            {
                var t = obj as ITitled;
                if (t != null)
                {
                    items.AddTitledItem(t);
                }
                else
                {
                    if (obj != null)
                    {
                        items.Add(obj.ToString()); 
                    }
                }
            }
            return SetCurrentObject(obj);
        }

        public FFTypeDescriptor SetContext([CanBeNull]object[] objects, [CanBeNull]IEnumerable<ITitled> scope, [CanBeNull]Action<object, FFTypeDescriptor> onSelectedObjectChanged)
        {
            if (objects == null || objects.Length == 0)
            {
                return SetContext((object)null, null, onSelectedObjectChanged);
            }
            else
            {
                if (objects.Length == 1)
                {
                    return SetContext(objects[0], scope, onSelectedObjectChanged);
                }
                else
                {
                    _SelectedObjectChanged = onSelectedObjectChanged;
                    var descriptors = new FFTypeDescriptor[objects.Length];
                    for (int i = objects.Length - 1; i >= 0; i--)
                    {
                        descriptors[i] = new FFTypeDescriptor(objects[i]);
                    }
                    propertyGrid.SelectedObjects = objects;
                    _ChangingSelectionInternal = true;
                    cbScope.Text = string.Empty;
                    _ChangingSelectionInternal = false;
                    cbScope.Enabled = false;
                    return descriptors[0];
                }
            }
        }

        public void RefreshContent()
        {
            propertyGrid.Refresh();
        }

        private FFTypeDescriptor SetCurrentObject([CanBeNull]object obj)
        {
            FFTypeDescriptor res;
            if (obj != null)
            {
                var t = obj as ITitled;
                _ChangingSelectionInternal = true;
                cbScope.Text = t != null ? t.Title : obj.ToString();
                _ChangingSelectionInternal = false;
                propertyGrid.SelectedObject = res = new FFTypeDescriptor(obj);
            }
            else
            {
                propertyGrid.SelectedObject = res = null;
            }
            return res;
        }

        private void cbScope_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!_ChangingSelectionInternal)
            {
                object newObject = ((ComboBoxItem) cbScope.SelectedItem).Value;
                FFTypeDescriptor typeDescriptor = SetCurrentObject(newObject);
                if (_SelectedObjectChanged != null)
                {
                    _SelectedObjectChanged(newObject, typeDescriptor);
                }
            }
        }

        private bool _ChangingSelectionInternal;
        private Action<object, FFTypeDescriptor> _SelectedObjectChanged;
    }
}