namespace Catalog.Application.Requests
{
    public class UpdateCategoryRequest : CreateCategoryRequest
    {
        public bool RemoveThumbnail { get; set; }
    }
}
