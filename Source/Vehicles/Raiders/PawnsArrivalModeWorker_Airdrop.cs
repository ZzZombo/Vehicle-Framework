using System.Collections.Generic;
using RimWorld;
using UnityEngine.Assertions;
using Verse;

namespace Vehicles.Raiders;

internal class PawnsArrivalModeWorker_Airdrop : PawnsArrivalModeWorker
{
	public override void Arrive(List<Pawn> pawns, IncidentParms parms)
	{
		Map map = parms.target as Map;
		Assert.IsNotNull(map);
		bool roofPunch = parms.faction != null && parms.faction.HostileTo(Faction.OfPlayer);

		//for (int i = 0; i < pawns.Count; i++)
		//{
		//	DropPodUtility.DropThingsNear(DropCellFinder.RandomDropSpot(map, true), map, Gen.YieldSingle<Thing>(pawns[i]),
		//		parms.podOpenDelay, false, true, parms.canRoofPunch ?? flag, true, true, parms.faction);
		//}
	}

	public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
	{
		return true;
	}
}