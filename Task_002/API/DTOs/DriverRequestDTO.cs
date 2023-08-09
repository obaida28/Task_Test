using API.Helpers;
using Core.Entites;
namespace API.DTOs;
public class DriverRequestDTO : RequestDTO<Driver>
{    
    public IQueryable<Driver> ApplySearching(IQueryable<Driver> query)
    {
        switch(SearchingColumn)
        {
            case "Id" : 
                return query.Where(d => d.Id == new Guid(SearchingValue));
            default :
                return query.Where(d => d.DriverName.Contains(SearchingValue));
        }
    }
    public IQueryable<Driver> ApplySorting(IQueryable<Driver> query)
    {
        switch(OrderByData)
        {
            case "Id" : 
                return ASC ? query.OrderBy(c => c.Id) : query.OrderByDescending(c => c.Id);
            default :
                return ASC ? query.OrderBy(c => c.DriverName) : query.OrderByDescending(c => c.DriverName);
        }
    }
}