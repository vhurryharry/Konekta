using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace WCA.Domain.Validators
{
    /// <summary>
    /// TODO: Move to use FluentValidation
    /// </summary>
    public class ABNAttribute : ValidationAttribute
    {
        private const string DEFAULT_MESSAGE = "Must be a valid Australian Busines Number.";

        public ABNAttribute() : base(DEFAULT_MESSAGE)
        {
        }

        public ABNAttribute(bool allowNullOrEmpty) : base(DEFAULT_MESSAGE)
        {
            AllowNullOrEmpty = allowNullOrEmpty;
        }

        public ABNAttribute(string errorMessage, bool allowNullOrEmpty) : base(errorMessage)
        {
            AllowNullOrEmpty = allowNullOrEmpty;
        }

        public ABNAttribute(Func<string> errorMessageAccessor) : base(errorMessageAccessor)
        {
        }

        public ABNAttribute(string errorMessage) : base(errorMessage)
        {
        }

        public bool AllowNullOrEmpty { get; set; } = false;

        public override bool IsValid(object abn)
        {
            string strAbn = abn as string;
            if (string.IsNullOrEmpty(strAbn))
            {
                return AllowNullOrEmpty;
            }
            else
            {
                strAbn = strAbn?.Replace(" ", "", StringComparison.Ordinal); // strip spaces

                int[] weight = { 10, 1, 3, 5, 7, 9, 11, 13, 15, 17, 19 };
                int weightedSum = 0;

                //0. ABN must be 11 digits long
                if (string.IsNullOrEmpty(strAbn) || !Regex.IsMatch(strAbn, @"^\d{11}$"))
                {
                    return false;
                }

                //Rules: 1,2,3
                for (int i = 0; i < weight.Length; i++)
                {
                    weightedSum += (int.Parse(strAbn[i].ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture) - (i == 0 ? 1 : 0)) * weight[i];
                }

                //Rules: 4,5
                return weightedSum % 89 == 0;
            }
        }
    }
}