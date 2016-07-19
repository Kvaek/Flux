using MyvarCraft.Core.Objects.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Objects
{
    public interface IWorldGenerator
    {
        Chunck GetChunck(int x, int y);
    }
}
