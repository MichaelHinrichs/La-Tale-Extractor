using System.IO;

namespace La_Tale_Extractor
{
    class Program
    {
        static BinaryReader br;
        static void Main(string[] args)
        {
            br = new BinaryReader(File.OpenRead(args[0]));
            br.BaseStream.Position = br.BaseStream.Length - 152;
            int table = br.ReadInt32() + br.ReadInt32();
            ushort count = br.ReadUInt16();
            br.ReadInt32();

            br.BaseStream.Position = table;
            System.Collections.Generic.List<Subfile> subfiles = new();
            for (int i = 0; i < count; i++)
            {
                subfiles.Add(new());
                br.ReadInt32();
            }

            string path = Path.GetDirectoryName(args[0]);
            foreach (Subfile file in subfiles)
            {
                br.BaseStream.Position = file.start;
                Directory.CreateDirectory(path + "//" + Path.GetDirectoryName(file.name));
                BinaryWriter bw = new(File.Create(path + "//" + file.name));
                bw.Write(br.ReadBytes(file.size));
                bw.Close();
            }
        }

        class Subfile
        {
            public string name = new(new string(System.Text.Encoding.GetEncoding("ISO-8859-1").GetChars(br.ReadBytes(128))).TrimEnd('\0'));
            public int start = br.ReadInt32();
            public int size = br.ReadInt32();
        }
    }
}
