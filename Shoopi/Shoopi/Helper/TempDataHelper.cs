using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Shoopi.Helper
{
    public class TempDataHelper
    {
        public static void AddNotification(ITempDataDictionary tempData, string key, string message)
        {
            tempData[key] = message;
        }

        public static string GetNotification(ITempDataDictionary tempData, string key)
        {
            return tempData.ContainsKey(key) ? tempData[key].ToString() : null;
        }
    }
}
