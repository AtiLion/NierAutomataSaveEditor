using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NierAutomataSaveEditor
{
    public enum Offsets
    {
        MONEY_START = 0x3056C,
        MONEY_END = 0x3056F,
        EXP_START = 0x3871C,
        EXP_END = 0x3871F,
        ITEM_START = 0x30570,
        ITEM_END = 0x31D4D,
        TIME_START = 0x24,
        TIME_END = 0x26,
        NAME_START = 0x34,
        NAME_END = 0x52
    }
}
