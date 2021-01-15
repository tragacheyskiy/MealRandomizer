namespace MealRandomizer.Services.Validation
{
    internal interface IValidationRule<T>
    {
        string ValidationMessage { get; }

        bool Check(T value);
    }
}
