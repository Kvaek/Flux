using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flux.Core.Objects.Meta;

namespace Flux.Core.Objects.WorldGenerators {
	public class FlatLand : IWorldGenerator {
		public Chunk GetChunk(int ax, int ay) {
			Chunk ch = new Chunk();

			for (int y1 = 0; y1 < 4; y1++)
			for (int x1 = 0; x1 < 16; x1++)
			for (int z1 = 0; z1 < 16; z1++)
				ch.RawData.Set(x1, y1, z1, (1 << 4) | 0);

			for (int x1 = 0; x1 < 16; x1++)
			for (int z1 = 0; z1 < 16; z1++)
				ch.RawData.Set(x1, 4, z1, (2 << 4) | 0);

			return ch;
		}
	}
}