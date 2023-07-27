namespace Servirform.Models.DTO;

public class DataPaginatorDTO<T>
{
    public List<T> Data { get; set; } = new List<T>();
    public Paginator? Paginator { get; set; }
}

public class Paginator
{
    public int CurrentPage { get; set; }
    public int LastPage { get; set; }
    public PaginatorItems? Items { get; set; } = new PaginatorItems();

}

public class PaginatorItems
{
    public int count { set; get; }
    public int total { get; set; }
}
