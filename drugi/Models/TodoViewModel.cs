using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trecazadaca;

namespace drugi.Models
{
    public class TodoViewModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }

        public bool IsCompleted
        {
            get
            {
                return DateCompleted.HasValue;
            }
        }
        public DateTime? DateCompleted { get; set; }
        public DateTime DateCreated { get; set; }
        public TodoViewModel(string text)
        {
            Id = Guid.NewGuid();

            DateCreated = DateTime.UtcNow;
            Text = text;
        }

        public bool MarkAsCompleted()
        {
            if (!IsCompleted)
            {
                DateCompleted = DateTime.Now;
                return true;
            }
            return false;
        }
        public override bool Equals(object obj)
        {
            return this.Id == (obj as TodoItem)?.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public Guid UserId { get; set; }
        
        public List<TodoItemLabel> Labels { get; set; }
        public DateTime? DateDue { get; set; }
        public TodoViewModel(string text, Guid userId)
        {
            Id = Guid.NewGuid();
            Text = text;
            DateCreated = DateTime.UtcNow;
            UserId = userId;
            Labels = new List<TodoItemLabel>();
        }
        public TodoViewModel()
        {
           
        }
        public string TimeLeft()
        {
            if (DateDue.HasValue && !IsCompleted)
            {
                int days = ((DateTime) DateDue - DateTime.Now).Days;
                if (days >= 0) return "(za " + days.ToString() + " dana!)";
                return "(Deadline has passed!)";
            }
            return "";
        }
        public object GetDate()
        {   if(DateDue.HasValue)
                if (!IsCompleted)
                    return ((DateTime)DateDue).ToShortDateString();
            return DateCompleted.HasValue == true ? ((DateTime)DateCompleted).ToShortDateString() : "";
        }
        public class TodoItemLabel
        {
            public Guid Id { get; set; }
            public string Value { get; set; }
            /// <summary >
            /// All TodoItems that are associated with this label
            /// </ summary >
            public List<TodoItem> LabelTodoItems { get; set; }
            public TodoItemLabel(string value)
            {
                Id = Guid.NewGuid();
                Value = value;
                LabelTodoItems = new List<TodoItem>();
            }
        }
    }
}
