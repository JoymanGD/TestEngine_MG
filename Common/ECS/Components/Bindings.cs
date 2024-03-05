using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Common.ECS.Components
{
    public struct Bindings
    {
        public Dictionary<string, int> Pairs { get; private set; }

        public Bindings(string fileName)
        {
            Pairs = new Dictionary<string, int>();
            InitializeBindings(fileName);
        }

        void InitializeBindings(string fileName)
        {
            var json = File.ReadAllText(@".\Content\Json\" + fileName + ".json");
            
            var objects = JObject.Parse(json);
            var list = objects.AsJEnumerable();
            
            foreach (var item in list)
            {
                var firstElement = item.First;
                
                if (firstElement != null)
                {
                    Pairs.Add(firstElement.Path, firstElement.Value<int>());
                }
            }
        }
    }
}