using System.IO;

namespace DummyLibrary
{
    public class BahyrWithBacon
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(Name);
                    writer.Write(Description);
                }
                return m.ToArray();
            }
        }
    }
}
