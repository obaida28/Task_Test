using API.DTOs;
using API.Helpers;
using AutoMapper;
using Core.Entites;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
// using System.Linq.Dynamic.Core;
using System.Net;

namespace API.Controllers;
public class CarController : ControllerBase 
{
    private readonly ICarRepository _repository; 
    private readonly IMapper _map;
    public CarController(ICarRepository repository , IMapper map)
    {
        _repository = repository;
        _map = map;
    }
    
    [HttpGet(template: "GetListAsync")]
    public async Task<PagingModel<Car>> GetListAsync(CarRequestDTO input) 
    {
        var query = _repository.GetQueryable();
        // var searchingResult = ApplySearching(query , input.SearchingColumn, input.SearchingValue);
        // int countFilterd = searchingResult.Count();
        // var sortingResult = searchingResult.ApplySorting(input.OrderByData);
        // var pagingResult = sortingResult.ApplyPaging(input.CurrentPage, input.RowsPerPage , false);
        // var finalQuery = await pagingResult.GetResult(input.CurrentPage, input.RowsPerPage , countFilterd);
        return new PagingModel<Car> {};
    }
    [ApiExplorerSettings(IgnoreApi = true)]
    private IQueryable<Car> ApplySearching(IQueryable<Car> query , string colName , string value)
    {
        switch(colName)
        {
            case "Id" : 
                return query.Where(c => c.Id == new Guid(value));
            case "Type" : 
                return query.Where(c => c.Type.Contains(value.ToString()));
            case "Color" : 
                return query.Where(c => c.Color.Contains(value.ToString()));
            case "EngineCapacity" : 
                return query.Where(c => c.EngineCapacity == Convert.ToDecimal(value));
            case "DailyRate" : 
                return query.Where(c => c.DailyRate == Convert.ToInt32(value));
            default :
                return query.Where(c => c.CarNumber.Contains(value.ToString()));
        }
    }
    [ApiExplorerSettings(IgnoreApi = true)]
    private IQueryable<Car> ApplySorting(IQueryable<Car> query , string colName = "default" , bool ASC = true)
    {
        switch(colName)
        {
            case "Id" : 
                return ASC ? query.OrderBy(c => c.Id) : query.OrderByDescending(c => c.Id);
            case "Type" : 
                return ASC ? query.OrderBy(c => c.Type) : query.OrderByDescending(c => c.Type);
            case "Color" : 
                return ASC ? query.OrderBy(c => c.Color) : query.OrderByDescending(c => c.Color);
            case "EngineCapacity" : 
                return ASC ? query.OrderBy(c => c.EngineCapacity) : query.OrderByDescending(c => c.EngineCapacity);
            case "DailyRate" : 
                return ASC ? query.OrderBy(c => c.DailyRate) : query.OrderByDescending(c => c.DailyRate);
            default :
                return ASC ? query.OrderBy(c => c.CarNumber) : query.OrderByDescending(c => c.CarNumber);
        }
    }
    [HttpGet("{id}")]
    public async Task<CarDTO> GetAsync(Guid id) 
    {
        var getOne = await _repository.GetByIdAsync(id);
        var result = _map.Map<CarDTO>(getOne);
        return result;
    }
    
    [HttpPost]
    public async Task<CarDTO> CreateAsync(CarCreateDto carCreateDto)
    {
        if (!ModelState.IsValid)
            throw new BadHttpRequestException("Validation failed. Please check the input and correct any errors.");
        bool isExist = await _repository.IsExistAsync(carCreateDto.Number);
        if(isExist) throw new BadHttpRequestException("The car number is unique !");
        var entity = _map.Map<Car>(carCreateDto);
        await _repository.AddAsync(entity);
        var res = _map.Map<CarDTO>(entity);
        return res;
    }

    [HttpPut("{id}")]
    public async Task UpdateAsync(Guid id, CarUpdateDto carUpdateDTO)
    {
        if (id != carUpdateDTO.Id)
            throw new BadHttpRequestException("Object id is not compatible with the pass id");
        Car car = _map.Map<Car>(carUpdateDTO);
        await _repository.UpdateAsync(car);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id) ?? 
            throw new BadHttpRequestException("This id is invalid");
        await _repository.DeleteAsync(entity);
    }
}