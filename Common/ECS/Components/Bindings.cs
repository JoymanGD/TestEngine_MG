using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Common.ECS.Components
{
    public struct Bindings
    {
        public Dictionary<string, int> Pairs { get; private set; }

        public Bindings(string _fileName){
            Pairs = new Dictionary<string, int>();
            InitializeBindings(_fileName);
        }

        void InitializeBindings(string _fileName){
            var json = File.ReadAllText(@".\Content\Json\" + _fileName + ".json");

            var objects = JObject.Parse(json);
            var list = objects.AsJEnumerable();
            foreach (var item in list)
            {
                Pairs.Add(item.First.Path, item.First.Value<int>());
            }
        }
    }
}