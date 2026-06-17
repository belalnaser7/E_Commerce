using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
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

    [Authorize(Roles = "Customer")]
    [HttpPost]
    public IActionResult Create(CreateCategoryDto dto)
    {
        var result = _categoryService.Add(dto);

        if (!result)
            return BadRequest("Invalid category data");

        throw new Exception("Test Exception");
    }

    // 🟢 Get All Categories
   
    [Authorize("CanShow")]
    [HttpGet]
    public IActionResult GetAll()
    {
        var categories = _categoryService.GetAll();
        return Ok(categories);
    }

    [Authorize("CanShow")]
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var category = _categoryService.GetById(id);

        if (category is null)
            return NotFound("Category not found");

        return Ok(category);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateCategotyDto dto)
    {
        var result = _categoryService.Update(id, dto);

        if (!result)
            return NotFound("Category not found");

        return Ok("Category updated successfully");
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _categoryService.Del(id);

        if (!result)
            return NotFound("Category not found");

        return Ok("Category deleted successfully");
    }
}