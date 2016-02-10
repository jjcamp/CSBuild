// Copyright 2015 John Camp
// Released under MIT License
using System;
using System.Diagnostics;
using System.Management.Automation;

namespace CSBuild {
	[Cmdlet("New", "CSBuild")]
	public class New_CSBuild: PSCmdlet {
		protected override void ProcessRecord() {
			BuildObject build = new BuildObject();
			build.Framework = "4.0.30319";

			this.WriteObject(build);
		}
	}

	[Cmdlet("Compile", "CSBuild")]
	public class Compile_CSBuild: PSCmdlet {
		[Parameter(Mandatory=true, Position=0)]
		public CSBuild.BuildObject Build;

		protected override void ProcessRecord() {
			string srcs = "";
			foreach (string s in Build.Sources) {
				srcs += s + " ";
			}
			string refs = "";
			foreach (string r in Build.References) {
				refs += "/reference:\"" + r + "\" ";
			}
			string libs = "";
			foreach (string l in Build.LibDirectories) {
				libs += "/lib:\"" + l + "\" ";
			}
			string res = "";
			foreach (string r in Build.Resources) {
				res += "/resource:\"" + r + "\" ";
			}
			string opts = "";
			foreach (string o in Build.Options) {
				opts += "/" + o + " ";
			}

			string frameworkPath = @"C:\Windows\Microsoft.NET\Framework";
			string cscPath = frameworkPath + "\\v" + Build.Framework + "\\csc.exe";

			string passArgs = opts + libs + refs + res + srcs;

			this.WriteObject("Compiling with options: " + passArgs);
			Process csc = new Process();
			csc.StartInfo.UseShellExecute = false;
			csc.StartInfo.RedirectStandardOutput = true;
			csc.StartInfo.FileName = cscPath;
			csc.StartInfo.Arguments = passArgs;
			csc.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
			csc.Start();
			string output = csc.StandardOutput.ReadToEnd();
			csc.WaitForExit();
			this.WriteObject(output);
		}
	}

	public class BuildObject {
		public string[] Sources;
		public string[] References;
		public string[] LibDirectories;
		public string[] Resources;
		public string[] Options;
		public string Framework;

		public BuildObject() {
			Sources = new string[0];
			References = new string[0];
			LibDirectories = new string[0];
			Resources = new string[0];
			Options = new string[0];
			Framework = "";
		}
	}
}
