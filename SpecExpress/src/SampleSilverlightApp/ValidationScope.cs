using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SampleSilverlightApp
{
    public class ValidationScope
    {
        public FrameworkElement ScopeElement { get; private set; }

        private readonly ObservableCollection<ValidationError> _errors = new ObservableCollection<ValidationError>();

        public ObservableCollection<ValidationError> Errors
        {
            get { return _errors; }
        }

        public bool IsValid()
        {
            return _errors.Count == 0;
        }

        public static string GetValidateBoundProperty(DependencyObject obj)
        {
            return (string)obj.GetValue(ValidateBoundPropertyProperty);
        }

        public static void SetValidateBoundProperty(DependencyObject obj, string value)
        {
            obj.SetValue(ValidateBoundPropertyProperty, value);
        }

        public static readonly DependencyProperty ValidateBoundPropertyProperty =
            DependencyProperty.RegisterAttached("ValidateBoundProperty", typeof (string), typeof (ValidationScope),
                                                new PropertyMetadata(null));

        public static ValidationScope GetValidationScope(DependencyObject obj)
        {
            return (ValidationScope)obj.GetValue(ValidationScopeProperty);
        }

        public static void SetValidationScope(DependencyObject obj, ValidationScope value)
        {
            obj.SetValue(ValidationScopeProperty, value);
        }

        public static readonly DependencyProperty ValidationScopeProperty =
            DependencyProperty.RegisterAttached("ValidationScope", typeof (ValidationScope), typeof (ValidationScope),
                                                new PropertyMetadata(null, ValidationScopeChanged));

        private static void ValidationScopeChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            ValidationScope oldScope = args.OldValue as ValidationScope;
            if (oldScope != null)
            {
                oldScope.ScopeElement.BindingValidationError -= oldScope.ScopeElement_BindingValidationError;
                oldScope.ScopeElement = null;
            }

            FrameworkElement scopeElement = source as FrameworkElement;
            if (scopeElement == null)
            {
                throw new ArgumentException(string.Format(
                    "'{0}' is not a valid type.ValidationScope attached property can only be specified on types inheriting from FrameworkElement.",
                    source));
            }

            ValidationScope newScope = (ValidationScope)args.NewValue;
            newScope.ScopeElement = scopeElement;
            newScope.ScopeElement.BindingValidationError += newScope.ScopeElement_BindingValidationError;
        }

        private void ScopeElement_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Removed)
            {
                Errors.Remove(e.Error);
            }
            else if (e.Action == ValidationErrorEventAction.Added)
            {
                Errors.Add(e.Error);
            }
        }

        public void ValidateScope()
        {
            ForEachElement(ScopeElement, delegate(DependencyObject obj)
            {
                string propertyName = GetValidateBoundProperty(obj);
                if (!string.IsNullOrEmpty(propertyName))
                {
                    FrameworkElement element = (FrameworkElement)obj;
                    var field = element.GetType().GetFields(BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.Public)
                        .Where(p => p.FieldType == typeof(DependencyProperty) && p.Name == (propertyName + "Property"))
                        .FirstOrDefault();

                    if (field == null)
                    {
                        throw new ArgumentException(string.Format(
                            "Dependency property '{0}' could not be found on type '{1}'; ValidationScope.ValidateBoundProperty",
                            propertyName, element.GetType()));
                    }
                    var be = element.GetBindingExpression((DependencyProperty)field.GetValue(null));
                    be.UpdateSource();
                }
            });
        }

        private static void ForEachElement(DependencyObject root, Action<DependencyObject> action)
        {
            int childCount = VisualTreeHelper.GetChildrenCount(root);
            for (int i = 0; i < childCount; i++)
            {
                var obj = VisualTreeHelper.GetChild(root, i);
                action(obj);
                ForEachElement(obj, action);
            }
        }
    }
}