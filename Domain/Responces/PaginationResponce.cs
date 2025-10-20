using System.Net;

namespace Domain.Responces;

public class PaginationResponce<T> : Responce<T>
{
    public int PageNumber {get;set;}
    public int PageSize {get;set;}
    public int TotalPages {get;set;}
    public int TotalRecords {get;set;}
    
    public PaginationResponce(T data, int totalRecords,int pageNumber,int pageSize) : base(data)
    {
        TotalPages= (int)Math.Ceiling((double)totalRecords / pageSize);
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;
    }
    public PaginationResponce(HttpStatusCode statusCode, string message) : base(statusCode, message)
    {
    }
}