using EquinoxsModUtils;
using EquinoxsModUtils.Additions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace VoidChests
{
    public static class ContentAdder
    {
        public static void AddVoidChest() {
            NewResourceDetails details = new NewResourceDetails() {
                name = "Void Chest",
                description = "Voids all items inserted into it",
                craftingMethod = CraftingMethod.Assembler,
                craftTierRequired = 0,
                headerTitle = "Logistics",
                subHeaderTitle = "Utility",
                maxStackCount = 50,
                sortPriority = 998,
                unlockName = EMU.Names.Unlocks.BasicLogistics,
                parentName = EMU.Names.Resources.Container,
                sprite = EMU.Images.LoadSpriteFromFile("VoidChests.Images.VoidChest.png")
            };

            ChestDefinition definition;
            definition = ScriptableObject.CreateInstance<ChestDefinition>();
            definition.inventorySizes = new List<Vector2Int>() { new Vector2Int(1, 1) };
            definition.numberOfSlots = 1;

            EMUAdditions.AddNewMachine(definition, details);

            EMUAdditions.AddNewRecipe(new NewRecipeDetails() {
                GUID = VoidChestsPlugin.MyGUID,
                craftingMethod = CraftingMethod.Assembler,
                craftTierRequired = 0,
                duration = 0.1f,
                ingredients = new List<RecipeResourceInfo>() {
                    new RecipeResourceInfo() {
                        name = EMU.Names.Resources.IronComponents,
                        quantity = 8
                    },
                    new RecipeResourceInfo() {
                        name = EMU.Names.Resources.IronIngot,
                        quantity = 6
                    }
                },
                outputs = new List<RecipeResourceInfo>() {
                    new RecipeResourceInfo() {
                        name = "Void Chest",
                        quantity = 1
                    }
                },
                sortPriority = 10,
                unlockName = EMU.Names.Unlocks.BasicLogistics
            });
        }
    }
}
