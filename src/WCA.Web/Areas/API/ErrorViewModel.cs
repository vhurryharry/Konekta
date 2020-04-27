using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WCA.Web.Areas.API
{
    public class ErrorViewModel
    {
        public ErrorViewModel(string message)
        {
            Message = message;
        }

        public ErrorViewModel(string message, ModelStateDictionary modelState)
        {
            Message = message;

            if (modelState != null)
            {
                ErrorList = (from k in modelState
                             from e in k.Value.Errors
                             select $"{k.Key}: {e.ErrorMessage}"
                             ).ToArray();
            }
            else
            {
                ErrorList = Array.Empty<string>();
            }
        }

        public ErrorViewModel(ValidationException validationException):
            this (string.Empty, validationException)
        {
        }

        public ErrorViewModel(string message, ValidationException validationException)
        {
            Message = message;

            if (validationException != null &&
                validationException.Errors != null)
            {
                ErrorList = (from k in validationException.Errors
                             select k.ErrorMessage).ToArray();
            }
            else
            {
                ErrorList = Array.Empty<string>();
            }
        }

        public IEnumerable<string> ErrorList { get; private set; }
        public string Message { get; private set; }
    }
}