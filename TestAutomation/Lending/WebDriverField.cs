using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestAutomation.LendingTree.tl
{
    public class WebDriverField:IFormField
    {
        public WebDriverField(Action<String, String> fillAction, String fieldID, String columnName)
        {
            this.FillAction2Params = fillAction;
            this.FieldID = fieldID;
            this.ColumnName = columnName;
        }

        public WebDriverField(Action<String> fillActionJustID, String fieldID)
        {
            this.FillAction1Param = fillActionJustID;
            this.FieldID = fieldID;
        }

        public void FillField(Dictionary<string, string> testData = null)
        {
            if (FillAction2Params != null) FillAction2Params(FieldID, testData[ColumnName]);
            else if (FillAction1Param != null) FillAction1Param(FieldID);
            else throw new ArgumentException("This WebDriverFieldField action does not contain an actual action!");
        }

        /// <summary>
        /// The BasePage function that should be called to set this field. The first param represents a DOM attribute value, such as element ID, or Xpath and the second param represents a column name in the test DB.
        /// </summary>
        private Action<String, String> FillAction2Params { get; set; }

        /// <summary>
        /// The BasePage function that should be called to set this field. The first param represents a DOM attribute value, element ID is prefered.
        /// </summary>
        private Action<String> FillAction1Param { get; set; }

        /// <summary>
        /// The ID of this form field on the web page.
        /// </summary>
        private String FieldID { get; set; }

        /// <summary>
        /// The column name corresponding to this field in the test database.
        /// </summary>
        private String ColumnName { get; set; }
    }
}
