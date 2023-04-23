using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Newtonsoft.Json;
using Xceed.Wpf.Toolkit;

namespace MegaEzhednevnich
{
    internal class Notes
    {
        public string name;
        public string description;
        public DateTime date;       

        public static void Serialize(List<Notes> notes)
        {
            string json_text = JsonConvert.SerializeObject(notes);
            File.WriteAllText("Notes.json", json_text);

        }

        public static List<Notes> Deserialize(List<Notes> notes)
        {
            string json_text = File.ReadAllText("Notes.json");
            notes = JsonConvert.DeserializeObject<List<Notes>>(json_text);
            List<Notes> result = new List<Notes>();
            foreach (Notes note in notes)
            {
                Notes one_notes = new Notes();
                one_notes.name = note.name;
                one_notes.description = note.description;
                one_notes.date = note.date;
                result.Add(one_notes);
            }
            return result;
        }
    }

}
