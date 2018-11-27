using System;
using System.Collections.Generic;
using System.Text;

namespace Seed
{
    public interface IAttack
    {
        bool IsAggressive { get; }
        void Attack(Character character, World world);
    }
}
