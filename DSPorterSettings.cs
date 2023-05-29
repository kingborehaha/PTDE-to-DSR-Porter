using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSRPorter
{
    public static class DSPorterSettings
    {
        public static bool Setting_CompileLua;

#if DEBUG
        public const bool IS_SOTE = true;
#else
        public const bool IS_SOTE = false;
#endif
    }
}
