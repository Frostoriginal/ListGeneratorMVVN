using ListGenerator.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListGenerator.ViewModel
{
    public class Generate_ViewModel:BaseViewModel
    {
        public DateTime timeSelectedReference { get; set; }
        public string timeSelectedString { get; set; } = "";

        public Generate_ViewModel()
        {
                timeSelectedReference = DateTime.Now;
        }

    }
}
