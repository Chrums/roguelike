using System;
using System.Collections.Generic;
using Fizz6.Roguelike.Trainer;
using Fizz6.Roguelike.World.Region.Zone;
using Newtonsoft.Json;

namespace Fizz6.Roguelike.Spawner
{
    [Serializable]
    public class TrainerSpawnerImplementation : Spawner.Implementation
    {
        public class TrainerSpawnable : Spawnable<Trainer.Trainer>
        {
            [JsonProperty]
            public TrainerData TrainerData { get; private set; }

            public TrainerSpawnable(int level, ZoneType zoneType) =>
                TrainerData = TrainerData.Generate(level);
            
            protected override void Initialize()
            {
                base.Initialize();
                Component.Initialize(TrainerData);
            }
        }

        public override ISpawnable Spawn(ZoneData zoneData) => 
            new TrainerSpawnable(zoneData.Level, zoneData.ZoneType);
    }
}