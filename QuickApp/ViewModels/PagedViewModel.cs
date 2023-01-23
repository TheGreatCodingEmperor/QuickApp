namespace QuickApp.ViewModels
{
    public class PagedViewModel<TEntity>:PageHeader
    where TEntity: class
    {
        public PagedViewModel(
            int currentPage, 
            int itemsPerPage, 
            int totalItems, 
            int totalPages,
            TEntity list) : base(
                currentPage, 
                itemsPerPage, 
                totalItems, 
                totalPages)
        {
            this.List = list;
        }
        public TEntity List {get;set;}
    }
}