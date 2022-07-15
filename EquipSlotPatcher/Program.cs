using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.FormKeys.SkyrimSE;
namespace EquipSlotPatcher
{
    public class Program
    {
        static List<WeaponAnimationType> TwoHandedMelees = new() { WeaponAnimationType.TwoHandSword, WeaponAnimationType.TwoHandAxe };
        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetTypicalOpen(GameRelease.SkyrimSE, "YourPatcher.esp")
                .Run(args);
        }

        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            foreach (var weapon in state.LoadOrder.PriorityOrder.Weapon().WinningOverrides())
            {
                IWeapon? nw = null;
                if (weapon.Data != null && TwoHandedMelees.Contains(weapon.Data.AnimationType))
				{
					nw = nw == null ? state.PatchMod.Weapons.GetOrAddAsOverride(weapon)! : nw!;
					nw.EquipmentType.SetTo(Skyrim.EquipType.EitherHand);
                    Console.WriteLine("Changing " + nw.Name + " equipslot to EitherHand");
                }
			}
        }
    }
}