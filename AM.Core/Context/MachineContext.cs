using AM.Model.Interfaces;
using AM.Model.Interfaces.MotionCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Core.Context
{
    public sealed class MachineContext
    {
        public static MachineContext Instance { get; } = new MachineContext();

        public IMotionCardService MotionCard { get; set; }

        //public IPLC Plc { get; set; }

        //public ICamera Camera { get; set; }
    }
}
