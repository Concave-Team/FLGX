using FLUX.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLUX.ECS
{
    public class EntityBase
    {
        public int Id { get; internal set; }
        public string Name { get; internal set; } = string.Empty;
        public UniqueList<ComponentBase> Components = new UniqueList<ComponentBase>();

        public ComponentBase GetComponent<T>() where T : ComponentBase
        {
            return Components.Get<T>();
        }

        public void AddComponent<T>(T cmp) where T : ComponentBase
        {
            Components.Add<T>(cmp);
        }

        public void RemoveComponent<T>() where T : ComponentBase
        {
            Components.Remove<T>();
        }
    }
}
