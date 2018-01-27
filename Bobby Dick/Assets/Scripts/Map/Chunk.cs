
using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public enum ChunkState {
	//
	UndiscoveredSea,
	DiscoveredSea,
	UndiscoveredIsland,
	DiscoveredIsland,
	VisitedIsland

}

public class Chunk
{
	public static Dictionary<Coords,Chunk> chunks = new Dictionary<Coords, Chunk>();

	public ChunkState State;
	private IslandData islandData;

	public Chunk () {
		
	}

	public void SetIslandData (IslandData _islandData ) {
		State = ChunkState.UndiscoveredIsland;
		islandData = _islandData;
	}

	public IslandData IslandData {
		get {
			return islandData;
		}
		set {
			islandData = value;
		}
	}

	public static Chunk currentChunk {
		get {
			return chunks[Boats.playerBoatInfo.coords];
		}
	}

	public static Chunk GetChunk (Coords c) {
		
		if (chunks.ContainsKey (c) == false) {
			return chunks [new Coords ()];
		}

		return chunks [c];
	}
}

