using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using SmashTools;
using Verse;

namespace Vehicles;

public static class DropZoneFinder
{
	private const string MountainCategory = "Mountain";

	public static bool CanAirdropInMap(this Map map)
	{
		IList<TileMutatorDef> mutators = map.Tile.Tile.Mutators;
		if (!mutators.NotNullAndAny(InvalidDropArea))
			return false;

		return map.GetDetachedMapComponent<DropZoneCache>().DropZoneCount > 0;

		static bool InvalidDropArea(TileMutatorDef def)
		{
			if (def.IsCave)
				return true;

			return !def.categories.NullOrEmpty() && def.categories.Contains(MountainCategory);
		}
	}
}