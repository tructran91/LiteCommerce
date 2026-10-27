namespace Catalog.Core.Enums
{
    // Values are explicit because they are persisted as integers. 1 was Super, dropped
    // when variants became their own entity; don't reuse it.
    public enum ProductLinkType
    {
        Related = 2,

        CrossSell = 3,

        UpSell = 4
    }
}
