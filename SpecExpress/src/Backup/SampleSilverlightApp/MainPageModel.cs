using System;
using System.ComponentModel;
using System.Text;
using System.Windows;
using SpecExpress;
using SpecExpress.Test.Domain.Entities;

namespace SampleSilverlightApp
{
    public class MainPageModel : ViewModelBase
    {
        private Project _currentProject;

        public ValidationScope ValidationScope1 { get; private set; }

        public MainPageModel(Project currentProject)
        {
            _currentProject = currentProject;
            ValidationScope1 = new ValidationScope();
        }

        /// <summary>
        /// A proxy to the ProjectName on the entity.
        /// Setter is required to do any validation on the proposed value and if
        /// invalid then an exception is thrown containing a message to disply to the user.
        /// 
        /// Sucky restrictions needed to overcome:
        /// 
        /// - The message thrown in the exception is tied to the control bound to the property.
        ///     Impact: Since a given property value may trigger other properties to be invalid, we need to somehow
        ///     update the view in such a way that any field bound to a property that is associated with a broken
        ///     rule is updated.  The below solution really is not suitable since it consolodates all the messages
        ///     into the single bound control.
        /// 
        ///     Example: Consider an entity having a StartDate and EndDate property with rule for StartDate
        ///     dictating that StartDate must be before EndDate, and a rule for EndDate dictating that 
        ///     EndDate must be after StartDate.  If I change the EndDate to a value that is before StartDate,
        ///     both rules are broken resulting in two ValidationResults - one tied to StartDate and one tied to 
        ///     EndDate.  The user interface needs to reflect that both dates are invalid.  
        /// </summary>
        public string ProjectName
        {
            get { return _currentProject.ProjectName; }
            set
            {
                SetEntityPropertyValue(_currentProject, "ProjectName", value);
            }
        }

        public DateTime StartDate
        {
            get { return _currentProject.StartDate; }
            set
            {
                SetEntityPropertyValue(_currentProject, "StartDate", value);
            }
        }

        public DateTime EndDate
        {
            get { return _currentProject.EndDate; }
            set
            {
                SetEntityPropertyValue(_currentProject, "EndDate", value);
            }
        }

        public void Save()
        {
            ValidationScope1.ValidateScope();
            if (ValidationScope1.IsValid())
            {
                //ToDo: Save Entity
                MessageBox.Show("Project Saved");
            }
        }
    }
}