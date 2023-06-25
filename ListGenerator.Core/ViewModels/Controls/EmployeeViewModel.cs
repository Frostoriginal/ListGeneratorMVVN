using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListGenerator.Core.ViewModels
{
    public class EmployeeViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public bool IsSelected { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
