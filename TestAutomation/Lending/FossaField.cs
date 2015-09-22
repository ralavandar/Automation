using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace TestAutomation.LendingTree
{
    public class FossaField:IFormField
    {
        public FossaField(Action<String, String> fillAction, String fieldID, String columnName)
        {
            this.FillAction2Params = fillAction;
            this.FieldID = fieldID;
            this.ColumnName = columnName;
        }

        public FossaField(Action<String> fillActionJustID, String fieldID)
        {
            this.FillAction1Param = fillActionJustID;
            this.FieldID = fieldID;
        }

        public FossaField(Action<By, String> fillAction, By elementLocator, String columnName)
        {
            this.FillActionBy2Params = fillAction;
            this.ElementLocator = elementLocator;
            this.ColumnName = columnName;
        }

        public FossaField(Action<By> fillActionJustBy, By elementLocator)
        {
            this.FillActionBy1Param = fillActionJustBy;
            this.ElementLocator = elementLocator;
        }

        public void FillField(Dictionary<string, string> testData = null)
        {
            if (FillAction2Params != null) FillAction2Params(FieldID, testData[ColumnName]);
            else if (FillAction1Param != null) FillAction1Param(FieldID);
            else if (FillActionBy2Params != null) FillActionBy2Params(ElementLocator, testData[ColumnName]);
            else if (FillActionBy1Param != null) FillActionBy1Param(ElementLocator);
            else throw new ArgumentException("This FossaField action does not contain an actual action!");
        }

        /// <summary>
        /// The BasePage function that should be called to set this field. The first param represents a DOM ID and the second param represents a column name in the test DB.
        /// </summary>
        private Action<String, String> FillAction2Params { get; set; }

        /// <summary>
        /// The BasePage function that should be called to set this field. The first param represents a DOM ID.
        /// </summary>
        private Action<String> FillAction1Param { get; set; }

        /// <summary>
        /// The BasePage function that should be called to set this field. The first param represents locator BY object and the second param represents a column name in the test DB.
        /// </summary>
        private Action<By, String> FillActionBy2Params { get; set; }

        /// <summary>
        /// The BasePage function that should be called to set this field. The first param represents a locator BY object.
        /// </summary>
        private Action<By> FillActionBy1Param { get; set; }

        /// <summary>
        /// The ID of this form field on the web page.
        /// </summary>
        private String FieldID { get; set; }

        /// <summary>
        /// The locator object used to find this form field on the web page
        /// </summary>
        private By ElementLocator { get; set; }

        /// <summary>
        /// The column name corresponding to this field in the test database.
        /// </summary>
        private String ColumnName { get; set; }
    }
}
