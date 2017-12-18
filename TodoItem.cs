using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trecazadaca
{
    public class TodoItem
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        // Shorter syntax for single line getters in C#6
        // public bool IsCompleted = > DateCompleted . HasValue ;
        public bool IsCompleted
        {
            get
            {
                return DateCompleted.HasValue;
            }
            set { }
        }
        public DateTime? DateCompleted { get; set; }
        public DateTime DateCreated { get; set; }
        public TodoItem(string text)
        {
            // Generates new unique identifier
            Id = Guid.NewGuid();
            // DateTime .Now returns local time , it wont always be what you expect
            //(depending where the server is).
            // We want to use universal (UTC ) time which we can easily convert to
            //local when needed.
            // ( usually done in browser on the client side )
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

        /// <summary >
        /// User id that owns this TodoItem
        /// </ summary >
        public Guid UserId { get; set; }
        /// < summary >
        /// List of labels associated with TodoItem
        /// </ summary >
        public List<TodoItemLabel> Labels { get; set; }
        /// <summary >
        /// Date due . If null , no date was set by the user
        /// </ summary >
        public DateTime? DateDue { get; set; }
        public TodoItem(string text, Guid userId)
        {
            Id = Guid.NewGuid();
            Text = text;
            DateCreated = DateTime.UtcNow;
            UserId = userId;
            Labels = new List<TodoItemLabel>();
        }
        public TodoItem()
        {
            // entity framework needs this one
            // not for use :)
        }
        /// <summary >
        /// Label describing the TodoItem .
        /// e.g. Critical , Personal , Work ...
        /// </ summary >
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
