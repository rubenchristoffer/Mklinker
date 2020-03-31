using System.Reflection;
using System.Diagnostics;
using System.IO.Abstractions;
using Autofac;
using Mklinker.Abstractions;

namespace Mklinker {

	public static class Program {

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

			using (var scope = builder.Build().BeginLifetimeScope()) {
				scope.Resolve<ICommandExecutor>().Execute(args);
			}
		}

	}

}
