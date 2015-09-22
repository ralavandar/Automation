using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestAutomation
{
    /// <summary>
    /// Represents a form field on a page that can be filled out with some given data.
    /// </summary>
    public interface IFormField
    {
        /// <summary>
        /// Fills out the field associated with this action with the data given.
        /// </summary>
        void FillField(Dictionary<string, string> testData = null);
    }
}
