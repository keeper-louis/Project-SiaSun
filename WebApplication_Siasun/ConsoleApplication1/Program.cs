using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceReference1.BD_MATERIAL_View _bdMaterialService = new ServiceReference1.BD_MATERIAL_View();
            _bdMaterialService.CreateOrgId = 0;
            _bdMaterialService.Number = "";

        }
    }
}
