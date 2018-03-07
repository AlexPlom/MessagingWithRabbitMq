using System.IO;
using DummyLibrary;


public static class Deserializer
{
    public static BahyrWithBacon DeserializeToBacon(byte[] data)
    {
        var result = new BahyrWithBacon();
        using (MemoryStream m = new MemoryStream(data))
        {
            using (BinaryReader reader = new BinaryReader(m))
            {
                result.Description = reader.ReadString();
                result.Name = reader.ReadString();
            }
        }
        return result;
    }
}