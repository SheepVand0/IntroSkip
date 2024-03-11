using System;
using System.Threading.Tasks;

namespace SheepIntroSkip.Core
{
    internal class WaitUtils
    {
        public static async Task<Task> Wait(
          Func<bool> p_Func,
          int p_ToleranceMs,
          int p_DelayAfter = 0,
          int p_MaxTryCount = 0,
          int p_CodeLine = -1)
        {
            int l_TryCount = 0;
            bool l_ShouldTryCount = p_MaxTryCount > 0;
            do
            {
                try
                {
                    if (!p_Func())
                    {
                        ++l_TryCount;
                        if (p_ToleranceMs > 0)
                            await Task.Delay(p_ToleranceMs);
                    }
                    else
                        break;
                }
                catch (Exception ex)
                {
                    Plugin.Log.Error(string.Format("[{0}][{1}]{2}", (object)nameof(WaitUtils), (object)nameof(Wait), (object)ex));
                    if (p_CodeLine != -1)
                        Plugin.Log.Error(string.Format("[{0}][{1}] At line {2}", (object)nameof(WaitUtils), (object)nameof(Wait), (object)p_CodeLine));
                }
            }
            while (l_ShouldTryCount ? l_TryCount < p_MaxTryCount : l_TryCount < 10000);
            if (p_DelayAfter != 0)
                await Task.Delay(p_DelayAfter);
            return Task.CompletedTask;
        }
    }
}
