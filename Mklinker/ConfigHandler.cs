using System.IO.Abstractions;
using Mklinker.Abstractions;

namespace Mklinker {

	class ConfigHandler : IConfigHandler {

		readonly IFileSystem fileSystem;
		readonly IConfigSerializer configSerializer;

		public ConfigHandler (IFileSystem fileSystem, IConfigSerializer configSerializer) {
			this.fileSystem = fileSystem;
			this.configSerializer = configSerializer;
		}

		public IConfig LoadConfig(string pathToConfigFile) {
			return configSerializer.Deserialize(fileSystem.File.ReadAllText(pathToConfigFile));
		}

		public void SaveConfig(IConfig config, string pathToConfigFile) {
			fileSystem.File.WriteAllText(pathToConfigFile, configSerializer.Serialize (config));
		}

		public void DeleteConfig(string pathToConfigFile) {
			fileSystem.File.Delete(pathToConfigFile);
		}

		public bool DoesConfigExist(string pathToConfigFile) {
			return fileSystem.File.Exists(pathToConfigFile);
		}

	}

}
