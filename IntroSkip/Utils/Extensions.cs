using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SheepIntroSkip.Utils
{
    internal static class Extensions
    {

        public static void ForEach<t_ItemType>(this t_ItemType[] array, Action<t_ItemType> callback)
        {
            int l_Index = 0;
            foreach (var l_Item in array)
            {
                try
                {
                    callback.Invoke(l_Item);
                } catch (Exception ex)
                {
                    Plugin.Log.Error($"Error in foreach, iteration : {l_Index}");
                }
                l_Index += 1;
            }
        }

    }
}
