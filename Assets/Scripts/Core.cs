using System;
using System.IO;
using System.Threading.Tasks;
using Fizz6.Roguelike.World.Region;
using Fizz6.Roguelike.World.Region.Zone;
using Fizz6.Serialization;
using UnityEngine;

namespace Fizz6.Roguelike
{
    public static class Core
    {
        private const string DataPath = "Assets/Resources/data.json";
        
        public static async Task Save()
        {
            var serializer = new Serializer();
            
            var regionData = Region.Instance.RegionData;
            var regionJson = serializer.Serialize(regionData);

            var zoneData = Zone.Instance.ZoneData;
            var zoneJson = serializer.Serialize(zoneData);
            
            Debug.LogError(zoneJson);
            
            var writer = new StreamWriter(DataPath);
            await writer.WriteAsync(zoneJson);
            writer.Close();
        }

        public static async Task Load()
        {
            // string json = null;
            //
            // try
            // {
            //     var reader = new StreamReader(DataPath);
            //     json = await reader.ReadToEndAsync();
            //     reader.Close();
            // }
            // catch (Exception exception)
            // {
            //     // No existing data
            // }
            //
            // var serializer = new Serializer();
            // var regionData = json == null
            //     ? null
            //     : serializer.Deserialize<RegionData>(json);
            // Region.Instance.Load(regionData);
            Zone.Instance.Load();
        }
    }
}