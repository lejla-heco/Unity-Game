using System;
namespace Spacecraft.Consts
{
	public static class GameConsts
	{
		public static int ChunkLength = 100;
		public static int InitialChunksNumber = 3;
		public static int ChunkGenerationOffset = 40; // we need this to specify initial offset for each chunk, if we generate it at 0, 50 will be in negative
		public static float GravityValue = -9.81f;

		public static int HowManyUnitsUntilWorldResets = 400;

		public static Random Rnd = new Random();

	}
}
