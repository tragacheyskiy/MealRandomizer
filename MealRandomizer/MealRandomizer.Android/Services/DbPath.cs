using MealRandomizer.Droid.Services;
using MealRandomizer.Services;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(DbPath))]
namespace MealRandomizer.Droid.Services
{
    public class DbPath : IDbPath
    {
        public string GetDbPath(string fileName)
        {
            return Path.Combine(Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath, $"{fileName}.db");
        }
    }
}