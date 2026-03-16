using CP_SDK_BS.UI;
using CP_SDK.XUI;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace SheepIntroSkip.Core
{
    internal class TextViewController : ViewController<TextViewController>
    {
        XUIText VeryUsefulInfoText;

        protected override void OnViewCreation()
        {
            XUIText l_Text;
            XUIVLayout.Make(
                l_Text = XUIText.Make(string.Empty)
                .SetFontSize(10),
                VeryUsefulInfoText = XUIText.Make(string.Empty)
                .SetFontSize(5)
                .SetColor(Color.yellow)
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

        public void SetUsefulInfoTextValue(EValueType valueType)
        {
            VeryUsefulInfoText.SetText(SerializeValueType(valueType));
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////
        
        public enum EValueType
        {
            Nothing,
            LeanLeft,
            LeanRight,
            LeanLeftFar,
            LeanRightFar,
            Crouch,
            THEREISBOMBS
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////

        public static string SerializeValueType(EValueType valueType)
        {
            switch (valueType)
            {
                case EValueType.LeanLeft:
                    return "Lean left";
                case EValueType.LeanRight:
                    return "Lean right";
                case EValueType.LeanLeftFar:
                    return "Lean left far";
                case EValueType.LeanRightFar:
                    return "Lean right far";
                case EValueType.Crouch:
                    return "Crouch";
                case EValueType.THEREISBOMBS:
                    return "THERE IS BOMBS";
                case EValueType.Nothing:
                    return string.Empty;
                default:
                    return string.Empty;
            }
        }
    }
}
