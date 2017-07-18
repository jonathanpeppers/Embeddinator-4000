using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Text;
using Mono.Cecil;
using MonoDroid.Generation;
using NUnit.Framework;

namespace MonoEmbeddinator4000.Tests
{
    [TestFixture]
    public class InterfaceGenTest : TempFileTest
    {
        AssemblyDefinition LoadAssembly(string resourceFile)
        {
            var parameters = new CompilerParameters
            {
                OutputAssembly = temp,
            };
            parameters.ReferencedAssemblies.Add(XamarinAndroid.FindAssembly("System.dll"));
            parameters.ReferencedAssemblies.Add(XamarinAndroid.FindAssembly("Mono.Android.dll"));

            AssemblyGenerator.CreateFromResource(resourceFile, parameters);
            return AssemblyDefinition.ReadAssembly(temp);
        }

        [Test]
        public void Leeroy()
        {
            var dir = Path.GetDirectoryName(GetType().Assembly.Location);
            var assembly = LoadAssembly("IJavaCallback");
            var type = assembly.MainModule.Types.First(t => t.Name == "IJavaCallback");
            var options = new CodeGenerationOptions();
            var gen = new ManagedInterfaceGen(type);

            gen.Validate(options, new GenericParameterDefinitionList());

            string cs;
            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    gen.Generate(writer, "\t", options, new GenerationInfo(dir, dir, temp));
                }

                cs = Encoding.Default.GetString(stream.ToArray());
            }

            Approvals.Approve(cs);
        }
    }
}
