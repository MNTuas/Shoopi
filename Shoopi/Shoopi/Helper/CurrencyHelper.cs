namespace Shoopi.Helper
{
    public static class CurrencyHelper
    {
        public static string ToVnd(this decimal price)
        {
            return $"{price:N0} VND";
        }
    }
}
