using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FLUX.ECS
{
    public class World
    {
        public List<EntityBase> Entities = new List<EntityBase>();

        public int EntityCount
        {
            get
            {
                return Entities.Count;
            }
        }

        public void AddEntity(EntityBase entity)
        {
            entity.Id = EntityCount+1;
            Entities.Add(entity);
        }

        public void RemoveEntity(EntityBase ent)
        {
            Entities.Remove(ent);
        }

        public EntityBase GetEntity(int id)
        {
            return Entities.FirstOrDefault(e => e.Id == id);
        }

        public string SerializeToJSON()
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                IncludeFields = true,
                PreferredObjectCreationHandling = System.Text.Json.Serialization.JsonObjectCreationHandling.Replace,
                WriteIndented = true,
            };
            return JsonSerializer.Serialize(this, typeof(World), options);
        }

        public static World DeserializeFromJSON(string json)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                IncludeFields = true,
                PreferredObjectCreationHandling = System.Text.Json.Serialization.JsonObjectCreationHandling.Replace
            };
            return JsonSerializer.Deserialize<World>(json, options);
        }

        public World() { }
    }
}
