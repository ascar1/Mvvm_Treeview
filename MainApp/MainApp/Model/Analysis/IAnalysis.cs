using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Model.Analysis
{
    public interface IAnalysis
    {
        List<string> ResultArr ();
        string result();
        void GetAnalysis();
    }

}
