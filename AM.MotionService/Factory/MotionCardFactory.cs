using AM.Model.Interfaces.MotionCard;
using AM.Model.MotionCard;
using AM.MotionCard.Googo;
using AM.MotionService.Leisai;
using AM.MotionService.Virtual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.MotionService.Factory
{
    public static class MotionCardFactory
    {
        public static IMotionCardService Create(MotionCardConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            switch (config.CardType)
            {
                case MotionCardType.GOOGO:
                    return new GoogoMotionCardService(config);

                case MotionCardType.LEISAI:
                    return new LeisaiMotionCardService(config);

                case MotionCardType.VIRTUAL:
                    return new VirtualMotionCardService(config);

                default:
                    return new VirtualMotionCardService(config);
                    throw new NotSupportedException($"不支持的运动控制卡类型: {config.CardType}");
            }
        }
    }










}
