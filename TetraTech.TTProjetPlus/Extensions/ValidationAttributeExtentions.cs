using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;

namespace TetraTech.TTProjetPlus
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class DateValidationAttribute : ValidationAttribute
    {
        public DateValidationAttribute()
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime date;

                if (value is string)
                {
                    if (!DateTime.TryParse((string)value, out date))
                    {
                        return new ValidationResult(string.Format(Resources.TTProjetPlusResource.InvalidDateFormat, validationContext.DisplayName));
                    }
                }
                else
                    date = (DateTime)value; 
            }
            return null;
        }
    }

    public class RequiredIfOtherFieldAttribute : ValidationAttribute 
    {
        private readonly string _otherProperty;
        public RequiredIfOtherFieldAttribute(string otherProperty)
        {
            _otherProperty = otherProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_otherProperty);
            if (property == null)
            {
                return new ValidationResult( $"Unknown property { _otherProperty}");
            }
            var otherPropertyValue = property.GetValue(validationContext.ObjectInstance, null);

            if (otherPropertyValue != null && (bool)otherPropertyValue )
            {
                if (value == null || value as string == string.Empty)
                {
                    return new ValidationResult(string.Format(
                        CultureInfo.CurrentCulture,
                        FormatErrorMessage(validationContext.DisplayName),
                        new[] { _otherProperty }
                    ));
                }
            }

            return null;
        }
         
    }
} 