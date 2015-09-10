using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Api.Map.Generator
{
    public class FlatLandGenerator : GenericGenerator
    {
        public override World GenWorld()
        {
            return new World();
        }
    }
}
