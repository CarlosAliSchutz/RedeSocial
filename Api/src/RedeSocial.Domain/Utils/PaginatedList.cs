namespace RedeSocial.Domain.Utils;

public class PaginatedList<T>
{
    public List<T> Itens { get; }
    public int Pagina { get; }
    public int TotalPaginas { get; }
    public int TotalItens { get; }

    public PaginatedList(List<T> items, int page, int totalPages, int totalItems)
    {
        Itens = items;
        Pagina = page;
        TotalPaginas = totalPages;
        TotalItens = totalItems;
    }
}
