using Serilog.Core;
using Serilog.Events;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace VSDocumentReopen.Infrastructure.Logging.Logentries
{
	public class MemoryInfoEnricher : ILogEventEnricher
	{
		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool GetPhysicallyInstalledSystemMemory(out long TotalMemoryInKilobytes);

		private class MemoryInfo
		{
			private const double Kb = 1024.0;
			private const string ValueFormatter = "#,##0 Kb";

			public string SystemMemory { get; }

			public string PrivateMemorySize64 { get; }
			public string PagedMemorySize64 { get; }
			public string VirtualMemorySize64 { get; }

			public string PeakVirtualMemorySize64 { get; }
			public string PeakPagedMemorySize64 { get; }
			public string PeakWorkingSet64 { get; }

			public string WorkingSet64 { get; }

			public MemoryInfo(long systemMemory, Process proc)
			{
				SystemMemory = systemMemory.ToString(ValueFormatter);

				PrivateMemorySize64 = (proc.PrivateMemorySize64 / Kb).ToString(ValueFormatter);
				PagedMemorySize64 = (proc.PagedMemorySize64 / Kb).ToString(ValueFormatter);
				VirtualMemorySize64 = (proc.VirtualMemorySize64 / Kb).ToString(ValueFormatter);
				WorkingSet64 = (proc.WorkingSet64 / Kb).ToString(ValueFormatter);

				PeakVirtualMemorySize64 = (proc.PeakVirtualMemorySize64 / Kb).ToString(ValueFormatter);
				PeakPagedMemorySize64 = (proc.PeakPagedMemorySize64 / Kb).ToString(ValueFormatter);
				PeakWorkingSet64 = (proc.PeakWorkingSet64 / Kb).ToString(ValueFormatter);
			}
		}

		private readonly Lazy<long> _systemMemoryReader;

		public MemoryInfoEnricher()
		{
			_systemMemoryReader = new Lazy<long>(GetSystemMemory, true);
		}

		private long GetSystemMemory()
		{
			GetPhysicallyInstalledSystemMemory(out long memoryInKb);
			return memoryInKb;
		}

		public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
		{
			if (logEvent != null)
			{
				try
				{
					var currentProcess = Process.GetCurrentProcess();

					logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Memory Info",
						new MemoryInfo(_systemMemoryReader.Value, currentProcess),
						true));
				}
				catch (Exception ex)
				{
					LoggerContext.Current.Logger.Error($"Unexpected error happened in {nameof(MemoryInfoEnricher)}", ex);
				}
			}
		}
	}
}