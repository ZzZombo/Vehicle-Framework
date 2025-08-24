using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmashTools;
using Verse;

namespace Vehicles;

public class DropZoneCache : DetachedMapComponent
{
	private readonly List<DropZone> dropZones = [];

	protected DropZoneCache(Map map) : base(map)
	{
	}

	public int DropZoneCount => dropZones.Count;

	public static void CalculateDropZones(Map map)
	{
		const int AnchorPoints = 3;

		IntVec2 size = map.Size.ToIntVec2;

		// 6 connections for a square (2 parallel, 4 diagonal)
		for (int s = 0; s < 6; s++)
		{
			for (int i = 0; i < AnchorPoints; i++)
			{
				int x = size.x * i / AnchorPoints;
				for (int j = 0; j < AnchorPoints; j++)
				{
					int z = size.z * j / AnchorPoints;
				}
			}
		}
	}
}