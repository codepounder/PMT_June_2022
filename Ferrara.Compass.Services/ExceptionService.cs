using System;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Services
{
    public class ExceptionService : IExceptionService
    {
        private ILoggerService loggerService;
        public ExceptionService(ILoggerService loggerService)
        {
            this.loggerService = loggerService;
        }

        public bool Handle(LogCategory category, Exception exception)
        {
            LogEntry logEntry = new LogEntry();
            logEntry.Title = exception.Message.Length > 250 ? exception.Message.Substring(0, 250) : exception.Message;
            logEntry.Message = FormatErrorMessage(exception);
            logEntry.Message = logEntry.Message.Length > 1000 ? logEntry.Message.Substring(0, 1000) : logEntry.Message;
            logEntry.Category = category.ToString();
            loggerService.InsertLog(logEntry);

            bool rethrow = false; //TODO: need to take up from configuration
            return rethrow;
        }

        public bool Handle(LogCategory category, Exception exception, string form, string method)
        {
            LogEntry logEntry = new LogEntry();
            logEntry.Title = exception.Message.Length > 250 ? exception.Message.Substring(0, 250) : exception.Message;
            logEntry.Message = FormatErrorMessage(exception);
            logEntry.Message = logEntry.Message.Length > 1000 ? logEntry.Message.Substring(0, 1000) : logEntry.Message;
            logEntry.Category = category.ToString();
            logEntry.Form = form;
            logEntry.Method = method;
            loggerService.InsertLog(logEntry);

            bool rethrow = false; //TODO: need to take up from configuration
            return rethrow;
        }

        public bool Handle(LogCategory category, Exception exception, string form, string method, string additionalInfo)
        {
            LogEntry logEntry = new LogEntry();
            logEntry.Title = exception.Message.Length > 250 ? exception.Message.Substring(0, 250) : exception.Message;
            logEntry.Message = FormatErrorMessage(exception);
            logEntry.Message = logEntry.Message.Length > 1000 ? logEntry.Message.Substring(0, 1000) : logEntry.Message;
            logEntry.Category = category.ToString();
            logEntry.Form = form;
            logEntry.Method = method;
            logEntry.AdditionalInfo = additionalInfo;
            loggerService.InsertLog(logEntry);

            bool rethrow = false; //TODO: need to take up from configuration
            return rethrow;
        }

        public bool Handle(LogCategory category, Exception exception, string form, string method, string additionalInfo, string webUrl)
        {
            LogEntry logEntry = new LogEntry();
            logEntry.Title = exception.Message.Length > 250 ? exception.Message.Substring(0, 250) : exception.Message;
            logEntry.Message = FormatErrorMessage(exception);
            logEntry.Message = logEntry.Message.Length > 1000 ? logEntry.Message.Substring(0, 1000) : logEntry.Message;
            logEntry.Category = category.ToString();
            logEntry.Form = form;
            logEntry.Method = method;
            logEntry.AdditionalInfo = additionalInfo;
            loggerService.InsertLog(logEntry, webUrl);

            bool rethrow = false; //TODO: need to take up from configuration
            return rethrow;
        }

        public bool Handle(LogCategory category, string error, string form, string method, string additionalInfo)
        {
            LogEntry logEntry = new LogEntry();
            logEntry.Title = error.Length > 250 ? error.Substring(0, 250) : error;
            logEntry.Message = error;
            logEntry.Message = logEntry.Message.Length > 1000 ? logEntry.Message.Substring(0, 1000) : logEntry.Message;
            logEntry.Category = category.ToString();
            logEntry.Form = form;
            logEntry.Method = method;
            logEntry.AdditionalInfo = additionalInfo;
            loggerService.InsertLog(logEntry);

            bool rethrow = false; //TODO: need to take up from configuration
            return rethrow;
        }

        public bool HandleGraphicsImport(GraphicsLogCategory category, string error, string form, string materialNumber)
        {
            LogEntry logEntry = new LogEntry();
            logEntry.Title = materialNumber;
            logEntry.Message = error;
            logEntry.Message = logEntry.Message.Length > 1000 ? logEntry.Message.Substring(0, 1000) : logEntry.Message;
            logEntry.Category = category.ToString();
            logEntry.Form = form;
            loggerService.InsertGraphicsImportLog(logEntry);

            bool rethrow = false; //TODO: need to take up from configuration
            return rethrow;
        }

        private static string FormatErrorMessage(Exception exception)
        {
            string messageFormat = string.Empty;

            if (exception != null)
            {
                const string format = "{0}; StackTrace:{1}; Source:{2}";
                messageFormat = string.Format(format, string.IsNullOrEmpty(exception.Message) ? string.Empty : exception.Message
                                , string.IsNullOrEmpty(exception.StackTrace) ? string.Empty : exception.StackTrace
                                , string.IsNullOrEmpty(exception.Source) ? string.Empty : exception.Source);
                if (exception.InnerException != null)
                {
                    messageFormat += "\n";
                }
            }
            return messageFormat;
        }
    }
}
