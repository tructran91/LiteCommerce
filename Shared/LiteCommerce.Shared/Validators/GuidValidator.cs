namespace LiteCommerce.Shared.Validators
{
    public static class GuidValidator
    {
        public static bool IsValidGuid(string id)
        {
            return Guid.TryParse(id, out _);
        }
    }
}
