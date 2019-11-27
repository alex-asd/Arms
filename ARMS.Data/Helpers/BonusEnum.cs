using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARMS.Data.Helpers
{
    public static class BonusEnum
    {
        public enum UpsertType
        {
            Upsert = 0,
            Insert = 1,
            Update = 2
        }
    }
}