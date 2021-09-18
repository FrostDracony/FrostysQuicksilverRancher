using Configs;
using System.Linq;
using SRML.Console;
using System.Collections.Generic;
using UnityEngine;
using FrostysQuicksilverRancher.Other;

namespace FrostysQuicksilverRancher.Other
{
    class PrintComponentsOfGadgetCommand : ConsoleCommand
    {
        // The command's command ID; how someone using the console will call your command.
        public override string ID => "print_components_of_gadget";

        // What will appear when the command is used incorrectly or viewed via the help command.
        // Remember, <> is a required argument while [] is an optional one.
        public override string Usage => "print_components_of_gadget <argument>";

        // A description of the command that will appear when using the help command.
        public override string Description => "From the \"FrostDracony's QuicksilverRancher\" mod, with this command you can print the Components of a Gadget. Used for Debugging.";

        // The code that the command runs. Requires you to return a bool.
        public override bool Execute(string[] args)
        {
            // Checks if the code has enough arguments.
            if (args == null || args.Length > 1)
            {
                Console.LogError("Incorrect amount of arguments!", true);
                return false;
            }

            if (System.Enum.TryParse(args[0], out Gadget.Id id))
            {
                GameContext.Instance.LookupDirector.GetGadgetDefinition(id).prefab.PrintComponents<Component>();
            }

            return true;

        }

        // A list that the autocomplete references from. You must return a List<string>.
        public override List<string> GetAutoComplete(int argIndex, string argText)
        {
            List<string> result;
            if (argIndex == 0)
            {
                result = System.Enum.GetValues(typeof(Gadget.Id)).Cast<Gadget.Id>().Select(value => value.ToString()).ToList();
            }
            else
            {
                // If you don't need arguments, replace the if statement with this line of code.
                result = base.GetAutoComplete(argIndex, argText);
            }
            return result;
        }
    }


    class ChangeVacVisualCommand : ConsoleCommand
    {
        // The command's command ID; how someone using the console will call your command.
        public override string ID => "change_vac_visual";

        // What will appear when the command is used incorrectly or viewed via the help command.
        // Remember, <> is a required argument while [] is an optional one.
        public override string Usage => "change_vac_visual <argument>";

        // A description of the command that will appear when using the help command.
        public override string Description => "From the \"FrostDracony's QuicksilverRancher\" mod, with this command you can change the renderer mode of your Vacpack (that only works when you already bought the upgrade).";

        // The code that the command runs. Requires you to return a bool.
        public override bool Execute(string[] args)
        {
            // Checks if the code has enough arguments.
            if (args == null || args.Length > 1)
            {
                Console.LogError("Incorrect amount of arguments!", true);
                return false;
            }
            bool flag = SRSingleton<SceneContext>.Instance.PlayerState.HasUpgrade(Ids.MOCHI_HACK);
            if (flag)
            {
                if (args[0].ToLower() == "automatic")
                {
                    Console.LogError("Not implemented yet");
                    return true;
                }

                Console.LogSuccess("Changed Vacpack renderer mode to: " + args[0], true);

                if (System.Enum.TryParse(args[0], out VACPACK_ENUMS vACPACK))
                {
                    PlayerState.AmmoMode ammoMode = (vACPACK == VACPACK_ENUMS.NIMBLE_VALLEY) ? PlayerState.AmmoMode.NIMBLE_VALLEY : PlayerState.AmmoMode.DEFAULT;
                    Object.FindObjectOfType<VacDisplayChanger>().SetDisplayMode(ammoMode);
                }

                //foreach (VACPACK_ENUMS vacPACK in System.Enum.GetValues(typeof(VACPACK_ENUMS))) {}

            }
            else
            {
                Console.LogError("You have to first have bought the \"Gate Hack\" upgrade, else you can't use this command");
            }
            return true;

        }

        // A list that the autocomplete references from. You must return a List<string>.
        public override List<string> GetAutoComplete(int argIndex, string argText)
        {
            // Checks which argument you're on.
            List<string> result;
            if (argIndex == 0)
            {
                result = System.Enum.GetValues(typeof(VACPACK_ENUMS)).Cast<VACPACK_ENUMS>().Select(value => value.ToString()).ToList();
            }
            else
            {
                // If you don't need arguments, replace the if statement with this line of code.
                result = base.GetAutoComplete(argIndex, argText);
            }
            return result;
        }
    }
}
