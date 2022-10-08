using MeuTodo.Data;
using MeuTodo.Models;
using MeuTodo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace MeuTodo.Controller
{
    [ApiController]
    [Route(template:"v1")]
    public class ToDoController : ControllerBase
    {
        [HttpGet]
        [Route(template: "todos")]
        public async Task<IActionResult> GetAsync([FromServices] AppDbContext context)
        {
            var todos = await context.
                                ToDos.
                                AsNoTracking().
                                ToListAsync();

            return Ok(todos);
        }

        [HttpGet]
        [Route(template: "todos/{id}")]
        public async Task<IActionResult> GetByIdAsync(
                [FromServices] AppDbContext context, 
                [FromRoute] int id)
        {
            var todo = await context.
                                ToDos.
                                AsNoTracking().
                                FirstOrDefaultAsync(x => x.Id == id);

            return todo == null ? NotFound() : Ok(todo);
        }

        [HttpPost]
        [Route(template: "todos")]
        public async Task<IActionResult> PostAsync(
                [FromServices] AppDbContext context,
                [FromBody] CreateTodoViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var todo = new ToDo
            {
                Date = DateTime.Now,
                Done = false,
                Title = model.Title
            };

            try
            {
                await context.ToDos.AddAsync(todo);
                await context.SaveChangesAsync();
                return Created(uri: $"v1/todos/{todo.Id}", todo);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route(template: "todos/{id}")]
        public async Task<IActionResult> PutAsync(
                [FromServices] AppDbContext context,
                [FromBody] ModifyTodoViewModel model,
                [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var todo = await context.ToDos.FirstAsync(x => x.Id == id);

            if (todo == null)
                return NotFound();



            try
            {
                todo.Title = model.Title;

                context.ToDos.Update(todo);
                await context.SaveChangesAsync();

                return Ok(todo);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route(template: "todos/{id}")]
        public async Task<IActionResult> DeleteAsync(
                [FromServices] AppDbContext context,
                [FromRoute] int id)
        {
            var todo = await context.ToDos.FirstOrDefaultAsync(x => x.Id == id);

            if (todo == null)
                return NotFound();

            try
            {   context.ToDos.Remove(todo);
                await context.SaveChangesAsync();

                return Ok(todo);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
