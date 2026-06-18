using ECommerce.Api.Extentions;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Result_pattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly IServicesCategory _categoryService;

    public CategoryController(IServicesCategory categoryService)
    {
        _categoryService = categoryService;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult Create(CreateCategoryDto dto)
    {
        var result = _categoryService.Add(dto);

        return result.ToActionResult();
    }

    // 🟢 Get All Categories
   
    [Authorize("CanShow")]
    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _categoryService.GetAll();
        return result.ToActionResult();
     
    }

    [Authorize("CanShow")]
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var result = _categoryService.GetById(id);

        return result.ToActionResult();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateCategotyDto dto)
    {
        var result = _categoryService.Update(id, dto);

        return result.ToActionResult();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _categoryService.Del(id);

        return result.ToActionResult();
    }
}