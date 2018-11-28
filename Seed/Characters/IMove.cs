using System;
using System.Collections.Generic;
using System.Text;

namespace Seed
{
    public interface IMove
    {
        bool IsLazy { get; }
        void Move(Direction direction);
    }
}
