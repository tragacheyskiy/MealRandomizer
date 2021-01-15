using System.Collections.Generic;

namespace MealRandomizer.Services.Validation
{
    internal class ValidatableObject<T>
    {
        private readonly List<IValidationRule<T>> validationRules = new List<IValidationRule<T>>();

        public string ValidationMessage { get; protected set; }
        public T Value { get; set; }

        public void AddValidationRule(IValidationRule<T> validationRule)
        {
            validationRules.Add(validationRule);
        }

        public bool Validate()
        {
            ValidationMessage = string.Empty;

            foreach (var rule in validationRules)
            {
                if (!rule.Check(Value))
                {
                    ValidationMessage = rule.ValidationMessage;
                    return false;
                }
            }

            return true;
        }
    }
}
