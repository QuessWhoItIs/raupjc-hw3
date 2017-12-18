using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using drugi.Data;
using drugi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using trecazadaca;

namespace drugi.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TodoController(ITodoRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null) return View();
            var activeItems = await _repository.GetActiveAsync(new Guid(currentUser.Id));
            var activeTodoList = Mapper.Map<List<TodoItem>, List<TodoViewModel>>(activeItems);
            return View(new IndexViewModel(activeTodoList));
        }
        public async Task<IActionResult> Completed()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var CompletedItems = await _repository.GetCompletedAsync(new Guid(currentUser.Id));
            var CompleteTodoList = Mapper.Map<List<TodoItem>, List<TodoViewModel>>(CompletedItems);
            return View(new CompletedViewModel(CompleteTodoList));
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddTodoViewModel item)
        {
            if (!ModelState.IsValid) return View();
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var _item = new TodoItem(item.Text, new Guid((currentUser.Id))) { DateDue = item.DateDue };

            if (item.Labels != null)
            {
                string[] labels = item.Labels.Split(',');

                foreach (var i in labels)
                {
                    var todoItemLabel = new TodoItem.TodoItemLabel(i.Trim());
                    if (_item.Labels.Contains(todoItemLabel))
                        continue;

                    _item.Labels.Add(todoItemLabel);
                }
            }
            await _repository.AddAsync(_item);
            return RedirectToAction("Index");
        }
        [HttpGet("RemoveFromCompleted/{Id}")]
        public async Task<IActionResult> RemoveFromCompleted(Guid id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            await _repository.RemoveAsync(id, new Guid(currentUser.Id));
            return RedirectToAction("Index");
        }

        [HttpGet("MarkAsCompleted/{Id}")]
        public async Task<IActionResult> MarkAsCompleted(Guid id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            await _repository.MarkAsCompletedAsync(id, new Guid(currentUser.Id));
            return RedirectToAction("Index");
        }
    }
}