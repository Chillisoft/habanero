using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;

namespace Habanero.Test
{
    public class MockExceptionNotifier : IExceptionNotifier
    {
        private Exception _exception;
        private string _furtherMessage;
        private string _title;

        public void Notify(Exception ex, string furtherMessage, string title)
        {
            _exception = ex;
            _furtherMessage = furtherMessage;
            _title = title;
        }

        public Exception Exception
        {
            get { return _exception; }
            set { _exception = value; }
        }

        public string FurtherMessage
        {
            get { return _furtherMessage; }
            set { _furtherMessage = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
    }
}
