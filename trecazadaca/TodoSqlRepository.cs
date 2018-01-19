using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trecazadaca;
using static trecazadaca.TodoItem;

namespace prvi
{
    public class TodoSqlRepository : ITodoRepository
    {
        private readonly TodoDbContext _context;
        public TodoSqlRepository(TodoDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(TodoItem todoItem)
        {
            if (_context.TodoItem.Any(s => s.Id == todoItem.Id))
                throw new DuplicateTodoItemException(" duplicate id: { " + todoItem.Id + "}");
            _context.TodoItem.Add(todoItem);
            await _context.SaveChangesAsync();
        }

        public async Task<TodoItem> GetAsync(Guid todoId, Guid userId)
        {
            var x = await _context.TodoItem.FirstAsync(s => s.Id.Equals(todoId));
            if (x == null)
                return null;
            if (!x.UserId.Equals(userId))
                throw new DuplicateTodoItemException(" duplicate id: { " + todoId + "}");
            return x;
        }

        public async Task<List<TodoItem>> GetActiveAsync(Guid userId)
        {
            return await _context.TodoItem.Where(s => s.UserId.Equals(userId) && !s.IsCompleted).ToListAsync();
        }

        public async Task<List<TodoItem>> GetAllAsync(Guid userId)
        {
            return await _context.TodoItem.Where(s => s.UserId.Equals(userId)).OrderByDescending(s => s.DateCreated).ToListAsync();
        }

        public async Task<List<TodoItem>> GetCompletedAsync(Guid userId)
        {
            return await _context.TodoItem.Where(s => s.UserId.Equals(userId) && s.IsCompleted).ToListAsync();
        }

        public async Task<List<TodoItem>> GetFilteredAsync(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            return await _context.TodoItem.Where(s => s.UserId.Equals(userId) && filterFunction(s)).ToListAsync();
        }

        public async Task<bool> MarkAsCompletedAsync(Guid todoId, Guid userId)
        {
            var x = await _context.TodoItem.FirstOrDefaultAsync(s => s.Id.Equals(todoId));

            if (x == null) return false;

            if (!x.UserId.Equals(userId))
                throw new TodoAccessDeniedException("User " + userId + " is not the owner of the TodoItem.");
            x.MarkAsCompleted();
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> RemoveAsync(Guid todoId, Guid userId)
        {
            var x = await _context.TodoItem.FirstAsync(s => s.Id.Equals(todoId));

            if (x == null) return false;

            if (!x.UserId.Equals(userId))
                throw new TodoAccessDeniedException("User " + userId + " is not the owner of the TodoItem.");

            _context.TodoItem.Remove(x);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task UpdateAsync(TodoItem todoItem, Guid userId)
        {

            if (!_context.TodoItem.Contains(todoItem))
                await AddAsync(todoItem);


            if (!todoItem.UserId.Equals(userId))
                throw new TodoAccessDeniedException("User " + userId + " is not the owner of the TodoItem.");

            _context.Entry(GetAsync(todoItem.Id, userId)).CurrentValues.SetValues(todoItem);
        }
    //    public void AddLabel(TodoItemLabel item)
        //{
      //      // ensuring that the label won't be put to the database if the same label allready exists
      //      TodoItemLabel todoItemLabel = _context.TodoItemLabel.FirstOrDefault(l => l.Value == item.Value);
        //    if (todoItemLabel == null)
          //      _context.TodoItemLabel.Add(item);
            //    _context.SaveChanges();
            //}

    }

}
