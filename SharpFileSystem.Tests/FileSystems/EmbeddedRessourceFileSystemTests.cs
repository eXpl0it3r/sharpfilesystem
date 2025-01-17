using System;
using System.IO;
using System.Linq;
using System.Reflection;
using SharpFileSystem.FileSystems;
using Xunit;


namespace SharpFileSystem.Tests.FileSystems
{
    public class EmbeddedRessourceFileSystemTests
    {
        [Fact]
        void eResFSTest()
        {
            string content = "test embedded resource";
            string deepContent = "deep file";
            var filePath = FileSystemPath.Root.AppendFile("test.txt");
            var resDir = FileSystemPath.Root.AppendDirectory("resDir");
            var deepFilePath = resDir.AppendFile("deepFile.txt");
            EmbeddedResourceFileSystem eRscFS = new EmbeddedResourceFileSystem(Assembly.GetAssembly(typeof(EmbeddedRessourceFileSystemTests)));
            Assert.True(eRscFS.Exists(filePath));
            using (var stream = eRscFS.OpenFile(filePath,FileAccess.Read))
            {
                using (var reader = new StreamReader(stream))
                {
                    string value = reader.ReadToEnd();
                    Assert.Equal(content,value);
                }
            }

            var readContent = eRscFS.ReadAllText(filePath);
            Assert.Equal(content,readContent);

            Assert.True(eRscFS.Exists("/resDir/deepFile.txt"));
            using (var stream = eRscFS.OpenFile(deepFilePath,FileAccess.Read))
            {
                using (var reader = new StreamReader(stream))
                {
                    string value = reader.ReadToEnd();
                    Assert.Equal(deepContent,value);
                }
            }

            var entities = eRscFS.GetEntities(FileSystemPath.Root);
            Assert.Equal(4,entities.Count);
        }

        [Fact]
        void eResFSnotSupportedFeaturesTest()
        {

            EmbeddedResourceFileSystem eFS = new EmbeddedResourceFileSystem(Assembly.GetAssembly(typeof(EmbeddedRessourceFileSystemTests)));
            Assert.True(eFS.IsReadOnly);
            Assert.Throws<NotSupportedException>(() => eFS.CreateDirectory("/test/"));
            Assert.Throws<NotSupportedException>(() => eFS.CreateFile("/test.txt"));
            Assert.Throws<NotSupportedException>(() => eFS.Delete("/test.txt"));
        }


    }
}
