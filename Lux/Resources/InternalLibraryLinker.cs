using System;
using System.IO;
using System.Reflection;

namespace Lux.Resources
{
    static internal class InternalLibraryLinker
    {
        internal static void LoadDLLs()
        {
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
        }

        private static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Console.WriteLine("Loaded DLL \"" + args.Name.Substring(0, args.Name.IndexOf(',')) + ".dll" + "\".");
            return Assembly.Load(StreamToBytes(Assembly.GetExecutingAssembly().GetManifestResourceStream("Lux.Resources." + args.Name.Substring(0, args.Name.IndexOf(',')) + ".dll")));
        }

        private static byte[] StreamToBytes(Stream input)
        {
            var capacity = input.CanSeek ? (int)input.Length : 0;
            using (var output = new MemoryStream(capacity))
            {
                int readLength;
                var buffer = new byte[4096];

                do
                {
                    readLength = input.Read(buffer, 0, buffer.Length);
                    output.Write(buffer, 0, readLength);
                }
                while (readLength != 0);

                return output.ToArray();
            }
        }
    }
}
