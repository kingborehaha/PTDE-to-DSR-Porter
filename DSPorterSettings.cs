using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSRPorter
{
    public static class DSPorterSettings
    {
        public static bool CompileLua = true;

#if DEBUG
        public static bool IS_SOTE = true;
#else
        public static bool IS_SOTE = false;
#endif
    }
}
