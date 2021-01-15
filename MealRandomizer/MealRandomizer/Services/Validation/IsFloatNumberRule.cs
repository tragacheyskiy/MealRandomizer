namespace MealRandomizer.Services.Validation
{
    internal sealed class IsFloatNumberRule : IValidationRule<string>
    {
        public string ValidationMessage { get; }

        public IsFloatNumberRule(string validationMessage)
        {
            ValidationMessage = validationMessage;
        }

        public bool Check(string value) => float.TryParse(value, out _);
    }
}
