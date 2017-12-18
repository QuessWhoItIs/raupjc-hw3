using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace trecazadaca
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(string cnnstr) : base(cnnstr) { }


            public IDbSet<TodoItem> TodoItem { get; set; }
        public IDbSet<TodoItem.TodoItemLabel> TodoItemLabel { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TodoItem>().HasKey(s => s.Id);
            modelBuilder.Entity<TodoItem>().Property(s => s.Id).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(s => s.Text).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(s => s.DateCreated).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(s => s.UserId).IsRequired();
            modelBuilder.Entity<TodoItem>().HasMany(s => s.Labels).WithMany(t => t.LabelTodoItems);

            modelBuilder.Entity<TodoItem.TodoItemLabel>().HasKey(s => s.Id);
            modelBuilder.Entity<TodoItem.TodoItemLabel>().Property(s => s.Id).IsRequired();
            modelBuilder.Entity<TodoItem.TodoItemLabel>().Property(s => s.Value).IsRequired();
            modelBuilder.Entity<TodoItem.TodoItemLabel>().HasMany(s => s.LabelTodoItems).WithMany(t => t.Labels);


        }
    }
}

