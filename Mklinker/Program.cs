using System.Reflection;
using System.Diagnostics;
using System.IO.Abstractions;
using Autofac;
using Mklinker.Abstractions;

namespace Mklinker {

	public static class Program {

		public const string DEFAULT_LINKER_PATH = "linker.config";

		public static string GetVersion() {
			return FileVersionInfo.GetVersionInfo(Assembly.GetAssembly(typeof (Program)).Location).ProductVersion;
		}

		public static void Main(string[] args) {
			var builder = new ContainerBuilder();

			builder.RegisterInstance<Config>(new Config(Program.GetVersion())).As<IConfig>();

			builder.RegisterType<FileSystem>().As<IFileSystem>();
			builder.RegisterType<ConfigHandler>().As<IConfigHandler>();
			builder.RegisterType<ArgumentParser>().As<IArgumentParser>();
			builder.RegisterType<CommandExecutor>().As<ICommandExecutor>();
			builder.RegisterType<Process>().As<IProcess>();
			builder.RegisterType<Console>().As<IConsole>();
			builder.RegisterType<XMLConfigSerializer>().As<IConfigSerializer>();
			builder.RegisterType<PathResolver>().As<IPathResolver>();

			// Platform dependent
			builder.RegisterType<WindowsLinker>().As<ILinker>();

			using (var scope = builder.Build().BeginLifetimeScope()) {
				scope.Resolve<ICommandExecutor>().Execute(args);
			}
		}

	}

}
