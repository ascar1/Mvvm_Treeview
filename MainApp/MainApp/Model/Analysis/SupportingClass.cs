using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Model.Analysis
{
    /// <summary>
    /// Класс где реализованны вспомогательные функции 
    /// </summary>
    
    class SupportingClass
    {
        // TODO: Реализовать нормализацию данных

        private static SupportingClass instance;
        private SupportingClass()
        {

        }
        public List<double> GetNormData ()
        {
            return null;
        }
        public static SupportingClass getInstance()
        {
            if (instance == null)
                instance = new SupportingClass();
            return instance;
        }
    }
}
