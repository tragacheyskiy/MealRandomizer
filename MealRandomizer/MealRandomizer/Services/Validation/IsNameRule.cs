namespace MealRandomizer.Services.Validation
{
    internal sealed class IsNameRule : IValidationRule<string>
    {
        public string ValidationMessage { get; }

        public IsNameRule(string validationMessage)
        {
            ValidationMessage = validationMessage;
        }

        public bool Check(string value) => !string.IsNullOrWhiteSpace(value);
    }
}
