using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MealRandomizer.Views.ProductsViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchEntryView : ContentView
    {
        public SearchEntryView()
        {
            InitializeComponent();
        }

        private void SearchEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchEntry = sender as Entry;

            if (searchEntry == null || searchEntry.ReturnCommand == null)
            {
                return;
            }

            if (searchEntry.ReturnCommand.CanExecute(e.NewTextValue))
            {
                searchEntry.ReturnCommand.Execute(e.NewTextValue);
            }
        }
    }
}