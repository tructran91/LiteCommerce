namespace LiteCommerce.Shared.Constants
{
    public static class ErrorMessages
    {
        // One wording for every delete blocked by dependent records, e.g.
        // Cannot delete brand "Apple" because 2 products are still linked to it.
        public static string CannotDeleteInUse(string entityName, string displayName, int dependentCount, string dependentSingular, string dependentPlural)
        {
            var dependents = dependentCount == 1
                ? $"1 {dependentSingular} is"
                : $"{dependentCount} {dependentPlural} are";

            return $"Cannot delete {entityName} \"{displayName}\" because {dependents} still linked to it.";
        }
    }
}
