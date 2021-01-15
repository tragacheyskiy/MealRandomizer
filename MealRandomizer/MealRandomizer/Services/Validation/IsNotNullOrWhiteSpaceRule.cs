namespace MealRandomizer.Services.Validation
{
    internal sealed class IsNotNullOrWhiteSpaceRule : IValidationRule<string>
    {
        public string ValidationMessage { get; }

        public IsNotNullOrWhiteSpaceRule(string validationMessage)
        {
            ValidationMessage = validationMessage;
        }

        public bool Check(string value) => !string.IsNullOrWhiteSpace(value);
    }
}
