using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Ferrara.Compass.Classes
{
    public class ErrorSummary : IValidator
    {
        private string _message;
        private ErrorSummary(string message)
        {
            _message = message;
        }

        public string ErrorMessage
        {
            get { return _message; }
            set { }
        }

        public bool IsValid
        {
            get { return false; }
            set { }
        }

        public static void AddError(string message, Page page)
        {
            if (page == null)
            {
                page = (System.Web.UI.Page)System.Web.HttpContext.Current.Handler;
            }

            ErrorSummary error = new ErrorSummary(message);
            page.Validators.Add(error);
        }

        public void Validate()
        { }
    }
}
