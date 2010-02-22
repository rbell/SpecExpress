using System;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using SpecExpress;

namespace SampleSilverlightApp
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Set Property on an entity, validate entity and if any errors for property on entity, throw an ArgumentException.
        /// If Property is updated raises the PropertyChanged event by calling OnPropertyChanged().
        /// </summary>
        /// <typeparam name="T">Type of Entity</typeparam>
        /// <typeparam name="TProperty">Type of Property being set</typeparam>
        /// <param name="entity">Instance of entity to change property on.</param>
        /// <param name="propertyName">Name of property to set.</param>
        /// <param name="value">Value of to set property to.</param>
        protected virtual void SetEntityPropertyValue<T, TProperty>(T entity, string propertyName, TProperty value)
        {
            PropertyInfo propertyInfo = typeof(T).GetProperty(propertyName);
            TProperty currentValue = (TProperty)propertyInfo.GetValue(entity, null);
            propertyInfo.SetValue(entity, value, null);
            var results = ValidationCatalog.Validate(entity);
            if (!results.IsValid)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var result in results.Errors)
                {
                    if (result.Property.Name == propertyName)
                    {
                        sb.AppendLine(result.Message);
                    }
                }
                if (sb.Length > 0)
                {
                    throw new ArgumentException(sb.ToString());
                }
            }
            if (!currentValue.Equals(value))
            {
                OnPropertyChanged(propertyName);
            }
        }
    }
}