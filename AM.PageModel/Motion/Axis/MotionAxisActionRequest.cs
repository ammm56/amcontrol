using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.PageModel.Motion.Axis
{
    public sealed class MotionAxisActionRequest
    {
        public MotionAxisActionKey ActionKey { get; set; }

        public string TargetPositionText { get; set; }

        public string MoveDistanceText { get; set; }

        public string VelocityText { get; set; }
    }
}
