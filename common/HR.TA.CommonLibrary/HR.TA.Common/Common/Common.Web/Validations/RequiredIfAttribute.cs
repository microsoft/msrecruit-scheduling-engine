//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.Common.Common.Common.Web.Validations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using HR.TA.CommonDataService.Common;

    /// <summary>
    /// The <see cref="RequiredIfAttribute"/> class mandates requirement of value presence based on another propertry value.
    /// </summary>
    /// <seealso cref="RequiredAttribute" />
    public class RequiredIfAttribute : RequiredAttribute
    {
        /// <summary>
        /// The source property name
        /// </summary>
        private readonly string sourcePropertyName;

        /// <summary>
        /// The expected source property value
        /// </summary>
        private readonly object expectedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredIfAttribute"/> class.
        /// </summary>
        /// <param name="sourcePropertyName">Name of the source property.</param>
        /// <param name="expectedValue">The expected source property value.</param>
        public RequiredIfAttribute(string sourcePropertyName, object expectedValue)
        {
            Contract.CheckNonEmpty(sourcePropertyName, nameof(sourcePropertyName));
            Contract.CheckValue(expectedValue, nameof(expectedValue));
            this.sourcePropertyName = sourcePropertyName;
            this.expectedValue = expectedValue;
        }

        /// <summary>
        /// Returns true if source property has specified value.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>
        /// An instance of the <see cref="ValidationResult"></see> class.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult result = ValidationResult.Success;
            PropertyInfo propertyInfo = validationContext.ObjectType.GetProperty(this.sourcePropertyName);
            var propertyValue = propertyInfo.GetValue(validationContext.ObjectInstance);
            if (propertyValue.ToString().Equals(this.expectedValue.ToString(), StringComparison.Ordinal)
                && base.IsValid(value, validationContext) == null)
            {
                result = new ValidationResult(this.ErrorMessage);
            }

            return result;
        }
    }
}
