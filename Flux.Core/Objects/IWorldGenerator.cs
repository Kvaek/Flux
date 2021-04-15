using Flux.Core.Objects.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Core.Objects {
	public interface IWorldGenerator {
		Chunk GetChunk(int x, int y);
	}
}