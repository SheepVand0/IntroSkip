using BeatSaberPlus.SDK.UI;
using CP_SDK.XUI;
using System;
using System.Collections;
using System.Diagnostics;

namespace SheepIntroSkip.Core
{
    internal class TextViewController : ViewController<TextViewController>
    {
        protected override void OnViewCreation()
        {
            XUIText l_Text;
            XUIVLayout.Make(
                l_Text = XUIText.Make(string.Empty)
                .SetFontSize(10)
            ).SetHeight(20)
             .BuildUI(transform);

            bool l_IsFpfc = false;
            
            foreach (var l_Variable in Environment.GetCommandLineArgs())
            {
                //Plugin.Log.Info($"{l_Variable.Key} {l_Variable.Value}");

                if (l_Variable.ToLower() == "fpfc")
                {
                    l_IsFpfc = true;
                    break;
                }
            }

            if (l_IsFpfc)
                l_Text.SetText("Press J to skip");
            else
                l_Text.SetText("Press trigger to skip");
        }
    }
}
