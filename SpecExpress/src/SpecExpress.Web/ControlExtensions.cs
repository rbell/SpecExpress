using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace SpecExpress.Web
{
    public static class ControlExtensions
    {
        public static IEnumerable<Control> All(this ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                foreach (var grandchild in control.Controls.All())
                    yield return grandchild;

                yield return control;
            }
        }
    }
}
