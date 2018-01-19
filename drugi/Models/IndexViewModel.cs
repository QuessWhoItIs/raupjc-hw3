using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace drugi.Models
{
    public class IndexViewModel
    {
        public List<TodoViewModel> TodoViewModels { get; set; }

        public IndexViewModel(List<TodoViewModel> todoViewModels)
        {
            TodoViewModels = todoViewModels;
        }
    }
}
