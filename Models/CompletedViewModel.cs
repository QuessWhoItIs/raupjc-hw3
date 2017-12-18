using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace drugi.Models
{
    public class CompletedViewModel
    {
        public List<TodoViewModel> TodoViewModels { get; set; }

        public CompletedViewModel(List<TodoViewModel> todoViewModels)
        {
            TodoViewModels = todoViewModels;
        }
    }
}
