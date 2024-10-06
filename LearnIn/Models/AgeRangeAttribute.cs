using System.ComponentModel.DataAnnotations;

namespace LearnIn.Models
{
    public class AgeRangeAttribute : ValidationAttribute
    {
        private readonly int _minimumAge;
        private readonly int _maximumAge;

        public AgeRangeAttribute(int minimumAge, int maximumAge)
        {
            _minimumAge = minimumAge;
            _maximumAge = maximumAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateOfBirth)
            {
                var today = DateTime.Today;
                var age = today.Year - dateOfBirth.Year;

                // Adjust if the user's birthday hasn't occurred yet this year
                if (dateOfBirth > today.AddYears(-age))
                {
                    age--;
                }

                if (age < _minimumAge)
                {
                    return new ValidationResult($"You must be at least {_minimumAge} years old.");
                }

                if (age > _maximumAge)
                {
                    return new ValidationResult($"You must be younger than {_maximumAge} years old.");
                }

                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid date of birth");
        }
    }

}
