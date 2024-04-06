using CP_SDK.UI;
using CP_SDK.XUI;
using SheepIntroSkip.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SheepIntroSkip.UI
{
    internal class SettingsView : ViewController<SettingsView>
    {
        protected override void OnViewCreation()
        {
            Templates.FullRectLayoutMainView(
                XUIText.Make("Delay before pressing trigger works: "),
                XUISlider.Make()
                    .SetMinValue(1)
                    .SetMaxValue(1000)
                    .SetValue(ISConfig.Instance.PressDuration)
                    .SetInteger(true)
                    .OnValueChanged(x =>
                    {
                        ISConfig.Instance.PressDuration = (int)x;
                    }),
                XUIText.Make("After skip, time before new notes spawn: "),
                XUISlider.Make()
                    .SetMinValue(0.7f)
                    .SetMaxValue(2)
                    .SetValue(ISConfig.Instance.BeforeNoteTime)
                    .OnValueChanged(x =>
                    {
                        ISConfig.Instance.BeforeNoteTime = x;
                    }),
                XUIText.Make("Minimum pause length: "),
                XUISlider.Make()
                    .SetMinValue(3)
                    .SetMaxValue(10)
                    .SetValue(ISConfig.Instance.MinimumDelayToBeSkippable)
                    .OnValueChanged(x =>
                    {
                        ISConfig.Instance.MinimumDelayToBeSkippable = x;
                    })
            ).BuildUI(transform);
        }

    }
}
