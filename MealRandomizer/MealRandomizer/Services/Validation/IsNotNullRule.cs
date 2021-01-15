namespace MealRandomizer.Services.Validation
{
    internal sealed class IsNotNullRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; }

        public IsNotNullRule(string validationMessage)
        {
            ValidationMessage = validationMessage;
        }

        public bool Check(T value) => value != null;
    }
}
