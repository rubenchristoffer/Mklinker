using System.IO.Abstractions;
using Mklinker.Abstractions;

namespace Mklinker {

	class ConfigHandler : IConfigHandler {

		readonly IFileSystem fileSystem;
		readonly IConfig referenceConfig;

		public ConfigHandler (IFileSystem fileSystem, IConfig referenceConfig) {
			this.fileSystem = fileSystem;
			this.referenceConfig = referenceConfig;
		}

		public IConfig LoadConfig(string pathToConfigFile) {
			return referenceConfig.Deserialize(fileSystem.File.ReadAllText(pathToConfigFile));
		}

		public void SaveConfig(IConfig config, string pathToConfigFile) {
			fileSystem.File.WriteAllText(pathToConfigFile, config.Serialize());
		}

		public void DeleteConfig(string pathToConfigFile) {
			fileSystem.File.Delete(pathToConfigFile);
		}

		public bool DoesConfigExist(string pathToConfigFile) {
			return fileSystem.File.Exists(pathToConfigFile);
		}

	}

}
