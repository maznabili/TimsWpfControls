﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace TimsWpfControls
{
    [TemplatePart (Name = nameof(PART_EditableTextBox), Type = typeof(TextBox))]
    [TemplatePart (Name = nameof(PART_Popup), Type = typeof(Popup))]
    public class MultiSelectionComboBox : ListBox
    {
        private TextBox PART_EditableTextBox;
        private Popup PART_Popup;

        static MultiSelectionComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiSelectionComboBox), new FrameworkPropertyMetadata(typeof(MultiSelectionComboBox)));
        }


        // Using a DependencyProperty as the backing store for IsReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(MultiSelectionComboBox), new PropertyMetadata(true));

        // Using a DependencyProperty as the backing store for IsEditable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEditableProperty = DependencyProperty.Register("IsEditable", typeof(bool), typeof(MultiSelectionComboBox), new PropertyMetadata(false));

        // Using a DependencyProperty as the backing store for IsDropDownOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(MultiSelectionComboBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty = 
            DependencyProperty.Register("Text", typeof(string), typeof(MultiSelectionComboBox), 
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextChanged));


        // Using a DependencyProperty as the backing store for TextSeparator.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextSeparatorProperty = DependencyProperty.Register("TextSeparator", typeof(string), typeof(MultiSelectionComboBox), new PropertyMetadata(", "));


        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }


        public bool IsEditable
        {
            get { return (bool)GetValue(IsEditableProperty); }
            set { SetValue(IsEditableProperty, value); }
        }

        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, value); }
        }


        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MultiSelectionComboBox multiSelectionComboBox)
            {
                multiSelectionComboBox.UpdateEditableText();
            }
        }

        private void UpdateEditableText()
        {
            if (PART_EditableTextBox is null)
                return;

            isUpdatingText = true;
            if (string.IsNullOrEmpty(Text))
            {
                PART_EditableTextBox.SetCurrentValue(TextBox.TextProperty, string.Join(TextSeparator, (IEnumerable<object>)SelectedItems));
            }
            else
            {
                PART_EditableTextBox.SetCurrentValue(TextBox.TextProperty, Text);
            }
            isUpdatingText = false;
        }


        public string TextSeparator
        {
            get { return (string)GetValue(TextSeparatorProperty); }
            set { SetValue(TextSeparatorProperty, value); }
        }


        #region Override

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (PART_EditableTextBox == null)
            {
                PART_EditableTextBox = GetTemplateChild(nameof(PART_EditableTextBox)) as TextBox;
                PART_EditableTextBox.TextChanged += PART_EditableTextBox_TextChanged; ;
                UpdateEditableText();
            }
        }

        bool isUpdatingText = false;
        private void PART_EditableTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!isUpdatingText)
            {
                Text = PART_EditableTextBox?.Text;

                SelectedItems.Clear();
                if (!string.IsNullOrEmpty(Text))
                {
                    var items = Text.Split(new string[] { TextSeparator }, StringSplitOptions.RemoveEmptyEntries);

                }
            }
        }


        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            UpdateEditableText();
        }


        #endregion
    }
}
